using UnityEngine;

public class MinecartTerminalAnimator : MonoBehaviour
{
    public RectTransform terminalRoot;    // Panel que se moverá
    public float animationTime = 0.5f;    // Velocidad de la animación
    public float hiddenY = -500f;         // Posición fuera de la pantalla
    public float shownY = 0f;             // Posición visible

    private float animTimer = 0f;
    private bool isVisible = false;

    private void Awake()
    {
        // Lo forzamos a empezar escondido
        Vector2 pos = terminalRoot.anchoredPosition;
        pos.y = hiddenY;
        terminalRoot.anchoredPosition = pos;
    }

    private void Update()
    {
        if (isVisible && animTimer < animationTime)
        {
            animTimer += Time.deltaTime;
            float t = animTimer / animationTime;
            t = Mathf.SmoothStep(0, 1, t);

            Vector2 pos = terminalRoot.anchoredPosition;
            pos.y = Mathf.Lerp(hiddenY, shownY, t);
            terminalRoot.anchoredPosition = pos;
        }
        else if (!isVisible && animTimer > 0f)
        {
            animTimer -= Time.deltaTime;
            float t = animTimer / animationTime;
            t = Mathf.SmoothStep(0, 1, t);

            Vector2 pos = terminalRoot.anchoredPosition;
            pos.y = Mathf.Lerp(hiddenY, shownY, t);
            terminalRoot.anchoredPosition = pos;
        }
    }

    // ---------- API ----------
    public void ShowTerminal()
    {
        gameObject.SetActive(true);
        isVisible = true;
        animTimer = 0f;
    }

    public void HideTerminal()
    {
        isVisible = false;
        animTimer = animationTime;
    }
}
