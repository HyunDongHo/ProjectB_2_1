using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoCombo : MonoBehaviour
{
    public ButtonOnOff buttonOnOff;

    private void Awake()
    {
        AddEvent();

        buttonOnOff.SetState(UserDataManager.instance.GetAutoCombo());
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
        (StageManager.instance.playerControl as PlayerControl_GamePlay).SetAttackAuto(isOn);

        UserDataManager.instance.SetAutoAttack(isOn);
    }
}
