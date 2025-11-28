using UnityEngine;

public class TriggerFadeOut : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(ScreenFader.Instance.FadeOut());
        }
    }
}
