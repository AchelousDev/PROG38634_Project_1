using UnityEngine;
using UnityEngine.SceneManagement;

public class ARSceneUI : MonoBehaviour
{
    public void ReturnHome()
    {
        SceneManager.LoadScene("HomeScene");
    }
}