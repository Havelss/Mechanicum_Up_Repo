using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenFader : MonoBehaviour
{
    public static ScreenFader Instance { get; private set; }

    [Header("Fade UI")]
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 1.5f;

    [Header("Triggers que activan FadeOut")]
    [SerializeField] private Collider[] fadeOutTriggers;

    [Header("Triggers que activan FadeIn")]
    [SerializeField] private Collider[] fadeInTriggers;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Registrar eventos de triggers
        RegisterTriggers();
    }

    private void RegisterTriggers()
    {
        foreach (var trigger in fadeOutTriggers)
        {
            trigger.isTrigger = true;
            TriggerHook hook = trigger.gameObject.AddComponent<TriggerHook>();
            hook.onEnter += () => StartCoroutine(FadeOut());
        }

        foreach (var trigger in fadeInTriggers)
        {
            trigger.isTrigger = true;
            TriggerHook hook = trigger.gameObject.AddComponent<TriggerHook>();
            hook.onEnter += () => StartCoroutine(FadeIn());
        }
    }

    public IEnumerator FadeOut()
    {
        fadeImage.gameObject.SetActive(true);
        Color c = fadeImage.color;
        c.a = 0f;
        fadeImage.color = c;

        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            c.a = Mathf.Clamp01(elapsed / fadeDuration);
            fadeImage.color = c;
            yield return null;
        }

        c.a = 1f;
        fadeImage.color = c;
    }

    public IEnumerator FadeIn()
    {
        fadeImage.gameObject.SetActive(true);
        Color c = fadeImage.color;
        c.a = 1f;
        fadeImage.color = c;

        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            c.a = 1f - Mathf.Clamp01(elapsed / fadeDuration);
            fadeImage.color = c;
            yield return null;
        }

        fadeImage.gameObject.SetActive(false);
    }
}
