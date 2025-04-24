using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BON_SoundPlayer : MonoBehaviour
{
    /*
     * FIELDS
     */

    [SerializeField] BON_SoundCollection _sounds;
    [SerializeField] AudioSource _audioSource;

    /*
     * CLASS METHODS
     */

    public void PlayRandom()
    {
        _audioSource.PlayOneShot(_sounds.GetRandom());
    }

    public void PlayIndex(int index)
    {
        _audioSource.PlayOneShot(_sounds.GetAt(index));
    }
}
