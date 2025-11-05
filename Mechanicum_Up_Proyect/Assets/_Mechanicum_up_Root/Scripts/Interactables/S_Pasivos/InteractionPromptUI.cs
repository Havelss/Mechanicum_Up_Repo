using UnityEngine;
using TMPro;

public class InteractionPromptUI : MonoBehaviour
{
    [SerializeField] private GameObject uiPanel;
    [SerializeField] private TextMeshProUGUI promptText;
    [SerializeField] private Camera playerCamera;

    public bool IsDisplayed { get; private set; }

    private void Start()
    {
        if (playerCamera == null)
            playerCamera = Camera.main;

        uiPanel.SetActive(false);
        IsDisplayed = false;
    }

    private void LateUpdate()
    {
        // Si el prompt está visible, hacer que mire a la cámara sin invertirse
        if (IsDisplayed && playerCamera != null)
        {
            Vector3 lookDirection = transform.position - playerCamera.transform.position;
            lookDirection.y = 0f; // Evita inclinaciones raras
            transform.rotation = Quaternion.LookRotation(lookDirection);
        }
    }

    public void SetUp(string promptMessage)
    {
        promptText.text = promptMessage;
        uiPanel.SetActive(true);
        IsDisplayed = true;
    }

    public void Close()
    {
        uiPanel.SetActive(false);
        IsDisplayed = false;
    }
}
