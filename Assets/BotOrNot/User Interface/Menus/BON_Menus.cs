using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BON_Menus : MonoBehaviour
{
    /*
    * FIELDS 
    */

    [SerializeField] GameObject InitialCanva;
    [SerializeField] GameObject InitialPanel;
    [SerializeField] GameObject InitialButton;

    private GameObject _activeCanva;
    private GameObject _activePanel;
    private GameObject _activeButton;



    /*
    * METHODS 
    */



    public void MMSwitchCanva(GameObject Canva)
    {
        _activeCanva.SetActive(false);
        _activeCanva = Canva;
        _activeCanva.SetActive(true);
    }

    public void SMSwitchPanel(GameObject Panel)
    {
        _activePanel.SetActive(false);
        _activePanel = Panel;
        _activePanel.SetActive(true);

        PRIVSMSwitchButton(Panel.transform.parent.gameObject);
    }

    private void PRIVSMSwitchButton(GameObject Button)
    {
        var newcolor = _activeButton.GetComponent<Image>().color;
        Debug.Log(newcolor);
        newcolor.a = 0;
        _activeButton.GetComponent<Image>().color = newcolor;

        _activeButton = Button;

        newcolor = _activeButton.GetComponent<Image>().color;
        newcolor.a = 130.0f/255.0f;
        Debug.Log(newcolor);
        _activeButton.GetComponent<Image>().color = newcolor;
    }




    /*
    * UNITY METHODS 
    */




    // Start is called before the first frame update
    void Start()
    {
        _activeCanva = InitialCanva;
        _activePanel = InitialPanel;
        _activeButton = InitialButton;

        InitialCanva.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
