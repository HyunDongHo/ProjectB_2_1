using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonOnOff : MonoBehaviour
{
    public Action<bool> OnStateChanged;

    public ButtonOnOffView view;
    private bool isOn;

    private void Awake()
    {
        AddEvent();  
    }

    private void OnDestroy()
    {
        RemoveEvent();
    }

    private void AddEvent()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    private void RemoveEvent()
    {
        GetComponent<Button>().onClick.RemoveListener(OnClick);
    }

    private void OnClick()
    {
        SetState(!isOn);
    }

    public void SetState(bool isOn)
    {
        this.isOn = isOn;

        view.ButtonStateChanged(isOn);
        OnStateChanged?.Invoke(isOn);
    }
}
