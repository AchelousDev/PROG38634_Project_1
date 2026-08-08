using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadHomeScene()
    {
        SceneManager.LoadScene("HomeScene");
    }

    public void LoadGameScene()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void LoadSettingsScene()
    {
        SceneManager.LoadScene("SettingScene");
    }
    
    public void OpenARScoreboard()
    {
        SceneManager.LoadScene("ARScoreboardScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    
    public void ResetStats()
    {
        StatsManager.ResetStats();
    }
}