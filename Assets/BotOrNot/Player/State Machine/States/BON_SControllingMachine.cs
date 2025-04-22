public class BON_SControllingMachine : BON_State
{
    public override void Enter()
    {
        //cam unzoom(?) for show player + machine 

        BON_GameManager.Instance().GiveControl();
    }

    public override void Exit()
    {
        //cam focus on player

        BON_GameManager.Instance().RecoverControl();
    }

    public override void UpState()
    {
        if (!_player.AvatarState.IsConstrollingMachine)
        {
            _player.AvatarState.ChangeState(BON_AvatarState.State.Idle);
        }
    }
}
