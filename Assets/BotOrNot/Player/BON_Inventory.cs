using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BON_Inventory : MonoBehaviour
{
    /*
     *  FIELDS
     */

    private List<int> _items = new();

    public List<int> Items {  get { return _items; } }
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
        _items.Remove(i);
    }

    public int CountItem() //count how much item are on the list
    {
        return _items.Count;
    }

    public bool HasItem(int i)
    {
        foreach (var item in _items) 
        {
            if (item == i) return true;
        }
        return false;
    }

    // DEBUGGING METHODS
#if UNITY_EDITOR
    public void PrintInventory()
    {
        for (int i = 0; i < _items.Count; i++)
        {
            print(_items[i]);
        }
    }
#endif
}
