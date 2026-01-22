using UnityEngine;

public class PuertaUp : MonoBehaviour
{
    public Transform puerta;

    [Header("Movimiento")]
    public float alturaSubida = 3f;
    public float velocidad = 2f;

    [Header("Detección")]
    public string playerTag = "Player";

    private Vector3 posicionInicial;
    private Vector3 posicionFinal;
    private bool subir = false;

    void Start()
    {
        posicionInicial = puerta.position;
        posicionFinal = posicionInicial + Vector3.up * alturaSubida;
    }

    void Update()
    {
        if (subir)
        {
            puerta.position = Vector3.MoveTowards(
                puerta.position,
                posicionFinal,
                velocidad * Time.deltaTime
            );
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            subir = true;
        }
    }
}
