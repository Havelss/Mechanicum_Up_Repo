using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager instance;

    private void Awake()
    {
        instance = this;
    }

    public void HideAllTutorials()
    {
        TutorialInteractionTrigger[] tutorials =
            FindObjectsOfType<TutorialInteractionTrigger>();

        foreach (var t in tutorials)
        {
            if (t.TutorialUI != null) // ✅ ahora usamos la propiedad pública
                t.TutorialUI.SetActive(false);
        }
    }
}
