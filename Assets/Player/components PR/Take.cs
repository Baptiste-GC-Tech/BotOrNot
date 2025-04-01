using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Take : MonoBehaviour
{
    /*
     *  FIELDS
     */
    InputAction TakeAction;

    // Object-interaction related
    GameObject collectible;
    private bool _isCollectibleInRange; //devant un collectible (de la DR)
    private bool _isDRInRange = false; //devant la dame robot 


    // Start is called before the first frame update
    void Start()
    {
        TakeAction = InputSystem.actions.FindAction("Player/Take");
        collectible = GetComponent<CCPlayer>().getCollectible();
    }

    // Update is called once per frame
    void Update()
    {
        // Take item action handling
        //if (TakeAction.WasPressedThisFrame()) //interact
        //{
        //    if (_isCollectibleInRange && collectible != null)
        //    {
        //        print(collectible.name);
        //        if (!(_DRCollectibles.Contains(collectible)))
        //        {
        //            _DRCollectibles.Add(collectible);
        //        }

        //        collectible.SetActive(false);

        //        collectible = null;
        //    }
        //    else if (_isDRInRange)
        //    {
        //        print(_DRCollectibles.Count);
        //        if (_DRCollectibles.Count > 0)
        //        {
        //            for (int i = _DRCollectibles.Count - 1; i >= 0; i--)
        //            {
        //                Destroy(_DRCollectibles[i]);
        //                _DRCollectibles.Remove(_DRCollectibles[i]);
        //                _DRCount++;
        //            }
        //            print("objets déposés");
        //            print(_DRCount);
        //        }
        //    }
        //}
    }
}
