using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class BON_CinematicManager : MonoBehaviour
{
    [SerializeField] VideoClip _cinematic1;
    [SerializeField] VideoClip _cinematic2;
    [SerializeField] GameObject _cinematic1GameObject;
    [SerializeField] GameObject _cinematic2GameObject;
    private double _cinematic1Time;
    private double _cinematic2Time;

    // Start is called before the first frame update
    void Awake()
    {
        _cinematic1Time = _cinematic1.length;
        _cinematic2Time = _cinematic2.length;
        StartCoroutine(StartCinematique2Coroutine());
    }

    IEnumerator StartCinematique2Coroutine()
    {
        yield return new WaitForSeconds((float)_cinematic1Time + 0.01f);
        _cinematic2GameObject.SetActive(true);
        StartCoroutine(StartLevel1Coroutine());
        _cinematic1GameObject.SetActive(false);
    }

    IEnumerator StartLevel1Coroutine()
    {
        yield return new WaitForSeconds((float)_cinematic2Time + 0.01f);
        SceneManager.LoadScene("Level1", LoadSceneMode.Single);
    }
}
