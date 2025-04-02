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

    private int _currenCharacterPlayed; //0 = PR, 1= DR, autre = machine
    private int _lestCharacterPlayed; //var tampon pour recup le controle

    /*// First level completion condition : Should have its own class
    // TODO: Generic Level class featuring states and a win cond, then make a child that's Level 1's class
    private List<GameObject> _DRCollectibles = new List<GameObject>();
    private int _DRCount = 0;*/

    /*
     *  CLASS METHODS
     */


    public void SwitchControl() //donner le controle a une machine
    {
        _lestCharacterPlayed = _currenCharacterPlayed; //save l'id du perso
        _currenCharacterPlayed = -1;
        GetComponent<PlayerInput>().SwitchCurrentActionMap("MachineControl");
    }
    public void RecoverControl() //reprendre le controle
    {
        _currenCharacterPlayed = _lestCharacterPlayed;
        if (_currenCharacterPlayed == 0)
        {
           GetComponent<PlayerInput>().SwitchCurrentActionMap("ActionsMapPR");
        }
        else
        {
            GetComponent<PlayerInput>().SwitchCurrentActionMap("ActionsMapDR");
        }
    }

    public void SwitchPlayer()
    {
        _currenCharacterPlayed = -1;
        GetComponent<PlayerInput>().SwitchCurrentActionMap("MachineControl");
    }


    /*
     *  UNITY METHODS
     */
    private void Start()
    {
        
    }

    void Update()
    {
        
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
        else if (other.gameObject.tag == "Hook") //trigger with machine
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
        else if (other.gameObject.tag == "Hook") //trigger with machine
        {
            _isMachineInRange = false;
        }
    }
}