using UnityEngine;

/*

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

        
        if (validCommand) //si es correcta la combinacion te saca de la terminal
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

*/



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

    // Recibe la secuencia y la terminal que la ejecuta
    public void ProcessCommand(string command, SymbolTerminalController terminal)
    {
        command = command.Trim().ToLower();
        Debug.Log($"Comando recibido: {command}");

        bool validCommand = false;

        switch (command)
        {
            case "up":
                if (terminal != null && terminal.GetComponentInParent<SO_Terminal>().controlledObject is Elevator e)
                    e.MoveUp();
                validCommand = true;
                break;

            case "no,up":
                if (terminal != null && terminal.GetComponentInParent<SO_Terminal>().controlledObject is Elevator e2)
                    e2.MoveDown();
                validCommand = true;
                break;

            default:
                Debug.LogWarning($"Comando desconocido: {command}");
                break;
        }

        // Si el comando es válido, cerramos la terminal automáticamente
        if (validCommand)
        {
            var parentTerminal = terminal.GetComponentInParent<SO_Terminal>();
            if (parentTerminal != null)
                parentTerminal.CloseTerminal();
        }
    }
}
