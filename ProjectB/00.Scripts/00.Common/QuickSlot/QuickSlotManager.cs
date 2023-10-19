//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class PlayerQuickSlotItemData
//{
//    public int slotNum;
//    public string itemName;
//    public bool isAuto;
//}

//public class QuickSlotManager : Singleton<QuickSlotManager>
//{
//    public Action<UpgradeData> OnSetUpgrade;

//    private Dictionary<UpgradeTarget, int> upgrades = new Dictionary<UpgradeTarget, int>();

//    public void InitUpgrade(Dictionary<UpgradeTarget, int> upgrades)
//    {
//        this.upgrades = upgrades;

//        for (int i = 0; i < (int)UpgradeTarget.Max; i++)
//        {
//            for (int j = 0; j < upgrades.Count; j++)
//                SetValue((UpgradeTarget)i, upgrades[(UpgradeTarget)i]);
//        }
//    }

//    public void ExecuteAfterSceneLoad()
//    {
//        for (int i = 0; i < (int)UpgradeTarget.Max; i++)
//        {
//            for (int j = 0; j < upgrades.Count; j++)
//                SetValue((UpgradeTarget)i, upgrades[(UpgradeTarget)i]);
//        }
//    }

//    public UpgradeData[] AddValue(UpgradeTarget target, int addValue)
//    {
//        return SetValue(target, upgrades[target] + addValue);
//    }

//    public UpgradeData[] SetValue(UpgradeTarget target, int level)
//    {
//        upgrades[target] = level;
//        UpgradeData[] upgradeDatas = GetValue(target);

//        for (int i = 0; i < upgradeDatas.Length; i++)
//        {
//            if(upgradeDatas[i].targetValue != UPGRADE_CORE_CONSUM)
//                OnSetUpgrade?.Invoke(upgradeDatas[i]);
//        }

//        UserDataManager.instance.UpdateUpgrade(target, level);

//        return upgradeDatas;
//    }

//    public UpgradeData[] GetValue(UpgradeTarget target)
//    {
//        if(!upgrades.ContainsKey(target))
//        {
//            Debug.LogError($"[UpgradeManager] {target} 가 Dictionary에 존재하지 않습니다.");
//            return null;
//        }

//        return BackEndServerManager.instance.GetUpgradeData(target, upgrades[target]);
//    }
//}
