using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CanvasManager_Boss : CanvasManager
{
    public void ToggleSpawnUI(bool isOn)
    {
        if (isOn == true)
            GetUIManager<UIManager_Spawn>().gameObject.SetActive(true);
        else
            GetUIManager<UIManager_Spawn>().gameObject.SetActive(false);
    }

    public void ToggleSpawnResultUI(bool isOn)
    {
        if (isOn == true)
            GetUIManager<UIManager_SpawnResult>().gameObject.SetActive(true);
        else
            GetUIManager<UIManager_SpawnResult>().gameObject.SetActive(false);
    }

    public void OpenPost()
    {
        GetUIManager<UI_PostPopup>().Open();
    }
}
