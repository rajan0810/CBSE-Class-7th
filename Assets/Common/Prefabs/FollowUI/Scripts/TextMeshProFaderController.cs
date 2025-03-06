using UnityEngine;
using System.Collections;

public class TextMeshProFadeController : MonoBehaviour
{
    public UIAnimation uianimation;

    // Dropdowns for Entry and Exit Animations
    public EntryAnimationType entryAnimation;
    public ExitAnimationType exitAnimation;

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

    // Public methods to control the UI from other scripts
    public void UIEnable()
    {
        gameObject.SetActive(true);
        PlayEntryAnimation();
    }

    public void UIDisable()
    {
        StartCoroutine(DisableAfterAnimation());
    }

    private void Start()
    {
        if (uianimation != null)
        {
            PlayEntryAnimation();
            StartCoroutine(StartTextFadeIn());
        }
    }

    // Plays the Entry Animation selected from the dropdown
    private void PlayEntryAnimation()
    {
        switch (entryAnimation)
        {
            case EntryAnimationType.SlideUpIn:
                uianimation.SlideUpIn();
                break;
            case EntryAnimationType.SlideDownIn:
                uianimation.SlideDownIn();
                break;
            case EntryAnimationType.SlideLeftIn:
                uianimation.SlideLeftIn();
                break;
            case EntryAnimationType.SlideRightIn:
                uianimation.SlideRightIn();
                break;
            case EntryAnimationType.FadeIn:
                uianimation.TextFadeIn();
                break;
        }
    }

    // Plays the Exit Animation selected from the dropdown
    private void PlayExitAnimation()
    {
        switch (exitAnimation)
        {
            case ExitAnimationType.SlideUpOut:
                uianimation.SlideUpOut();
                break;
            case ExitAnimationType.SlideDownOut:
                uianimation.SlideDownOut();
                break;
            case ExitAnimationType.SlideLeftOut:
                uianimation.SlideLeftOut();
                break;
            case ExitAnimationType.SlideRightOut:
                uianimation.SlideRightOut();
                break;
            case ExitAnimationType.FadeOut:
                uianimation.TextFadeOut();
                break;
        }
    }

    // Disables the GameObject after exit animation is completed
    private IEnumerator DisableAfterAnimation()
    {
        PlayExitAnimation();
        yield return new WaitForSeconds(uianimation.fadeDuration);
        gameObject.SetActive(false);
    }

    private IEnumerator StartTextFadeIn()
    {
        yield return new WaitForSeconds(uianimation.fadeDuration);
        uianimation.TextFadeIn();
    }
}
