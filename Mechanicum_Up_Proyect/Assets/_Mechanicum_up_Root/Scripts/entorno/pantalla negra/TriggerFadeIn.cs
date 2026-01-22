using UnityEngine;

public class TriggerFadeIn : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(ScreenFader.Instance.FadeIn());
        }
    }
}
