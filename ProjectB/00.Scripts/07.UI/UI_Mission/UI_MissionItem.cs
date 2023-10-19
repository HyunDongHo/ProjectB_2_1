using BackendData.Chart.Mission;
using BackendData.Post;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_MissionItem : UI_Base
{
    float _nowMissionStep;
    int _rewardId;
    RewardItemType _rewardType;
    double _rewardCount;
    bool _missionIsDone;
    bool _missionAdsIsDone;

    /* Chart */  
    int chartMissionId;
    string chartMissionContent;
    float chartRequestCount;
    MissionType chartMissionType;
    MissionContentType chartMissionContentType;

    [SerializeField]
    UI_Reward_item rewardItemObj;

    [SerializeField]
    TextMeshProUGUI missionContentText, missionProgressText;  

    [SerializeField]
    Button getButton;
    [SerializeField]
    Button AdsButton;
    [SerializeField]
    int adsId = 10002;
    [SerializeField]
    int adsCount = 100;

    [SerializeField]
    Image MissionFillImage;

    public Action GetRewardAction ; // Action
    int nextRewardLevel = 0;

    public void Awake()
    {
        //GetRewardAction = () =>
        //{

        //};
    }
    enum GameObjects
    {

    }

    public override void init()
    {
        Bind<GameObject>(typeof(GameObjects)); //Binding 

        getButton.gameObject.AddUIEvent(OnGetButtonClicked);
        AdsButton.gameObject.AddUIEvent(OnAdsButtonClicked);
        missionContentText.text = chartMissionContent;

        //missionProgressText.text = $"{_nowMissionStep} / {chartRequestCount}";

        nextRewardLevel = (int)System.Math.Truncate(_nowMissionStep / chartRequestCount) + 1;
        //Debug.Log($"nextRewardLevel : {nextRewardLevel}");
    }
    public void InitNowStep() // init 
    {
        if(chartMissionType == MissionType.Repeat)
        {
            if (_nowMissionStep >= chartRequestCount)
            {
                double quotient = System.Math.Truncate(_nowMissionStep / chartRequestCount);
                if (quotient != 0)
                {
                    rewardItemObj.SetRewardLevel((int)quotient);
                    rewardItemObj.UpdateView();
                    nextRewardLevel = (int)quotient + 1;
                }
            }
            else
            {
                rewardItemObj.SetRewardLevel(0);
                rewardItemObj.UpdateView();  
                nextRewardLevel = 1;
            }
        }
    }
    public void SetNowStep(float step) // Setting 
    {
        _nowMissionStep = step;

        if (missionProgressText == null || MissionFillImage == null)
            return;

        if (chartMissionType == MissionType.Repeat)
        {
            missionProgressText.text = $"{_nowMissionStep} / {chartRequestCount}";
            MissionFillImage.fillAmount = _nowMissionStep / chartRequestCount;

            if (_nowMissionStep >= chartRequestCount)
            {
                double quotient = System.Math.Truncate(_nowMissionStep / chartRequestCount);
                if (quotient >= nextRewardLevel)
                {
                    rewardItemObj.SetRewardLevel(nextRewardLevel);
                    rewardItemObj.UpdateView();
                    nextRewardLevel += 1;      
                }
            }
            UpdateButtonsState();
        }
        else if(chartMissionType == MissionType.Daily)
        {
            if(_nowMissionStep >= chartRequestCount)
            {
                missionProgressText.text = $"{chartRequestCount} / {chartRequestCount}";
                MissionFillImage.fillAmount = 1;
            }
            else
            {
                missionProgressText.text = $"{_nowMissionStep} / {chartRequestCount}";
                MissionFillImage.fillAmount = _nowMissionStep / chartRequestCount;
            }

            rewardItemObj.SetRewardLevel(0);
            rewardItemObj.UpdateView();
            UpdateButtonsState();

        }
        else if(chartMissionType == MissionType.DailyTotal) // Daily Total Content
        {
            missionProgressText.text = $"{step} / {chartRequestCount}";
            MissionFillImage.fillAmount = step / chartRequestCount;
            UpdateButtonsState();

        }
        else
        {

        }
    }

    public void SetRewardItemObj(RewardItemType type, int id, double count)
    {
        _rewardId = id;
        _rewardType = type;
        _rewardCount = count;

        if (rewardItemObj == null)
            return;
        rewardItemObj.SetChartItemId(id);
        rewardItemObj.SetChartCount(count);
        rewardItemObj.SetChartItemType(type);
        rewardItemObj.UpdateView();
    }
    public void SetChartMissionId(int id)
    {
        chartMissionId = id;
    }
    public void SetChartContentName(string contentName)
    {
        chartMissionContent = contentName;
    }
    public void SetChartMissionType(MissionType type)
    {
        chartMissionType = type;
    }
    public void SetChartMissionContentType(MissionContentType type)
    {
        chartMissionContentType = type;
    }
    public void SetChartRequestCount(float count)
    {
        chartRequestCount = count;
        if (missionContentText != null)
            missionContentText.text = chartMissionContent;
    }
    public void SetNextRewardLevel(int level)
    {
        nextRewardLevel = level;
    }
    public void SetProgressAndFillAmount()
    {
        missionProgressText.text = $"{_nowMissionStep} / {chartRequestCount}";
        MissionFillImage.fillAmount = _nowMissionStep / chartRequestCount;
    }
    /* getter */
    public int GetMissionId()
    {
        return chartMissionId;
    }
    public string GetMissionContent()
    {
        return chartMissionContent;
    }
    public MissionType GetMissionType()  
    {
        return chartMissionType;
    }
    public MissionContentType GetMissionContentType()
    {
        return chartMissionContentType;
    }
    public float GetRequestCount()
    {
        return chartRequestCount;
    }
    public int GetRewardItemId()
    {
        return _rewardId;
    }
    public RewardItemType GetRewardItemType()
    {
        return _rewardType;
    }
    public double GetRewardItemCount()
    {
        return _rewardCount;
    }
    public float GetNowMissionStep()
    {
        return _nowMissionStep;
    }

    public UI_Reward_item GetRewardItemObj()
    {
        return rewardItemObj;
    }
    public void UpdateAllText()
    {
        if (missionContentText != null)
            missionContentText.text = chartMissionContent;
    }
    public void SetGetButtonState(bool activeFlag)
    {
        _missionIsDone = activeFlag;
        //if (getButton != null)
        //    getButton.gameObject.SetActive(activeFlag);
    }
    public void SetAdsButtonState(bool activeFlag)
    {
        _missionAdsIsDone = activeFlag;
    }
    public bool GetGetButtonState()
    {
        return _missionIsDone;
    }
    public bool GetAdsButtonState()
    {
        return _missionAdsIsDone;
    }
    public void UpdateButtonsState()
    {
        if (_missionIsDone == true)
        {
            getButton.gameObject.SetActive(false);

            if (chartMissionType == MissionType.Daily || chartMissionType == MissionType.DailyTotal)
            {
                if (_missionAdsIsDone == true)
                {
                    AdsButton.gameObject.SetActive(false);  
                }
                else
                {
                    AdsButton.gameObject.SetActive(true);
                }
            }
        }
        else
        {
            getButton.gameObject.SetActive(true);
        }

    }
    void OnGetButtonClicked(PointerEventData data) // 완료 보상 
    {
        Debug.Log("OnGetButtonClicked");
        if (_nowMissionStep < chartRequestCount)
            return;

        if(chartMissionType == MissionType.Daily)  
        {       
            StaticManager.Backend.GameData.PlayerQuest.UpdateMissionIsDone(chartMissionId, true);
            Debug.Log($"reward count : {_rewardCount}");
            SetGetButtonState(true);
            UpdateButtonsState(); 
            RewardManager.instance.ShowRewardWindow(new List<int>() { _rewardId }, new List<double>() { _rewardCount }, true);

            /* 미션 : total Daily 미션*/
            QuestsManager.instance.UpdateMissionUI(BackendData.Chart.Mission.MissionContentType.None, BackendData.Chart.Mission.MissionType.DailyTotal, 0);

            //AdsButton.gameObject.SetActive(true);
        }
        else if(chartMissionType == MissionType.Repeat)  
        {
            /* 보상 적용 */
            double count = 0;
            if (nextRewardLevel <= 1)
            {
                count = _rewardCount;  
            }
            else
            {
                count = _rewardCount * (nextRewardLevel - 1);  
            }
            Debug.Log($"reward count : {count}");
            SetGetButtonState(false);
            RewardManager.instance.ShowRewardWindow(new List<int>() { _rewardId }, new List<double>() { count }, true);

            /* 초기화 */
            _nowMissionStep = _nowMissionStep % chartRequestCount; // 나머지 
            StaticManager.Backend.GameData.PlayerQuest.SetMission(chartMissionId, _nowMissionStep);

            //missionProgressText.text = $"{_nowMissionStep} / {chartRequestCount}";
            //MissionFillImage.fillAmount = _nowMissionStep / chartRequestCount;
            SetNowStep(_nowMissionStep);

            rewardItemObj.SetRewardLevel(0);
            rewardItemObj.UpdateView();

            nextRewardLevel = 1;


            //GetRewardAction?.Invoke();
        }
        else if(chartMissionType == MissionType.DailyTotal)
        {
            StaticManager.Backend.GameData.PlayerQuest.UpdateDailyTotalMissionDone(true);
            SetGetButtonState(true);
            UpdateButtonsState();

            RewardManager.instance.ShowRewardWindow(new List<int>() { _rewardId }, new List<double>() { _rewardCount }, true);

        }
        else
        {

        }

    }

    void OnAdsButtonClicked(PointerEventData data) // 추가 광고 보상 
    {
        Debug.Log("OnAdsButtonClicked");

        GoogleAdMobController.AdMobManager.RewardCompleteAction = () =>
        {
            if(chartMissionType== MissionType.Daily)
            {
                StaticManager.Backend.GameData.PlayerQuest.UpdateMissionAdsIsDone(chartMissionId, true);
            }
            else if(chartMissionType == MissionType.DailyTotal)
            {
                StaticManager.Backend.GameData.PlayerQuest.UpdateDailyTotalMissionAdsIsDone(true);
            }
            else
            {

            }
            SetAdsButtonState(true);
            UpdateButtonsState();
            // 보상 처리
            RewardManager.instance.ShowRewardWindow(new List<int>() { adsId }, new List<double>() { adsCount }, true);    
            Debug.Log("추가 보상 적용");
        };
        GoogleAdMobController.AdMobManager.ShowRewardedAd();

    }
}
