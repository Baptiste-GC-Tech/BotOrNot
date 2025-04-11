using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BON_MachineControllerPR : MonoBehaviour
{
    /*
     *  FIELDS
     */
    //Input related
    InputAction _TakeControlOfMachineAction;    // Upon machine interaction, represents taking control of it and losing control of PR
    InputAction _QuitControlOfMachineAction;    // Upon machine interaction, represents forfeiting control of it and gaining back control of PR
    InputAction _JoystickMachineAction;         // When controling a machine, sends the input over to it so it can do stuff
    Vector2 _moveMachineValue;

    // Player & State related
    [SerializeField] private BON_CCPlayer _player;

    // TODO: Wait for BON_Controllable to be implemented and then use it here so we can pass the input to whatever is being controlled.
    //private GameObject _machine = null; //ou classe de la machine directement <-- Y E S
    //public GameObject Machine
    //{
    //    get { return Machine; }
    //    set { _machine = value; }
    //}

    BON_Actionnable _MachinePossesed;


    /*
     *  CLASS METHODS
     */

    public void MoveMachine(/*class machine*/)
    {
        // Reads input values
        _moveMachineValue = _JoystickMachineAction.ReadValue<Vector2>();

        if (_moveMachineValue.x > 0)
        {
            /* machine.GoRight()*/
        }
        else if (_moveMachineValue.x < 0)
        {
            /* machine.GoLeft()*/
        }
        if (_moveMachineValue.y > 0)
        {
            /* machine.GoUp()*/
        }
        else if (_moveMachineValue.y < 0)
        {
            /* machine.GoDown()*/
        }


        if(_QuitControlOfMachineAction.WasPressedThisFrame())
        {
            if (BON_GameManager.Instance().IsSwitching)
            {
                print("control switch to " + GetComponent<PlayerInput>().currentActionMap);
                _player.AvatarState.IsConstrollingMachine = false;
                StartCoroutine(BON_GameManager.Instance().CooldownSwitchControl());
                BON_GameManager.Instance().RecoverControl();
            }
        }
    }

    /*
     *  UNITY METHODS
     */
    void Start()
    {
        _TakeControlOfMachineAction = InputSystem.actions.FindAction("ActionsMapPR/Interact"); //take control machine
        _QuitControlOfMachineAction = InputSystem.actions.FindAction("MachineControl/Interact"); //recover control
        _JoystickMachineAction = InputSystem.actions.FindAction("MachineControl/Move"); //control machine
    }

    void Update()
    { 
        // Control management (gaining control of the machine or taking back control of PR)
        if (_TakeControlOfMachineAction.WasReleasedThisFrame()) //interact
        {
            if (_player.AvatarState.IsNearIOMInteractible && BON_GameManager.Instance().IsSwitching) //machine pas loin et pas en cours d'activation
            {
                _player.AvatarState.IsConstrollingMachine = true;
                StartCoroutine(BON_GameManager.Instance().CooldownSwitchControl());
                BON_GameManager.Instance().GiveControl();
            }
        }
        if (_player.AvatarState.IsConstrollingMachine)
        {
            MoveMachine();
        }

        if (_TakeControlOfMachineAction == null)
            Debug.LogError("_TakeControlOfMachineAction introuvable");
        if (_QuitControlOfMachineAction == null)
            Debug.LogError("_QuitControlOfMachineAction introuvable");
        if (_JoystickMachineAction == null)
            Debug.LogError("_JoystickMachineAction introuvable");
    }
}
