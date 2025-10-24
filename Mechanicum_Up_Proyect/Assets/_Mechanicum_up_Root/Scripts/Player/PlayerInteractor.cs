using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    [Header("Respawn Configuration")]
    [SerializeField] Transform respawnPoint; //Posicion del respawn
    [SerializeField] float respawnFallLimit; // Limite en -y que de ser alcanzado, respawnea
    Rigidbody playerRB;
   

    private void Awake()
    {
        playerRB = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (transform.position.y <= respawnFallLimit) Respawn();  
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Respawn();
        }
    }

    void Respawn()
    {
        playerRB.linearVelocity = new Vector3(0,0,0);  //reinicia la velocidad del player
        transform.position = respawnPoint.position;
    }
}
