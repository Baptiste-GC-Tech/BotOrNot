using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class BON_ActionnableVFX : BON_Actionnable
{
    /*
     * FIELDS
     */

    VisualEffect _vfx;
    
    /*
     * CLASS METHODS
     */

    public override void On()
    {
        _vfx.Play();
    }
    public override void Off()
    {
        _vfx.Stop();
    }

    /*
     * UNITY METHODS
     */
    private void Awake()
    {
        _vfx = GetComponent<VisualEffect>();
    }
}
