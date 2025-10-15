using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour
{
    [SerializeField] private Transform interactionPoint;
    [SerializeField] private float interactionPointRadius = 0.5f;
    [SerializeField] private LayerMask interactable;
    [SerializeField] private InteractionPromptUI _interactionPrompUI;

    private readonly Collider[] colliders = new Collider[3];
    [SerializeField] private int numFound;

    private IInteractable _interactable;

    void Update()
    {
        
        numFound = Physics.OverlapSphereNonAlloc(interactionPoint.position, interactionPointRadius, colliders, interactable);

        if (numFound > 0)
        {
            _interactable = colliders[0].GetComponent<IInteractable>();

            if (_interactable != null)
            {
                if (!_interactionPrompUI.IsDisplayed) _interactionPrompUI.SetUp(_interactable.InteractionPrompt);

                if (Keyboard.current.eKey.wasPressedThisFrame) _interactable.Interact(this);
            }
           
        }
        else
        {
            if (_interactable != null) _interactable = null;
            if(_interactionPrompUI.IsDisplayed) _interactionPrompUI.Close();
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(interactionPoint.position, interactionPointRadius);
    }
}
