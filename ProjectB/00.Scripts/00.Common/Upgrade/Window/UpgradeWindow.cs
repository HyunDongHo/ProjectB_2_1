using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeWindow : MonoBehaviour, IOpenClose
{
    public UpgradeWindowView view;

    public ButtonToggle buttonToggle;
    public GameObject[] toggleWindows;

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
        StageManager.instance.playerControl.utility.belongings.playerWallet.OnCoreSet += HandleOnCoreSet;
        buttonToggle.OnButtonClicked += HandleOnButtonClicked;

    }

    private void RemoveEvent()
    {
        StageManager.instance.playerControl.utility.belongings.playerWallet.OnCoreSet -= HandleOnCoreSet;
        buttonToggle.OnButtonClicked -= HandleOnButtonClicked;

    }

    private void HandleOnCoreSet(int core)
    {
        view.SetCoreAmount(core);
    }

    private void HandleOnButtonClicked(int buttonType)
    {
        for (int i = 0; i < toggleWindows.Length; i++)
            toggleWindows[i].SetActive(buttonType == i);
    }

    public void Close(bool isAnimation)
    {
        view.OpenCloseUpgradeWindow(isOpen: false, isAnimation: isAnimation);
    }

    public void Open(bool isAnimation)
    {
        view.OpenCloseUpgradeWindow(isOpen: true, isAnimation: isAnimation);
    }
}
