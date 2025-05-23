using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class BON_Menus : MonoBehaviour
{
    /*
    * FIELDS 
    */

    [SerializeField] AudioMixer AudioMixer;

    [SerializeField] GameObject InitialCanva;
    [SerializeField] GameObject InitialPanel;
    [SerializeField] GameObject InitialButton;

    [SerializeField] GameObject OtherCanva;

    private GameObject _activeCanva;
    private GameObject _activePanel;
    private GameObject _activeButton;

    [SerializeField] GameObject InitialCBButton;

    private GameObject _activeCBButton;

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
        SceneManager.LoadScene("Cinematic_1", LoadSceneMode.Single);
        SceneManager.UnloadSceneAsync("MainMenu");
    }

    public void MMContinue()
    {
        SceneManager.LoadScene("Level1Implem", LoadSceneMode.Single);
        SceneManager.UnloadSceneAsync("MainMenu");
    }

    public void MMOpenCredits()
    {
        SceneManager.LoadScene("Credits", LoadSceneMode.Single);
        SceneManager.UnloadSceneAsync("MainMenu");
    }

    public void SMSwitchPanel(GameObject panel)
    {
        _activePanel.SetActive(false);
        _activePanel = panel;
        _activePanel.SetActive(true);

        if (_activeButton != null)
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

    public void SMSetSliderVolume(Slider slider) { AudioMixer.SetFloat(slider.name, Mathf.Log10(slider.value) * 20); }

    public void SMSwitchCBMode(GameObject check)
    {
        if (_activeCBButton == null)
            _activeCBButton = InitialCBButton;

        check.SetActive(true);
        if (check != _activeCBButton)
            _activeCBButton.SetActive(false);

        _activeCBButton = check;
    }

    public void PMPauseStart(GameObject panel)
    {
        _activePanel = panel;
        _activePanel.SetActive(true);

        var cMana = OtherCanva.GetComponent<BON_ControlsManager>();
        cMana.IsTouchEnabled = false;
        cMana.CJoystick.ComponentToggle();
        cMana.CPButtons.ComponentToggle();
        cMana.CHUDButtons.ComponentToggle();

        BON_GameManager.Instance().PauseGame();
        PRIVSMUpdateSliders();
    }

    public void PMQuitGame()
    {
        BON_GameManager.Instance().UnpauseGame();
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    public void PMPauseEnd()
    {
        _activePanel.SetActive(false);
        _activePanel = null;

        var cMana = OtherCanva.GetComponent<BON_ControlsManager>();
        cMana.IsTouchEnabled = true;
        cMana.CJoystick.ComponentToggle();
        cMana.CPButtons.ComponentToggle();
        cMana.CHUDButtons.ComponentToggle();

        BON_GameManager.Instance().UnpauseGame();
    }

    public void PMResetScene()
    {
        BON_GameManager.Instance().UnpauseGame();
        SceneManager.LoadScene("Level1Implem", LoadSceneMode.Single);
    }

    private void PRIVSMUpdateSliders()
    {
        foreach (Transform child in transform)
        {
            var children = child.GetComponentsInChildren<Slider>();

            for (int i = 0; i < children.Length; i++)
            {
                AudioMixer.GetFloat(children[i].name, out float value);
                children[i].value = Mathf.Pow(value, 10);
            }
        }   
    }

    public void CRQuit()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        SceneManager.UnloadSceneAsync("Level1Implem");
    }




    /*
    * UNITY METHODS 
    */





    void Start()
    {
        if (InitialCanva != null)
            _activeCanva = InitialCanva;
        if (InitialPanel != null)
            _activePanel = InitialPanel;
        if (InitialButton != null)
            _activeButton = InitialButton;
    }
}
