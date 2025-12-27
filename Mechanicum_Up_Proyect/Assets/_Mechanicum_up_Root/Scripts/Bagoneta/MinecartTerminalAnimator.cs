using UnityEngine;

public class MinecartTerminalAnimator : MonoBehaviour
{
    [Header("Panel de terminal")]
    public RectTransform terminalPanel;
    public CanvasGroup canvasGroup;

    [Header("Animación")]
    public float animationTime = 0.5f;
    public float hiddenY = -500f;
    public float shownY = 0f;

    [Header("Inspector Toggle")]
    public bool isVisible = false;

    private float animTimer = 0f;

    private void Awake()
    {
        if (terminalPanel != null)
        {
            Vector2 pos = terminalPanel.anchoredPosition;
            pos.y = hiddenY;
            terminalPanel.anchoredPosition = pos;
        }

        if (canvasGroup != null)
            canvasGroup.alpha = 0f;
    }

    private void Update()
    {
        if (terminalPanel == null || canvasGroup == null) return;

        float dir = isVisible ? 1f : -1f;
        animTimer += dir * Time.deltaTime;
        animTimer = Mathf.Clamp(animTimer, 0f, animationTime);

        float t = Mathf.SmoothStep(0f, 1f, animTimer / animationTime);

        Vector2 pos = terminalPanel.anchoredPosition;
        pos.y = Mathf.Lerp(hiddenY, shownY, t);
        terminalPanel.anchoredPosition = pos;

        canvasGroup.alpha = t;
    }

    public void ShowTerminal() => isVisible = true;
    public void HideTerminal() => isVisible = false;
}

