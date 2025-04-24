using UnityEngine;

[CreateAssetMenu(fileName = "SoundCollection", menuName = "ScriptableObjects/SoundCollection", order = 1)]
public class SoundCollection : ScriptableObject
{
    [SerializeField] AudioClip[] _Sounds;
}

