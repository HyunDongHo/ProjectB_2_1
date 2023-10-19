// Copyright 2013-2022 AFI, INC. All rights reserved.

using System;
using System.Collections.Generic;
using System.Globalization;
using BackEnd;
using LitJson;
using UnityEngine;

namespace BackendData.GameData {
    //===============================================================
    // UserData 테이블의 데이터를 담당하는 클래스(변수)
    //===============================================================
    public partial class PlayerAdsBuff  
    {
        public List<AdsBuffData> AdsBuffList = new List<AdsBuffData>();
    }
    public class AdsBuffData
    {
        public int AdsBuffID { get; set; }
        public double AdsBuffLevel { get; set; }
        public double AdsBuffNowStep { get; set; }

        public bool AdsBuffing { get; set; }

        public string LastAdsBuffTime { get; set; }
        public bool isFirstEnd { get; set; }

    }
    //===============================================================
    // UserData 테이블의 데이터를 담당하는 클래스(함수)
    //===============================================================
    public partial class PlayerAdsBuff : Base.GameData {

        // 데이터가 존재하지 않을 경우, 초기값 설정
        protected override void InitializeData() {
            for (int i = 1; i < 5; i++)
            {
                //BackendData.Chart.Mission.Item _missionChartItem = null;
                //StaticManager.Backend.Chart.Mission.Dictionary.TryGetValue(i, out _missionChartItem);
                BackendData.GameData.AdsBuffData initData = new BackendData.GameData.AdsBuffData() { AdsBuffID = i, AdsBuffLevel = 0, AdsBuffNowStep = 0, AdsBuffing = false , isFirstEnd = false};
                StaticManager.Backend.GameData.PlayerAdsBuff.ResetLastAdsBuffTime(i);
                StaticManager.Backend.GameData.PlayerAdsBuff.AdsBuffList.Add(initData);  
            }
        }

        // Backend.GameData.GetMyData 호출 이후 리턴된 값을 파싱하여 캐싱하는 함수
        // 서버에서 데이터를 불러오늖 함수는 BackendData.Base.GameData의 BackendGameDataLoad() 함수를 참고해주세요
        protected override void SetServerDataToLocal(JsonData gameDataJson) {

            if (gameDataJson.ContainsKey("AdsBuffList") == true && gameDataJson["AdsBuffList"].Count > 0)
            {
                AdsBuffList.Clear();
                for (int i=0;i< gameDataJson["AdsBuffList"].Count; i++)
                {
                    AdsBuffData data = new AdsBuffData();
                    data.AdsBuffID = int.Parse(gameDataJson["AdsBuffList"][i]["AdsBuffID"].ToString());
                    data.AdsBuffLevel = double.Parse(gameDataJson["AdsBuffList"][i]["AdsBuffLevel"].ToString());
                    data.AdsBuffNowStep = double.Parse(gameDataJson["AdsBuffList"][i]["AdsBuffNowStep"].ToString());
                    data.AdsBuffing = bool.Parse(gameDataJson["AdsBuffList"][i]["AdsBuffing"].ToString());
                    data.isFirstEnd = bool.Parse(gameDataJson["AdsBuffList"][i]["isFirstEnd"].ToString());
                    if (gameDataJson["AdsBuffList"][i].ContainsKey("LastAdsBuffTime") == false || gameDataJson["AdsBuffList"][i]["LastAdsBuffTime"].ToString() == "True")
                    {
                        data.LastAdsBuffTime = string.Empty;
                    }
                    else
                    {
                        data.LastAdsBuffTime = gameDataJson["AdsBuffList"][i]["LastAdsBuffTime"].ToString();
                    }
                    //Debug.Log($"{data.AdsBuffID} / {data.LastAdsBuffTime}");
                    AdsBuffList.Add(data);  
                    //Debug.Log($"{gameDataJson["AdsBuffList"][i]["AdsBuffID"]} / {gameDataJson["AdsBuffList"][i]["LastAdsBuffTime"]}");
                }  
                //AdsBuffList = LitJson.JsonMapper.ToObject<List<AdsBuffData>>(gameDataJson["AdsBuffList"].ToJson());
            }
            else
            {

            }
        }

