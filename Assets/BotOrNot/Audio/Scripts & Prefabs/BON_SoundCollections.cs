using UnityEngine;

[CreateAssetMenu(fileName = "SoundCollection", menuName = "ScriptableObjects/SoundCollection", order = 1)]
public class BON_SoundCollection : ScriptableObject
{
    /*
     * FIELDS
     */
    [SerializeField] AudioClip[] _sounds;

    /*
     * CLASS METHODS
     */
    public AudioClip GetRandom()
    {
        return _sounds[Random.Range(0, _sounds.Length - 1)];
    }

    public AudioClip GetAt(int index)
    {
        return _sounds[index];
    }
}

