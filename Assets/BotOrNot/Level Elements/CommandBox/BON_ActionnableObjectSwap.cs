using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BON_ActionnableObjectSwap : BON_Actionnable
{
    /*
     * FIELDS
     */

    [SerializeField] GameObject _offObject;
    [SerializeField] GameObject _onObject;

    /*
     * CLASS METHODS
     */

    public override void On()
    {
        _onObject.SetActive(true);
        _offObject.SetActive(false);
    }

    public override void Off()
    {
        _onObject.SetActive(false);
        _offObject.SetActive(true);
    }

}
