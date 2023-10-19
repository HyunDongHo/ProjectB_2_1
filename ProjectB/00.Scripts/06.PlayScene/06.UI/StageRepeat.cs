using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageRepeat : MonoBehaviour
{
    public ButtonOnOff buttonOnOff;

    private void Awake()
    {
        AddEvent();

        buttonOnOff.SetState(UserDataManager.instance.GetStageRepeat());
    }

    private void OnDestroy()
    {
        RemoveEvent();
    }

    private void AddEvent()
    {
        buttonOnOff.OnStateChanged += HandleOnStateChanged;
    }

    private void RemoveEvent()
    {
        buttonOnOff.OnStateChanged -= HandleOnStateChanged;
    }

    private void HandleOnStateChanged(bool isOn)
    {
        (StageManager.instance as GamePlayManager).enemyManager.isStageRepeat = isOn;

        UserDataManager.instance.SetStageRepeat(isOn);
    }
}
