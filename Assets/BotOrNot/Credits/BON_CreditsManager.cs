using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BON_CreditsManager : MonoBehaviour
{
    /*
     * FIELDS
     */

    [SerializeField]
    GameObject _creditText;

    [SerializeField]
    AudioClip _music;

    [SerializeField] float _stopCinematique = 900;

    float _scrollSpeed;

    List<GameObject> _scrollingObjects = new List<GameObject>();

    bool _shouldScroll = true;

    /*
     * CLASS METHODS
     */


    /*
     * UNITY METHODS
     */

    void Start()
    {
        AudioSource _audioSource = GetComponent<AudioSource>();
        _audioSource.PlayOneShot(_music);

        _scrollSpeed = _creditText.GetComponent<RectTransform>().rect.size.y / _music.length;
        _scrollingObjects.Add(_creditText);
    }

    void Update()
    {
        if (_shouldScroll)
        {
            Vector3 moveVec = new Vector2( 0, _scrollSpeed * Time.deltaTime);

            foreach (GameObject obj in _scrollingObjects) {
                obj.transform.position += moveVec;
                if (obj.transform.position.y >= _stopCinematique)
                {
                    _shouldScroll = false;
                }
            }
        }
    }
}
