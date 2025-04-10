using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BON_SThrowingCable : BON_State
{
    public override void Enter()
    {

    }

    public override void Exit()
    {

    }

    public override void UpState()
    {
        if (!_player.AvatarState.IsthrowingCable) //a la fin du cable
        {
            if (_player.AvatarState.IsMoving) //si on bouge -> etat moving
            {
                _player.AvatarState.ChangeState(BON_AvatarState.States.Moving);
            }
            else //sinon idle
            {
                _player.AvatarState.ChangeState(BON_AvatarState.States.Idle);
            }
        }
        else
        {
            
        }
    }
}
