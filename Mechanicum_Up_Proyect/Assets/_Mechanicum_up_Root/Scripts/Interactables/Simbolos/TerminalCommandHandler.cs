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

        bool validCommand = false;

        switch (command)
        {
            case "up":
                Elevator.Instance.MoveUp();
                validCommand = true;
                break;

            case "no,up":
                Elevator.Instance.MoveDown();
                validCommand = true;
                break;

            default:
                Debug.LogWarning($"Comando desconocido: {command}");
                break;
        }

        // Si el comando fue válido, cierra la terminal automáticamente
        if (validCommand)
        {
            SO_Terminal terminal = FindFirstObjectByType<SO_Terminal>();
            if (terminal != null)
            {
                Debug.Log("Comando correcto — cerrando terminal automáticamente.");
                terminal.CloseTerminal();
            }
        }
    }
}
