using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClothItem : MonoBehaviour
{
    // Start is called before the first frame update
    public Button clickButton;
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
    }

}
