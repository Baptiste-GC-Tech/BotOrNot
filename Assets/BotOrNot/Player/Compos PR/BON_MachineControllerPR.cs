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
    InputAction _takeControlOfMachineAction;    //E Upon machine interaction, represents taking control of it and losing control of PR
    InputAction _quitControlOfMachineAction;    //F Upon machine interaction, represents forfeiting control of it and gaining back control of PR
    InputAction _joystickMachineAction;         // When controling a machine, sends the input over to it so it can do stuff
    Vector2 _moveMachineValue;

    // Player & State related
    [SerializeField] private BON_CCPlayer _player; 

    private BON_Interactive _machineToActivate;
    public BON_Interactive MachineToActivate
    {
        get { return _machineToActivate; }
        set { _machineToActivate = value; }
    }

    private BON_Controllable _machinePossessed;
    public BON_Controllable MachinePossessed
    {
        get { return _machinePossessed; }
        set { _machinePossessed = value; }
    }


    /*
     *  CLASS METHODS
     */

    public void MoveMachine(BON_Controllable _machine)
    {
        // Reads input values
        _moveMachineValue = _joystickMachineAction.ReadValue<Vector2>();
        _machine.ProcessInput(_moveMachineValue);

        if(_quitControlOfMachineAction.WasReleasedThisFrame())
        {
            if (!BON_GameManager.Instance().IsSwitching)
            {
                _machineToActivate.Activate();
                BON_GameManager.Instance().RecoverControl();
                StartCoroutine(BON_GameManager.Instance().CooldownSwitchControl());
                _player.AvatarState.IsConstrollingMachine = false;
            }
        }
    }

    /*
     *  UNITY METHODS
     */
    void Start()
    {
        _takeControlOfMachineAction = InputSystem.actions.FindAction("ActionsMapPR/Interact"); //take control machine
        _quitControlOfMachineAction = InputSystem.actions.FindAction("MachineControl/Interact"); //recover control
        _joystickMachineAction = InputSystem.actions.FindAction("MachineControl/Move"); //control machine
        _player.AvatarState.IsConstrollingMachine = false ;
    }

    void Update()
    {
        // Control management (gaining control of the machine or taking back control of PR)
        if (_takeControlOfMachineAction.WasPressedThisFrame()) //interact
        {
            if (_player.AvatarState.IsNearIOMInteractible && !BON_GameManager.Instance().IsSwitching) //machine pas loin et pas en cours d'activation
            {                                     
                _machineToActivate = _player.MachineToActivate;
                _machineToActivate.Activate();
                _machinePossessed = (BON_Controllable)_machineToActivate.ActionnablesList[0];
                StartCoroutine(BON_GameManager.Instance().CooldownSwitchControl());
                BON_GameManager.Instance().GiveControl();
                _player.AvatarState.IsConstrollingMachine = true;
            }
        }
        if (_player.AvatarState.IsConstrollingMachine)
        {
            MoveMachine(_machinePossessed);
        }

        if (_takeControlOfMachineAction == null)
            Debug.LogError("_TakeControlOfMachineAction introuvable");
        if (_quitControlOfMachineAction == null)
            Debug.LogError("_QuitControlOfMachineAction introuvable");
        if (_joystickMachineAction == null)
            Debug.LogError("_JoystickMachineAction introuvable");
    }
}
