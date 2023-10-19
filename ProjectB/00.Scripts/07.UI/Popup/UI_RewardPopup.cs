using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_RewardPopup : CanvasManager
{
    [SerializeField]
    RectTransform RewardPanel, ScrollView, AchiveButton, BackDim;

    [SerializeField]
    CanvasGroup canvasGroup, whiteImage;

    [SerializeField]
    float startSize, endSize;

    [SerializeField]
    UI_Reward_item uiRewardItemObj;

    [SerializeField]
    HorizontalLayoutGroup horizontalLayout;    

    [SerializeField]
    List<UI_Reward_item> rewardItems;

    [SerializeField] private float duration = 0.3f;
    [SerializeField] private Ease ease = Ease.OutQuad;
    [SerializeField] private Ease whiteEase = Ease.OutQuad;

    private void Awake()
    {
        AchiveButton.gameObject.AddUIEvent(HandleAchiveButtonClicked);
        BackDim.gameObject.AddUIEvent(HandleAchiveButtonClicked);
        CloseWindow();
    }
    public void CloseWindow()
    {
        RewardPanel.gameObject.SetActive(false);
        DeleteAllRewardItem();
        RewardPanel.sizeDelta = new Vector2(RewardPanel.sizeDelta.x, startSize);
    }

    public void PopupWindow()
    {
        RewardPanel.gameObject.SetActive(true);

        // Test
        //SetItem(new List<int>() { 10002, 10002 }, new List<double>() { 300, 500 });

        SetWindow();
    }

    public void SetWindow()
    {
        canvasGroup.DOKill();
        RewardPanel.DOKill();
        whiteImage.DOKill();
        canvasGroup.alpha = 0;
        whiteImage.alpha = 1;

        RewardPanel.sizeDelta = new Vector2(RewardPanel.sizeDelta.x, startSize);
        ToggleItems(false);

        canvasGroup.DOFade(1f, duration).SetDelay(duration / 2f).SetEase(ease);
        whiteImage.DOFade(0f, duration).SetEase(whiteEase);
        RewardPanel.DOSizeDelta(new Vector2(RewardPanel.sizeDelta.x, endSize), duration).SetEase(ease).OnComplete(() => 
        {
            ToggleItems(true);
            ShowItems();
        });
    }

    private void ToggleItems(bool isOn)
    {
        ScrollView.gameObject.SetActive(isOn);
        AchiveButton.gameObject.SetActive(isOn);
    }

    private void ShowItems()
    {
        for (int i = 0; i < rewardItems.Count; ++i)
        {
            rewardItems[i].gameObject.SetActive(true);
           // yield return waitForSeconds;
        }
      //  StartCoroutine(CoShowItem());
    }

    WaitForSeconds waitForSeconds = new WaitForSeconds(0.1f);

    IEnumerator CoShowItem()
    {
        yield return waitForSeconds;

        for (int i=0; i < rewardItems.Count; ++i)
        {
            rewardItems[i].gameObject.SetActive(true);
            yield return waitForSeconds;
        }
    }

    private void DeleteAllRewardItem()
    {
        for(int i=0; i < rewardItems.Count; i++) 
        {
            Destroy(rewardItems[i].gameObject);
        }

        rewardItems.Clear();
    }

    public void SetItem(List<int> itemIds, List<double> itemCounts)
    {
        if (itemIds.Count != itemCounts.Count)
            return;

        for(int  i=0; i < itemIds.Count; ++i)
        {
            UI_Reward_item rewardItem = GameObject.Instantiate(uiRewardItemObj, horizontalLayout.transform);
            rewardItem.transform.localScale = Vector3.one;
            rewardItem.SetChartItemId(itemIds[i]);
            rewardItem.SetChartCount(itemCounts[i]);
            rewardItem.gameObject.SetActive(false);
            rewardItems.Add(rewardItem);
        }
    }
    void HandleAchiveButtonClicked(PointerEventData data)
    {
        Debug.Log("HandleAchiveButtonClicked");
        CloseWindow();

    }

}
