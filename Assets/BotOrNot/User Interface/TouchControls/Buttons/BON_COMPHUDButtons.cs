using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BON_COMPHUDButtons : BON_TouchComps
{
    /*
    * FIELDS
    */

    private List<Transform> _children = new List<Transform>();





    /*
    * METHODS 
    */

    override public void TouchStart(Touch touch, Vector2 initialTouchPos)
    {
        base.TouchStart(touch, initialTouchPos);
    }

    override public void TouchEnd()
    {
        base.TouchEnd();
    }

    override public void ComponentToggle()
    {
        base.ComponentToggle();

        foreach (Transform child in _children)
        {
            child.gameObject.SetActive(_isEnabled);
        }
    }

    public bool TryIsButtonThere(Vector2 touchPos)
    {
        for (int i = 0; i < _children.Count; i++)
        {
            if (_children[i].gameObject.activeSelf)
            {
                RectTransform rect = _children[i].GetComponent<RectTransform>();
                if (PRIVTryIsInRect(touchPos, rect))
                {
                    return true;
                }
            }
        }
        return false;
    }

    private bool PRIVTryIsInRect(Vector2 posToTry, RectTransform rectTr)
    {
        Rect rect = rectTr.rect;

        float leftSide = rectTr.anchoredPosition.x - rect.width / 2 + (Screen.width / 2);
        float rightSide = rectTr.anchoredPosition.x + rect.width / 2 + (Screen.width / 2);
        float topSide = rectTr.anchoredPosition.y + rect.height + (Screen.height / 2);
        float bottomSide = rectTr.anchoredPosition.y + (Screen.height / 2);

        if (posToTry.x <= rightSide &&
            posToTry.x >= leftSide &&
            posToTry.y <= topSide &&
            posToTry.y >= bottomSide)
        {
            return true;
        }

        return false;
    }





    /*
     *  UNITY METHODS
     */

    override protected void Start()
    {
        base.Start();

        foreach (Transform child in transform)
        {
            _children.Add(child);
            child.gameObject.SetActive(IsEnabled);
        }
    }

    override protected void Update()
    {
        base.Update();
    }
}
