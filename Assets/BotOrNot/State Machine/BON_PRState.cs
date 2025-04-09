using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "State/PRState")]
public class BON_PRState : BON_AvatarState
{
    /*
    *  CLASS METHODS
    */

    protected override bool CheckStatePossible(States currentState) // <-- (eg robot cannot Jump, Dame robot cannot use cable)
    {
        if (currentState ==  States.Jump)
        {
            MonoBehaviour.print("état pas possible pour le robot");
            return false;
        }
        return true;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (BON_GameManager.Instance().IsPlayingNut == true) //test current state
        {
            if (!CheckStatePossible(_currentState))
            {
                InitState(States.Idle);
            }
        }
    }
}
