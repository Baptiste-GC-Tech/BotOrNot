using UnityEngine;

public class BON_SIdle : BON_State
{
    public override void Enter()
    {
        //anim Idle        
    }

    public override void Exit()
    {
        //MonoBehaviour.print("exit idle");
    }

    public override void UpState()
    {
        //transitions d'etats -> from idle to .... :

        if (_player.AvatarState.HasCableOut && BON_GameManager.Instance().IsPlayingNut) //si on joue le petit robot qui lance le cable-> etat 
        {
            _player.AvatarState.ChangeState(BON_AvatarState.State.ThrowingCable);
        }
        else if (!_player.AvatarState.IsGrounded && !BON_GameManager.Instance().IsPlayingNut) //si on joue dame robot et on saute -> etat
        {
            _player.AvatarState.ChangeState(BON_AvatarState.State.Jump);
        }
        else if (_player.AvatarState.IsInElevator) //si on prend l'ascenseur
        {
            _player.AvatarState.ChangeState(BON_AvatarState.State.Elevator);
        }
        else if (_player.AvatarState.IsConstrollingMachine && BON_GameManager.Instance().IsPlayingNut) //si on joue le petit robot qui lance interagit avec une machine-> etat
        {
            _player.AvatarState.ChangeState(BON_AvatarState.State.ControllingMachine);
        }
        else if (_player.AvatarState.IsMovingByPlayer) //si on bouge (en dernier, moins prio sur les autres(eg. si on bouge pendant le cable , l'etat est ThrowingCable en prio ))
        {
            _player.AvatarState.ChangeState(BON_AvatarState.State.Moving);
        }
    }
}
