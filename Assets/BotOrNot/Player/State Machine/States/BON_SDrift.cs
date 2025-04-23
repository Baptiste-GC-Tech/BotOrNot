using UnityEngine;

public class BON_SDrift : BON_State
{
    public override void Enter()
    {
        //Debug.LogWarning("Enterring the Drift");
        _player.GetComponent<BON_MovePR>().CurSpeed = 0;
        _player.AvatarState.IsDrifting = false;
    }

    public override void Exit()
    {
        _player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        _player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
    }

    public override void UpState()
    {
        if (_player.AvatarState.IsMovingByPlayer && _player.AvatarState.IsDrifting == false)
        {
            _player.AvatarState.ChangeState(BON_AvatarState.State.Moving);
        }
    }
}
