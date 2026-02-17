using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class CambiarEscenaPorTiempo : MonoBehaviour
{
    [SerializeField] float tiempoEspera = 5f;
    [SerializeField] string nombreEscena;

    bool escenaCambiada = false;

    void Start()
    {
        StartCoroutine(CambiarEscena());
    }

    void Update()
    {
        // Detecta cualquier tecla o botón
        if (!escenaCambiada && Input.anyKeyDown)
        {
            CambiarEscenaInstantaneo();
        }
    }

    IEnumerator CambiarEscena()
    {
        yield return new WaitForSeconds(tiempoEspera);
        CambiarEscenaInstantaneo();
    }

    void CambiarEscenaInstantaneo()
    {
        if (escenaCambiada) return;

        escenaCambiada = true;
        SceneManager.LoadScene(nombreEscena);
    }
}
