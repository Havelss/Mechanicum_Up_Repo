using UnityEngine;

public class OjoSeguirPlayer : MonoBehaviour
{
    public string playerTag = "Player";
    public float velocidadRotacion = 5f;

    private Transform player;

    void Update()
    {
        // Si aún no tenemos referencia, intentamos encontrarla
        if (player == null)
        {
            GameObject obj = GameObject.FindGameObjectWithTag(playerTag);
            if (obj != null)
                player = obj.transform;

            return;
        }

        Vector3 direccion = player.position - transform.position;
        Quaternion rotacionObjetivo = Quaternion.LookRotation(direccion);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            rotacionObjetivo,
            velocidadRotacion * Time.deltaTime
        );
    }
}
