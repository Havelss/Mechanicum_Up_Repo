using UnityEngine;
using Cinemachine; // obligatorio si usas CinemachineVirtualCamera


public class PlayerGetItem : MonoBehaviour
{
    [Header("Visual")]
    public Transform itemPoint;
    public Animator animator;

    [Header("Cámara")]
    public CinemachineVirtualCamera pickupCam;
    public float cameraBlendTime = 0.5f;

    [Header("Duración")]
    public float duration = 2f; // tiempo que dura el momento

    private GameObject currentItem;
    private bool gettingItem;

    public bool IsGettingItem => gettingItem;

    public void StartGetItem(GameObject itemPrefab)
    {
        if (gettingItem) return;
        gettingItem = true;

        // 1️⃣ Animación del player (opcional)
        if (animator != null)
            animator.SetTrigger("getItem");

        // 2️⃣ Instanciar el objeto sobre la cabeza
        if (itemPrefab != null && itemPoint != null)
        {
            currentItem = Instantiate(itemPrefab, itemPoint.position, Quaternion.identity);
            currentItem.transform.SetParent(itemPoint);
        }

        // 3️⃣ Activar cámara especial
        if (pickupCam != null)
        {
            pickupCam.Priority = 20; // aumenta prioridad para que Cinemachine la active
        }

        // 4️⃣ Rotación y efecto del objeto
        if (currentItem != null)
            StartCoroutine(RotateObject(currentItem.transform));

        // 5️⃣ Terminar secuencia después de 'duration'
        Invoke(nameof(EndGetItem), duration);
    }

    private System.Collections.IEnumerator RotateObject(Transform obj)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            obj.Rotate(Vector3.up * 90f * Time.deltaTime, Space.World); // rotación alrededor del eje Y
            elapsed += Time.deltaTime;
            yield return null;
        }
    }

    public void EndGetItem()
    {
        // Reset cámara
        if (pickupCam != null)
            pickupCam.Priority = 0;

        // Destruir objeto visual
        if (currentItem != null)
            Destroy(currentItem);

        // Reset animación (opcional)
        if (animator != null)
            animator.SetTrigger("endGetItem");

        gettingItem = false;
    }
}
