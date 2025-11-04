using UnityEngine;
using System.Collections;

public class A_ElevatorCaller : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private ElevatorCall targetCall; // Referencia a la tubería (ElevatorCall)
    [SerializeField] private float callInterval = 8f; // Cada cuántos segundos llama
    [SerializeField] private float detectionRange = 15f; // A qué distancia reacciona al jugador
    [SerializeField] private LayerMask playerLayer; // Capa del jugador (opcional)
    [SerializeField] private bool destroyOnSeen = true; // Si desaparece al ser visto

    private bool isCalling = true;
    private Transform player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (targetCall == null)
        {
            Debug.LogWarning($"{name} no tiene asignada una ElevatorCall para llamar.");
            isCalling = false;
            return;
        }

        StartCoroutine(CallRoutine());
    }

    private IEnumerator CallRoutine()
    {
        while (isCalling)
        {
            if (IsPlayerSeeingMe())
            {
                StopCalling();
                yield break;
            }

            // Simula que el mob usa la tubería
            Debug.Log($"{name} llama al ascensor con {targetCall.name}");
            ForceCallElevator();

            yield return new WaitForSeconds(callInterval);
        }
    }

    private void ForceCallElevator()
    {
        if (targetCall == null) return;

        var controlledObject = targetCall.GetComponent<ElevatorCall>()?.GetControlledObject();
        if (controlledObject is Elevator elevator)
        {
            if (targetCall.callUp)
                elevator.MoveUp();
            else
                elevator.MoveDown();
        }
        else
        {
            Debug.LogWarning($"{name}: la ElevatorCall no tiene un Elevator válido asignado.");
        }
    }

    private bool IsPlayerSeeingMe()
    {
        if (player == null) return false;

        Vector3 dirToPlayer = player.position - transform.position;
        float dist = dirToPlayer.magnitude;
        if (dist > detectionRange) return false;

        // Comprobar línea de visión directa con un raycast
        if (Physics.Raycast(transform.position + Vector3.up, dirToPlayer.normalized, out RaycastHit hit, detectionRange, ~0))
        {
            if (hit.collider.CompareTag("Player"))
                return true;
        }

        return false;
    }

    private void StopCalling()
    {
        isCalling = false;

        if (destroyOnSeen)
        {
            Debug.Log($"{name} ha sido visto y desaparece.");
            Destroy(gameObject);
        }
        else
        {
            Debug.Log($"{name} deja de llamar y huye.");
            // Aquí podrías poner animación o IA de escape
        }
    }
}
