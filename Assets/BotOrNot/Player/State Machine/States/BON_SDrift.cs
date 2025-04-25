using UnityEngine;

public class BON_SDrift : BON_State
{
    public override void Enter()
    {
        //Debug.LogWarning("Enterring the Drift");
        _player.AvatarState.IsDrifting = true;
    }

    public override void Exit()
    {
        _player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        _player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
        _player.AvatarState.IsDrifting = false; //au cas ou
    }

    public override void UpState()
    {
        if (_player.AvatarState.IsMovingByPlayer && !_player.AvatarState.IsDrifting)
        {
            _player.AvatarState.ChangeState(BON_AvatarState.State.Moving);
        }
        else if (!_player.AvatarState.IsMovingByPlayer)
        {
            BON_MovePR _movePR = _player.GetComponent<BON_MovePR>();
            if (_movePR == null)
            {
                Debug.LogError("_movePR introuvable");
            }
            if (_movePR.CurSpeed <= 0.1f)
            {
                _player.AvatarState.ChangeState(BON_AvatarState.State.Idle);
            }
        }
    }
}
