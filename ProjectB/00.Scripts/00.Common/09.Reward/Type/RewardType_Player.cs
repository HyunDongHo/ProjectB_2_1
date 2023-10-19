using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardType_Player : RewardType
{
    //public override void ReceiveReward(Control control, ItemData receiveItemData, int rewardAmount)
    //{
    //    PlayerControl playerControl = control as PlayerControl;

    //    switch (receiveItemData.itemName)
    //    {
    //        case "Gold":
    //            playerControl.utility.belongings.playerWallet.AddGold(rewardAmount);
    //            return;
    //        case "Fame_Coin":
    //            playerControl.utility.belongings.playerWallet.AddFameCoin(rewardAmount);
    //            break;
    //        case "Core":
    //            playerControl.utility.belongings.playerWallet.AddCore(rewardAmount);
    //            return;
    //        case "Exp":
    //            playerControl.GetStats<Stats>().exp.AddExp(rewardAmount);
    //            return;
    //    }

    //    playerControl.utility.belongings.GetInventory(receiveItemData).Add(receiveItemData, rewardAmount);
    //}
}
