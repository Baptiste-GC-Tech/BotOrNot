using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BON_MachineInteractDR : MonoBehaviour
{
    ///*
    // *  FIELDS
    // */
    ////Input related
    //InputAction _TakeControlOfMachineAction;    // Upon machine interaction, represents taking control of it and losing control of PR
    //InputAction _QuitControlOfMachineAction;    // Upon machine interaction, represents forfeiting control of it and gaining back control of PR
    //InputAction _JoystickMachineAction;         // When controling a machine, sends the input over to it so it can do stuff
    //Vector2 _moveMachineValue;

    //// Player & State related
    //[SerializeField] private BON_CCPlayer _player;

    //private BON_Interactive _machineToActivate;
    //public BON_Interactive MachineToActivate
    //{
    //    get { return _machineToActivate; }
    //    set { _machineToActivate = value; }
    //}

    //private BON_Controllable _machinePossessed;
    //public BON_Controllable MachinePossessed
    //{
    //    get { return _machinePossessed; }
    //    set { _machinePossessed = value; }
    //}


    /*
     *  CLASS METHODS
     */

    //public void MoveMachine(BON_Controllable _machine)
    //{
    //    // Reads input values
    //    _moveMachineValue = _JoystickMachineAction.ReadValue<Vector2>();

    //    _machine.ProcessInput(_moveMachineValue);

    //    if (_QuitControlOfMachineAction.WasPressedThisFrame())
    //    {
    //        if (!BON_GameManager.Instance().IsSwitching)
    //        {
    //            _machineToActivate.Activate();
    //            BON_GameManager.Instance().RecoverControl();
    //            StartCoroutine(BON_GameManager.Instance().CooldownSwitchControl());
    //            _player.AvatarState.IsConstrollingMachine = false;
    //        }
    //    }
    //}

    /*
     *  UNITY METHODS
     */
    void Start()
    {
    //    _TakeControlOfMachineAction = InputSystem.actions.FindAction("ActionsMapDR/Interact"); //take control machine
    //    _QuitControlOfMachineAction = InputSystem.actions.FindAction("MachineControl/Interact"); //recover control
    //    _JoystickMachineAction = InputSystem.actions.FindAction("MachineControl/Move"); //control machine
    }

    void Update()
    {
        //Debug.Log(_player.AvatarState.IsConstrollingMachine);
        //// Control management (gaining control of the machine or taking back control of PR)
        //if (_TakeControlOfMachineAction.WasPressedThisFrame()) //interact
        //{
        //    Debug.Log("click");
        //    if (_player.AvatarState.IsNearIOMInteractible && !BON_GameManager.Instance().IsSwitching) //machine pas loin et pas en cours d'activation
        //    {                                       // 2 machines : free crane et "levier"
        //        Debug.Log("activate");
        //        _machineToActivate = _player.MachineToActivate;
        //        _machineToActivate.Activate();
        //        StartCoroutine(BON_GameManager.Instance().CooldownSwitchControl());
        //        BON_GameManager.Instance().GiveControl();
        //        if (_machineToActivate.GetType() == typeof(BON_Controllable)) //si la machine est controllable (free crane)
        //        {
        //            Debug.Log("passe a true");
        //            _player.AvatarState.IsConstrollingMachine = true;
        //        }
        //    }
        //}
        //if (_player.AvatarState.IsConstrollingMachine)
        //{
        //    MoveMachine(_machinePossessed);
        //}

        //if (_TakeControlOfMachineAction == null)
        //    Debug.LogError("_TakeControlOfMachineAction introuvable");
        //if (_QuitControlOfMachineAction == null)
        //    Debug.LogError("_QuitControlOfMachineAction introuvable");
        //if (_JoystickMachineAction == null)
        //    Debug.LogError("_JoystickMachineAction introuvable");
    }
}
