// Copyright 2013-2022 AFI, INC. All rights reserved.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using BackEnd;
using LitJson;
using UnityEngine;

namespace BackendData.GameData {
    //===============================================================
    // UserData 테이블의 데이터를 담당하는 클래스(변수)
    //===============================================================
    public partial class PlayerPartner
    {
        public List<PartnerData> PartnerList = new List<PartnerData>();
        public double PartnerGem { get; private set; }
        public double TopGem { get; private set; }
        public string LastDispatchTime { get; private set; }

        public Dictionary<string, int> DispatchParterList { get; private set; }
        //public bool IsGetReward { get; private set; }
    }
    public class PartnerData
    {
        public int PartnerID { get; set; }
        public int PartnerLevel { get; set; }
        public int PartnerCount { get; set; }

    }

    //===============================================================
    // UserData 테이블의 데이터를 담당하는 클래스(함수)
    //===============================================================
    public partial class PlayerPartner : Base.GameData {
        
        // 데이터가 존재하지 않을 경우, 초기값 설정
        protected override void InitializeData() {
            PartnerGem = 0;
            TopGem = 0;
            DispatchParterList = new Dictionary<string, int>() { { "PartnerID_0", -1 }, { "PartnerID_1", -1 }, { "PartnerID_2", -1 }, { "PartnerID_3", -1 }, { "PartnerID_4", -1 } };
            //IsGetReward = false;
            LastDispatchTime = String.Empty;
        }

        // Backend.GameData.GetMyData 호출 이후 리턴된 값을 파싱하여 캐싱하는 함수
        // 서버에서 데이터를 불러오늖 함수는 BackendData.Base.GameData의 BackendGameDataLoad() 함수를 참고해주세요
        protected override void SetServerDataToLocal(JsonData gameDataJson)
        {
            PartnerGem = gameDataJson.ContainsKey("PartnerGem") ? double.Parse(gameDataJson["PartnerGem"].ToString()) : 0;
            TopGem = gameDataJson.ContainsKey("TopGem") ? double.Parse(gameDataJson["TopGem"].ToString()) : 0;
            //LastDispatchTime = gameDataJson.ContainsKey("LastDispatchTime") ? gameDataJson["LastDispatchTime"].ToString() : string.Format("{0:MM-DD:HH:mm:ss.fffZ}", DateTime.Now.ToString(CultureInfo.InvariantCulture));
            LastDispatchTime = gameDataJson.ContainsKey("LastDispatchTime") ? gameDataJson["LastDispatchTime"].ToString() : String.Empty;
            //IsGetReward = gameDataJson.ContainsKey("IsGetReward") ? bool.Parse(gameDataJson["IsGetReward"].ToString()) : false;

            if (gameDataJson.ContainsKey("PartnerList") == true && gameDataJson["PartnerList"].Count > 0)
            {

                PartnerList = LitJson.JsonMapper.ToObject<List<PartnerData>>(gameDataJson["PartnerList"].ToJson());
            }

            if (gameDataJson.ContainsKey("DispatchParterList") == true)
            {
                DispatchParterList = new Dictionary<string, int>();
                foreach (var column in gameDataJson["DispatchParterList"].Keys)
                {
                    DispatchParterList.Add(column, int.Parse(gameDataJson["DispatchParterList"][column].ToString()));
                }
            }
            else
            {
                DispatchParterList = new Dictionary<string, int>() { { "PartnerID_0", -1 }, { "PartnerID_1", -1 }, { "PartnerID_2", -1 }, { "PartnerID_3", -1 }, { "PartnerID_4", -1 } };  
            }
        }

        // 테이블 이름 설정 함수
        public override string GetTableName() {
            return "PlayerPartner";
        }

        // 컬럼 이름 설정 함수
        public override string GetColumnName() {
            return null;
        }

        // 데이터 저장 시 저장할 데이터를 뒤끝에 맞게 파싱하는 함수
        public override Param GetParam() {
            Param param = new Param();

            param.Add("PartnerList", PartnerList);
            param.Add("PartnerGem", PartnerGem);
            param.Add("TopGem", TopGem);
            param.Add("LastDispatchTime", LastDispatchTime);    
            param.Add("DispatchParterList", DispatchParterList);    
            //param.Add("IsGetReward", IsGetReward);    

            return param;
        }
        
        // 적 처치 횟수를 갱신하는 함수
        public void CountDefeatEnemy() {
        }

        // 유저의 정보를 변경하는 함수
        public void UpdateUserData() {
            IsChangedData = true;
           

        }
        public void ChangePartnerData(int partnerID, double needCount)
        {
            IsChangedData = true;
            PartnerGem -= needCount;

            BackendData.GameData.PartnerData partnerData = null;
            partnerData = PartnerList.Find(item => item.PartnerID == partnerID);
            partnerData.PartnerLevel++;

        }
        public void ChangeTopData(List<int> partners_slot)
        {
            IsChangedData = true;
            if (partners_slot == null)
                return;

            for (int i = 0; i < DispatchParterList.Count; i++)
            {
                DispatchParterList["PartnerID_" + i] = partners_slot[i];
            }


            TopGem -= 1;
            LastDispatchTime = string.Format("{0:MM-DD:HH:mm:ss.fffZ}", DateTime.Now.ToString(CultureInfo.InvariantCulture));

        }
        public void ResetLastDispatchTime()
        {
            IsChangedData = true;
            LastDispatchTime = String.Empty;
        }

        public bool IsHaveItem(int partnerID)
        {
            return PartnerList.Find(item => item.PartnerID == partnerID) != null ? true : false;
        }
    }
}