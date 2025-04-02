using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BON_MachineInteract : MonoBehaviour
{
    /*
     *  FIELDS
     */

    InputAction InteractMachineAction;
    InputAction MoveMachineAction;

    // player script reference
    [SerializeField] private BON_CCPlayer player;

    Vector2 moveMachineValue;

    /*
     *  CLASS METHODS
     */

    public void MoveMachine(/*class machine*/)
    {
        moveMachineValue = MoveMachineAction.ReadValue<Vector2>();
        if (moveMachineValue.x > 0)
        {
            /* machine.GoRight()*/
        }
        else if (moveMachineValue.x < 0)
        {
            /* machine.GoLeft()*/
        }
        if (moveMachineValue.y > 0)
        {
            /* machine.GoUp()*/
        }
        else if (moveMachineValue.y < 0)
        {
            /* machine.GoDown()*/
        }
    }

    /*
     *  UNITY METHODS
     */

    // Start is called before the first frame update
    void Start()
    {
        InteractMachineAction = InputSystem.actions.FindAction("ActionsMapPR/Interact");
        MoveMachineAction = InputSystem.actions.FindAction("MachineControl/Move");
    }

    // Update is called once per frame
    void Update()
    {
        // Take item action handling
        if (InteractMachineAction.WasPressedThisFrame()) //interact
        {
            if (player.IsMachineInRange) //machine pas loin
            {
                player.SwitchControl();
                MoveMachine();
            }
        }
    }
}
