using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BON_SElevator : BON_State
{
    public override void Enter()
    {
        //anim ?
        //cancel move ?
    }

    public override void Exit()
    {
        //regive move ?
    }

    public override void UpState()
    {
        if (!_player.AvatarState.IsInElevator) //switch state ton Idle at the end of elevator
        {
            _player.AvatarState.ChangeState(BON_AvatarState.States.Moving); _player.AvatarState.ChangeState(BON_AvatarState.States.Idle);
        }
    }
}
