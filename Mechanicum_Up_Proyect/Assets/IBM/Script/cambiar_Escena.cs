using UnityEngine;
using UnityEngine.SceneManagement;

public class cambiar_Escena : MonoBehaviour
{

    public void CambiarEscena(string nombre)
    {
        SceneManager.LoadScene(nombre);
    }
}
