using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionWindow : MonoBehaviour, IOpenClose
{
    public OptionWindowView view;

    public ButtonToggle buttonToggle;
    public GameObject[] optionWindowParents;

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
        buttonToggle.OnButtonClicked += HandleOnButtonClick;
    }

    private void RemoveEvent()
    {
        buttonToggle.OnButtonClicked -= HandleOnButtonClick;
    }

    private void HandleOnButtonClick(int buttonType)
    {
        for (int i = 0; i < optionWindowParents.Length; i++)
        {
            optionWindowParents[i].SetActive(buttonType == i);
        }
    }

    public void Open(bool isAnimation)
    {
        view.OpenCloseBossWindow(true, isAnimation);
    }

    public void Close(bool isAnimation)
    {
        view.OpenCloseBossWindow(false, isAnimation);
    }
}
