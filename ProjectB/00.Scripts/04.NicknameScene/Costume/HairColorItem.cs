using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HairColorItem : MonoBehaviour
{
    // Start is called before the first frame update
    public Button clickButton;
    public Image buttonColor;

    private void Awake()
    {
        AddEvent();
    }

    private void OnDestroy()
    {
        RemoveEvent();
    }

    void AddEvent()
    {
    }

    void RemoveEvent()
    {
        clickButton.onClick.RemoveAllListeners();
    }

    public void SetHairColor()
    {
    }
}
