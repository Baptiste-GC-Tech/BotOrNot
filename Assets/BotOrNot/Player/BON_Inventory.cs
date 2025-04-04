using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BON_Inventory : MonoBehaviour
{
    /*
     *  FIELDS
     */

    private List<int> _items = new();

    /*
     *  CLASS METHODS
     */

    public void AddItem(int objId) //add item to the list
    {
        if (!(_items.Contains(objId)))    
        {
            _items.Add(objId);
        }
    }
    public void DeleteItem(int i) //remove the item i from the list and destroy it
    {
        _items.RemoveAt(i);
    }

    public int CountItem() //count how much item are on the list
    {
        return _items.Count;
    }

    public void PrintInventory() //for debug
    {
        for (int i = 0; i < _items.Count; i++)
        {
            print(_items[i]);
        }
    }

    /*
     *  UNITY METHODS
     */

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
