using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardManager : Singleton<RewardManager>
{

    public void ShowRewardWindow(List<int> itemIds, List<double> itemCounts, bool isOpen)
    {
        UI_RewardPopup rewardPopup = StageManager.instance.canvasManager.GetUIManager<UIManager_RewardPopup>().gameObject.GetComponent<UI_RewardPopup>();
        if (rewardPopup == null)
            return;

        /* 보상 적용 */
        for(int i=0;i< itemIds.Count;i++)  
        {
            BackendData.Chart.Item.Item chartItem = StaticManager.Backend.Chart.Item.GetItem(itemIds[i]);
            if (chartItem == null)
            {
                Debug.Log($"아이템 ID : {itemIds[i]} 가 존재하지 않습니다. ");
                continue;
            }

            if(chartItem.ItemType == Define.ItemType.Equipment) // 무기 
            {
                BackendData.GameData.ItemData itemData = null;
                itemData = StaticManager.Backend.GameData.PlayerEquipment.WeaponList.Find(item => item.ItemID == chartItem.ItemID);
                if (itemData == null)
                {
                    StaticManager.Backend.GameData.PlayerEquipment.WeaponList.Add(new BackendData.GameData.ItemData() { ItemID = chartItem.ItemID, ItemLevel = 1, ItemIsEquip = false, ItemCount = (int)itemCounts[i]-1});
                    StageManager.instance.canvasManager.GetUIManager<UIManager_Inven>().RefreshInvenUI(chartItem.ItemID, (int)itemCounts[i]-1);
                }
                else
                {
                    itemData.ItemCount += (int)itemCounts[i];
                    StageManager.instance.canvasManager.GetUIManager<UIManager_Inven>().RefreshInvenUI(itemData.ItemID, itemData.ItemCount);

                }
            }
            else if(chartItem.ItemType == Define.ItemType.Goods) // 강화석
            {
                switch (itemIds[i])
                {
                    case 10001: // 골드
                        StaticManager.Backend.GameData.PlayerGameData.UpdateUserData((int)PlayerType.None, itemCounts[i]);
                        StageManager.instance.canvasManager.GetUIManager<UIManager_Common>().RefreshCommonUI();
                        break;
                    case 10002: // 다이아 
                        StaticManager.Backend.GameData.PlayerGameData.UpdateUserData_DDiamondCoin(itemCounts[i]);
                        StageManager.instance.canvasManager.GetUIManager<UIManager_Common>().RefreshCommonUI();
                        break;
                    case 10003: // 무기강화석
                        StaticManager.Backend.GameData.PlayerEquipment.UpdateEquipGem(itemCounts[i]);
                        StageManager.instance.canvasManager.GetUIManager<UIManager_Inven>().RefreshInvenUI(-1, 0);
                        break;
                    case 10004: // 주사위 
                        StaticManager.Backend.GameData.PlayerDice.UpdateDiceGem(itemCounts[i]);
                        StageManager.instance.canvasManager.GetUIManager<UIManager_Stat>().RefreshStatUI();
                        break;
                    case 10005: // 던전열쇠 
                        break;
                    case 10006: // 성장의 물약   
                        break;
                }
            }
            else
            {

            }
        }
    
        if (isOpen == true)
        {
            rewardPopup.SetItem(itemIds, itemCounts);
            rewardPopup.PopupWindow();
        }
    }
    public void CloseRewardWindow()
    {
        UI_RewardPopup rewardPopup = StageManager.instance.canvasManager.GetUIManager<UIManager_RewardPopup>().gameObject.GetComponent<UI_RewardPopup>();
        if (rewardPopup == null)
            return;

        rewardPopup.CloseWindow();
    }
}
