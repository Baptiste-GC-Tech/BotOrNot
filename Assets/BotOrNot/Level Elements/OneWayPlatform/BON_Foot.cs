using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class BON_Foot : MonoBehaviour
{
    /*
     * FIELDS
     */

    [SerializeField] BON_Actionnable[] _vfxActionnables;

    /*
     * UNITY METHODS
     */

    private void Start()
    {
        foreach (var actionnable in _vfxActionnables) 
        {
            actionnable.Status = false;
        }
    }
}
