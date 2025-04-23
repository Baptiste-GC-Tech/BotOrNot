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
        if (_player.AvatarState.IsDrifting)
        {
            _player.AvatarState.ChangeState(BON_AvatarState.State.Drift);
        }
        else if (_player.AvatarState.HasCableOut)
        {
            _player.AvatarState.ChangeState(BON_AvatarState.State.ThrowingCable);
        }
        else if (_player.AvatarState.IsConstrollingMachine && BON_GameManager.Instance().IsPlayingNut) //si on joue le petit robot qui lance interagit avec une machine-> etat
        {
            _player.AvatarState.ChangeState(BON_AvatarState.State.ControllingMachine);
        }
        else if (!_player.AvatarState.IsGrounded && !BON_GameManager.Instance().IsPlayingNut) //si on joue dame robot et on saute -> etat
        {
            _player.AvatarState.ChangeState(BON_AvatarState.State.Jump);
        }
        else if (!_player.AvatarState.IsMovingByPlayer && !_player.AvatarState.IsDrifting)
        {
            BON_MovePR _movePR = _player.GetComponent<BON_MovePR>();
            if (_movePR == null) 
            {
                Debug.LogError("_movePR introuvable");
            }
            if (_movePR.CurSpeed <= 0.5f)
            {
                _player.AvatarState.ChangeState(BON_AvatarState.State.Idle);
            }
        }
    }
}
