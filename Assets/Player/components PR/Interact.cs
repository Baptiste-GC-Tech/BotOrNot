using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interact : MonoBehaviour
{
    /*
     *  FIELDS
     */
    InputAction TakeAction;

    // player script reference
    [SerializeField] private CCPlayer player;


    // Start is called before the first frame update
    void Start()
    {
        TakeAction = InputSystem.actions.FindAction("Player/Take");
    }

    // Update is called once per frame
    void Update()
    {
        // Take item action handling
        if (TakeAction.WasPressedThisFrame()) //interact
        {
            if (player.IsCollectibleInRange && player.Collectible != null) //item a porté
            {
                //stock in inventory

                print(player.Collectible.name);
                /*if (!(_DRCollectibles.Contains(collectible)))
                {
                    _DRCollectibles.Add(collectible);
                }*/

                player.Collectible.SetActive(false);

                player.Collectible = null;
            }
            else if (player.IsDRInRange) //dame robot pas loin
            {
                //give inventory item(s) to DR

                //if (_DRCollectibles.Count > 0)
                //{
                //    for (int i = _DRCollectibles.Count - 1; i >= 0; i--)
                //    {
                //        Destroy(_DRCollectibles[i]);
                //        _DRCollectibles.Remove(_DRCollectibles[i]);
                //        _DRCount++;
                //    }
                //    print("objets déposés");
                //}
            }
        }
    }
}
