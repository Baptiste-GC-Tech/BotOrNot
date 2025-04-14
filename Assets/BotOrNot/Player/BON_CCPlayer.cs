using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BON_CCPlayer : MonoBehaviour 
{
    /*
     *  FIELDS
     */

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

    /*  triggers bool */

    private bool _isDRInRange = false;
    public bool IsDRInRange
    {
        get { return _isDRInRange; }
    }

    //Instance gameManager
    private BON_GameManager _instance;    

    /*
     *  UNITY METHODS
     */
    void Start()
    {
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
            _isDRInRange = true;
        }
        else if (other.gameObject.tag == "Machine") //trigger with machine
        {
            _avatarState.IsNearIOMInteractible = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Finish") //trigger with DR (broken)
        {
            _isDRInRange = false;
        }
        else if (other.gameObject.tag == "Machine") //trigger with machine
        {
            _avatarState.IsNearIOMInteractible = false;
        }
    }

    // Handles the grounded state
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            Debug.Log("Touching Ground !");
            _avatarState.IsGrounded = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            Debug.Log("Leaving Ground !");
            _avatarState.IsGrounded = false;
        }
    }
}