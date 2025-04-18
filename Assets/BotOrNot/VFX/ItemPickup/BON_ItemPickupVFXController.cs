using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class BON_ItemPickupVFXController : MonoBehaviour
{
    private VisualEffect _VFX;
    private BON_CCPlayer _player;

    void Start()
    {
        _VFX = GetComponent<VisualEffect>();
        _player = BON_GameManager.Instance().Player;
    }

    private void FixedUpdate()
    {
        _VFX.SetVector3("CharacterPosition_position", _player.transform.position);
    }
}
