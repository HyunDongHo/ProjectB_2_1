// Copyright 2013-2022 AFI, INC. All rights reserved.

using System;
using System.Collections.Generic;
using System.Globalization;
using BackEnd;
using LitJson;
using UnityEngine;

namespace BackendData.GameData
{
    //===============================================================
    // UserData ���̺��� �����͸� ����ϴ� Ŭ����(����)
    //===============================================================
    public partial class PlayerPurchase
    {
        public List<PassPurchaseData> PassPurchaseList = new List<PassPurchaseData>();
        public List<LevelPassData> LevelPassList = new List<LevelPassData>();
        public List<StagePassData> StagePassList = new List<StagePassData>();
    }
    public class PassPurchaseData
    {
        public int PassID { get; set; }
        public bool IsActivate { get; set; }

    }
    public class LevelPassData
    {
        public double Level { get; set; }
        public bool isGetNormal  { get; set; }
        public bool IsGetPremium { get; set; }
    }
    public class StagePassData
    {
        public double Level { get; set; }
        public bool isGetNormal { get; set; }
        public bool IsGetPremium { get; set; }
    }
    //===============================================================
    // UserData ���̺��� �����͸� ����ϴ� Ŭ����(�Լ�)
    //===============================================================
    public partial class PlayerPurchase : Base.GameData
    {

        // �����Ͱ� �������� ���� ���, �ʱⰪ ����
        protected override void InitializeData()
        {
            PassPurchaseList.Clear();
            Dictionary<int, BackendData.Chart.PassInfo.Item> PassInfoData = null;
            PassInfoData = StaticManager.Backend.Chart.PassInfo.GetChartPassInfoData();
            foreach (var data in PassInfoData)
            {
                //BackendData.Chart.Mission.Item _missionChartItem = null;
                //StaticManager.Backend.Chart.Mission.Dictionary.TryGetValue(i, out _missionChartItem);
                BackendData.GameData.PassPurchaseData initData = new BackendData.GameData.PassPurchaseData() { PassID = data.Value.PassID, IsActivate = false };
                PassPurchaseList.Add(initData);
            }
            //Dictionary<double, BackendData.Chart.LevelPass.Item> LevelPassData = null;
            //LevelPassData = StaticManager.Backend.Chart.LevelPass.GetChartLevelPassData();
            //Dictionary<double, BackendData.Chart.StagePass.Item> StagePassData = null;
            //StagePassData = StaticManager.Backend.Chart.StagePass.GetChartStagePassData();

            LevelPassList.Clear();
            //LevelPassList.Add(new BackendData.GameData.LevelPassData() { Level = 1, isGetNormal = false, IsGetPremium = false });

            //foreach (var data in LevelPassData)
            //{
            //    BackendData.GameData.LevelPassData initData = new BackendData.GameData.LevelPassData() { Level = data.Value.Level, isGetNormal = false, IsGetPremium = false };
            //    LevelPassList.Add(initData);
            //}
            StagePassList.Clear();
            //StagePassList.Add(new BackendData.GameData.StagePassData() { Level = 1, isGetNormal = false, IsGetPremium = false });  
            //foreach (var data in StagePassData)
            //{
            //    BackendData.GameData.StagePassData initData = new BackendData.GameData.StagePassData() { Level = data.Value.Level, isGetNormal = false, IsGetPremium = false };
            //    StagePassList.Add(initData);    
            //}

        }

        // Backend.GameData.GetMyData ȣ�� ���� ���ϵ� ���� �Ľ��Ͽ� ĳ���ϴ� �Լ�
        // �������� �����͸� �ҷ����e �Լ��� BackendData.Base.GameData�� BackendGameDataLoad() �Լ��� �������ּ���
        protected override void SetServerDataToLocal(JsonData gameDataJson)
        {

            if (gameDataJson.ContainsKey("PassPurchaseList") == true && gameDataJson["PassPurchaseList"].Count > 0)
            {
                PassPurchaseList.Clear();
                for (int i = 0; i < gameDataJson["PassPurchaseList"].Count; i++)
                {
                    PassPurchaseData data = new PassPurchaseData();
                    data.PassID = int.Parse(gameDataJson["PassPurchaseList"][i]["PassID"].ToString());
                    data.IsActivate = bool.Parse(gameDataJson["PassPurchaseList"][i]["IsActivate"].ToString());

                    PassPurchaseList.Add(data);
                    //Debug.Log($"{gameDataJson["AdsBuffList"][i]["AdsBuffID"]} / {gameDataJson["AdsBuffList"][i]["LastAdsBuffTime"]}");
                }
                //AdsBuffList = LitJson.JsonMapper.ToObject<List<AdsBuffData>>(gameDataJson["AdsBuffList"].ToJson());
            }
            else
            {

            }
            if (gameDataJson.ContainsKey("LevelPassList") == true && gameDataJson["LevelPassList"].Count > 0)
            {
                LevelPassList.Clear();
                for (int i = 0; i < gameDataJson["LevelPassList"].Count; i++)
                {
                    LevelPassData data = new LevelPassData();
                    data.Level = int.Parse(gameDataJson["LevelPassList"][i]["Level"].ToString());
                    data.isGetNormal = bool.Parse(gameDataJson["LevelPassList"][i]["isGetNormal"].ToString());
                    data.IsGetPremium = bool.Parse(gameDataJson["LevelPassList"][i]["IsGetPremium"].ToString());

                    LevelPassList.Add(data);
                    //Debug.Log($"{gameDataJson["AdsBuffList"][i]["AdsBuffID"]} / {gameDataJson["AdsBuffList"][i]["LastAdsBuffTime"]}");
                }
                //AdsBuffList = LitJson.JsonMapper.ToObject<List<AdsBuffData>>(gameDataJson["AdsBuffList"].ToJson());
            }
            else
            {

            }
            if (gameDataJson.ContainsKey("StagePassList") == true && gameDataJson["StagePassList"].Count > 0)
            {
                StagePassList.Clear();
                for (int i = 0; i < gameDataJson["StagePassList"].Count; i++)
                {
                    StagePassData data = new StagePassData();
                    data.Level = int.Parse(gameDataJson["StagePassList"][i]["Level"].ToString());
                    data.isGetNormal = bool.Parse(gameDataJson["StagePassList"][i]["isGetNormal"].ToString());
                    data.IsGetPremium = bool.Parse(gameDataJson["StagePassList"][i]["IsGetPremium"].ToString());  

                    StagePassList.Add(data);
                    //Debug.Log($"{gameDataJson["AdsBuffList"][i]["AdsBuffID"]} / {gameDataJson["AdsBuffList"][i]["LastAdsBuffTime"]}");
                }
                //AdsBuffList = LitJson.JsonMapper.ToObject<List<AdsBuffData>>(gameDataJson["AdsBuffList"].ToJson());
            }
            else
            {

            }
        }

