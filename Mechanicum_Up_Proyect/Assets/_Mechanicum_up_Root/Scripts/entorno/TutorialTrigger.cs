using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    public string playerLayerName = "Player"; // Layer del player
    public GameObject imageCanvas; // El canvas o imagen que se mostrará

    private void OnTriggerEnter(Collider other)
    {
        // Si el player entra en el trigger
        if (other.gameObject.layer == LayerMask.NameToLayer(playerLayerName))
        {
            if (imageCanvas != null)
                imageCanvas.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Si el player sale del trigger
        if (other.gameObject.layer == LayerMask.NameToLayer(playerLayerName))
        {
            if (imageCanvas != null)
                imageCanvas.SetActive(false);
        }
    }
}
