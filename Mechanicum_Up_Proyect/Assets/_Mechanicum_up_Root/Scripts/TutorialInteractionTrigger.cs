using UnityEngine;

public class TutorialInteractionTrigger : MonoBehaviour, IInteractable
{
    [Header("Tutorial completo a mostrar")]
    [SerializeField] private GameObject tutorialUI;

    [Header("Indicador de interacción (Tecla E)")]
    [SerializeField] private GameObject teclaEIndicator;

    [Header("Mensaje del Interactor")]
    [SerializeField] private string promptMessage = "E";

    [Header("Opciones")]
    [SerializeField] private bool destroyAfterUse = true;
    [SerializeField] private float cooldownTime = 0.5f;

    [Header("Comportamiento Extra")]
    [SerializeField] private bool hideTutorialOnMove = true; // NUEVA OPCIÓN

    private bool hasBeenUsed = false;
    private bool isPlayerInside = false;
    private bool cooldownFinished = false;
    private float timer = 0f;
    private bool tutorialAlreadyShown = false;

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

        // Cooldown
        if (!cooldownFinished)
        {
            timer += Time.deltaTime;
            if (timer >= cooldownTime)
                cooldownFinished = true;

            return;
        }

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        bool isMoving = Mathf.Abs(moveX) > 0.1f || Mathf.Abs(moveZ) > 0.1f;

        // Si se mueve
        if (isMoving)
        {
            HideEIndicator();

            // NUEVO: ocultar tutorial si está activado y la opción lo permite
            if (hideTutorialOnMove && tutorialUI != null && tutorialUI.activeSelf)
            {
                tutorialUI.SetActive(false);
            }
        }

        // Pulsar E
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

        HideEIndicator();

        if (TutorialManager.instance != null)
            TutorialManager.instance.HideAllTutorials();

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

            if (teclaEIndicator != null)
                teclaEIndicator.SetActive(true);

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

    public void Interact(Interactor interactor)
    {
        ShowTutorial();
    }
}