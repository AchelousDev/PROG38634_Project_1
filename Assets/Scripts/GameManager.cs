using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Timer Settings")]
    public float startTime = 60f;
    public TMP_Text timerText;
    
    [Header("Player")]
    public Animator playerAnimator;
    public MovementInput playerMovement;
    public string finishAnimationStateName = "Victory Idle";
    
    [Header("Debug Teleport")]
    public Transform player;
    public Transform debugTeleportExit;
    public KeyCode debugTeleportKey = KeyCode.K;

    private float currentTime;
    private bool gameFinished = false;

    private void Start()
    {
        currentTime = startTime;
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

        currentTime -= Time.deltaTime;
        UpdateTimerUI();
    }

    private void UpdateTimerUI()
    {
        float displayTime = currentTime;

        int minutes = Mathf.FloorToInt(Mathf.Abs(displayTime) / 60f);
        int seconds = Mathf.FloorToInt(Mathf.Abs(displayTime) % 60f);
        int milliseconds = Mathf.FloorToInt((Mathf.Abs(displayTime) * 1000f) % 1000f);

        if (displayTime < 0)
        {
            timerText.text = $"- Time: {minutes:00}:{seconds:00}.{milliseconds:000}";
            timerText.color = Color.red;
        }
        else
        {
            timerText.text = $"Time: {minutes:00}:{seconds:00}.{milliseconds:000}";
            timerText.color = Color.white;
        }
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

        float score = Mathf.Max(0f, currentTime);
        StatsManager.AddGameResult(score);

        timerText.text = "Finished! Score: " + score.ToString("F2");

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