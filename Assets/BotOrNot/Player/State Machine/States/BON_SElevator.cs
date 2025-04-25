using UnityEngine;

public class BON_SElevator : BON_State
{
    public override void Enter()
    {
        _player.AvatarState.IsInElevator = true;
    }

    public override void Exit()
    {
        _player.AvatarState.IsInElevator = false;
    }

    public override void UpState()
    {
        if (!_player.AvatarState.IsInElevator || !_player.AvatarState.IsNearElevator) //fin de l'elevator ou trop loin => fin state
        {
            _player.AvatarState.ChangeState(BON_AvatarState.State.Idle);
        }
    }
}
