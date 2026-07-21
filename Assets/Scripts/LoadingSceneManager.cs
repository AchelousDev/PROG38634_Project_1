using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class LoadingSceneManager : MonoBehaviour
{
    public float loadingTime = 5f;
    public TMP_Text loadingText;
    public RectTransform progressFill;
    public float progressBarWidth = 600f;

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

            float progress = Mathf.Clamp01(timer / loadingTime);
            int progressPercent = Mathf.RoundToInt(progress * 100f);

            if (loadingText != null)
            {
                loadingText.text = "Establishing Satellite Link..." + progressPercent + "%";
            }

            if (progressFill != null)
            {
                progressFill.sizeDelta = new Vector2(progressBarWidth * progress, progressFill.sizeDelta.y);
            }

            yield return null;
        }

        SceneManager.LoadScene("HomeScene");
    }
}