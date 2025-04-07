using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BON_DRState : BON_AvatarState
{

    /*
    *  CLASS METHODS
    */

    protected override bool CheckStatePossible(States currentState) // <-- (eg robot cannot Jump, Dame robot cannot use cable)
    {
        if (currentState == States.ControllingMachine || currentState == States.ThrowingCable)
        {
            MonoBehaviour.print("état pas possible pour la dame robot");
            return false;
        }
        return true;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    public override void Update()
    {
        if (BON_GameManager.Instance().IsPlayingNut == false) //test current state
        {
            CheckStatePossible(_currentState);
        }
    }
}
