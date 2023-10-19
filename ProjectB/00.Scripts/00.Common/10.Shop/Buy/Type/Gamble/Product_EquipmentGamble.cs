using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RNG;

public class Product_EquipmentGamble : Product
{
    //public int gambleItemCount = 0;

    //protected override void BuyProduct()
    //{
    //    ItemData[] randomItems = new ItemData[gambleItemCount];

    //    for (int i = 0; i < gambleItemCount; i++)
    //        randomItems[i] = GambleManager.instance.GambleItem((gambleData as EquipmentGambleData).GetGambleItemData());

    //    foreach (var item in randomItems)
    //        AddItemsToInventory(item);

    //    GambleManager.instance.LoadGamebleScene(SceneSettingManager.RANDOM_LOTTERY_PRODUCT,
    //        () =>
    //        {
    //            LoadedGambleScene(randomItems);
    //        });
    //}

    //protected void AddItemsToInventory(ItemData itemData)
    //{
    //    StageManager.instance.playerControl.utility.belongings.GetInventory(itemData).Add(itemData);
    //}

    //protected virtual void LoadedGambleScene(ItemData[] randomItems)
    //{
    //    (GambleManager.instance.GetRandomLotteryManager() as RandomLotteryManager_Product).product = this;

    //    GambleManager.instance.GetRandomLotteryManager().ShowRandomItem(randomItems,
    //                criteriaCount: (int)Mathf.Ceil(randomItems.Length / 2.0f),
    //                criteria: Criteria.Horizontal,
    //                anchor: TextAnchor.MiddleCenter);
    //}
    protected override void BuyProduct()
    {
        throw new System.NotImplementedException();
    }
}
