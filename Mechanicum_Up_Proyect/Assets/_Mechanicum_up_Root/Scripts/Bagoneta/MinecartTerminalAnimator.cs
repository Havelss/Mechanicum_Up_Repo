using UnityEngine;

public class MinecartTerminalAnimator : MonoBehaviour
{
    [Header("Panel de terminal")]
    public RectTransform terminalPanel;  // El panel que se mueve
    public CanvasGroup canvasGroup;      // Para controlar alpha
    public float animationTime = 0.5f;   // Duración de la animación
    public float hiddenY = -500f;        // Posición fuera de pantalla
    public float shownY = 0f;            // Posición visible en pantalla

    private float animTimer = 0f;
    private bool isVisible = false;

    private void Awake()
    {
        // Forzar posición inicial y alpha
        Vector2 pos = terminalPanel.anchoredPosition;
        pos.y = hiddenY;
        terminalPanel.anchoredPosition = pos;
        if (canvasGroup != null)
            canvasGroup.alpha = 0f;
    }

    private void Update()
    {
        // Animación de subida
        if (isVisible && animTimer < animationTime)
        {
            animTimer += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, animTimer / animationTime);

            Vector2 pos = terminalPanel.anchoredPosition;
            pos.y = Mathf.Lerp(hiddenY, shownY, t);
            terminalPanel.anchoredPosition = pos;

            if (canvasGroup != null)
                canvasGroup.alpha = t;
        }
        // Animación de bajada
        else if (!isVisible && animTimer > 0f)
        {
            animTimer -= Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, animTimer / animationTime);

            Vector2 pos = terminalPanel.anchoredPosition;
            pos.y = Mathf.Lerp(hiddenY, shownY, t);
            terminalPanel.anchoredPosition = pos;

            if (canvasGroup != null)
                canvasGroup.alpha = t;
        }
    }

    // ---------- API ----------
    public void ShowTerminal()
    {
        gameObject.SetActive(true);
        isVisible = true;
        animTimer = 0f; // reset animación
    }

    public void HideTerminal()
    {
        isVisible = false;
        animTimer = animationTime; // empezar desde visible
    }
}
