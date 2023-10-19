using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupQuestReward : Popup<PopupQuestReward>
{
    public Button closeButton;

    [System.Serializable]
    public class RewardUI
    {
        public Image rewardImage;
        public Text rewardAmount;
    }
    public RewardUI[] rewardUI;
    private int currentIndex = 0;
   
    public void SetRewardUI(Sprite sprite, int amount)
    {
        rewardUI[currentIndex].rewardImage.sprite = sprite;
        rewardUI[currentIndex].rewardAmount.text = amount.ToString();

        currentIndex++;
    }

    public override PopupQuestReward GetPopup()
    {
        return this;
    }
}
