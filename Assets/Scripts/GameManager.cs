using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Score Settings")]
    public TMP_Text scoreText;
    public int scorePerMonster = 100;

    [Header("Player")]
    public Animator playerAnimator;
    public MovementInput playerMovement;
    public string finishAnimationStateName = "Victory Idle";

    [Header("Debug Teleport")]
    public Transform player;
    public Transform debugTeleportExit;
    public KeyCode debugTeleportKey = KeyCode.K;

    private int currentScore = 0;
    private bool gameFinished = false;

    public int CurrentScore => currentScore;

    private void Start()
    {
        currentScore = 0;
        UpdateScoreUI();
    }

    private void Update()
    {
        if (Input.GetKeyDown(debugTeleportKey))
        {
            DebugTeleportToExit();
        }
    }

    public void AddMonsterKill()
    {
        if (gameFinished)
        {
            return;
        }

        currentScore += scorePerMonster;
        UpdateScoreUI();

        Debug.Log($"Monster defeated! Score: {currentScore}");
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score: {currentScore}";
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

        StatsManager.AddGameResult(currentScore);

        if (scoreText != null)
        {
            scoreText.text = $"Final Score: {currentScore}";
        }

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
            Debug.LogWarning(
                "Debug teleport failed: Player or Debug Teleport Exit is not assigned."
            );
            return;
        }

        MovementInput movement = player.GetComponent<MovementInput>();
        CharacterController controller =
            player.GetComponent<CharacterController>();

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