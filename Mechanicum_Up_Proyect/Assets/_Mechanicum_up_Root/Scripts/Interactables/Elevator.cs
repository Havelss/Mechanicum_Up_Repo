using UnityEngine;

public class Elevator : MonoBehaviour
{
    public static Elevator Instance;
    [SerializeField] private Transform topPosition;
    [SerializeField] private Transform groundPosition;
    [SerializeField] private float speed = 2f;

    private Transform target;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }
    }

    #region Limites arriba/abajo

    public void MoveUp()
    {
        Debug.Log("Ascensor subiendo...");
        target = topPosition;
    }

    public void MoveDown()
    {
        Debug.Log("Ascensor bajando...");
        target = groundPosition;
    }

    #endregion
}
