using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    public string playerLayerName = "Player";
    public GameObject imageCanvas;

    private bool playerInside = false;

    private void Start()
    {
        if (imageCanvas != null)
            imageCanvas.SetActive(false);
    }

    private void Update()
    {
        if (playerInside && Input.GetKeyDown(KeyCode.E))
        {
            if (imageCanvas != null)
                imageCanvas.SetActive(!imageCanvas.activeSelf);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(playerLayerName))
        {
            playerInside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(playerLayerName))
        {
            playerInside = false;

            if (imageCanvas != null)
                imageCanvas.SetActive(false);
        }
    }
}
