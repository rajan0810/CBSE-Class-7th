using UnityEngine;
using System.Collections;
using TMPro;

public class UIAnimation : MonoBehaviour
{
    private static readonly int Opacity = Shader.PropertyToID("_opacity");
    private static readonly int SlideOffsetX = Shader.PropertyToID("_SlideX");
    private static readonly int SlideOffsetY = Shader.PropertyToID("_SlideY");

    public Material targetMaterial;
    public TextMeshProUGUI textMeshPro;

    public float fadeDuration = 1.0f;
    public float slideDuration = 1.0f;

    private Coroutine _materialCoroutine;
    private Coroutine _textCoroutine;

    // ----------------------
    // Material Control
    // ----------------------
    private void SetMaterialOpacity(float opacity)
    {
        if (targetMaterial)
        {
            targetMaterial.SetFloat(Opacity, opacity);
        }
    }

    private void SetMaterialSlideOffsetX(float slideX)
    {
        if (targetMaterial)
        {
            targetMaterial.SetFloat(SlideOffsetX, slideX);
        }
    }

    private void SetMaterialSlideOffsetY(float slideY)
    {
        if (targetMaterial)
        {
            targetMaterial.SetFloat(SlideOffsetY, slideY);
        }
    }

    // ----------------------
    // Material Fade Animations
    // ----------------------
    public void FadeMaterialIn()
    {
        if (_materialCoroutine != null) StopCoroutine(_materialCoroutine);
        _materialCoroutine = StartCoroutine(FadeMaterialCoroutine(0f, 1f, fadeDuration));
    }

    public void FadeMaterialOut()
    {
        if (_materialCoroutine != null) StopCoroutine(_materialCoroutine);
        _materialCoroutine = StartCoroutine(FadeMaterialCoroutine(1f, 0f, fadeDuration));
    }

    private IEnumerator FadeMaterialCoroutine(float startOpacity, float targetOpacity, float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float newOpacity = Mathf.Lerp(startOpacity, targetOpacity, elapsedTime / duration);
            SetMaterialOpacity(newOpacity);
            yield return null;
        }
        SetMaterialOpacity(targetOpacity);
    }

    // ----------------------
    // Material Slide Animations
    // ----------------------
    public void SlideLeftIn()
    {
        if (_materialCoroutine != null) StopCoroutine(_materialCoroutine);
        _materialCoroutine = StartCoroutine(SlideXMaterialCoroutine(-1f, 0f, slideDuration));
    }

    public void SlideRightOut()
    {
        if (_materialCoroutine != null) StopCoroutine(_materialCoroutine);
        _materialCoroutine = StartCoroutine(SlideXMaterialCoroutine(0f, 1f, slideDuration));
    }

    public void SlideRightIn()
    {
        if (_materialCoroutine != null) StopCoroutine(_materialCoroutine);
        _materialCoroutine = StartCoroutine(SlideXMaterialCoroutine(1f, 0f, slideDuration));
    }

    public void SlideLeftOut()
    {
        if (_materialCoroutine != null) StopCoroutine(_materialCoroutine);
        _materialCoroutine = StartCoroutine(SlideXMaterialCoroutine(0f, -1f, slideDuration));
    }

    public void SlideUpIn()
    {
        if (_materialCoroutine != null) StopCoroutine(_materialCoroutine);
        _materialCoroutine = StartCoroutine(SlideYMaterialCoroutine(-1f, 0f, slideDuration));
    }

    public void SlideDownOut()
    {
        if (_materialCoroutine != null) StopCoroutine(_materialCoroutine);
        _materialCoroutine = StartCoroutine(SlideYMaterialCoroutine(0f, 1f, slideDuration));
    }

    public void SlideDownIn()
    {
        if (_materialCoroutine != null) StopCoroutine(_materialCoroutine);
        _materialCoroutine = StartCoroutine(SlideYMaterialCoroutine(1f, 0f, slideDuration));
    }

    public void SlideUpOut()
    {
        if (_materialCoroutine != null) StopCoroutine(_materialCoroutine);
        _materialCoroutine = StartCoroutine(SlideYMaterialCoroutine(0f, -1f, slideDuration));
    }

    private IEnumerator SlideXMaterialCoroutine(float startX, float targetX, float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float newSlideX = Mathf.Lerp(startX, targetX, elapsedTime / duration);
            SetMaterialSlideOffsetX(newSlideX);
            yield return null;
        }
        SetMaterialSlideOffsetX(targetX);
    }

    private IEnumerator SlideYMaterialCoroutine(float startY, float targetY, float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float newSlideY = Mathf.Lerp(startY, targetY, elapsedTime / duration);
            SetMaterialSlideOffsetY(newSlideY);
            yield return null;
        }
        SetMaterialSlideOffsetY(targetY);
    }

    // ----------------------
    // Text Fade Animations
    // ----------------------
    public void TextFadeIn()
    {
        if (_textCoroutine != null) StopCoroutine(_textCoroutine);
        _textCoroutine = StartCoroutine(FadeTextCoroutine(0f, 1f, fadeDuration));
    }

    public void TextFadeOut()
    {
        if (_textCoroutine != null) StopCoroutine(_textCoroutine);
        _textCoroutine = StartCoroutine(FadeTextCoroutine(1f, 0f, fadeDuration));
    }

    private IEnumerator FadeTextCoroutine(float startAlpha, float targetAlpha, float duration)
    {
        float elapsedTime = 0f;
        Color color = textMeshPro.color;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / duration);
            textMeshPro.color = new Color(color.r, color.g, color.b, newAlpha);
            yield return null;
        }

        textMeshPro.color = new Color(color.r, color.g, color.b, targetAlpha);
    }
}
