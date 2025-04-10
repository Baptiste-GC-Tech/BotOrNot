using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BON_CCPlayer : MonoBehaviour 
{
    /*
     *  FIELDS
     */

    [SerializeField] protected List<BON_AvatarState> _listAvatarsStates = new List<BON_AvatarState>(2); //stock les 2 state machines robots
    // state machine reference

    private BON_AvatarState _avatarState;
    public BON_AvatarState AvatarState
    {
        get { return _avatarState; }
        set { _avatarState = value; }
    }

    private List<List<MonoBehaviour>> _componentsAvatar;

    [SerializeField] protected List<MonoBehaviour> _componentsPR;
    [SerializeField] protected List<MonoBehaviour> _componentsDR;


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

    private bool _isSwitching = false;  //bool for switch player/machines
    public bool IsSwitching
    {
        get { return _isSwitching; }
        set { _isSwitching = value; }
    }

    private int _currentCharacterPlayed; //0 = PR, 1= DR, autre = machine
    private int _lastCharacterPlayed; //var tampon pour recup le controle

    /*
     *  CLASS METHODS
     */

    public void GiveControl() //donner le controle a une machine
    {
        _lastCharacterPlayed = _currentCharacterPlayed; //save l'id du perso
        _currentCharacterPlayed = -1;
        GetComponent<PlayerInput>().SwitchCurrentActionMap("MachineControl");
        DisableCompPlayer(_lastCharacterPlayed);
        _componentsAvatar[_lastCharacterPlayed][2].enabled = true;
        print("control switch to " + GetComponent<PlayerInput>().currentActionMap);
    }

    public void SwitchPlayer()
    {
        //switch PR to DR
        if (_currentCharacterPlayed == 0) //switch PR to DR
        {
            EnableCompPlayer(1);
            DisableCompPlayer(0);
            GetComponent<PlayerInput>().SwitchCurrentActionMap("ActionsMapDR");
            _currentCharacterPlayed = 1;
        }
        else if (_currentCharacterPlayed == 1)//switch DR to PR
        {
            EnableCompPlayer(0);
            DisableCompPlayer(1);
            GetComponent<PlayerInput>().SwitchCurrentActionMap("ActionsMapPR");
            _currentCharacterPlayed = 0;
        }
        _avatarState = _listAvatarsStates[_currentCharacterPlayed];
        print("control switch to " + GetComponent<PlayerInput>().currentActionMap);
    }
    public void RecoverControl() //reprendre le controle
    {
        _currentCharacterPlayed = _lastCharacterPlayed;
        EnableCompPlayer(_lastCharacterPlayed);
        if (_currentCharacterPlayed == 0)
        {
           GetComponent<PlayerInput>().SwitchCurrentActionMap("ActionsMapPR");
        }
        else
        {
            GetComponent<PlayerInput>().SwitchCurrentActionMap("ActionsMapDR");
        }
        print("control switch to " + GetComponent<PlayerInput>().currentActionMap);
    }

    private void DisableCompPlayer(int CharacterStopPlaying)
    {
        for (int i = 0; i < _componentsAvatar[CharacterStopPlaying].Count;i++) //disable all comps in list
        {
            if (_componentsAvatar[CharacterStopPlaying][i].enabled)
            {
                _componentsAvatar[CharacterStopPlaying][i].enabled = false;
            }
        }
    }

    private void EnableCompPlayer(int CharacterWillPlay)
    {
        for (int i = 0; i < _componentsAvatar[CharacterWillPlay].Count; i++) //enable all comps in list
        {
            if (!_componentsAvatar[CharacterWillPlay][i].enabled)
            {
                _componentsAvatar[CharacterWillPlay][i].enabled = true;
            }
        }
    }
    public IEnumerator CooldownSwitchControl()
    {
        _isSwitching = true;
        yield return new WaitForSeconds(0.5f); //durée anim?
        _isSwitching = false;
    }

    /*
     *  UNITY METHODS
     */
    void Start()
    {
        //init values
        _currentCharacterPlayed = 0; //PR joue en premier
        _lastCharacterPlayed = _currentCharacterPlayed;
         _componentsAvatar = new();
         _componentsAvatar.Add(_componentsPR);
         _componentsAvatar.Add(_componentsDR);

        //start with PR => disable DR
        DisableCompPlayer(1);
        EnableCompPlayer(0);

        BON_GameManager instance = BON_GameManager.Instance();
        DontDestroyOnLoad(instance);

        //set state machine PR or DR
        if (instance.IsPlayingNut)
        {
            _avatarState = _listAvatarsStates[0];
        }
        else
        {
            _avatarState = _listAvatarsStates[1];
        }

        _avatarState.Init(); //init state machine (current state, player ref, dictionnary)
    }

    void Update()
    {
        //print(_avatarState.CurrentState);
        _avatarState.UpdateState();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "collectibles_DR") //trigger with items <- change tag name
        {
            _collectible = other.gameObject;
            _avatarState.IsNearItem = true;
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
        if (other.gameObject.tag == "collectibles_DR")
        {
            _avatarState.IsNearItem = false;
        }
        else if (other.gameObject.tag == "Finish") //trigger with DR (broken)
        {
            _isDRInRange = false;
        }
        else if (other.gameObject.tag == "Machine") //trigger with machine
        {
            _avatarState.IsNearIOMInteractible = false;
        }
    }
}