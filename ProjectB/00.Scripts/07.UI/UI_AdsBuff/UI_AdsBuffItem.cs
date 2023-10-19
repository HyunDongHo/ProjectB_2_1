using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_AdsBuffItem : UI_Base
{
    public const double AdsBuffTime = 50;

    double _level;
    double _buffStatRatio;
    double _nowStep;
    double _needCount;

    Sprite adsBuffImage;
    /* chart */
    int chartAdsBuffId;
    string chartAdsBuffName;
    BackendData.Chart.AdsBuff.AdsType chartAdsRewardType;
    double chartBaseBuffStatRatio;  // Base가 되는 스텟 Ratio : 1 --> 100 곱해서 사용하기 
    double chartStatGrowingRatio;   // 증가되는 스탯 : 0.1
    double chartNeedNextCount;      // 100
    double chartNeedGrowingRatio;   // 10

    [SerializeField] Button showAdsButton;
    [SerializeField] TextMeshProUGUI AdsBuffTitleText, RemainTime;
    [SerializeField] Image AdsBuffImage;
    //Button AdsBuffImageBtn;
    [SerializeField] Image CountFillAmount;
    [SerializeField] TextMeshProUGUI CountText;
    [SerializeField] TextMeshProUGUI LevelText;
    [SerializeField] TextMeshProUGUI BuffStatRatioText;
    [SerializeField] Button LevelUpButton;
    [SerializeField] Button LockButton;

    public Action<int> StartAdsBuff;
    public Action<int> EndAdsBuff;
    //public bool isAdsBuffing;
    public void Awake()
    {
        AddEvent();
    }

    void AddEvent()
    {
        showAdsButton.onClick.RemoveListener(ShowAds);
        showAdsButton.onClick.AddListener(ShowAds);

        //AdsBuffImageBtn = AdsBuffImage.gameObject.GetComponent<Button>();
        //AdsBuffImageBtn.onClick.RemoveListener(ShowFirstLevelState);
        //AdsBuffImageBtn.onClick.AddListener(ShowFirstLevelState);   

        LevelUpButton.onClick.RemoveListener(OnLevelUpButtonClicked);
        LevelUpButton.onClick.AddListener(OnLevelUpButtonClicked);

        LockButton.onClick.RemoveListener(OnLockButtonClicked);
        LockButton.onClick.AddListener(OnLockButtonClicked);

    }
    private void OnEnable()
    {
        BackendData.GameData.AdsBuffData data = StaticManager.Backend.GameData.PlayerAdsBuff.AdsBuffList.Find(item => item.AdsBuffID == chartAdsBuffId);
        if (data == null)
            return;
        Debug.Log($"Ad buff id : {data.AdsBuffID}");
        if (data.AdsBuffing == true) // 광고버프 적용중 
        {
            DateTime nowTime = DateTime.Now;
            DateTime serverTime = DateTime.Parse(data.LastAdsBuffTime);
            TimeSpan timeSpan = nowTime - serverTime;
            double timeCal = timeSpan.TotalSeconds;
            double SetTime = AdsBuffTime - timeCal;
            if (SetTime > 0) // 버프 시작
            {
                StartCoroutine(CoStartAdsBuffTime(Math.Truncate(SetTime)));
                //isAdsBuffing = true;
                SetShowAdsButton(true);
            }
            else // 버프 완료 
            {
                //EndAdsBuff?.Invoke(chartAdsBuffId);
                EndAdsBuffTime();
            }
        }
        else // 광고 봐야되 
        {
            if (_level == 0)
            {
                if(data.isFirstEnd == false) 
                {
                    SetShowAdsButton(true); // 처음 광고 안보이게 
                    RemainTime.text = $"남은 시간 - {AdsBuffTime}초)";
                    LockButton.gameObject.SetActive(true);
                }
                else
                {
                    EndAdsBuffTime();
                    LockButton.gameObject.SetActive(false);
                }
            }
            else
            {
                EndAdsBuffTime();
            }
        }

        SetAllText();  

    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
    enum GameObjects
    {

    }

    public override void init()
    {
        Bind<GameObject>(typeof(GameObjects)); //Binding 

        AdsBuffTitleText.text = chartAdsBuffName;
        AdsBuffImage.sprite = adsBuffImage;  
    }
    public void SetAdsBuffLevel(double level)
    {
        _level = level;
        _buffStatRatio = (chartBaseBuffStatRatio + (chartStatGrowingRatio * _level)) * 100;
        _needCount = chartNeedNextCount * Mathf.Pow((float)chartNeedGrowingRatio, (float)_level);
    
    }

    public void SetAdsBuffNowStep(double step)
    {
        _nowStep = step;

        CountText.text = $"{_nowStep} / {_needCount}";
        CountFillAmount.fillAmount = (float)((float)_nowStep / _needCount);
    }
    public void SetAdsBuffImage(Sprite sprite)
    {
        adsBuffImage = sprite;
    }
    /* Chart */
    public void SetChartAddBuffId(int id)
    {
        chartAdsBuffId = id;
    }
    public void SetChartAddBuffName(string name)
    {
        chartAdsBuffName = name;
    }
    public void SetChartAdsRewardType(BackendData.Chart.AdsBuff.AdsType type)
    {
        chartAdsRewardType = type;
    }

    public void SetChartBaseBuffStatRatio(double baseRatio)
    {
        chartBaseBuffStatRatio = baseRatio;
    }
    public void SetChartStatGrowingRatio(double growingRatio)
    {
        chartStatGrowingRatio = growingRatio;
    }
    public void SetChartNeedNextCount(double needNextCount)
    {
        chartNeedNextCount = needNextCount;
    }
    public void SetChartNeedGrowingRatio(double needRatio)
    {
        chartNeedGrowingRatio = needRatio;
    }
    /* Button Set*/
    public void SetShowAdsButton(bool flag) // true 이면 버튼 버튼 false
    {
        if(flag == true)
        {
            showAdsButton.gameObject.SetActive(false);
        }
        else
        {
            showAdsButton.gameObject.SetActive(true);

        }
    }
    
    public void SetAllText()
    {
        if (LevelText == null || BuffStatRatioText == null)
            return;
        LevelText.text = $"LV.{_level}";
        BuffStatRatioText.text = $" + {_buffStatRatio} %";
        CountText.text = $"{_nowStep} / {_needCount}";
        CountFillAmount.fillAmount = (float)((float)_nowStep /_needCount);
    }
    public int GetAdsBuffId()
    {
        return chartAdsBuffId;
    }
    public void ShowFirstLevelState() // 레벨 0 일때는 광고 보지 않고 버프 아이콘 클릭 시 돌아가고 끝나면 광고로 닫김 
    {
        if(_level == 0)
        {
            BackendData.GameData.AdsBuffData data = null;
            data = StaticManager.Backend.GameData.PlayerAdsBuff.AdsBuffList.Find(item => item.AdsBuffID == chartAdsBuffId);
            if (data.AdsBuffing == true)
                return;
            ActivateBuff();
            //isAdsBuffing = true;
        }
    }
    public void ShowAds() // 광고 보기 
    {
        GoogleAdMobController.AdMobManager.RewardCompleteAction = () =>
        {
            ActivateBuff();
        };

        GoogleAdMobController.AdMobManager.ShowRewardedAd();    
    }
    void ActivateBuff()
    {
        // 보상 처리
        showAdsButton.gameObject.SetActive(false);
        // 광고 버프 Buff 시간 
        StaticManager.Backend.GameData.PlayerAdsBuff.UpdateAdsBuffData(chartAdsBuffId);
        StartAdsBuff?.Invoke(chartAdsBuffId);
        StartCoroutine(CoStartAdsBuffTime(AdsBuffTime));
        //isAdsBuffing = true;

        // 버프 적용 
        Debug.Log(chartAdsRewardType.ToString() + "버프 적용");
    }
    IEnumerator CoStartAdsBuffTime(double nRemain)
    {
        WaitForSecondsRealtime waitForSeconds = new WaitForSecondsRealtime(1f); 
        while (nRemain >= 0)
        {
            RemainTime.text = $"남은 시간 - {nRemain}초)";
            yield return waitForSeconds;
            nRemain--;
        }

        EndAdsBuffTime();
    }

    public void EndAdsBuffTime()
    {
        Debug.Log("EndAdsBuffTime");
        if (_level == 0)
        {
            if(StaticManager.Backend.GameData.PlayerAdsBuff.AdsBuffList.Find(item => item.AdsBuffID == chartAdsBuffId).isFirstEnd == false)
            {
                StaticManager.Backend.GameData.PlayerAdsBuff.SetIsFirstEnd(chartAdsBuffId); // 처음은 광고 안봐두 되지만 다음부터는 봐야됨   
                Debug.Log("처음 보기 끝~~~~~~~~~~~~~~~");  
            }
        }

        StaticManager.Backend.GameData.PlayerAdsBuff.ResetLastAdsBuffTime(chartAdsBuffId);

        showAdsButton.gameObject.SetActive(true);
        RemainTime.text = $"남은 시간 - {AdsBuffTime}초)";  
        EndAdsBuff?.Invoke(chartAdsBuffId);
        //isAdsBuffing = false;
    }
    void OnLevelUpButtonClicked() // level up 버튼 
    {
        Debug.Log("OnLevelUpButtonClicked");
        if (_nowStep < _needCount)
            return;

        StaticManager.Backend.GameData.PlayerAdsBuff.SetNowStep(chartAdsBuffId, (_nowStep % _needCount));
        StaticManager.Backend.GameData.PlayerAdsBuff.SetLevelUp(chartAdsBuffId);
        SetAdsBuffLevel(StaticManager.Backend.GameData.PlayerAdsBuff.AdsBuffList.Find(item => item.AdsBuffID == chartAdsBuffId).AdsBuffLevel);
        SetAdsBuffNowStep(StaticManager.Backend.GameData.PlayerAdsBuff.AdsBuffList.Find(item => item.AdsBuffID == chartAdsBuffId).AdsBuffNowStep);
        LevelText.text = $"LV.{_level}";
        BuffStatRatioText.text = $" + {_buffStatRatio} %";           
    }

    void OnLockButtonClicked()
    {
        Debug.Log("OnLockButtonClicked");
        LockButton.gameObject.SetActive(false);
        ShowFirstLevelState();    
    }
}
