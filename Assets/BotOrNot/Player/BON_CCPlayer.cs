using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;

public class BON_CCPlayer : MonoBehaviour 
{
    /*
     *  FIELDS
     */

    [SerializeField] protected List<MonoBehaviour> _componentsPR;
    [SerializeField] protected List<MonoBehaviour> _componentsDR;

    private List<List<MonoBehaviour>> _CompoentsAvatar;

    // Object-interaction related
    private GameObject _collectible = null;
    public GameObject Collectible
    {
        get { return _collectible; }
        set { _collectible = value; }
    }

    private bool _isCollectibleInRange = false;
    public bool IsCollectibleInRange
    {
        get { return _isCollectibleInRange; }
    }

    private bool _isDRInRange = false;
    public bool IsDRInRange
    {
        get { return _isDRInRange; }
    }

    private bool _isMachineInRange = false;
    public bool IsMachineInRange
    {
        get { return _isMachineInRange; }
    }

    //for stock machine ref ( for control it)
    private GameObject _machine = null; //ou classe de la machine directement
    public GameObject Machine
    {
        get { return Machine; }
        set { _machine = value; }
    }

    private bool _isSwitching = false;  //bool for switch player/machines
    public bool IsSwitching
    {
        get { return _isSwitching; }
        set { _isSwitching = value; }
    }


    private int _currentCharacterPlayed; //0 = PR, 1= DR, autre = machine
    private int _lastCharacterPlayed; //var tampon pour recup le controle

    /*// First level completion condition : Should have its own class
    // TODO: Generic Level class featuring states and a win cond, then make a child that's Level 1's class
    private List<GameObject> _DRCollectibles = new List<GameObject>();
    private int _DRCount = 0;*/

    /*
     *  CLASS METHODS
     */


    public void GiveControl() //donner le controle a une machine
    {
        _lastCharacterPlayed = _currentCharacterPlayed; //save l'id du perso
        _currentCharacterPlayed = -1;
        GetComponent<PlayerInput>().SwitchCurrentActionMap("MachineControl");
        DisableCompPlayer(_lastCharacterPlayed);
        print("control switch to " + "Machine");
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
        for (int i = 0; i < _CompoentsAvatar[CharacterStopPlaying].Count;i++) //disable all comps in list
        {
            if (_CompoentsAvatar[CharacterStopPlaying][i].enabled)
            {
                _CompoentsAvatar[CharacterStopPlaying][i].enabled = false;
            }
        }
    }

    private void EnableCompPlayer(int CharacterWillPlay)
    {
        for (int i = 0; i < _CompoentsAvatar[CharacterWillPlay].Count; i++) //enable all comps in list
        {
            if (!_CompoentsAvatar[CharacterWillPlay][i].enabled)
            {
                _CompoentsAvatar[CharacterWillPlay][i].enabled = true;
            }
        }
    }

    public IEnumerator CooldownSwitchControl()
    {
        _isSwitching = true;
        yield return new WaitForSeconds(0.5f); //durée anim
        _isSwitching = false;
    }

    /*
     *  UNITY METHODS
     */
    private void Start()
    {
        //init values
        //_currentCharacterPlayed = 0;
        _lastCharacterPlayed = _currentCharacterPlayed;
         _CompoentsAvatar = new();
         _CompoentsAvatar.Add(_componentsPR);
         _CompoentsAvatar.Add(_componentsDR);

        //start with PR => disable DR
        DisableCompPlayer(0);
        EnableCompPlayer(1);
    }

    void Update()
    {
        //print(_currentCharacterPlayed);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "collectibles_DR") //trigger with DR's items
        {
            _isCollectibleInRange = true;
            _collectible = other.gameObject;
        }
        else if (other.gameObject.tag == "Finish") //trigger with DR
        {
            _isDRInRange = true;
        }
        else if (other.gameObject.tag == "Machine") //trigger with machine
        {
            _isMachineInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "collectibles_DR")
        {
            _isCollectibleInRange = false;
        }
        else if (other.gameObject.tag == "Finish") //trigger with DR
        {
            _isDRInRange = false;
        }
        else if (other.gameObject.tag == "Machine") //trigger with machine
        {
            _isMachineInRange = false;
        }
    }
}