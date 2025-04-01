using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;  

public class Player : MonoBehaviour    // BAD NAME : Character Controller taken tho. TODO: Find a better name for this class
{
    /*
     *  FIELDS
     */
    // 2. These variables are to hold the Action references
    InputAction MoveAction;
    InputAction CableAction;
    InputAction TakeAction;

    // Movement action related
    [SerializeField] private float _maxSpeed;

    // Object-interaction related
    GameObject collectible = null;
    private bool _isCollectibleInRange = false; //devant un collectible (de la DR)
    private bool _isDRInRange = false; //devant la dame robot 

    // First level completion condition : Should have its own class
    // TODO: Generic Level class featuring states and a win cond, then make a child that's Level 1's class
    private List<GameObject> _DRCollectibles = new List<GameObject>();
    private int _DRCount = 0;


    /*
     *  CLASS METHODS
     */
    private void PrintListCollectibles() //debug 
    {
        for (int i = 0; i < _DRCollectibles.Count; i++)
        {
            print("element "+i + " "+_DRCollectibles[i].name);
        }

        print("quest = "+_DRCount);
    }


    /*
     *  UNITY METHODS
     */
    private void Start()
    {
        MoveAction = InputSystem.actions.FindAction("Player/Move");
        CableAction = InputSystem.actions.FindAction("Player/Cable");
        TakeAction = InputSystem.actions.FindAction("Player/Take");
    }

    void Update()
    {
        // Left-right movement action handling
        Vector2 moveValue = MoveAction.ReadValue<Vector2>();
        GetComponent<Rigidbody>().AddForce(new Vector3(moveValue.x, moveValue.y, 0.0f), ForceMode.Acceleration);
        Mathf.Clamp(GetComponent<Rigidbody>().velocity.x, 0.0f, _maxSpeed);
        Mathf.Clamp(GetComponent<Rigidbody>().velocity.y, 0.0f, _maxSpeed);
        //transform.position += new Vector3(moveValue.x * Time.deltaTime, 0, 0);

        // Cable action handling
        // ## Currently serves as debug
        if (CableAction.WasPressedThisFrame()) //cable => ""jump""
        {
            print(_DRCollectibles.Count);
            PrintListCollectibles();
        }

        // Take item action handling
        if (TakeAction.WasPressedThisFrame()) //interact
        {
            if (_isCollectibleInRange && collectible !=null)
            {
                print(collectible.name);
                if(!(_DRCollectibles.Contains(collectible)))
                {
                    _DRCollectibles.Add(collectible);
                }
                
                collectible.SetActive(false);
                
                collectible = null;
            }
            else if (_isDRInRange)
            {
                print(_DRCollectibles.Count);
                if (_DRCollectibles.Count > 0)
                {
                    for (int i= _DRCollectibles.Count-1; i>=0 ; i--)
                    {
                        Destroy(_DRCollectibles[i]);
                        _DRCollectibles.Remove(_DRCollectibles[i]);
                        _DRCount++;
                    }
                    print("objets déposés");
                    print(_DRCount);
                }
                else
                {
                    print("objets non déposés");
                }
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "collectibles_DR")
        {
            _isCollectibleInRange = true;
            collectible = other.gameObject;
        }
        else if (other.gameObject.tag == "Finish")
        {
            _isDRInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "collectibles_DR")
        {
            _isCollectibleInRange = false;
        }
        else if (other.gameObject.tag == "Finish")
        {
            _isDRInRange = false;
        }
    }
}