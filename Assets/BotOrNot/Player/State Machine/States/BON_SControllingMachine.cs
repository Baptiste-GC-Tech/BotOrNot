public class BON_SControllingMachine : BON_State
{
    public override void Enter()
    {
        //cam unzoom(?) for see player + machine 
    }

    public override void Exit()
    {
        //cam focus on player
    }

    public override void UpState()
    {
        if (!_player.AvatarState.IsConstrollingMachine)
        {
            _player.AvatarState.ChangeState(BON_AvatarState.State.Idle);
        }
    }
}
