using UnityEngine;

public class TutorialInteractionTrigger : MonoBehaviour
{
    [Header("UI a Controlar")]
    public GameObject uiElement;

    [Header("Configuración")]
    public bool destroyAfterUse = true;

    private bool isPlayerInside = false;

    private void Start()
    {
        if (uiElement != null) uiElement.SetActive(false);
    }

    private void Update()
    {
        // IMPORTANTE: Solo chequeamos la tecla si el jugador está dentro
        if (isPlayerInside)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("¡Tecla E detectada correctamente!");
                HandleInteraction();
            }
        }
    }

    private void HandleInteraction()
    {
        if (uiElement != null) uiElement.SetActive(false);

        if (destroyAfterUse)
        {
            Debug.Log("Destruyendo Trigger de Tutorial.");
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Detectar si es el player o si es la vagoneta donde va el player
        bool isPlayer = other.CompareTag("Player") || other.GetComponentInParent<PlayerController>() != null;
        bool isVagoneta = other.CompareTag("Vagoneta") || other.GetComponentInParent<MinecartController>() != null;

        if (isPlayer || isVagoneta)
        {
            Debug.Log("Entidad entró en el Trigger del Tutorial");
            isPlayerInside = true;
            if (uiElement != null) uiElement.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Al salir, desactivamos la posibilidad de pulsar E
        isPlayerInside = false;
        if (uiElement != null) uiElement.SetActive(false);
    }
}