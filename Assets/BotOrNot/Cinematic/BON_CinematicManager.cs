using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class BON_CinematicManager : MonoBehaviour
{
    [SerializeField] VideoClip _cinematic;
    [SerializeField] string _nextSceneName;
    private double _cinematicTime;


    // Start is called before the first frame update
    void Awake()
    {
        _cinematicTime = _cinematic.length;
        StartCoroutine(ChangeSceneCoroutine());
    }

    IEnumerator ChangeSceneCoroutine()
    {
        yield return new WaitForSeconds((float)_cinematicTime + 0.01f);
        SceneManager.LoadScene(_nextSceneName, LoadSceneMode.Single);
    }
}
