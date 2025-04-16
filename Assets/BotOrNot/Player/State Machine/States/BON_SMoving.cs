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
        if (_player.AvatarState.HasCableOut)
        {
            _player.AvatarState.ChangeState(BON_AvatarState.State.ThrowingCable);
        }
        if (!_player.AvatarState.IsGrounded && !BON_GameManager.Instance().IsPlayingNut) //si on joue dame robot et on saute -> etat
        {
            _player.AvatarState.ChangeState(BON_AvatarState.State.Jump);
        }
        if (!_player.AvatarState.IsMovingByPlayer)
        {
            _player.AvatarState.ChangeState(BON_AvatarState.State.Idle);
        }
        if (_player.AvatarState.IsDrifting)
        {
            _player.AvatarState.ChangeState(BON_AvatarState.State.Drift);
        }
    }
}
