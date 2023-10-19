using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Scheduler;

public class GetRewardItemUI : MonoBehaviour
{
    public const float REWARD_SHOW_TIME = 2.0f;

    [System.Serializable]
    public class GetRewardItemSetting
    {
        public Text reward;

        public TimerBuffer life = new TimerBuffer(REWARD_SHOW_TIME);
    }
    public GetRewardItemSetting[] getRewardItemSettings;

    public void ShowRewardItemText(RewardData reward)
    {
        for (int i = getRewardItemSettings.Length - 1; i >= 0; i--)
        {
            if (getRewardItemSettings[i].life.isRunningTimer && i != getRewardItemSettings.Length - 1)
            {
                SetRewardSetting(index: i + 1, rewardText: getRewardItemSettings[i].reward.text, lifeTimer: getRewardItemSettings[i].life.timer);
            }
        }

    }

    private void SetRewardSetting(int index, string rewardText, float lifeTimer = 0)
    {
        Timer.instance.TimerStop(getRewardItemSettings[index].life, isReset: false);

        getRewardItemSettings[index].reward.text = rewardText;
        getRewardItemSettings[index].life.timer = lifeTimer;

        Timer.instance.TimerStart(getRewardItemSettings[index].life,
            OnFrame: () =>
            {
                getRewardItemSettings[index].reward.color = new Color(getRewardItemSettings[index].reward.color.r,
                                                                      getRewardItemSettings[index].reward.color.g,
                                                                      getRewardItemSettings[index].reward.color.b,
                                                                      1 - getRewardItemSettings[index].life.timer / getRewardItemSettings[index].life.time);
            },
            OnComplete: () =>
            {
                getRewardItemSettings[index].reward.text = string.Empty;
            }, isReset: false);
    }
}
