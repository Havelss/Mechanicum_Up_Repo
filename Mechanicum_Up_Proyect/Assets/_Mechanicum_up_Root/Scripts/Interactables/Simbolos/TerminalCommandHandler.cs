using UnityEngine;

public class TerminalCommandHandler : MonoBehaviour
{
    public static TerminalCommandHandler Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void ProcessCommand(string command)
    {
        command = command.Trim().ToLower();
        Debug.Log($"Comando recibido: {command}");

        switch (command)
        {
            case "up":
                Elevator.Instance.MoveUp();
                break;

            case "no,up":
            case "up,no":
                Elevator.Instance.MoveDown();
                break;

            default:
                Debug.LogWarning($"Comando desconocido: {command}");
                break;
        }
    }
}


