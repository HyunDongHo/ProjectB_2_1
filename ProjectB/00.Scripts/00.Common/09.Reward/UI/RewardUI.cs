using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardUI : MonoBehaviour
{
    public RectTransform rewardUISlotParent;
    public ScrollRect rewardUISlotScrollRect;

    public GameObject rewardUISlotPrefab;

    public int xSpace = 20;

    private List<RewardUISlot> rewardUISlots = new List<RewardUISlot>();

    public void SetReward(Sprite sprite, int amount)
    {
        RewardUISlot rewardUISlot = Instantiate(rewardUISlotPrefab, rewardUISlotParent).GetComponent<RewardUISlot>();

        rewardUISlot.rewardImage.sprite = sprite;
        rewardUISlot.rewardAmount.text = amount.ToString();

        rewardUISlots.Add(rewardUISlot);

        SetRewardUISlotParentSizeDelta();
    }

    public void SetRewardUISlotParentSizeDelta()
    {
        rewardUISlotParent.sizeDelta = new Vector2(rewardUISlotPrefab.GetComponent<RectTransform>().sizeDelta.x * rewardUISlots.Count + xSpace , rewardUISlotParent.sizeDelta.y);
    }
}
