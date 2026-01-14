using UnityEngine;

public class Autodestruir : MonoBehaviour
{
    public float duracion = 1.5f;

    void Start()
    {
        Destroy(gameObject, duracion);
    }
}
