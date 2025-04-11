using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BON_SMoving : BON_State
{
    public override void Enter()
    {
        
    }

    public override void Exit()
    {

    }

    public override void UpState()
    {
        if (_player.AvatarState.IsthrowingCable)
        {
            _player.AvatarState.ChangeState(BON_AvatarState.States.ThrowingCable);
        }
        if (_player.AvatarState.IsJumping && !BON_GameManager.Instance().IsPlayingNut) //si on joue dame robot et on saute -> etat
        {
            _player.AvatarState.ChangeState(BON_AvatarState.States.Jump);
        }
        if (!_player.AvatarState.IsMoving)
        {
            _player.AvatarState.ChangeState(BON_AvatarState.States.Idle);
        }
    }
}
