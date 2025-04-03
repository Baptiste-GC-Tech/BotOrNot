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

    private bool _isSwitching = false;
    private bool _playingMachine = false;

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
        if(QuitMachineAction.WasPressedThisFrame() && !_isSwitching)
        {
            _playingMachine = false;
            player.RecoverControl();
        }
    }

    /*
     *  UNITY METHODS
     */

    // Start is called before the first frame update
    void Start()
    {
        InteractMachineAction = InputSystem.actions.FindAction("ActionsMapPR/Interact"); //take control machine
        QuitMachineAction = InputSystem.actions.FindAction("MachineControl/Interact"); //recover control
        MoveMachineAction = InputSystem.actions.FindAction("MachineControl/Move"); //control machine
    }

    // Update is called once per frame
    void Update()
    {
        // Take item action handling
        if (InteractMachineAction.WasPressedThisFrame()) //interact
        {
            if (player.IsMachineInRange && !_isSwitching) //machine pas loin et pas en cours d'activation
            {
                _playingMachine = true;
                StartCoroutine(CooldownSwitchControl());
                player.SwitchControl();
            }
        }
        if(_playingMachine)
        {
            MoveMachine();
        }
    }

    IEnumerator CooldownSwitchControl()
    {
        _isSwitching = true;
        yield return new WaitForSeconds(0.5f); //dur�e anim
        _isSwitching = false;
    }
}
