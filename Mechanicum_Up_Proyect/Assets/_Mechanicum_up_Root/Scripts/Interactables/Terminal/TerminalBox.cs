using UnityEngine;




/*
public class TerminalBox : MonoBehaviour
{
    [SerializeField] private SO_Terminal linkedTerminal;   // Referencia a la terminal específica
    [SerializeField] private MonoBehaviour controlledObject; // Objeto controlado (ej: ascensor)

    public string GetPrompt() => "E";

    public void Interact()
    {
        if (linkedTerminal == null)
        {
            Debug.LogWarning($"{name} no tiene asignada una terminal.");
            return;
        }

        // Asigna el objeto que controla y abre la terminal correspondiente
        linkedTerminal.OpenTerminal(controlledObject);
    }

    // Este método interno lo usa el Interactor para pasar el objeto controlado sin necesitar una interfaz global
    private void SetControlledObjectInternal(MonoBehaviour obj)
    {
        if (linkedTerminal != null)
            linkedTerminal.SendMessage("AssignControlledObject", obj, SendMessageOptions.DontRequireReceiver);
    }
} */


public class TerminalBox : MonoBehaviour
{
    [SerializeField] private SO_Terminal linkedTerminal;   // Terminal específica
    [SerializeField] private MonoBehaviour controlledObject; // Objeto controlado (ej: ascensor)

    public string GetPrompt() => "E";

    public void Interact()
    {
        if (linkedTerminal == null)
        {
            Debug.LogWarning($"{name} no tiene asignada una terminal.");
            return;
        }

        // Solo abrir la terminal si tiene electricidad
        if (linkedTerminal.IsElectrified())
        {
            linkedTerminal.OpenTerminal(controlledObject);
        }
        else
        {
            Debug.LogWarning($"{name}: la terminal está apagada, necesitas darle electricidad primero.");
        }
    }
}

