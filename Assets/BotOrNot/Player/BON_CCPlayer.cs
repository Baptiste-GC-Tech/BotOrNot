using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BON_CCPlayer : MonoBehaviour 
{
    /*
     *  FIELDS
     */
    private Rigidbody _rb;

    // state machine reference
    [SerializeField] BON_AvatarState _avatarState;
    public BON_AvatarState AvatarState
    {
        get { return _avatarState; }
        set { _avatarState = value; }
    }

    // for stock machine ref ?
    private BON_Interactive _machineToActivate = null;
    public BON_Interactive MachineToActivate
    {
        get { return _machineToActivate; }
        set { _machineToActivate = value; }
    }

    //Instance gameManager
    public BON_GameManager _instance;


    /*
     *  UNITY METHODS
     */
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _instance = BON_GameManager.Instance();
        DontDestroyOnLoad(_instance);

        _avatarState.Init(); //init state machine (current state, player ref, dictionnary)
    }

    void Update()
    {
        //print(_avatarState.CurrentState);
        _avatarState.UpdateState();
    }
    private void OnTriggerEnter(Collider other)
    { //tag with "Finish" (DR) remove -> useless
        if (other.gameObject.layer == LayerMask.NameToLayer("TriggerMachine")) //trigger with machine
        {
            _avatarState.IsNearIOMInteractible = true;
            _machineToActivate = other.GetComponentInParent<BON_Interactive>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("TriggerMachine")) //trigger with machine 
        {
            _avatarState.IsNearIOMInteractible = false;
            _machineToActivate = null;
        }
    }
}