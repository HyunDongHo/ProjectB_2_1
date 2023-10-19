using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_AdsBuffPopup : UIManagers
{
    [SerializeField]
    UI_AdsBuffItem ui_AdsBuffItem;
    [SerializeField]
    GameObject AdsBuffContent;

    [SerializeField]
    List<UI_StageAdsItem> stageAdsList;
    [SerializeField]
    List<Sprite> AddBuffImage;    

    [SerializeField]
    GameObject parent;
    [SerializeField]
    GameObject ButtonPanel;

    // Start is called before the first frame update
    private void Awake()
    {
        foreach (Transform child in AdsBuffContent.transform)
            GameObject.Destroy(child.gameObject);

        Dictionary<int, BackendData.Chart.AdsBuff.Item> AdsBuffChartData = null;
        AdsBuffChartData = StaticManager.Backend.Chart.AdsBuff.GetChartAdsBuffData();
        //Debug.Log($"AdsBuffChartData count : {AdsBuffChartData.Count}");
        int i = 0;  
        foreach (var AdsBuffData in AdsBuffChartData)// 차트로 적용 필요 
        {
            GameObject item = null;
            item = Instantiate<GameObject>(ui_AdsBuffItem.gameObject);
            item.transform.SetParent(AdsBuffContent.transform); // 부모 지정     
            item.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);

            UI_AdsBuffItem itemComponent = item.GetComponent<UI_AdsBuffItem>();
            itemComponent.SetAdsBuffImage(AddBuffImage[i]);
            stageAdsList[i].SetStageAdsId(AdsBuffData.Value.AdsID);

            /* chart */
            itemComponent.SetChartAddBuffId(AdsBuffData.Value.AdsID);
            itemComponent.SetChartAddBuffName(AdsBuffData.Value.AdsName);
            itemComponent.SetChartAdsRewardType(AdsBuffData.Value.AdsType);  
            itemComponent.SetChartBaseBuffStatRatio(AdsBuffData.Value.BaseBuffStatRatio);
            itemComponent.SetChartStatGrowingRatio(AdsBuffData.Value.StatGrowingRatio);
            itemComponent.SetChartNeedNextCount(AdsBuffData.Value.NeedNextCount);
            itemComponent.SetChartNeedGrowingRatio(AdsBuffData.Value.NeedGrowingRatio);
            i += 1;  

            BackendData.GameData.AdsBuffData data = StaticManager.Backend.GameData.PlayerAdsBuff.AdsBuffList.Find(item => item.AdsBuffID == AdsBuffData.Value.AdsID);
            if(data != null)
            {
                itemComponent.SetAdsBuffLevel(data.AdsBuffLevel);
                itemComponent.SetAdsBuffNowStep(data.AdsBuffNowStep);    

                if (data.AdsBuffing == true)
                {
                    DateTime nowTime = DateTime.Now;
                    DateTime serverTime = DateTime.Parse(data.LastAdsBuffTime);
                    TimeSpan timeSpan = nowTime - serverTime;
                    double timeCal = timeSpan.TotalSeconds;
                    double SetTime = UI_AdsBuffItem.AdsBuffTime - timeCal;  
                    if (SetTime < 0)  // 버프 완료 
                    {
                        itemComponent.EndAdsBuffTime();
                        SetUI(data.AdsBuffID, false);  
                    }
                    else
                    {  
                        SetUI(data.AdsBuffID, true);               
                    }
                }
                else
                {
                    //itemComponent.EndAdsBuffTime();
                    SetUI(data.AdsBuffID, false);        
                }
            }
            else
            {
                itemComponent.SetAdsBuffLevel(0);
                itemComponent.SetAdsBuffNowStep(0);
            }
            itemComponent.EndAdsBuff = (id) =>
            {
                SetUI(id, false);

                Debug.Log("동그라미 끝");    
            };  
            itemComponent.StartAdsBuff = (id) =>  
            {
                SetUI(id, true);
                Debug.Log("동그라미 시작");
            };
        }        
    }

    //IEnumerator CoStartAdsBuffTime(double nRemain, UI_AdsBuffItem itemComponent)
    //{
    //    WaitForSeconds waitForSeconds = new WaitForSeconds(1f);
    //    while (nRemain >= 0)
    //    {
    //        yield return waitForSeconds;
    //        Debug.Log(nRemain);
    //        nRemain--;
    //    }

    //    itemComponent.EndAdsBuffTime();

    //}

    void SetUI(int id, bool isLoding)
    {
        UI_StageAdsItem item = stageAdsList.Find(item => item.GetStageAdsId() == id);
        if (item == null)
            return;
        item.ToggleSetUI(isLoding);
    }
    private void FixedUpdate()
    {
        for(int i=0;i<4;i++)
        {
            BackendData.GameData.AdsBuffData data = null;
            data = StaticManager.Backend.GameData.PlayerAdsBuff.AdsBuffList.Find(item => item.AdsBuffID == i + 1);
            if (data == null)
                continue;
            if (data.AdsBuffing == false)
                continue;

            DateTime nowTime = DateTime.Now;
            DateTime serverTime = DateTime.Parse(data.LastAdsBuffTime);
            TimeSpan timeSpan = nowTime - serverTime;
            double timeCal = timeSpan.TotalSeconds;
            double SetTime = UI_AdsBuffItem.AdsBuffTime - timeCal;
            if (SetTime < 0)  // 버프 완료 
            {
                SetUI(data.AdsBuffID, false);
                StaticManager.Backend.GameData.PlayerAdsBuff.ResetLastAdsBuffTime(data.AdsBuffID);
                if(data.AdsBuffLevel == 0)
                {
                    if(data.isFirstEnd == false)
                        StaticManager.Backend.GameData.PlayerAdsBuff.SetIsFirstEnd(data.AdsBuffID);  
                }
            }
            else
            {

            }
        }
    }

    public void UpdateAdsBuffCountUI(int id, double count)
    {
        for(int i = 0; i < 4; i++)
        {
            GameObject item = null;
            item = AdsBuffContent.transform.GetChild(i).gameObject;
            if (item == null)
                continue;
            UI_AdsBuffItem itemComponent = item.GetComponent<UI_AdsBuffItem>();
            if (itemComponent == null)
                continue;

            if (itemComponent.GetAdsBuffId() == id)
            {
                itemComponent.SetAdsBuffNowStep(count);
            }
        }
    }
    void Start()
    {
        AddEvent();
    }

    void AddEvent()
    {
        foreach (var item in stageAdsList)
        {
            item.button.onClick.RemoveAllListeners();
            item.button.onClick.AddListener(() =>
            {
                OpenWindow();
            });            
        }
    }

    public void OpenWindow()
    {
        parent.gameObject.SetActive(true);
    }

    public void CloseWindow()
    {
        parent.gameObject.SetActive(false);
    }
    public void SetOpenOrCloseBuffButton(bool flag)
    {
        ButtonPanel.SetActive(flag);  
    }

}
