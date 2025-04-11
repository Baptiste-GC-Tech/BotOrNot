using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BON_Menus : MonoBehaviour
{
    /*
    * FIELDS 
    */

    [SerializeField] AudioMixer AudioMixer;

    [SerializeField] GameObject InitialCanva;
    [SerializeField] GameObject InitialPanel;
    [SerializeField] GameObject InitialButton;

    private GameObject _activeCanva;
    private GameObject _activePanel;
    private GameObject _activeButton;

    private bool _isCBActive = false;

    /*
    * METHODS 
    */



    public void MMSwitchCanva(GameObject canva)
    {
        _activeCanva.SetActive(false);
        _activeCanva = canva;
        _activeCanva.SetActive(true);
    }

    public void MMNewGame()
    {
        SceneManager.LoadScene("UI_Blockout", LoadSceneMode.Single);
    }

    public void MMContinue()
    {
        SceneManager.LoadScene("FullBlockout", LoadSceneMode.Single);
    }

    public void MMOpenCredits()
    {
        SceneManager.LoadScene("Credits", LoadSceneMode.Single);
    }

    public void SMSwitchPanel(GameObject panel)
    {
        _activePanel.SetActive(false);
        _activePanel = panel;
        _activePanel.SetActive(true);

        PRIVSMSwitchButton(panel.transform.parent.gameObject);
    }

    private void PRIVSMSwitchButton(GameObject button)
    {
        var newcolor = _activeButton.GetComponent<Image>().color;
        newcolor.a = 0;
        _activeButton.GetComponent<Image>().color = newcolor;

        _activeButton = button;

        newcolor = _activeButton.GetComponent<Image>().color;
        newcolor.a = 130.0f/255.0f;
        _activeButton.GetComponent<Image>().color = newcolor;
    }

    public void SMSetMusicVolume(Slider slider) { AudioMixer.SetFloat("MusicVolume", Mathf.Log10(slider.value) * 20); }
    public void SMSetSFXVolume(Slider slider) { AudioMixer.SetFloat("SFXVolume", Mathf.Log10(slider.value) * 20); }

    public void SMSwitchCBMode(GameObject check)
    {
        switch (_isCBActive)
        {
            case true:
                _isCBActive = false; break;
            case false:
                _isCBActive = true; break;
        }

        check.SetActive(_isCBActive);
    }




    /*
    * UNITY METHODS 
    */





    void Start()
    {
        _activeCanva = InitialCanva;
        _activePanel = InitialPanel;
        _activeButton = InitialButton;
    }
}
