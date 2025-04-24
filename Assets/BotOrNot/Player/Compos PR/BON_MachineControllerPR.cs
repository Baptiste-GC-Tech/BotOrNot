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
    InputAction _ControlMachineAction;         // When controling a machine, sends the input over to it so it can do stuff
    Vector2 _moveMachineValue;
    Rigidbody _machinePossessedRb;
    public Vector2 MoveMachineValue
    { get { return _moveMachineValue; } }


    // Player & State related
    private BON_CCPlayer _player;

    private BON_Interactive_Actionnables _machineToActivate;
    public BON_Interactive_Actionnables MachineToActivate
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
     *  CLASS METHODSs
     */

    public void MoveMachine(BON_Controllable _machine)
    {
        // Reads input values
        _moveMachineValue = _ControlMachineAction.ReadValue<Vector2>(); //input pc
        //_moveMachineValue = _player.GetComponent<BON_MovePR>().MoveInputValue; //input joystick mobile

        _machine.ProcessInput(_moveMachineValue);
    }

    public void ActivateMachine() //button
    {
        if (!_player.AvatarState.IsConstrollingMachine) //si machine pas controlé -> on la controle
        {
            TakeControlOfMachine();
        }
        else //sinon on la quitte
        {
            QuitControlOfMachine();
        }
    }

    public void TakeControlOfMachine()
    {
        //si machine pas loin et pas deja en cours d'activation ou changement de perso
        if (_player.AvatarState.IsNearIOMInteractible && !BON_GameManager.Instance().IsSwitching)
        {
            _machineToActivate = _player.MachineToActivate;
            _machineToActivate.Activate();
            _machinePossessed = (BON_Controllable)_machineToActivate.ActionnablesList[0];
            _machinePossessedRb = _machinePossessed.GetComponent<Rigidbody>();
            if (_machinePossessedRb == null)
            {
                Debug.LogError("_machinePossessedRb introuvable");
            }
            _machinePossessedRb.collisionDetectionMode = CollisionDetectionMode.Continuous;
            StartCoroutine(BON_GameManager.Instance().CooldownSwitchControl());
            _player.AvatarState.IsConstrollingMachine = true;
        }
    }

    public void QuitControlOfMachine()
    {
        if (!BON_GameManager.Instance().IsSwitching)
        {
            if (_machinePossessedRb != null)
            {
                _machinePossessedRb.collisionDetectionMode = CollisionDetectionMode.Discrete;
            }
            _machineToActivate.Activate();
            StartCoroutine(BON_GameManager.Instance().CooldownSwitchControl());
            _player.AvatarState.IsConstrollingMachine = false;
        }
    }

    /*
     *  UNITY METHODS
     */
    void Start()
    {
        _player = GameObject.FindFirstObjectByType<BON_CCPlayer>();
        _TakeControlOfMachineAction = InputSystem.actions.FindAction("ActionsMapPR/Interact"); //take control machine
        _QuitControlOfMachineAction = InputSystem.actions.FindAction("MachineControl/Interact"); //recover control
        _ControlMachineAction = InputSystem.actions.FindAction("MachineControl/Move"); //control machine
        _player.AvatarState.IsConstrollingMachine = false;
    }

    void Update()
    {
        // Control management (gaining control of the machine or taking back control of PR)

        if (_TakeControlOfMachineAction.WasPressedThisFrame() || _QuitControlOfMachineAction.WasReleasedThisFrame()) // pc only because inputAction
        {
            ActivateMachine();
        }

        if (_player.AvatarState.IsConstrollingMachine && _machinePossessed != null)
        {
            MoveMachine(_machinePossessed);
        }

        if (_TakeControlOfMachineAction == null)
            Debug.LogError("_TakeControlOfMachineAction introuvable");
        if (_QuitControlOfMachineAction == null)
            Debug.LogError("_QuitControlOfMachineAction introuvable");
        if (_ControlMachineAction == null)
            Debug.LogError("_ControlMachineAction introuvable");
    }
}