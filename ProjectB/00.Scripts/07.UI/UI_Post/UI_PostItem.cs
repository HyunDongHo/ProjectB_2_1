using BackendData.Post;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_PostItem : MonoBehaviour
{
    [SerializeField]
    UI_Reward_item rewardItemObj;

    [SerializeField]
    TextMeshProUGUI senderText, contentText;

    [SerializeField]
    HorizontalLayoutGroup horizontalLayoutGroup;

    [SerializeField]
    Button getButton;

    public Action GetPostAction = null;


    List<UI_Reward_item> rewardItemLists = new List<UI_Reward_item>();
    string _postID;

    public void Awake()
    {
        getButton.onClick.RemoveAllListeners();
        getButton.onClick.AddListener(GetPost);
    }

    public void SetPostID(string postID)
    {
        _postID = postID;
        SetRewardItemObj();
    }

    void OffItems()
    {
        for(int i=0; i<rewardItemLists.Count; i++)
        {
            rewardItemLists[i].gameObject.SetActive(false);
        }
    }

    public void SetRewardItemObj()
    {
        OffItems();

        foreach (var list in StaticManager.Backend.Post.Dictionary)
        {
            if(list.Key == _postID)
            {
                switch(list.Value.PostType)
                {
                    case BackEnd.PostType.Admin:
                        senderText.text = "관리자";
                        break;
                    case BackEnd.PostType.Rank:
                        senderText.text = "랭킹 보상";
                        break;
                }

                contentText.text = list.Value.content;

                int itemCount = list.Value.items.Count;
                int nowItemCount = 0;

                foreach(var item in list.Value.items)
                {
                    if(rewardItemLists.Count > nowItemCount)
                    {
                        rewardItemLists[nowItemCount].SetChartItemId(item.itemID);
                        rewardItemLists[nowItemCount].SetChartCount(item.itemCount);
                        rewardItemLists[nowItemCount].UpdateView();
                        rewardItemLists[nowItemCount].gameObject.SetActive(true);
                    }
                    else
                    {
                        UI_Reward_item uI_Reward_Item = GameObject.Instantiate(rewardItemObj, horizontalLayoutGroup.transform);
                        uI_Reward_Item.transform.localScale = Vector3.one;
                        uI_Reward_Item.SetChartItemId(item.itemID);
                        uI_Reward_Item.SetChartCount(item.itemCount);
                        rewardItemLists.Add(uI_Reward_Item);
                    }                    

                    nowItemCount++;
                }


                for (int i = nowItemCount; i < itemCount; i++)
                {
                    rewardItemLists[i].gameObject.SetActive(false);
                }
            }
        }
    }

    public void GetPost()
    {
        foreach (var list in StaticManager.Backend.Post.Dictionary)
        {
            if (list.Key != _postID)
                continue;

            list.Value.ReceiveItem((isSuccess) =>
            {
                List<PostChartItem> item = list.Value.items;

                if (isSuccess)
                {
                    // 아이템 보상 및 아이템 팝업
                    Debug.Log($"우편 보상 획득 : {list.Key} : {list.Value}\n");
                    List<int> itemIds = new();
                    List<double> itemCounts = new();
                    for(int i = 0; i < item.Count; i++)
                    {
                        itemIds.Add(item[i].itemID);
                        itemCounts.Add(item[i].itemCount);
                    }
                    RewardManager.instance.ShowRewardWindow(itemIds, itemCounts, true);  

                    // PostPopup 새로고침
                    GetPostAction?.Invoke();
                }
                else
                {

                }
            });
        }
    }
}
