using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BON_MachineInteractDR : MonoBehaviour
{
    /*
     *  FIELDS
     */

    InputAction _interactMachineAction; //interact pour prendre le controle
    InputAction _quitMachineAction; //rappuyer pour quitter le controle
    InputAction _moveMachineAction; //bouger quand la machine est controlable

    // player script reference
    [SerializeField] private BON_CCPlayer _player;

    Vector2 _moveMachineValue;

    /*
     *  CLASS METHODS
     */

    public void MoveMachine(/*class machine*/)
    {
        _moveMachineValue = _moveMachineAction.ReadValue<Vector2>();
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
        if (_quitMachineAction.WasPressedThisFrame())
        {
            if (!_player.IsSwitching)
            {
                print("control switch to " + GetComponent<PlayerInput>().currentActionMap);
                _player.AvatarState.IsConstrollingMachine = false;
                StartCoroutine(_player.CooldownSwitchControl());
                _player.RecoverControl();
            }
        }
    }

    /*
     *  UNITY METHODS
     */

    // Start is called before the first frame update
    void Start()
    {
        _interactMachineAction = InputSystem.actions.FindAction("ActionsMapDR/Interact"); //take control machine
        _quitMachineAction = InputSystem.actions.FindAction("MachineControl/Interact"); //recover control
        _moveMachineAction = InputSystem.actions.FindAction("MachineControl/Move"); //control machine
    }

    // Update is called once per frame
    void Update()
    {
        // Take item action handling
        if (_interactMachineAction.WasReleasedThisFrame()) //interact
        {
            if (_player.AvatarState.IsNearIOMInteractible && !_player.IsSwitching) //machine pas loin et pas en cours d'activation
            {
                _player.AvatarState.IsConstrollingMachine = true;
                StartCoroutine(_player.CooldownSwitchControl()); 
                _player.GiveControl();
            }
        }
        if (_player.AvatarState.IsConstrollingMachine)
        {
            MoveMachine();
        }
    }
}
