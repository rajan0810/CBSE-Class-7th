using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class UIEffectsController : MonoBehaviour
{
    #region Public Fields

    [Header("Animation Settings")]
    public TextMeshProUGUI textMeshPro;
    public Material targetMaterial;
    public float fadeDuration = 1.0f;
    public float slideDuration = 1.0f;

    [Header("Entry & Exit Animations")]
    public EntryAnimationType entryAnimation;
    public ExitAnimationType exitAnimation;

    #endregion

    #region Private Fields

    private static readonly int Opacity = Shader.PropertyToID("_opacity");
    private static readonly int SlideOffsetX = Shader.PropertyToID("_SlideX");
    private static readonly int SlideOffsetY = Shader.PropertyToID("_SlideY");

    private Coroutine animationCoroutine;
    private Coroutine textCoroutine;

    #endregion

    #region Enums

    public enum EntryAnimationType
    {
        SlideUpIn,
        SlideDownIn,
        SlideLeftIn,
        SlideRightIn,
        FadeIn
    }

    public enum ExitAnimationType
    {
        SlideUpOut,
        SlideDownOut,
        SlideLeftOut,
        SlideRightOut,
        FadeOut
    }

    #endregion

    #region MonoBehaviour Methods

    private void OnEnable()
    {
        PlayEntryAnimation();
    }

    #endregion

    #region UI Visibility Control

    public void DisableUIWithAnimation()
    {
        if (gameObject.activeSelf)
        {
            StartCoroutine(DisableAfterAnimation());
        }
    }

    private IEnumerator DisableAfterAnimation()
    {
        PlayExitAnimation();
        yield return new WaitForSeconds(fadeDuration);
        gameObject.SetActive(false);
    }

    #endregion

    #region Entry & Exit Animations

    private void PlayEntryAnimation()
    {
        switch (entryAnimation)
        {
            case EntryAnimationType.SlideUpIn:
                SlideYMaterial(-1f, 0f);
                break;
            case EntryAnimationType.SlideDownIn:
                SlideYMaterial(1f, 0f);
                break;
            case EntryAnimationType.SlideLeftIn:
                SlideXMaterial(-1f, 0f);
                break;
            case EntryAnimationType.SlideRightIn:
                SlideXMaterial(1f, 0f);
                break;
            case EntryAnimationType.FadeIn:
                FadeMaterialIn();
                break;
        }
        TextFadeIn();
    }

    private void PlayExitAnimation()
    {
        switch (exitAnimation)
        {
            case ExitAnimationType.SlideUpOut:
                SlideYMaterial(0f, -1f);
                break;
            case ExitAnimationType.SlideDownOut:
                SlideYMaterial(0f, 1f);
                break;
            case ExitAnimationType.SlideLeftOut:
                SlideXMaterial(0f, -1f);
                break;
            case ExitAnimationType.SlideRightOut:
                SlideXMaterial(0f, 1f);
                break;
            case ExitAnimationType.FadeOut:
                FadeMaterialOut();
                break;
        }
    }

    #endregion

    #region Material & Text Animations

    private void FadeMaterialIn() => 
        StartMaterialCoroutine(FadeMaterialCoroutine(0f, 1f));

    private void FadeMaterialOut() => 
        StartMaterialCoroutine(FadeMaterialCoroutine(1f, 0f));

    private void TextFadeIn() => 
        StartCoroutine(FadeTextCoroutine(0f, 1f));

    private void SlideXMaterial(float startX, float targetX) => 
        StartMaterialCoroutine(SlideMaterialCoroutine(SlideOffsetX, startX, targetX));

    private void SlideYMaterial(float startY, float targetY) => 
        StartMaterialCoroutine(SlideMaterialCoroutine(SlideOffsetY, startY, targetY));

    private void StartMaterialCoroutine(IEnumerator coroutine)
    {
        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
        }
        animationCoroutine = StartCoroutine(coroutine);
    }

    #endregion

    #region Coroutine Animations

    private IEnumerator FadeMaterialCoroutine(float startOpacity, float targetOpacity)
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float newOpacity = Mathf.Lerp(startOpacity, targetOpacity, elapsedTime / fadeDuration);
            targetMaterial.SetFloat(Opacity, newOpacity);
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
            float newValue = Mathf.Lerp(startValue, targetValue, elapsedTime / slideDuration);
            targetMaterial.SetFloat(propertyID, newValue);
            yield return null;
        }

        targetMaterial.SetFloat(propertyID, targetValue);
    }

    private IEnumerator FadeTextCoroutine(float startAlpha, float targetAlpha)
    {
        float elapsedTime = 0f;
        Color startColor = textMeshPro.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);
            textMeshPro.color = new Color(startColor.r, startColor.g, startColor.b, newAlpha);
            yield return null;
        }

        textMeshPro.color = new Color(startColor.r, startColor.g, startColor.b, targetAlpha);
    }

    #endregion
}