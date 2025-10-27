using UnityEngine;
using TMPro;

public class InteractionPromptUI : MonoBehaviour
{
    [SerializeField] private GameObject uiPanel;
    [SerializeField] private TextMeshProUGUI promptText;

    public bool IsDisplayed { get; private set; }

    private void Start()
    {
        uiPanel.SetActive(false);
        IsDisplayed = false;
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
