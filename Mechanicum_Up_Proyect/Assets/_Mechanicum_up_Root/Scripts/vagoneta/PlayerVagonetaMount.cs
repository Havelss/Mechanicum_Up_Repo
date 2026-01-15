using UnityEngine;

public class PlayerVagonetaMount : MonoBehaviour
{
    public bool IsMounted { get; private set; }

    private GameObject vagonetaCanvas;

    public void Montar(Transform asiento, GameObject canvas)
    {
        if (IsMounted) return;

        transform.SetParent(asiento);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        vagonetaCanvas = canvas;
        if (vagonetaCanvas != null)
            vagonetaCanvas.SetActive(true);

        IsMounted = true;
    }

    public void ForceUnmount()
    {
        if (!IsMounted) return;

        transform.SetParent(null);

        if (vagonetaCanvas != null)
            vagonetaCanvas.SetActive(false);

        IsMounted = false;
    }
}
