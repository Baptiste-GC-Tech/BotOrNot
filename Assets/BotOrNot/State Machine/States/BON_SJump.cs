using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BON_SJump : BON_State
{
    public override void Enter()
    {
        _player.AvatarState.IsGrounded = false;
        _player.GetComponent<Rigidbody>().AddForce(new Vector3(0, 300, 0));
    }

    public override void Exit()
    {
        _player.AvatarState.IsJumping = false;
    }

    public override void UpState()
    {
        if (_player.AvatarState.IsGrounded) //au moment de retomber, switch state to moving if move or idle else
        {
            if (_player.AvatarState.IsMoving)
            {
                _player.AvatarState.ChangeState(BON_AvatarState.States.Moving);
            }
            else
            {
                _player.AvatarState.ChangeState(BON_AvatarState.States.Idle);
            }
        }
    }
}