        // ���̺� �̸� ���� �Լ�
        public override string GetTableName()
        {
            return "PlayerPurchase";
        }

        // �÷� �̸� ���� �Լ�
        public override string GetColumnName()
        {
            return null;
        }

        // ������ ���� �� ������ �����͸� �ڳ��� �°� �Ľ��ϴ� �Լ�
        public override Param GetParam()
        {
            Param param = new Param();

            param.Add("PassPurchaseList", PassPurchaseList);
            param.Add("LevelPassList", LevelPassList);
            param.Add("StagePassList", StagePassList);  
            return param;
        }

        // ������ ������ �����ϴ� �Լ�
        public void AddPassData(BackendData.Chart.PassInfo.PassType passInfoType, double level)
        {
            IsChangedData = true;
            switch (passInfoType)
            {
                case BackendData.Chart.PassInfo.PassType.Level:
                    LevelPassData levelData = null;
                    levelData = LevelPassList.Find(item => item.Level == level);
                    if (levelData == null)
                    {
                        LevelPassList.Add(new LevelPassData() { Level = level, isGetNormal = false, IsGetPremium = false });
                    }
                    break;
                case BackendData.Chart.PassInfo.PassType.Stage:
                    StagePassData stageData = null;
                    stageData = StagePassList.Find(item => item.Level == level);
                    if (stageData == null)
                    {
                        StagePassList.Add(new StagePassData() { Level = level, isGetNormal = false, IsGetPremium = false });
                    }
                    break;
                case BackendData.Chart.PassInfo.PassType.Attend:
                    break;
            }


        }
        public void UpdatePassActivate(int passId , bool flag)
        {
            IsChangedData = true;
            PassPurchaseData data = null;
            data = PassPurchaseList.Find(item => item.PassID == passId);
            if (data != null)
            {
                data.IsActivate = flag;
            }
        }

        public void UpdateLevelPassList(BackendData.Chart.PassInfo.PassType passInfoType, PassRewardType type,  double level)
        {
            IsChangedData = true;

            switch (passInfoType)
            {
                case BackendData.Chart.PassInfo.PassType.Level:
                    LevelPassData levelData = null;
                    levelData = LevelPassList.Find(item => item.Level == level);
                    if (levelData != null)
                    {
                        if (type == PassRewardType.Normal) // normal 
                        {
                            levelData.isGetNormal = true;
                        }
                        else // premium
                        {
                            levelData.IsGetPremium = true;
                        }
                    }
                    else
                    {
                        if(level != 0)
                        {
                            if (type == PassRewardType.Normal) // normal 
                            {
                                LevelPassList.Add(new LevelPassData() { Level = level, isGetNormal = true, IsGetPremium = false });
                            }
                            else // premium
                            {
                                LevelPassList.Add(new LevelPassData() { Level = level, isGetNormal = false, IsGetPremium = true });
                            }
                        }
                    }

                    break;
                case BackendData.Chart.PassInfo.PassType.Stage:
                    StagePassData stageData = null;
                    stageData = StagePassList.Find(item => item.Level == level);
                    if (stageData != null)
                    {
                        if (type == PassRewardType.Normal) // normal 
                        {
                            stageData.isGetNormal = true;
                        }
                        else // premium
                        {
                            stageData.IsGetPremium = true;

                        }
                    }
                    else
                    {
                        if (level != 0)
                        {
                            if (type == PassRewardType.Normal) // normal 
                            {
                                StagePassList.Add(new StagePassData() { Level = level, isGetNormal = true, IsGetPremium = false });
                            }
                            else // premium
                            {
                                StagePassList.Add(new StagePassData() { Level = level, isGetNormal = false, IsGetPremium = true });
                            }
                        }
                    }
                    break;
                case BackendData.Chart.PassInfo.PassType.Attend:

                    break;
            }


        }
    }

}