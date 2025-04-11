using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BON_BrokenRobotLady : BON_Actionnable
{
    //FIELDS
    private List<int> _itemsList;
    [SerializeField] List<GameObject> _bodyParts;


    //CLASS METHODS
    public override void On()
    {
        BON_Inventory Inventaire = GameObject.FindWithTag("Player").GetComponent<BON_Inventory>();
        for (int k = 0; k < Inventaire.CountItem(); k++)
        {
            {
                _itemsList.Add(Inventaire.Items[k]);
                _bodyParts[k].SetActive(true);
            }
        }
        Toggle();
    }
}
