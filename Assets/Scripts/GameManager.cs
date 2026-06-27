using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Timer Settings")]
    public TMP_Text timerText;
    public float movementStartThreshold = 0.1f;
    
    [Header("Player")]
    public Animator playerAnimator;
    public MovementInput playerMovement;
    public string finishAnimationStateName = "Victory Idle";
    
    [Header("Debug Teleport")]
    public Transform player;
    public Transform debugTeleportExit;
    public KeyCode debugTeleportKey = KeyCode.K;

    private float elapsedTime;
    private bool timerStarted = false;
    private bool gameFinished = false;

    private void Start()
    {
        elapsedTime = 0f;
        UpdateTimerUI();
    }

    private void Update()
    {
        if (Input.GetKeyDown(debugTeleportKey))
        {
            DebugTeleportToExit();
        }

        if (gameFinished)
        {
            return;
        }

        if (!timerStarted)
        {
            timerStarted = HasPlayerStartedMoving();
        }

        if (timerStarted)
        {
            elapsedTime += Time.deltaTime;
        }

        UpdateTimerUI();
    }

    private void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);
        int milliseconds = Mathf.FloorToInt((elapsedTime * 1000f) % 1000f);

        if (timerStarted)
        {
            timerText.text = $"Time: {minutes:00}:{seconds:00}.{milliseconds:000}";
        }
        else
        {
            timerText.text = "Time: 00:00.000";
        }

        timerText.color = Color.white;
    }

    private bool HasPlayerStartedMoving()
    {
        if (playerMovement == null || !playerMovement.enabled)
        {
            return false;
        }

        Vector2 movementInput = new Vector2(playerMovement.InputX, playerMovement.InputZ);
        return movementInput.magnitude > movementStartThreshold;
    }

    public void FinishGame()
    {
        if (gameFinished)
        {
            return;
        }

        gameFinished = true;
        
        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }

        if (playerAnimator != null)
        {
            playerAnimator.Play(finishAnimationStateName);
        }
        
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayFinishSound();
        }

        float score = elapsedTime;
        StatsManager.AddGameResult(score);

        timerText.text = "Finished! Time: " + score.ToString("F2") + "s";

        Invoke(nameof(ReturnToHome), 5f);
    }

    private void ReturnToHome()
    {
        SceneManager.LoadScene("HomeScene");
    }
    
    private void DebugTeleportToExit()
    {
        if (player == null || debugTeleportExit == null)
        {
            Debug.LogWarning("Debug teleport failed: Player or Debug Teleport Exit is not assigned.");
            return;
        }

        MovementInput movement = player.GetComponent<MovementInput>();
        CharacterController controller = player.GetComponent<CharacterController>();

        if (movement != null)
        {
            movement.enabled = false;
        }

        if (controller != null)
        {
            controller.enabled = false;
        }

        player.position = debugTeleportExit.position;
        player.rotation = debugTeleportExit.rotation;

        if (controller != null)
        {
            controller.enabled = true;
        }

        if (movement != null)
        {
            movement.enabled = true;
        }

        Debug.Log("Debug teleport: Player moved to TeleportExit.");
    }
}