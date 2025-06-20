using System.Collections;
using UnityEngine;

public class ShowMap : MonoBehaviour
{
    public float slideDuration = 0.5f;

    public RectTransform panelRect;
    private Vector2 posOriginal;
    private Vector2 posCentro;
    private Coroutine currentSlide;

    void Start()
    {
        posOriginal = panelRect.anchoredPosition;
        RectTransform parentRect = panelRect.parent.GetComponent<RectTransform>();
        posCentro = Vector2.zero;
    }
    public void ShowPanel()
    {
        if (currentSlide != null)
            StopCoroutine(currentSlide);

        currentSlide = StartCoroutine(SlidePanel(panelRect.anchoredPosition, posCentro));
    }
    public void HidePanel()
    {
        if (currentSlide != null)
            StopCoroutine(currentSlide);

        currentSlide = StartCoroutine(SlidePanel(panelRect.anchoredPosition, posOriginal));
    }
    IEnumerator SlidePanel(Vector2 from, Vector2 to)
    {
        float elapsed = 0f;
        while (elapsed < slideDuration)
        {
            panelRect.anchoredPosition = Vector2.Lerp(from, to, elapsed / slideDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        panelRect.anchoredPosition = to;
    }
}
