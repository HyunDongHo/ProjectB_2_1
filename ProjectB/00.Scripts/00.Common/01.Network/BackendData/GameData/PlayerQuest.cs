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
    public partial class PlayerQuest
    {
        public int QuestID { get; private set; }
        public long QuestNowStep { get; private set; }

        public List<MissionData> MissionList = new List<MissionData>();
        public bool DailyTotalMissionDone { get; private set; } // Daily Total Mission 끝났는지
        public bool DailyTotalMissionAdsIsDone { get; private set; } // Daily Total Mission 추가 보상도 획득했는지 
    }
    public class MissionData
    {
        public int MissionID { get; set; }
        public double MissionNowStep { get; set; }
        public bool MissionIsDone { get; set; }

        public bool MissionAdsIsDone { get; set; }
    }
    //===============================================================
    // UserData 테이블의 데이터를 담당하는 클래스(함수)
    //===============================================================
    public partial class PlayerQuest : Base.GameData {

        // 데이터가 존재하지 않을 경우, 초기값 설정
        protected override void InitializeData() {
            QuestID = 1;
            QuestNowStep = 0;
            DailyTotalMissionDone = false;
            DailyTotalMissionAdsIsDone = false;

            int count = StaticManager.Backend.GameData.PlayerQuest.MissionList.Count;
            if (count == 0)
            {
                for (int i = 1; i < StaticManager.Backend.Chart.Mission.Dictionary.Count + 1; i++)
                {
                    BackendData.Chart.Mission.Item _missionChartItem = null;
                    StaticManager.Backend.Chart.Mission.Dictionary.TryGetValue(i, out _missionChartItem);
                    BackendData.GameData.MissionData initData = new BackendData.GameData.MissionData() { MissionID = _missionChartItem.MissionID, MissionNowStep = 0, MissionIsDone = false, MissionAdsIsDone = false };
                    StaticManager.Backend.GameData.PlayerQuest.MissionList.Add(initData);
                }  
            }
            
        }

        // Backend.GameData.GetMyData 호출 이후 리턴된 값을 파싱하여 캐싱하는 함수
        // 서버에서 데이터를 불러오늖 함수는 BackendData.Base.GameData의 BackendGameDataLoad() 함수를 참고해주세요
        protected override void SetServerDataToLocal(JsonData gameDataJson) {
            QuestID = int.Parse(gameDataJson["QuestID"].ToString());
            QuestNowStep = int.Parse(gameDataJson["QuestNowStep"].ToString());
            if (gameDataJson.ContainsKey("DailyTotalMissionDone"))
            {
                DailyTotalMissionDone = bool.Parse(gameDataJson["DailyTotalMissionDone"].ToString());
            }
            else
            {
                DailyTotalMissionDone = false;
            }
            if (gameDataJson.ContainsKey("DailyTotalMissionAdsIsDone"))
            {
                DailyTotalMissionAdsIsDone = bool.Parse(gameDataJson["DailyTotalMissionAdsIsDone"].ToString());  
            }
            else
            {
                DailyTotalMissionAdsIsDone = false;
            }
            if (gameDataJson.ContainsKey("MissionList") == true && gameDataJson["MissionList"].Count > 0)
            {

                MissionList = LitJson.JsonMapper.ToObject<List<MissionData>>(gameDataJson["MissionList"].ToJson());
            }
            else
            {
                for (int i = 1; i < StaticManager.Backend.Chart.Mission.Dictionary.Count + 1; i++)
                {
                    BackendData.Chart.Mission.Item _missionChartItem = null;
                    StaticManager.Backend.Chart.Mission.Dictionary.TryGetValue(i, out _missionChartItem);
                    MissionData initData = new MissionData() { MissionID = _missionChartItem.MissionID, MissionNowStep = 0, MissionIsDone = false, MissionAdsIsDone = false };
                    MissionList.Add(initData);    
                }
            }
        }

        // 테이블 이름 설정 함수
        public override string GetTableName() {
            return "PlayerQuest";
        }

        // 컬럼 이름 설정 함수
        public override string GetColumnName() {
            return null;
        }

        // 데이터 저장 시 저장할 데이터를 뒤끝에 맞게 파싱하는 함수
        public override Param GetParam() {
            Param param = new Param();

            param.Add("QuestID", QuestID);
            param.Add("QuestNowStep", QuestNowStep);
            param.Add("MissionList", MissionList);
            param.Add("DailyTotalMissionDone", DailyTotalMissionDone);
            param.Add("DailyTotalMissionAdsIsDone", DailyTotalMissionAdsIsDone);
            return param;
        }
        
        // 유저의 정보를 변경하는 함수
        public void UpdateNewQuestData(int questID) {
            IsChangedData = true;

            QuestID = questID;
            QuestNowStep = 0;
        }
        public void ClearQuestData()
        {
            IsChangedData = true;

            QuestID += 1;
            QuestNowStep = 0;
        }

        public void SetQuestData(long count)
        {
            IsChangedData = true;
            QuestNowStep = count;
        }

        public void AddQuestData(long count)
        {
            IsChangedData = true;
            QuestNowStep += count;
        }
        public void AddMission(int id, double count)
        {
            IsChangedData = true;

            MissionData missionData = MissionList.Find(item => item.MissionID == id);

            if (missionData == null)
            {
                MissionData newData = new MissionData() { MissionID = id, MissionNowStep = count };
                MissionList.Add(newData);
            }
            else
            {
                missionData.MissionNowStep += count;
                //Debug.Log($"id : {id} , now Step : {missionData.MissionNowStep}");
            }
        }
        public void SetMission(int id, double count)
        {
            IsChangedData = true;

            MissionData missionData = MissionList.Find(item => item.MissionID == id);

            if (missionData == null)
            {
                MissionData newData = new MissionData() { MissionID = id, MissionNowStep = 0 };
                MissionList.Add(newData);
            }
            else
            {
                missionData.MissionNowStep = count;
            }
        }
        public void UpdateMissionIsDone(int missionID, bool isMissionDone)
        {
            IsChangedData = true;

            MissionData itemData = null;
            itemData = MissionList.Find(item => item.MissionID == missionID);
            itemData.MissionIsDone = isMissionDone;
        }

        public void UpdateMissionAdsIsDone(int missionID, bool isAdsDone)
        {
            IsChangedData = true;
            MissionData itemData = null;
            itemData = MissionList.Find(item => item.MissionID == missionID);
            itemData.MissionAdsIsDone = isAdsDone;
        }
        public void UpdateDailyTotalMissionDone(bool flag)
        {
            IsChangedData = true;
            DailyTotalMissionDone = flag;
        }
        public void UpdateDailyTotalMissionAdsIsDone(bool flag)
        {
            IsChangedData = true;
            DailyTotalMissionAdsIsDone = flag;
        }
    }
}