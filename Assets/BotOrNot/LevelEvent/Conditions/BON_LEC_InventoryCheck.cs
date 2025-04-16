using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BON_LEC_InventoryCheck : BON_LevelEventCondition
{
    /*
     * FIELDS
     */

    [SerializeField] List<int> _itemIds;


    /*
     * CLASS METHODS
     */
    public override bool Check()
    {
        foreach (var item in _itemIds) 
        {
            if(BON_GameManager.Instance().Inventory.HasItem(item) == false)
            {
                return false;
            }
        }
        return true;
    }
}
