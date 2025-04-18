using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BON_LEC_CurrentAvatar : BON_LevelEventCondition
{

    /*
     * FIELDS
     */

    enum TargetAvatar { NUT, DR, CONTROLLABLEMACHINE }
    [SerializeField] TargetAvatar _targetCurrentAvatar;
    [SerializeField] bool _isTargetControlled;

    /*
     * CLASS METHODS
     */

    public override bool Check()
    {
        switch (_targetCurrentAvatar) 
        {
            case TargetAvatar.CONTROLLABLEMACHINE:
                {
                    return (BON_GameManager.Instance().CurrentCharacterPlayed != 0 
                        && BON_GameManager.Instance().CurrentCharacterPlayed != 1) 
                        == _isTargetControlled;
                }
            default:
                {
                    return (_targetCurrentAvatar.Equals(_isTargetControlled));
                }
        }
    }
}
