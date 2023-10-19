using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;

[System.Serializable]
public class RewardDataSetting
{
    private RewardData rewardData;

  //  public ItemData rewardItemData;

    public ObscuredInt minAmount = 0;
    public ObscuredInt maxAmount = 0;

    public void Init()
    {
        rewardData = ScriptableObject.CreateInstance<RewardData>();
      //  rewardData.Init(rewardItemData, minAmount, maxAmount);
    }

    public RewardData GetRewardData()
    {
        return rewardData;
    }
}

[CreateAssetMenu(fileName = "New Reward Data", menuName = "Custom/Reward/RewardData")]
public class RewardData : ScriptableObject
{
   // public ItemData rewardItemData;

    public ObscuredInt minAmount = 0;
    public ObscuredInt maxAmount = 0;

    private ObscuredInt getRewardAmount;

    //public void Init(ItemData itemData, int min, int max)
    //{
    //    this.rewardItemData = itemData;

    //    minAmount = min;
    //    maxAmount = max;

    //    getRewardAmount = Random.Range(minAmount, maxAmount);
    //}

    //public bool IsNullReward()
    //{
    //    return rewardItemData == null;
    //}

    //public Sprite GetRewardSprite()
    //{
    //    return rewardItemData.GetSlotSprite();
    //}

    public int GetRewardAmount()
    {
        return getRewardAmount;
    }

    //public int ReceiveReward(Control control)
    //{
    //    int rewardAmount = GetRewardAmount();
    //    switch (control)
    //    {
    //        case PlayerControl playerControl:
    //            RewardType_Player rewardType_Player = new RewardType_Player();
    //            rewardType_Player.ReceiveReward(control, rewardItemData, rewardAmount);
    //            break;
    //    }

    //    return rewardAmount;
    //}
}
