using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyReward : MonoBehaviour
{
    public List<RewardDataSetting> rewardDataSettings = new List<RewardDataSetting>();

    private void OnEnable()
    {
        foreach (var rewardDataSetting in rewardDataSettings)
            rewardDataSetting.Init();
    }
}
