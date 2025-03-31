using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;  

public class Example : MonoBehaviour
{
    // 2. These variables are to hold the Action references
    InputAction MoveAction;
    InputAction CableAction;
    InputAction TakeAction;

    GameObject collectible = null;
    private bool _isCollectible = false; //devant un collectible (de la DR)
    private bool _isDR = false; //devant la dame robot 

    private List<GameObject> _DRCollectibles = new List<GameObject>();
    private int _DRCount = 0;

    private void Start()
    {
        MoveAction = InputSystem.actions.FindAction("Player/Move");
        CableAction = InputSystem.actions.FindAction("Player/Cable");
        TakeAction = InputSystem.actions.FindAction("Player/Take");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "collectibles_DR")
        {
            _isCollectible = true;
            collectible = other.gameObject;
        }
        else if (other.gameObject.tag == "Finish")
        {
            _isDR = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "collectibles_DR")
        {
            _isCollectible = false;
        }
        else if (other.gameObject.tag == "Finish")
        {
            _isDR = false;
        }
    }

    private void PrintListCollectibles() //debug 
    {
        for (int i = 0; i < _DRCollectibles.Count; i++)
        {
            print("element "+i + " "+_DRCollectibles[i].name);
        }

        print("quest = "+_DRCount);
    }

    void Update()
    {
        Vector2 moveValue = MoveAction.ReadValue<Vector2>();
        GetComponent<Rigidbody>().transform.position += new Vector3(moveValue.x * Time.deltaTime, 0, 0);
        
        if (CableAction.WasPressedThisFrame()) //cable => ""jump""
        {
            print(_DRCollectibles.Count);
            PrintListCollectibles();
        }

        if (TakeAction.WasPressedThisFrame()) //interact
        {
            if (_isCollectible && collectible !=null)
            {
                print(collectible.name);
                if(!(_DRCollectibles.Contains(collectible)))
                {
                    _DRCollectibles.Add(collectible);
                }
                
                collectible.SetActive(false);
                
                collectible = null;
            }
            else if (_isDR)
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
}