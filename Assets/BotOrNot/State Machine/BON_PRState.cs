using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "State/PRState")]
public class BON_PRState : BON_AvatarState
{
    /*
    *  CLASS METHODS
    */

    protected override bool CheckStatePossible(States newState) // <-- (eg robot cannot Jump, Dame robot cannot use cable)
    {
        MonoBehaviour.print("check state PR");
        if (newState ==  States.Jump)
        {
            MonoBehaviour.print("état pas possible pour le robot");
            return false;
        }
        return true;
    }

    public override void ChangeState(States state)
    {
        //MonoBehaviour.print("current state =" + _currentState);
        if (_currentState != state && CheckStatePossible(state))
        {
            //SetState(state);
            MonoBehaviour.print("changement effectué");
            _currentState = state;
        }
        else
        {
            MonoBehaviour.print("changement non validé");
        }
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
