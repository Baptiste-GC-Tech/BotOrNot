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

    // for stock collectible ref ?
    private GameObject _collectible = null;
    public GameObject Collectible
    {
        get { return _collectible; }
        set { _collectible = value; }
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
        //print(_avatarState.IsAgainstWall);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "collectibles_DR") //trigger with items <- change tag name
        {
            _collectible = other.gameObject;
        }
        else if (other.gameObject.tag == "Finish") //trigger with DR (broken)
        {
            _avatarState.IsDRInRange = true;
        }
        else if (other.gameObject.tag == "TriggerMachine") //trigger with machine
        {
            _avatarState.IsNearIOMInteractible = true;
            _machineToActivate = other.GetComponentInParent<BON_Interactive>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Finish") //trigger with DR (broken)
        {
            _avatarState.IsDRInRange = false;
        }
        else if (other.gameObject.tag == "TriggerMachine") //trigger with machine
        {
            _avatarState.IsNearIOMInteractible = false;
            _machineToActivate = null;
        }
    }
}