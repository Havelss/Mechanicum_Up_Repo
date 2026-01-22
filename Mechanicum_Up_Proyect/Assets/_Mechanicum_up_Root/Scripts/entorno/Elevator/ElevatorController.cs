using UnityEngine;

public class ElevatorController : MonoBehaviour
{
    public AnimationController elevatorAnim;
    public AnimationController callValveAnim;

    private bool isMoving = false;

    public void MoveElevatorUp()
    {
        if (isMoving) return;
        isMoving = true;

        // 🔹 Activa animaciones
        elevatorAnim.PlayAnimation("MoveUp");
        callValveAnim.PlayAnimation("ValveTurn");

        // ... tu lógica de movimiento real ...
    }

    public void MoveElevatorDown()
    {
        if (isMoving) return;
        isMoving = true;

        elevatorAnim.PlayAnimation("MoveDown");
        callValveAnim.PlayAnimation("ValveTurn");
    }

    public void StopElevator()
    {
        isMoving = false;

        // 🔹 Cambiar a Idle cuando termine
        elevatorAnim.PlayAnimation("Idle");
        callValveAnim.PlayAnimation("Idle");
    }
}
