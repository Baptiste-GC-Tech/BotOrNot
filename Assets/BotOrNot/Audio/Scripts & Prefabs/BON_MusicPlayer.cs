using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class BON_MusicPlayer : MonoBehaviour
{
    /*
     * FIELDS
     */
    
    [SerializeField]
    List<AudioClip> _musics;

    AudioSource _audioSource;

    [SerializeField]
    bool _playOnStart;

    /*
     * CLASS METHODS
     */

    public void SetAudioId(int id)
    {
        _audioSource.resource = _musics[id];
    }

    public void PlayAudioId(int id)
    {
        if (_audioSource.isPlaying) 
        {
            _audioSource.Stop();
        }

        SetAudioId(id);
        _audioSource.Play();
    }

    public void PauseToggle()
    {
        if (_audioSource.isPlaying)
        {
            _audioSource.Pause();
        }

    }

    /*
     * UNITY METHODS
     */

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        if (_playOnStart)
        {
            PlayAudioId(0);
        }
        else
        {
            SetAudioId(0);
        }
    }
}
