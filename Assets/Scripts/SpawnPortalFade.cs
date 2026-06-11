using System.Collections;
using UnityEngine;

public class SpawnPortalFade : MonoBehaviour
{
    [Header("Timing")]
    public float waitBeforeFade = 2f;
    public float fadeDuration = 3f;

    [Header("Scale")]
    public float endScale = 0f;

    private Vector3 startScale;

    private void Start()
    {
        startScale = transform.localScale;

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySpawnSound();
        }

        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(waitBeforeFade);

        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;

            float t = elapsed / fadeDuration;
            transform.localScale = Vector3.Lerp(startScale, Vector3.one * endScale, t);

            yield return null;
        }

        gameObject.SetActive(false);
    }
}