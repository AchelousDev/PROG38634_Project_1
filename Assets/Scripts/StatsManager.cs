using TMPro;
using UnityEngine;

public class StatsManager : MonoBehaviour
{
    [Header("Home Scene Texts")]
    public TMP_Text totalScoreText;
    public TMP_Text bestRunText;
    public TMP_Text gamePlaysText;

    private const string TotalScoreKey = "TotalScoreTime";
    private const string BestRunKey = "BestRunTime";
    private const string GamePlaysKey = "GamePlays";

    private void Start()
    {
        UpdateStatsUI();
    }

    public void UpdateStatsUI()
    {
        float totalScoreTime = PlayerPrefs.GetFloat(TotalScoreKey, 0f);
        float bestRunTime = PlayerPrefs.GetFloat(BestRunKey, 0f);
        int gamePlays = PlayerPrefs.GetInt(GamePlaysKey, 0);

        if (totalScoreText != null)
        {
            totalScoreText.text = "Total Accumulated Score Time: " + FormatTime(totalScoreTime);
        }

        if (bestRunText != null)
        {
            bestRunText.text = "Best Run Time: " + FormatTime(bestRunTime);
        }

        if (gamePlaysText != null)
        {
            gamePlaysText.text = "Number of Game Plays: " + gamePlays;
        }
    }

    public static void AddGameResult(float scoreTime)
    {
        float currentTotal = PlayerPrefs.GetFloat(TotalScoreKey, 0f);
        float currentBest = PlayerPrefs.GetFloat(BestRunKey, 0f);
        int currentPlays = PlayerPrefs.GetInt(GamePlaysKey, 0);

        PlayerPrefs.SetFloat(TotalScoreKey, currentTotal + scoreTime);

        if (scoreTime > currentBest)
        {
            PlayerPrefs.SetFloat(BestRunKey, scoreTime);
        }

        PlayerPrefs.SetInt(GamePlaysKey, currentPlays + 1);
        PlayerPrefs.Save();
    }

    public static void ResetStats()
    {
        PlayerPrefs.DeleteKey(TotalScoreKey);
        PlayerPrefs.DeleteKey(BestRunKey);
        PlayerPrefs.DeleteKey(GamePlaysKey);
        PlayerPrefs.Save();
    }

    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);

        return $"{minutes:00}:{seconds:00}";
    }
}