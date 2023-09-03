using UnityEngine;
using System.Collections;

public class HitMarkerHeadshot : MonoBehaviour
{
    public float fadeDuration = 0.5f; // Duration in seconds for fading in/out
    public float fadeDelay = 1f; // Delay in seconds before fading out after a bullet hit
    public float minSize;
    public float maxSize;
    public float speed;

    private RectTransform reticle;
    private float currentSize;
    private CanvasGroup canvasGroup;

    private void Start()
    {
        // Get the CanvasGroup component attached to the hit marker UI
        canvasGroup = GetComponent<CanvasGroup>();

        // Start with the hit marker completely faded out
        canvasGroup.alpha = 0f;

        reticle = GetComponent<RectTransform>();
    }

    private void Update()
    {
        // Set the reticle's size to the currentSize value.
        reticle.sizeDelta = new Vector2(currentSize, currentSize);
    }

    public void ShowHitMarkerHeadshot()
    {
        // Cancel any existing fade coroutine
        StopAllCoroutines();

        // Fade in the hit marker
        StartCoroutine(FadeInHitMarkerHeadshot());
    }

    private IEnumerator FadeInHitMarkerHeadshot()
    {
        // Gradually increase the alpha value to fade in the hit marker
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            float normalizedTime = elapsedTime / fadeDuration;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, normalizedTime);
            currentSize = Mathf.Lerp(currentSize, minSize, Time.deltaTime * speed);


            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Wait for the fade delay before fading out the hit marker
        yield return new WaitForSeconds(fadeDelay);

        // Fade out the hit marker
        StartCoroutine(FadeOutHitMarkerHeadshot());
    }

    private IEnumerator FadeOutHitMarkerHeadshot()
    {
        // Gradually decrease the alpha value to fade out the hit marker
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            float normalizedTime = elapsedTime / fadeDuration;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, normalizedTime);
            currentSize = Mathf.Lerp(currentSize, maxSize, Time.deltaTime * speed);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Make sure the hit marker is completely faded out
        canvasGroup.alpha = 0f;
    }
}
