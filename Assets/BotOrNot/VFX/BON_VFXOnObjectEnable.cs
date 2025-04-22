using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class BON_VFXOnObjectEnable : MonoBehaviour
{
    /*
     * FIELDS
     */

    private VisualEffect _vfx;

    /*
     * UNITY METHODS
     */

    private void Start()
    {
        _vfx = GetComponent<VisualEffect>();
    }

    private void OnEnable()
    {
        _vfx.Play();
    }

    private void OnDisable()
    {
        _vfx.Stop();
    }

}
