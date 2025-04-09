using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "State/DRState")]
public class BON_DRState : BON_AvatarState
{
    /*
    *  CLASS METHODS
    */
    protected override bool CheckStatePossible(States currentState) // <-- (eg robot cannot Jump, Dame robot cannot use cable)
    {
        MonoBehaviour.print("check state DR");
        if (currentState == States.ControllingMachine || currentState == States.ThrowingCable)
        {
            MonoBehaviour.print("état pas possible pour la dame robot");
            return false;
        }
        return true;
    }
    public override void ChangeState(States state)
    {
        if (_currentState != state && CheckStatePossible(state))
        {
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
        if (BON_GameManager.Instance().IsPlayingNut == false) //test current state
        {
            if (!CheckStatePossible(_currentState))
            {
                InitState(States.Idle);
            }
        }
    }
}
