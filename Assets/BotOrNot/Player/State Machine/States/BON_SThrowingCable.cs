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
        if (!_player.AvatarState.HasCableOut) //a la fin du cable
        {
            if (_player.AvatarState.IsMovingByPlayer) //si on bouge -> etat moving
            {
                _player.AvatarState.ChangeState(BON_AvatarState.State.Moving);
            }
            else //sinon idle
            {
                _player.AvatarState.ChangeState(BON_AvatarState.State.Idle);
            }
        }
    }
}
