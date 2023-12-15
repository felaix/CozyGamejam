using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewLevelUnlocked : MonoBehaviour
{
    public float fadeDuration = 2.0f; // Adjust the duration of the fade
    private Image image;
    private float currentAlpha = 1.0f;

    void Start()
    {
        image = GetComponent<Image>();

        if (image == null)
        {
            Debug.LogError("Image component not found on this GameObject.");
        }else
        {
            StartFadeOut();
        }
    }

    void Update()
    {
        // Update the alpha value of the image
        Color newColor = image.color;
        newColor.a = currentAlpha;
        image.color = newColor;

        if (currentAlpha == 0) { gameObject.SetActive(false); }
    }

    void StartFadeOut()
    {
        // Start the fade-out coroutine
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        // Calculate the alpha change per frame
        float alphaChangePerFrame = Time.deltaTime / fadeDuration;

        // Loop until alpha becomes 0
        while (currentAlpha > 0.0f)
        {
            currentAlpha -= alphaChangePerFrame;

            // Clamp the alpha value to prevent going below 0
            currentAlpha = Mathf.Clamp01(currentAlpha);

            yield return null;
        }

        currentAlpha = 0.0f;

        // Deactivate the GameObject
        gameObject.SetActive(false);
    }

}
