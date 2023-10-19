using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSector : MonoBehaviour
{
    public Action<int> OnSectorClicked;
    public ButtonToggle buttonToggle;

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
        buttonToggle.OnButtonClicked += HandleOnClickSectorButton;
    }

    private void RemoveEvent()
    {
        buttonToggle.OnButtonClicked -= HandleOnClickSectorButton;
    }

    private void HandleOnClickSectorButton(int buttonType)
    {
        if (buttonType == -1) return;
        
        OnSectorClicked?.Invoke(buttonType);
    }
}
