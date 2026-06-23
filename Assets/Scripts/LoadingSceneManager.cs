using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LoadingSceneManager : MonoBehaviour
{
    public float loadingTime = 15f;
    public TMP_Text loadingText;

    private void Start()
    {
        StartCoroutine(LoadHomeAfterDelay());
    }

    private IEnumerator LoadHomeAfterDelay()
    {
        float timer = 0f;

        while (timer < loadingTime)
        {
            timer += Time.deltaTime;

            if (loadingText != null)
            {
                int progress = Mathf.Clamp(Mathf.RoundToInt((timer / loadingTime) * 100f), 0, 100);
                loadingText.text = "Loading... " + progress + "%";
            }

            yield return null;
        }

        SceneManager.LoadScene("HomeScene");
    }
}