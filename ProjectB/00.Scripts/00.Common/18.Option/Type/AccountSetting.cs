using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AccountSetting : MonoBehaviour
{
    public Button deleteAccount;

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
        deleteAccount.onClick.AddListener(HandleOnDeleteAccount);
    }

    private void RemoveEvent()
    {
        deleteAccount.onClick.RemoveListener(HandleOnDeleteAccount);
    }

    private void HandleOnDeleteAccount()
    {
        BackEndFunctions.instance.LogOut();
        BackEndFunctions.instance.DeleteAccount();

        SceneSettingManager.instance.LoadAccountScene(isFade: false);
    }
}
