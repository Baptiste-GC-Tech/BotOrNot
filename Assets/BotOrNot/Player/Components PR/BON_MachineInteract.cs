using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BON_MachineInteract : MonoBehaviour
{
    /*
     *  FIELDS
     */

    InputAction InteractMachineAction; //interact pour prendre le controle
    InputAction QuitMachineAction; //rappuyer pour quitter le controle
    InputAction MoveMachineAction; //bouger quand la machine est controlable

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
        if(QuitMachineAction.WasPressedThisFrame())
        {
            print("stop");
            player.RecoverControl();
        }
        if (InteractMachineAction.WasPressedThisFrame())
        {
            print("stopoui");
            player.RecoverControl();
        }
        print(player.GetComponent<PlayerInput>().currentActionMap);
    }

    /*
     *  UNITY METHODS
     */

    // Start is called before the first frame update
    void Start()
    {
        InteractMachineAction = InputSystem.actions.FindAction("ActionsMapPR/Interact");
        QuitMachineAction = InputSystem.actions.FindAction("MachineControl/Interact");
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
