using BackEnd;
using BackendData.Post;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_MissionPopup : UIManagers
{
    [SerializeField]
    UI_MissionItem missionItemObj;

    [SerializeField]
    Button getAllButton;

    [SerializeField]
    GameObject _parent;

    [Space]
    [SerializeField] GameObject DailyText;
    [SerializeField] GameObject RepeatText;
    [SerializeField] GameObject DailyClickImage;
    [SerializeField] GameObject RepeatClickImage;
    [SerializeField] GameObject DailyTotalMissionContents;
    [SerializeField] GameObject DailyMissionContents;
    [SerializeField] GameObject RepeatMissionContents;
    [SerializeField] GameObject DailyMissionContentsView;
    [SerializeField] GameObject RepeatMissionContentsView;

    List<UI_MissionItem> uiMissionItems = new List<UI_MissionItem>();
    private void Awake()
    {
        DailyClickImage.gameObject.SetActive(true);
        RepeatClickImage.gameObject.SetActive(false);

        DailyText.AddUIEvent(OnDailyButtonClicked);
        RepeatText.AddUIEvent(OnRepeatButtonClicked);
        getAllButton.gameObject.AddUIEvent(OnGetAllButtonClicked);
     
        //Debug.Log($"mission data count : {StaticManager.Backend.GameData.PlayerQuest.MissionList.Count}");

        MissionInit();
    }
    private void OnEnable()
    {
       // QuestsManager.instance.SetDailyMissionNowDateState();      
    }
    public void Open()
    {
        _parent.SetActive(true);
    }

    public void Close()
    {
        _parent.SetActive(false);
    }
    void MissionInit()
    {
        foreach (Transform child in DailyTotalMissionContents.transform)
            GameObject.Destroy(child.gameObject);

        foreach (Transform child in DailyMissionContents.transform)
            GameObject.Destroy(child.gameObject);

        foreach (Transform child in RepeatMissionContents.transform)
            GameObject.Destroy(child.gameObject);

        CreateUIMissionItems();
        FirstDailyMissionInit();
        FirstRepeatMissionInit();  
        if (DailyMissionContentsView == null || RepeatMissionContentsView == null)
            return;
        DailyMissionContentsView.SetActive(true);
        RepeatMissionContentsView.SetActive(false);  
    }
    void FirstDailyMissionInit()
    {
        Dictionary<int, BackendData.Chart.Mission.Item> MissionData = null;
        MissionData = StaticManager.Backend.Chart.Mission.GetChartMissionData(BackendData.Chart.Mission.MissionType.Daily);
        Dictionary<int, BackendData.Chart.Mission.Item> DailyTotalData = null;
        DailyTotalData = StaticManager.Backend.Chart.Mission.GetChartMissionData(BackendData.Chart.Mission.MissionType.DailyTotal);  

        /* Daily Total Content (고정)*/
        foreach(var data in DailyTotalData)  
        {
            GameObject dailyTotalItem = null;
            dailyTotalItem = Instantiate<GameObject>(missionItemObj.gameObject);
            dailyTotalItem.transform.SetParent(DailyTotalMissionContents.transform); // 부모 지정     
            dailyTotalItem.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);

            UI_MissionItem totalItemComponent = dailyTotalItem.GetComponent<UI_MissionItem>();
            totalItemComponent.SetChartContentName(data.Value.MissionContent);
            totalItemComponent.SetChartMissionType(data.Value.MissionType);
            totalItemComponent.SetChartRequestCount(MissionData.Count);
            int dailyMissionDoneCount = 0;
            for (int i = 1; i < StaticManager.Backend.Chart.Mission.Dictionary.Count + 1; i++)
            {
                BackendData.GameData.MissionData missionData = null;
                missionData = StaticManager.Backend.GameData.PlayerQuest.MissionList.Find(item => item.MissionID == i);
                if (missionData == null)
                    continue;
                if (missionData.MissionIsDone == true)
                    dailyMissionDoneCount += 1;
            }
            totalItemComponent.SetGetButtonState(StaticManager.Backend.GameData.PlayerQuest.DailyTotalMissionDone);
            totalItemComponent.SetAdsButtonState(StaticManager.Backend.GameData.PlayerQuest.DailyTotalMissionAdsIsDone);  
            totalItemComponent.SetNowStep(dailyMissionDoneCount);
            totalItemComponent.SetRewardItemObj(data.Value.RewardItem[0].RewardItemType, data.Value.RewardItem[0].Id, data.Value.RewardItem[0].Count);  
        }


        /* Daily Mission Init */
        foreach (var data in MissionData)
        {
            GameObject item = null;
            item = Instantiate<GameObject>(missionItemObj.gameObject);
            item.transform.SetParent(DailyMissionContents.transform); // 부모 지정     
            item.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);

            UI_MissionItem itemComponent = item.GetComponent<UI_MissionItem>();
            itemComponent.SetChartMissionId(data.Value.MissionID);
            itemComponent.SetChartContentName(data.Value.MissionContent);
            itemComponent.SetChartMissionType(data.Value.MissionType);
            itemComponent.SetChartRequestCount(data.Value.RequestCount);
            itemComponent.SetChartMissionContentType(data.Value.MissionContentType);
            itemComponent.SetRewardItemObj(data.Value.RewardItem[0].RewardItemType, data.Value.RewardItem[0].Id, data.Value.RewardItem[0].Count);
            //itemComponent.UpdateAllText();  
            //itemComponent.GetRewardAction = () =>
            //{
            //    Debug.Log("보상 받기 완료");
            //};

            BackendData.GameData.MissionData missionServerData = null;
            missionServerData = StaticManager.Backend.GameData.PlayerQuest.MissionList.Find(item => item.MissionID == data.Value.MissionID);
            if (missionServerData != null)
            {
                itemComponent.SetGetButtonState(missionServerData.MissionIsDone);
                itemComponent.SetAdsButtonState(missionServerData.MissionAdsIsDone);

                itemComponent.SetNowStep((float)missionServerData.MissionNowStep);

            }
            else
            {
                itemComponent.SetGetButtonState(false);
                itemComponent.SetNowStep(0);

            }
        }
    }
    void FirstRepeatMissionInit()
    {
        Dictionary<int, BackendData.Chart.Mission.Item> MissionData = null;
        MissionData = StaticManager.Backend.Chart.Mission.GetChartMissionData(BackendData.Chart.Mission.MissionType.Repeat);

        foreach (var data in MissionData)
        {
            GameObject item = null;
            item = Instantiate<GameObject>(missionItemObj.gameObject);
            item.transform.SetParent(RepeatMissionContents.transform); // 부모 지정     
            item.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);

            UI_MissionItem itemComponent = item.GetComponent<UI_MissionItem>();
            itemComponent.SetChartMissionId(data.Value.MissionID);
            itemComponent.SetChartContentName(data.Value.MissionContent);
            itemComponent.SetChartMissionType(data.Value.MissionType);
            itemComponent.SetChartRequestCount(data.Value.RequestCount);  
            itemComponent.SetChartMissionContentType(data.Value.MissionContentType);
            itemComponent.SetRewardItemObj(data.Value.RewardItem[0].RewardItemType, data.Value.RewardItem[0].Id, data.Value.RewardItem[0].Count);

            BackendData.GameData.MissionData missionServerData = null;
            missionServerData = StaticManager.Backend.GameData.PlayerQuest.MissionList.Find(item => item.MissionID == data.Value.MissionID);
            if (missionServerData != null)
            {
                itemComponent.SetNowStep((float)missionServerData.MissionNowStep);

            }
            else
            {
                itemComponent.SetNowStep(0);
            }
        }  
    }
    public List<UI_MissionItem> GetMissionList()
    {
        return uiMissionItems;
    }
    public void UpdateContentUI(BackendData.Chart.Mission.MissionType missionType , int missionId, float count)
    {
        if(missionType == BackendData.Chart.Mission.MissionType.DailyTotal) //Daily total Content 일때   
        {
            GameObject dailyTotalItem = DailyTotalMissionContents.transform.GetChild(0).gameObject;
            if (dailyTotalItem == null)
                return;

            UI_MissionItem dailyTotalItemComponent = dailyTotalItem.GetComponent<UI_MissionItem>();
            if (dailyTotalItemComponent == null)
                return;

            dailyTotalItemComponent.SetNowStep(count);  
            return;
        }

        if (DailyMissionContents == null || RepeatMissionContents == null)
            return;

        Dictionary<int, BackendData.Chart.Mission.Item> MissionData = null;
        MissionData = StaticManager.Backend.Chart.Mission.GetChartMissionData(missionType);  

        for (int i = 0; i < MissionData.Count; i++)
        {
            GameObject item = null;
            if (missionType == BackendData.Chart.Mission.MissionType.Daily)
            {
                item = DailyMissionContents.transform.GetChild(i).gameObject;
            }
            else if(missionType == BackendData.Chart.Mission.MissionType.Repeat)
            {
                item = RepeatMissionContents.transform.GetChild(i).gameObject;
            }
            else 
            {

            }
            if (item == null)
                continue;
            UI_MissionItem itemComponent = item.GetComponent<UI_MissionItem>();
            if (itemComponent == null)
                continue;
            if (itemComponent.GetMissionId() == missionId)
            {
                itemComponent.SetNowStep(count);  
                return;
            }
        }
    }
    void OnDailyButtonClicked(PointerEventData data) // Daily Button Click
    {
        Debug.Log("OnDailyButtonClicked");
        DailyClickImage.gameObject.SetActive(true);
        RepeatClickImage.gameObject.SetActive(false);

        /* Daily Total Mission */
        GameObject dailyTotalItem = null;
        dailyTotalItem = DailyTotalMissionContents.transform.GetChild(0).gameObject;
        if (dailyTotalItem == null)
            return;
        UI_MissionItem totalItemComponent = dailyTotalItem.GetComponent<UI_MissionItem>();
        int dailyMissionDoneCount = 0;
        for (int i = 1; i < StaticManager.Backend.Chart.Mission.Dictionary.Count + 1; i++)
        {
            BackendData.GameData.MissionData missionData = null;
            missionData = StaticManager.Backend.GameData.PlayerQuest.MissionList.Find(item => item.MissionID == i);
            if (missionData == null)
                continue;
            if (missionData.MissionIsDone == true)
                dailyMissionDoneCount += 1;
        }
        totalItemComponent.SetNowStep(dailyMissionDoneCount);

        /* Daily Mission */  
        SetUIMissionItems(BackendData.Chart.Mission.MissionType.Daily);

        if (DailyMissionContentsView == null || RepeatMissionContentsView == null)
            return;
        DailyMissionContentsView.SetActive(true);
        RepeatMissionContentsView.SetActive(false);

    }
    void OnRepeatButtonClicked(PointerEventData data) // Repeat Button Click
    {
        Debug.Log("OnRepeatButtonClicked");
        DailyClickImage.gameObject.SetActive(false);
        RepeatClickImage.gameObject.SetActive(true);

        /* Repeat Mission */
        SetUIMissionItems(BackendData.Chart.Mission.MissionType.Repeat);

        if (DailyMissionContentsView == null || RepeatMissionContentsView == null)
            return;

        DailyMissionContentsView.SetActive(false);
        RepeatMissionContentsView.SetActive(true);

    }
    public void SetUIMissionItems(BackendData.Chart.Mission.MissionType missionType)
    {
        int MissionIndex = 0;
        for (int i = 0; i < StaticManager.Backend.Chart.Mission.Dictionary.Count ; i++)
        {
            //if (uiMissionItems[i] == null)
            //    continue;
            BackendData.Chart.Mission.MissionType type = uiMissionItems[i].GetMissionType();
            if (type != missionType)
                continue;

            if (type == BackendData.Chart.Mission.MissionType.Daily)
            {
                GameObject item = null;
                UI_MissionItem itemComponent = null;
                item = DailyMissionContents.transform.GetChild(MissionIndex).gameObject;
                if (item == null)
                    continue;
                itemComponent = item.GetComponent<UI_MissionItem>();
                if (itemComponent == null)
                    continue;

                itemComponent.SetChartMissionId(uiMissionItems[i].GetMissionId());
                itemComponent.SetChartContentName(uiMissionItems[i].GetMissionContent());
                itemComponent.SetChartMissionType(uiMissionItems[i].GetMissionType());
                itemComponent.SetChartRequestCount(uiMissionItems[i].GetRequestCount());
                itemComponent.SetRewardItemObj(uiMissionItems[i].GetRewardItemType(), uiMissionItems[i].GetRewardItemId(), uiMissionItems[i].GetRewardItemCount());
                Debug.Log($"daily id : {uiMissionItems[i].GetMissionId()}");

                BackendData.GameData.MissionData missioServernData = null;
                missioServernData = StaticManager.Backend.GameData.PlayerQuest.MissionList.Find(item => item.MissionID == uiMissionItems[i].GetMissionId());
                if (missioServernData != null)
                {
                    itemComponent.SetGetButtonState(missioServernData.MissionIsDone);
                    itemComponent.SetAdsButtonState(missioServernData.MissionAdsIsDone);

                    itemComponent.SetNowStep((float)missioServernData.MissionNowStep);
                }
                else
                {
                    itemComponent.SetGetButtonState(false);
                    itemComponent.SetNowStep(0);
                }
                MissionIndex += 1;
            }
            else if (type == BackendData.Chart.Mission.MissionType.Repeat)
            {
                GameObject item = null;
                UI_MissionItem itemComponent = null;
                item = RepeatMissionContents.transform.GetChild(MissionIndex).gameObject;
                if (item == null)
                    continue;
                itemComponent = item.GetComponent<UI_MissionItem>();
                if (itemComponent == null)
                    continue;

                itemComponent.SetChartMissionId(uiMissionItems[i].GetMissionId());
                itemComponent.SetChartContentName(uiMissionItems[i].GetMissionContent());
                itemComponent.SetChartMissionType(uiMissionItems[i].GetMissionType());
                itemComponent.SetChartRequestCount(uiMissionItems[i].GetRequestCount());
                itemComponent.SetRewardItemObj(uiMissionItems[i].GetRewardItemType(), uiMissionItems[i].GetRewardItemId(), uiMissionItems[i].GetRewardItemCount());
                itemComponent.SetGetButtonState(false);
                Debug.Log($"repeat id : {uiMissionItems[i].GetMissionId()}");

                BackendData.GameData.MissionData missioServernData = null;
                missioServernData = StaticManager.Backend.GameData.PlayerQuest.MissionList.Find(item => item.MissionID == uiMissionItems[i].GetMissionId());
                if (missioServernData != null)
                {
                    itemComponent.SetNowStep((float)missioServernData.MissionNowStep);
                }
                else
                {
                    itemComponent.SetNowStep(0);  

                }

                itemComponent.InitNowStep();
                MissionIndex += 1;
            }
            else
            {

            }


        }
    }
    
    void CreateUIMissionItems()
    {  
        //Dictionary<int, BackendData.Chart.Mission.Item> MissionData = null;
        //MissionData = StaticManager.Backend.Chart.Mission.GetChartMissionData(BackendData.Chart.Mission.MissionType.Daily);

        for (int i = 1; i < StaticManager.Backend.Chart.Mission.Dictionary.Count + 1; i++)
        {

            UI_MissionItem itemComponent = new();
            BackendData.Chart.Mission.Item _missionChartItem = null;
            StaticManager.Backend.Chart.Mission.Dictionary.TryGetValue(i, out _missionChartItem);
            if (_missionChartItem == null)
                continue;
            itemComponent.SetChartMissionId(_missionChartItem.MissionID);
            itemComponent.SetChartContentName(_missionChartItem.MissionContent);
            itemComponent.SetChartMissionType(_missionChartItem.MissionType);
            itemComponent.SetChartRequestCount(_missionChartItem.RequestCount);
            itemComponent.SetChartMissionContentType(_missionChartItem.MissionContentType);
            itemComponent.SetRewardItemObj(_missionChartItem.RewardItem[0].RewardItemType, _missionChartItem.RewardItem[0].Id, _missionChartItem.RewardItem[0].Count);

            BackendData.GameData.MissionData missionData = null;
            missionData = StaticManager.Backend.GameData.PlayerQuest.MissionList.Find(item => item.MissionID == _missionChartItem.MissionID);
            if (missionData != null)
            {
                itemComponent.SetGetButtonState(missionData.MissionIsDone);
                itemComponent.SetAdsButtonState(missionData.MissionAdsIsDone);

                itemComponent.SetNowStep((float)missionData.MissionNowStep);
            }
            else
            {
                itemComponent.SetGetButtonState(false);
                itemComponent.SetNowStep(0);
            }

            uiMissionItems.Add(itemComponent);   
        }

    }

    void OnGetAllButtonClicked(PointerEventData data) // 모두 받기 보상 
    {
        Debug.Log("OnGetAllButtonClicked");

        GameObject MissionContents = null;
        Dictionary<int, double> FinalRewardItems = new();
        if(DailyMissionContentsView.activeSelf == true)
        {
            int dailyMissionDoneCount = 0;
            for (int i = 1; i < StaticManager.Backend.Chart.Mission.Dictionary.Count + 1; i++)
            {
                BackendData.GameData.MissionData missionData = null;
                missionData = StaticManager.Backend.GameData.PlayerQuest.MissionList.Find(item => item.MissionID == i);
                if (missionData == null)
                    continue;
                if (missionData.MissionIsDone == true)
                    dailyMissionDoneCount += 1;
            }

            if(dailyMissionDoneCount == StaticManager.Backend.Chart.Mission.GetChartMissionData(BackendData.Chart.Mission.MissionType.Daily).Count)
            {
                MissionContents = DailyTotalMissionContents;
            }
            else
            {
                MissionContents = DailyMissionContents;
            }
        }
        else if(RepeatMissionContentsView.activeSelf == true)
        {
            MissionContents = RepeatMissionContents;
        }
        else  
        {

        }
        for (int i=0; i< MissionContents.transform.childCount; i++)  
        {
            GameObject item = MissionContents.transform.GetChild(i).gameObject;
            if (item == null)
                continue;
            UI_MissionItem itemComponent = item.GetComponent<UI_MissionItem>();
            float fillAmount = itemComponent.GetNowMissionStep() / itemComponent.GetRequestCount();
            if (fillAmount < 1)
                continue;

            if(itemComponent.GetMissionType() == BackendData.Chart.Mission.MissionType.Daily)
            {
                if (itemComponent.GetGetButtonState() == true) // 일일보상 완료된거면 
                    continue;

                //rewardIds.Add(itemComponent.GetRewardItemId());
                //rewardCounts.Add(itemComponent.GetRewardItemCount());
                int id = itemComponent.GetRewardItemId();
                double count = itemComponent.GetRewardItemCount();
                if (FinalRewardItems.ContainsKey(id))
                {
                    FinalRewardItems[id] += count;
                }
                else
                {
                    FinalRewardItems.Add(id, count);
                }

                StaticManager.Backend.GameData.PlayerQuest.UpdateMissionIsDone(itemComponent.GetMissionId(), true);  
                itemComponent.SetGetButtonState(true);  
                itemComponent.UpdateButtonsState();

                /* 미션 : total Daily 미션*/
                QuestsManager.instance.UpdateMissionUI(BackendData.Chart.Mission.MissionContentType.None, BackendData.Chart.Mission.MissionType.DailyTotal, 0);

            }
            else if(itemComponent.GetMissionType() == BackendData.Chart.Mission.MissionType.Repeat)
            {
                itemComponent.SetGetButtonState(false);

                double level = System.Math.Truncate(fillAmount);
                //rewardIds.Add(itemComponent.GetRewardItemId());
                //rewardCounts.Add(itemComponent.GetRewardItemCount() * level);
                int id = itemComponent.GetRewardItemId();
                double count = itemComponent.GetRewardItemCount() * level;
                if (FinalRewardItems.ContainsKey(id))
                {
                    FinalRewardItems[id] += count;
                }
                else
                {
                    FinalRewardItems.Add(id, count);
                }

                /* 초기화 */
                double _nowMissionStep = itemComponent.GetNowMissionStep() % itemComponent.GetRequestCount(); // 나머지 
                StaticManager.Backend.GameData.PlayerQuest.SetMission(itemComponent.GetMissionId(), _nowMissionStep);

                itemComponent.SetNowStep((float)_nowMissionStep);

                itemComponent.GetRewardItemObj().SetRewardLevel(0);  
                itemComponent.GetRewardItemObj().UpdateView();

                itemComponent.SetNextRewardLevel(1);
            }else if(itemComponent.GetMissionType() == BackendData.Chart.Mission.MissionType.DailyTotal)// Datily total
            {
                if (itemComponent.GetGetButtonState() == true) // 일일보상 완료된거면 
                    continue;

                //rewardIds.Add(itemComponent.GetRewardItemId());
                //rewardCounts.Add(itemComponent.GetRewardItemCount());
                int id = itemComponent.GetRewardItemId();
                double count = itemComponent.GetRewardItemCount();
                if (FinalRewardItems.ContainsKey(id))
                {
                    FinalRewardItems[id] += count;
                }
                else
                {
                    FinalRewardItems.Add(id, count);
                }

                StaticManager.Backend.GameData.PlayerQuest.UpdateDailyTotalMissionDone(true);
                itemComponent.SetGetButtonState(true);
                itemComponent.UpdateButtonsState();
            }
        }

        if (FinalRewardItems.Count == 0)
            return;

        List<int> rewardItemIds = new();
        List<double> rewardItemCounts = new();
        foreach (int key in FinalRewardItems.Keys)
        {
            Debug.Log($"id : {key} -> {FinalRewardItems[key]}");
            rewardItemIds.Add(key);  
            rewardItemCounts.Add(FinalRewardItems[key]);  
        }

        if (rewardItemIds.Count != 0)  
            RewardManager.instance.ShowRewardWindow(rewardItemIds, rewardItemCounts, true);

        //RewardManager.instance.ShowRewardWindow(rewardIds, rewardCounts, true);

    }


}
