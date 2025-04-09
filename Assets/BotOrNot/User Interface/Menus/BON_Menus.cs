using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BON_Menus : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CloseMenu(GameObject PanelToDisable)
    {
        PanelToDisable.SetActive(false);
    }

    public void OpenMenu(GameObject PanelToEnable)
    {
        PanelToEnable.SetActive(true);
    }
}
