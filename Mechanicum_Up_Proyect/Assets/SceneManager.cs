using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class CambiarEscenaPorTiempo : MonoBehaviour
{
    [SerializeField] float tiempoEspera = 5f;
    [SerializeField] string nombreEscena;

    void Start()
    {
        StartCoroutine(CambiarEscena());
    }

    IEnumerator CambiarEscena()
    {
        yield return new WaitForSeconds(tiempoEspera);
        SceneManager.LoadScene(nombreEscena);
    }
}
