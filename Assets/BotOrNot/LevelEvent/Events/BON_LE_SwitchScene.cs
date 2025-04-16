using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BON_LE_SwitchScene : BON_LevelEvent
{

    [SerializeField] BON_GameManager.Scenes _targetScene;

    public override void Happen()
    {
        BON_GameManager.Instance().ChangeScene(_targetScene);
    }
}
