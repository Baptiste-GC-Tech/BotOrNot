using UnityEngine;

public class BON_SJump : BON_State
{
    public override void Enter()
    {
        _player.GetComponent<Rigidbody>().AddForce(new Vector3(0, 300, 0));
    }

    public override void Exit()
    {
        _player.AvatarState.IsGrounded = true;
    }

    public override void UpState()
    {
        if (_player.AvatarState.IsGrounded) //au moment de retomber,    switch state to moving if move or idle else
        {
            if (_player.AvatarState.IsMovingByPlayer)
            {
                _player.AvatarState.ChangeState(BON_AvatarState.State.Moving);
            }
            else
            {
                _player.AvatarState.ChangeState(BON_AvatarState.State.Idle);
            }
        }
    }
}
