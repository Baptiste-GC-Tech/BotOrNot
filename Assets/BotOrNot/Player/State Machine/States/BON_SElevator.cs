using UnityEngine;

public class BON_SElevator : BON_State
{
    public override void Enter()
    {
        //(anim ?)
        //cancel move ?
        BON_GameManager.Instance().DisableCompPlayer(BON_GameManager.Instance().CurrentCharacterPlayed);
    }

    public override void Exit()
    {
        //(regive move ?)
        BON_GameManager.Instance().EnableCompPlayer(BON_GameManager.Instance().CurrentCharacterPlayed);
    }

    public override void UpState()
    {
        if (!_player.AvatarState.IsInElevator) //elevator end
        {
            _player.AvatarState.ChangeState(BON_AvatarState.State.Idle);
        }
    }
}
