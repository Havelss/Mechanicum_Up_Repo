using UnityEngine;

public class EmissionOnPlayerTrigger : MonoBehaviour
{
    [Header("Texturas Emission")]
    public Texture emissionA;
    public Texture emissionB;

    [Header("Configuración")]
    public Color emissionColor = Color.white;

    private Renderer[] renderers;

    void Start()
    {
        renderers = GetComponentsInChildren<Renderer>();

        foreach (Renderer rend in renderers)
        {
            foreach (Material mat in rend.materials)
            {
                mat.EnableKeyword("_EMISSION");
            }
        }

        ApplyEmission(emissionA); // Empieza en A
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ApplyEmission(emissionB);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ApplyEmission(emissionA);
        }
    }

    void ApplyEmission(Texture tex)
    {
        foreach (Renderer rend in renderers)
        {
            foreach (Material mat in rend.materials)
            {
                mat.SetTexture("_EmissionMap", tex);
                mat.SetColor("_EmissionColor", emissionColor);
            }
        }
    }
}