        // 테이블 이름 설정 함수
        public override string GetTableName() {
            return "PlayerAdsBuff";
        }

        // 컬럼 이름 설정 함수
        public override string GetColumnName() {
            return null;
        }

        // 데이터 저장 시 저장할 데이터를 뒤끝에 맞게 파싱하는 함수
        public override Param GetParam() {
            Param param = new Param();

            param.Add("AdsBuffList", AdsBuffList);
            return param;
        }
        
        // 유저의 정보를 변경하는 함수
        public void UpdateAdsBuffData(int adsBuffID) {
            IsChangedData = true;

            AdsBuffData data = null;
            data = AdsBuffList.Find(item => item.AdsBuffID == adsBuffID);
            if (data != null)
            {
                data.LastAdsBuffTime = string.Format("{0:MM-DD:HH:mm:ss.fffZ}", DateTime.Now.ToString(CultureInfo.InvariantCulture));
                data.AdsBuffing = true;  
            }

        }
        public void ResetLastAdsBuffTime(int adsBuffID)
        {
            IsChangedData = true;
            AdsBuffData data = null;
            data = AdsBuffList.Find(item => item.AdsBuffID == adsBuffID);
            if(data != null)
            {
                data.LastAdsBuffTime = string.Empty;
                data.AdsBuffing = false;
            }
        }
        public bool IsSetAdsBuffTime(int adsBuffID) // 버프 적용중 == return true
        {
            IsChangedData = true;
            AdsBuffData data = null;
            data = AdsBuffList.Find(item => item.AdsBuffID == adsBuffID);
            bool isSetTime = false;
            if (data != null)
            {
                isSetTime = string.IsNullOrEmpty(AdsBuffList.Find(item => item.AdsBuffID == adsBuffID).LastAdsBuffTime);
            }
            else
            {
                Debug.Log("바프 데이터 없음");
                return false;
            }

            return isSetTime == false ? true : false;
        }
        public void SetIsFirstEnd(int adsBuffID)
        {
            IsChangedData = true;
            AdsBuffData data = null;
            data = AdsBuffList.Find(item => item.AdsBuffID == adsBuffID);
            if (data != null)
            {
                data.isFirstEnd = true;  
            }
        }
        public void AddNowStep(int id, double count)
        {
            IsChangedData = true;

            AdsBuffData adsbuffData = AdsBuffList.Find(item => item.AdsBuffID == id);
            if (adsbuffData != null)
                adsbuffData.AdsBuffNowStep += count;
        }
        public void SetNowStep(int id, double count)
        {
            IsChangedData = true;

            AdsBuffData adsbuffData = AdsBuffList.Find(item => item.AdsBuffID == id);
            if (adsbuffData != null)
                adsbuffData.AdsBuffNowStep = count;
        }
        public void SetLevelUp(int id)
        {
            IsChangedData = true;

            AdsBuffData adsbuffData = AdsBuffList.Find(item => item.AdsBuffID == id);
            if (adsbuffData != null)
                adsbuffData.AdsBuffLevel += 1;
        }
        public double GetAdsBuffRatio(BackendData.Chart.AdsBuff.AdsType adsType)
        {
            int id = -1;
            double baseStat = 0;
            double growingStat = 0;
            foreach (var item in StaticManager.Backend.Chart.AdsBuff.Dictionary.Values)
            {
                if (item.AdsType == adsType)
                {
                    id = item.AdsID;
                    baseStat = item.BaseBuffStatRatio;
                    growingStat = item.StatGrowingRatio;
                }
            }
            if (id == -1 || baseStat == 0 || growingStat == 0)
                return 0;

            if (StaticManager.Backend.GameData.PlayerAdsBuff.AdsBuffList.Find(item => item.AdsBuffID == id).AdsBuffing == false) // 버프 적용중이 아니라면 
                return 0;

            double finalStatRatio = 0;
            AdsBuffData adsbuffData = AdsBuffList.Find(item => item.AdsBuffID == id);
            if (adsbuffData == null)
                return 0;

            finalStatRatio = (baseStat + (growingStat * adsbuffData.AdsBuffLevel)) ;

            Debug.Log($"finalStatRatio : {finalStatRatio}");
            return finalStatRatio;
        }
    }

}