using UnityEngine;

public class PuertaUp : MonoBehaviour
{
    [Header("Movimiento")]
    public float alturaSubida = 3f;   // Altura que sube la puerta
    public float velocidad = 2f;

    [Header("Detección")]
    public string playerTag = "Player";
    public LayerMask playerLayer;

    private Vector3 posicionInicial;
    private Vector3 posicionFinal;
    private bool subir = false;

    void Start()
    {
        posicionInicial = transform.position;
        posicionFinal = posicionInicial + Vector3.up * alturaSubida;
    }

    void Update()
    {
        Vector3 objetivo = subir ? posicionFinal : posicionInicial;

        transform.position = Vector3.MoveTowards(
            transform.position,
            objetivo,
            velocidad * Time.deltaTime
        );
    }

    private void OnTriggerEnter(Collider other)
    {
        if (EsPlayer(other))
        {
            subir = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (EsPlayer(other))
        {
            subir = false;
        }
    }

    private bool EsPlayer(Collider other)
    {
        // Por TAG
        if (other.CompareTag(playerTag))
            return true;

        // Por LAYER
        if (((1 << other.gameObject.layer) & playerLayer) != 0)
            return true;

        return false;
    }
}
