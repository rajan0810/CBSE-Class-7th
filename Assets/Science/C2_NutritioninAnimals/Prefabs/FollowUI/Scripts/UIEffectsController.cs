using UnityEngine;
using System.Collections;
using TMPro;

public class UIEffectsController : MonoBehaviour
{
    [Header("Animation Settings")]
    public TextMeshProUGUI textMeshPro;
    public Material targetMaterial;
    public float fadeDuration = 1.0f;
    public float slideDuration = 1.0f;
    
    [Header("Entry & Exit Animations")]
    public EntryAnimationType entryAnimation;
    public ExitAnimationType exitAnimation;

    private static readonly int Opacity = Shader.PropertyToID("_opacity");
    private static readonly int SlideOffsetX = Shader.PropertyToID("_SlideX");
    private static readonly int SlideOffsetY = Shader.PropertyToID("_SlideY");

    private Coroutine animationCoroutine;
    private Coroutine textCoroutine;

    public enum EntryAnimationType { SlideUpIn, SlideDownIn, SlideLeftIn, SlideRightIn, FadeIn }
    public enum ExitAnimationType { SlideUpOut, SlideDownOut, SlideLeftOut, SlideRightOut, FadeOut }

    private void Start()
    {
        PlayEntryAnimation();
    }

    // ----------------------
    // UI Visibility Control
    // ----------------------
    public void UIEnable()
    {
        gameObject.SetActive(true);
        PlayEntryAnimation();
    }

    public void UIDisable()
    {
        StartCoroutine(DisableAfterAnimation());
    }

    private IEnumerator DisableAfterAnimation()
    {
        PlayExitAnimation();
        yield return new WaitForSeconds(fadeDuration);
        gameObject.SetActive(false);
    }

    // ----------------------
    // Entry & Exit Animations
    // ----------------------
    private void PlayEntryAnimation()
    {
        switch (entryAnimation)
        {
            case EntryAnimationType.SlideUpIn: SlideYMaterial(-1f, 0f); break;
            case EntryAnimationType.SlideDownIn: SlideYMaterial(1f, 0f); break;
            case EntryAnimationType.SlideLeftIn: SlideXMaterial(-1f, 0f); break;
            case EntryAnimationType.SlideRightIn: SlideXMaterial(1f, 0f); break;
            case EntryAnimationType.FadeIn: FadeMaterialIn(); break;
        }
    }

    private void PlayExitAnimation()
    {
        switch (exitAnimation)
        {
            case ExitAnimationType.SlideUpOut: SlideYMaterial(0f, -1f); break;
            case ExitAnimationType.SlideDownOut: SlideYMaterial(0f, 1f); break;
            case ExitAnimationType.SlideLeftOut: SlideXMaterial(0f, -1f); break;
            case ExitAnimationType.SlideRightOut: SlideXMaterial(0f, 1f); break;
            case ExitAnimationType.FadeOut: FadeMaterialOut(); break;
        }
    }

    // ----------------------
    // Material & Text Animations
    // ----------------------
    public void FadeMaterialIn() => StartMaterialCoroutine(FadeMaterialCoroutine(0f, 1f));
    public void FadeMaterialOut() => StartMaterialCoroutine(FadeMaterialCoroutine(1f, 0f));
    public void TextFadeIn() => StartTextCoroutine(FadeTextCoroutine(0f, 1f));
    public void TextFadeOut() => StartTextCoroutine(FadeTextCoroutine(1f, 0f));
    private void SlideXMaterial(float startX, float targetX) => StartMaterialCoroutine(SlideMaterialCoroutine(SlideOffsetX, startX, targetX));
    private void SlideYMaterial(float startY, float targetY) => StartMaterialCoroutine(SlideMaterialCoroutine(SlideOffsetY, startY, targetY));

    private void StartMaterialCoroutine(IEnumerator coroutine)
    {
        if (animationCoroutine != null) StopCoroutine(animationCoroutine);
        animationCoroutine = StartCoroutine(coroutine);
    }

    private void StartTextCoroutine(IEnumerator coroutine)
    {
        if (textCoroutine != null) StopCoroutine(textCoroutine);
        textCoroutine = StartCoroutine(coroutine);
    }

    // ----------------------
    // Coroutine Animations
    // ----------------------
    private IEnumerator FadeMaterialCoroutine(float startOpacity, float targetOpacity)
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            targetMaterial.SetFloat(Opacity, Mathf.Lerp(startOpacity, targetOpacity, elapsedTime / fadeDuration));
            yield return null;
        }
        targetMaterial.SetFloat(Opacity, targetOpacity);
    }

    private IEnumerator SlideMaterialCoroutine(int propertyID, float startValue, float targetValue)
    {
        float elapsedTime = 0f;
        while (elapsedTime < slideDuration)
        {
            elapsedTime += Time.deltaTime;
            targetMaterial.SetFloat(propertyID, Mathf.Lerp(startValue, targetValue, elapsedTime / slideDuration));
            yield return null;
        }
        targetMaterial.SetFloat(propertyID, targetValue);
    }

    private IEnumerator FadeTextCoroutine(float startAlpha, float targetAlpha)
    {
        float elapsedTime = 0f;
        Color color = textMeshPro.color;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            textMeshPro.color = new Color(color.r, color.g, color.b, Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration));
            yield return null;
        }
        textMeshPro.color = new Color(color.r, color.g, color.b, targetAlpha);
    }
}