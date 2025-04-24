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
    List<bool> _temporaryRemember = new List<bool>();

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

    public void SaveAndDisableParticles()
    {
        _temporaryRemember.Clear();
        foreach (var actionnable in _vfxActionnables)
        {
            _temporaryRemember.Add(actionnable.Status);
            actionnable.Status = false;
        }
    }

    public void EnableParticlesFromSave()
    {
        for (int i = 0; i < _temporaryRemember.Count; i++)
        {
            _vfxActionnables[i].Status = _temporaryRemember[i];
        }
    }
}
