using UnityEngine;

public class TerminalCommandHandler : MonoBehaviour
{
    public static TerminalCommandHandler Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void ProcessCommand(string command)
    {
        Debug.Log($"Comando recibido: {command}");

        // Ejemplo: combinaciones posibles
        switch (command)
        {
            case "UP":
                Elevator.Instance.MoveUp();
                break;
            case "NO,UP":
            case "UP,NO":
                Elevator.Instance.MoveDown();
                break;
            default:
                Debug.Log("Comando no reconocido");
                break;
        }
    }
}
