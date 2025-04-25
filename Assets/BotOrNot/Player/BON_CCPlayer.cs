using UnityEngine;

public class BON_CCPlayer : MonoBehaviour 
{
    /*
     *  FIELDS
     */

    // Used in a dirty hack
    private Rigidbody _rb;

    // state machine reference
    [SerializeField] BON_AvatarState _avatarState;
    public BON_AvatarState AvatarState
    {
        get { return _avatarState; }
        set { _avatarState = value; }
    }

    // for stock machine ref
    private BON_Interactive_Actionnables _machineToActivate = null;
    public BON_Interactive_Actionnables MachineToActivate
    {
        get { return _machineToActivate; }
        set { _machineToActivate = value; }
    }

    //Instance gameManager
    [Tooltip("GameManager instance initialized auto in start")]
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
        _avatarState.IsGrounded = false;
        _avatarState.IsNearElevator = false;
    }

    void Update()
    {
        // Dirty hack to stop sliding caused by the rb sometimes
        if (_avatarState.IsGrounded)
        {
            Vector3 newVeloc = _rb.velocity;
            newVeloc.x = 0.0f;
            _rb.velocity = newVeloc;
        }

        //print(_avatarState.CurrentState);
        _avatarState.UpdateState();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("TriggerMachine")) //trigger with machine
        {
            _avatarState.IsNearIOMInteractible = true;
            _machineToActivate = other.GetComponentInParent<BON_Interactive_Actionnables>();
            if (_machineToActivate == null)
            {
                Debug.LogError("_machineToActivate est introuvable");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("TriggerMachine")) //end trigger with machine 
        {
            _avatarState.IsNearIOMInteractible = false;
            _machineToActivate = null;
        }
    }
}