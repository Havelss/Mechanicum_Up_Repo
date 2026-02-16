using UnityEngine;

public class TutorialInteractionTrigger : MonoBehaviour, IInteractable
{
    [Header("Tutorial completo a mostrar")]
    [SerializeField] private GameObject tutorialUI; // privado, accesible via propiedad

    [Header("Indicador de interacción (Tecla E)")]
    [SerializeField] private GameObject teclaEIndicator;

    [Header("Mensaje del Interactor")]
    [SerializeField] private string promptMessage = "E";

    [Header("Opciones")]
    [SerializeField] private bool destroyAfterUse = true;
    [SerializeField] private float cooldownTime = 0.5f;

    private bool hasBeenUsed = false;       // Tutorial ya activado con E
    private bool isPlayerInside = false;
    private bool cooldownFinished = false;
    private float timer = 0f;
    private bool tutorialAlreadyShown = false; // Tutorial mostrado automáticamente al entrar

    // Propiedad pública para TutorialManager
    public GameObject TutorialUI => tutorialUI;

    public string InteractionPrompt => promptMessage;

    private void Start()
    {
        if (tutorialUI != null)
            tutorialUI.SetActive(false);

        if (teclaEIndicator != null)
            teclaEIndicator.SetActive(false);
    }

    private void Update()
    {
        if (!isPlayerInside || hasBeenUsed)
            return;

        // Contador de cooldown
        if (!cooldownFinished)
        {
            timer += Time.deltaTime;
            if (timer >= cooldownTime)
                cooldownFinished = true;

            return;
        }

        // Detectar movimiento del jugador (solo para ocultar la E)
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        if ((Mathf.Abs(moveX) > 0.1f || Mathf.Abs(moveZ) > 0.1f) && !hasBeenUsed)
        {
            HideEIndicator();
        }

        // Detecta que pulsa E
        if (Input.GetKeyDown(KeyCode.E))
        {
            ShowTutorial();
        }
    }

    private void ShowTutorial()
    {
        if (hasBeenUsed || !cooldownFinished)
            return;

        hasBeenUsed = true;

        // Oculta la E
        HideEIndicator();

        // Oculta otros tutoriales
        if (TutorialManager.instance != null)
            TutorialManager.instance.HideAllTutorials();

        // Muestra el tutorial completo
        if (tutorialUI != null)
            tutorialUI.SetActive(true);

        Debug.Log("Tutorial mostrado");

        if (destroyAfterUse)
            Destroy(gameObject);
    }

    private void ShowTutorialOnceOnEnter()
    {
        if (tutorialAlreadyShown)
            return;

        tutorialAlreadyShown = true;

        if (TutorialManager.instance != null)
            TutorialManager.instance.HideAllTutorials();

        if (tutorialUI != null)
            tutorialUI.SetActive(true);
    }

    private void HideEIndicator()
    {
        if (teclaEIndicator != null)
            teclaEIndicator.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        bool isPlayer = other.CompareTag("Player") ||
                        other.GetComponentInParent<PlayerController>() != null;

        if (isPlayer && !hasBeenUsed)
        {
            isPlayerInside = true;
            timer = 0f;
            cooldownFinished = false;

            // Aparece la tecla E
            if (teclaEIndicator != null)
                teclaEIndicator.SetActive(true);

            // Muestra el tutorial automáticamente solo la primera vez
            ShowTutorialOnceOnEnter();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        bool isPlayer = other.CompareTag("Player") ||
                        other.GetComponentInParent<PlayerController>() != null;

        if (isPlayer)
        {
            isPlayerInside = false;
            HideEIndicator();
        }
    }

    // Permite que Interactor también active el tutorial
    public void Interact(Interactor interactor)
    {
        ShowTutorial();
    }
}
