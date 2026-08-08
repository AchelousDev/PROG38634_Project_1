using TMPro;
using UnityEngine;

public class HomeStatsUI : MonoBehaviour
{
    [Header("Stats UI")]
    [SerializeField] private TMP_Text bestScoreText;
    [SerializeField] private TMP_Text totalScoreText;
    [SerializeField] private TMP_Text gamePlaysText;

    private void Start()
    {
        UpdateStatsUI();
    }

    public void UpdateStatsUI()
    {
        bestScoreText.text = "Best Score: " + StatsManager.GetBestScore();
        totalScoreText.text = "Total Score: " + StatsManager.GetTotalScore();
        gamePlaysText.text = "Game Plays: " + StatsManager.GetGamePlays();
    }
}