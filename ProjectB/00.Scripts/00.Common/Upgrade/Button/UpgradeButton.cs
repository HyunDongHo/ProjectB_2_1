using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;

public class UpgradeButton : MonoBehaviour
{
    private ObscuredInt upgradePrice;

    public UpgradeButtonView view;

    public UpgradeTarget upgradeType;

    private void Awake()
    {
        AddEvent();
    }

    private void Start()
    {
        SetUpgradeButton(UpgradeManager.instance.GetValue(upgradeType));
    }

    private void OnDestroy()
    {
        RemoveEvent();
    }

    private void AddEvent()
    {
        view.upgrade.onClick.AddListener(HandleOnUpgrade);
    }

    private void RemoveEvent()
    {
        view.upgrade.onClick.RemoveListener(HandleOnUpgrade);
    }

    private void HandleOnUpgrade()
    {
        PlayerWallet playerWallet = StageManager.instance.playerControl.utility.belongings.playerWallet;
        if (playerWallet.IsAvailiableRemoveCore(upgradePrice))
        {
            playerWallet.AddCore(-upgradePrice);

            SetUpgradeButton(UpgradeManager.instance.AddValue(upgradeType, 1));
        }
    }

    private void SetUpgradeButton(UpgradeData[] upgradeDatas)
    {
        string totalShowValue = string.Empty;

        foreach (var upgradeData in upgradeDatas)
        {
            if (upgradeData.targetValue == UpgradeManager.UPGRADE_CORE_CONSUM)
            {
                upgradePrice = (int)upgradeData.currentValue;
            }
            else
            {
                totalShowValue += $"{upgradeData.currentValue} -> {upgradeData.nextValue}\n";
            }
        }

        view.SetUpgradeButton(upgradePrice, totalShowValue);
    }
}
