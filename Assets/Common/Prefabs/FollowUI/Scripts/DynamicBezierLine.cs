using System.Collections;
using UnityEngine;

public class DynamicBezierLine : MonoBehaviour
{
    [Header("Line Renderer Settings")]
    public LineRenderer lineRenderer;
    public int curveResolution = 30;
    public float maxWidth = 2f;
    public float animationDuration = 2f;
    public AnimationCurve animationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("Bezier Curve Control Points")]
    public GameObject point1;
    public GameObject point2;
    public float bendHeight1 = 0.4f;
    public float bendHeight2 = 0.2f;

    private Coroutine animationCoroutine;

    void Start()
    {
        if (!lineRenderer) lineRenderer = GetComponent<LineRenderer>();
        SetLineWidth(0f);
    }

    private void OnEnable()
    {
        if (point1 != null && point2 != null)
        {
            DrawBezierCurve(point1.transform.position, point2.transform.position);
        }
        EnableLineAnimation();
    }

    void Update()
    {
        if (point1 != null && point2 != null)
        {
            DrawBezierCurve(point1.transform.position, point2.transform.position);
        }
    }

    public void EnableLineAnimation()
    {
        if (animationCoroutine != null) StopCoroutine(animationCoroutine);
        animationCoroutine = StartCoroutine(AnimateLineWidth(0f,1f));
    }

    public void DisableLineAnimation()
    {
        if (animationCoroutine != null) StopCoroutine(animationCoroutine);
        animationCoroutine = StartCoroutine(DisableAfterAnimation());
    }

    private IEnumerator DisableAfterAnimation()
    {
        yield return StartCoroutine(AnimateLineWidth(1f,0f));
        yield return new WaitForSeconds(animationDuration);
        gameObject.SetActive(false);
    }

    private IEnumerator AnimateLineWidth(float startValue, float endValue)
    {
        float elapsedTime = 0f;

        while (elapsedTime < animationDuration)
        {
            float progress = Mathf.Clamp01(elapsedTime / animationDuration);
            float easedProgress = animationCurve.Evaluate(progress); // Apply speed curve
            float widthControl = Mathf.Lerp(startValue, endValue, easedProgress);
            SetLineWidth(widthControl);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        SetLineWidth(endValue);
    }

    private void SetLineWidth(float widthControl)
    {
        if (!lineRenderer) return;

        int pointCount = lineRenderer.positionCount;
        int activeSegments = Mathf.RoundToInt(widthControl * pointCount);

        AnimationCurve widthCurve = new AnimationCurve();
        for (int i = 0; i < pointCount; i++)
        {
            float width = (i < activeSegments) ? maxWidth : 0;
            widthCurve.AddKey((float)i / (pointCount - 1), width);
        }

        lineRenderer.widthCurve = widthCurve;
    }

    private void DrawBezierCurve(Vector3 start, Vector3 end)
    {
        Vector3 control1 = Vector3.Lerp(start, end, 0.25f) + Vector3.up * bendHeight1;
        Vector3 control2 = Vector3.Lerp(start, end, 0.75f) + Vector3.up * bendHeight2;

        lineRenderer.positionCount = curveResolution;
        for (int i = 0; i < curveResolution; i++)
        {
            float t = i / (float)(curveResolution - 1);
            Vector3 point = Mathf.Pow(1 - t, 3) * start +
                            3 * Mathf.Pow(1 - t, 2) * t * control1 +
                            3 * (1 - t) * Mathf.Pow(t, 2) * control2 +
                            Mathf.Pow(t, 3) * end;

            lineRenderer.SetPosition(i, point);
        }
    }
}
