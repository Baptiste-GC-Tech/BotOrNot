using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BON_LE_PlaySound : BON_LevelEvent
{
    /*
     * FIELDS
     */

    private BON_SoundPlayer _soundPlayer;
    [SerializeField] private bool _playRandom = true;

    [Tooltip("Useless if _playRandom is enabled")]
    [SerializeField] private int _playIndex;

    /*
     * CLASS METHODS
     */
    public override void Happen()
    {
        if (_playRandom)
        {
            _soundPlayer.PlayRandom();
        }
        else
        {
            _soundPlayer.PlayIndex(_playIndex);
        }
    }

    /*
     * UNITY METHODS
     */
    private void Awake()
    {
        _soundPlayer = GetComponent<BON_SoundPlayer>();
    }
}