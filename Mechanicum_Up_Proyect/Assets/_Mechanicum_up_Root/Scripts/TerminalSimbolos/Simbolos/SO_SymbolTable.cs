using UnityEngine;
using System.Collections;

public class SO_SymbolTable : MonoBehaviour, IInteractable
{
    [SerializeField] private string interactionPrompt = "E";
    [SerializeField] private string symbolID;

    [SerializeField] private Animator animator;
    private string animationName = "Armature|Mano_Cerrar";

    public string InteractionPrompt => interactionPrompt;

    public void Interact(Interactor interactor)
    {
        if (SymbolManager.Instance != null)
        {
            SymbolManager.Instance.UnlockSymbol(symbolID);
            Debug.Log($"Símbolo recogido: {symbolID}");

            if (animator != null)
            {
                StartCoroutine(PlayAnimationCoroutine());
            }
            else
            {
                // Si no hay Animator, simplemente podrías desactivar la interacción visual
                gameObject.SetActive(false);
            }
        }
        else
        {
            Debug.LogWarning("SymbolManager.Instance no está asignado en la escena.");
        }
    }

    private IEnumerator PlayAnimationCoroutine()
    {
        animator.Play(animationName);

        // Buscar duración del clip
        float clipLength = 0f;
        foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == animationName)
            {
                clipLength = clip.length;
                break;
            }
        }

        // Esperar a que termine la animación
        yield return new WaitForSeconds(clipLength);

        // Aquí puedes desactivar algún componente de interacción si quieres
        // Pero el objeto permanece en la escena
        Debug.Log($"{gameObject.name} terminó la animación y sigue en escena.");
    }
}
