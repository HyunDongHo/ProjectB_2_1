using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnnounceSetting : MonoBehaviour
{
    public ButtonOnOff push;

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
        push.OnStateChanged += HandlePushOnStateChanged;
    }

    private void RemoveEvent()
    {
        push.OnStateChanged -= HandlePushOnStateChanged;
    }

    private void HandlePushOnStateChanged(bool isOn)
    {
        if(isOn)
            BackEndFunctions.instance.AddPush();
        else
            BackEndFunctions.instance.RemovePush();
    }

    public void Init()
    {
        push.SetState(false);
    }
}
