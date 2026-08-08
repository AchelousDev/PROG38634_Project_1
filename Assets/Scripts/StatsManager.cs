using UnityEngine;

public static class StatsManager
{
    private const string TotalScoreKey = "TotalScore";
    private const string BestScoreKey = "BestScore";
    private const string GamePlaysKey = "GamePlays";
    private const string LastScoreKey = "LastScore";

    public static void AddGameResult(int score)
    {
        int totalScore = PlayerPrefs.GetInt(TotalScoreKey, 0);
        int bestScore = PlayerPrefs.GetInt(BestScoreKey, 0);
        int gamePlays = PlayerPrefs.GetInt(GamePlaysKey, 0);

        totalScore += score;
        gamePlays++;

        if (score > bestScore)
        {
            bestScore = score;
        }

        PlayerPrefs.SetInt(TotalScoreKey, totalScore);
        PlayerPrefs.SetInt(BestScoreKey, bestScore);
        PlayerPrefs.SetInt(GamePlaysKey, gamePlays);
        PlayerPrefs.SetInt(LastScoreKey, score);

        PlayerPrefs.Save();

        Debug.Log(
            $"Game Result Saved | " +
            $"Score: {score} | " +
            $"Best Score: {bestScore} | " +
            $"Total Score: {totalScore} | " +
            $"Game Plays: {gamePlays}"
        );
    }

    public static int GetTotalScore()
    {
        return PlayerPrefs.GetInt(TotalScoreKey, 0);
    }

    public static int GetBestScore()
    {
        return PlayerPrefs.GetInt(BestScoreKey, 0);
    }

    public static int GetGamePlays()
    {
        return PlayerPrefs.GetInt(GamePlaysKey, 0);
    }

    public static int GetLastScore()
    {
        return PlayerPrefs.GetInt(LastScoreKey, 0);
    }

    public static void ResetStats()
    {
        PlayerPrefs.DeleteKey(TotalScoreKey);
        PlayerPrefs.DeleteKey(BestScoreKey);
        PlayerPrefs.DeleteKey(GamePlaysKey);
        PlayerPrefs.DeleteKey(LastScoreKey);

        PlayerPrefs.Save();

        Debug.Log("Game stats reset.");
    }
}