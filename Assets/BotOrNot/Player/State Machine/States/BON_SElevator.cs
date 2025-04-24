using UnityEngine;

public class BON_SElevator : BON_State
{
    public override void Enter()
    {
        //(anim ?)
        //cancel move ?
        //BON_GameManager.Instance().DisableCompPlayer(BON_GameManager.Instance().CurrentCharacterPlayed);
    }

    public override void Exit()
    {
        //(regive move ?)
        //BON_GameManager.Instance().EnableCompPlayer(BON_GameManager.Instance().CurrentCharacterPlayed);
    }

    public override void UpState()
    {
        if (!_player.AvatarState.IsInElevator || !_player.AvatarState.IsNearElevator) //fin de l'elevator ou trop loin => fin state
        {
            _player.AvatarState.ChangeState(BON_AvatarState.State.Idle);
        }
    }
}
