using BackEnd;
using BackendData.Post;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_PostPopup : UIManagers
{
    [SerializeField]
    UI_PostItem postItemObj;

    [SerializeField]
    VerticalLayoutGroup verticalLayoutGroup;

    [SerializeField]
    Button getAllButton;

    [SerializeField]
    TextMeshProUGUI _noPostAlertText;

    [SerializeField]
    GameObject _parent;

    bool _isEndGetPost = false;

    // Start is called before the first frame update
    
    Dictionary<string ,BackendData.Post.PostItem> postItems = new Dictionary<string, BackendData.Post.PostItem>();
    List<UI_PostItem> uiPostItems = new List<UI_PostItem>();

    public void Awake()
    {
        getAllButton.onClick.RemoveAllListeners();
        getAllButton.onClick.AddListener(GetAllPost);
    }

    void OffPostItems()
    {
        for (int i = 0; i < uiPostItems.Count; i++)
        {
            uiPostItems[i].gameObject.SetActive(false); 
        }

    }

    void SetPostItme()
    {
        OffPostItems();
        postItems.Clear();

        int nowPostIndex = 0;

        foreach (var list in StaticManager.Backend.Post.Dictionary)
        {
            // indate가 중복일 경우에는 패스
            if (postItems.ContainsKey(list.Value.inDate))
            {
                continue;
            }

            postItems.Add(list.Value.inDate, list.Value);

            if(uiPostItems.Count > nowPostIndex)
            {
                uiPostItems[nowPostIndex].SetPostID(list.Key);
                uiPostItems[nowPostIndex].gameObject.SetActive(true);
            }
            else
            {
                UI_PostItem uI_PostItem = GameObject.Instantiate(postItemObj, verticalLayoutGroup.transform);
                uI_PostItem.transform.localScale = Vector3.one;
                uI_PostItem.SetPostID(list.Key);
                uI_PostItem.GetPostAction = () => SetPostItme();
                uiPostItems.Add(uI_PostItem);
            }

            nowPostIndex++;
        }

        for(int i= nowPostIndex; i< uiPostItems.Count; i++)
        {
            uiPostItems[i].gameObject.SetActive(false);
        }
    }

    Dictionary<int, double> rewardItems = new Dictionary<int, double>();

    public void GetAllPost()
    {
        rewardItems.Clear();

        // Loading 창 출력
        {
            _isEndGetPost = false;
            StartCoroutine(CoWaitForGetPost());
        }

        int postCount = StaticManager.Backend.Post.Dictionary.Count;
        int nowGetPostCount = 0;

        foreach (var list in StaticManager.Backend.Post.Dictionary)
        {
            list.Value.ReceiveItem((isSuccess) =>
            {
                List<PostChartItem> item = list.Value.items;

                if (isSuccess) 
                {
                    for(int i=0; i < item.Count; i++)
                    {
                        if (rewardItems.ContainsKey(item[i].itemID))
                        {
                            rewardItems[item[i].itemID] += item[i].itemCount;
                        }
                        else
                        {
                            rewardItems.Add(item[i].itemID, item[i].itemCount); 
                        }
                    }

                    nowGetPostCount += 1;

                    //아이템 보상 처리
                    if (nowGetPostCount == postCount)
                    {
                        _isEndGetPost = true;
                    }
                }
                else
                {
                    _isEndGetPost = true;
                }
            });
        }
    }    

    IEnumerator CoWaitForGetPost()
    {
        while(_isEndGetPost == false)
        {
            yield return null;
        }

        // Loading 창 해제


        //보상 목록 띄우기
        List<int> itemIds = new();
        List<double> itemCounts = new();
        foreach (var list in rewardItems)
        {
            itemIds.Add(list.Key);
            itemCounts.Add(list.Value);
            Debug.Log($"우편 보상 획득 : {list.Key} : {list.Value}\n");
        }
        RewardManager.instance.ShowRewardWindow(itemIds, itemCounts, true);

        SetPostItme();
    }
    

    public void Open()
    {

        if (StaticManager.Backend.Post.Dictionary.Count <= 0)
        {
            _noPostAlertText.gameObject.SetActive(true);
            getAllButton.gameObject.SetActive(false);
        }
        else
        {
            _noPostAlertText.gameObject.SetActive(false);
            SetPostItme();
            getAllButton.gameObject.SetActive(true);
        }

        _parent.SetActive(true);
    }

    public void Close()
    {
        _parent.SetActive(false);
    }


}
