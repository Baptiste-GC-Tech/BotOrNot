using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;  

public class CCPlayer : MonoBehaviour 
{
    /*
     *  FIELDS
     */
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
        
    }

    void Update()
    {
        
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

    public GameObject getCollectible()
    {
        return collectible;
    }
}