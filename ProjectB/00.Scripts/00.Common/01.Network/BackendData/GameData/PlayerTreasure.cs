// Copyright 2013-2022 AFI, INC. All rights reserved.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using BackEnd;
using LitJson;
using UnityEngine;

namespace BackendData.GameData {
    //===============================================================
    // UserData 테이블의 데이터를 담당하는 클래스(변수)
    //===============================================================

    public partial class PlayerTreasure
    {
        public List<TreasureData> TreasureList = new List<TreasureData>();
    }

    public class TreasureData
    {
        public int TreasureLevel { get; set; }
        public int TreasureID { get; set; }
        public int TreasureCount { get; set; }        
    }

    //===============================================================
    // UserData 테이블의 데이터를 담당하는 클래스(함수)
    //===============================================================
    public partial class PlayerTreasure : Base.GameData {

        // 데이터가 존재하지 않을 경우, 초기값 설정
        protected override void InitializeData() 
        {
        }

        // Backend.GameData.GetMyData 호출 이후 리턴된 값을 파싱하여 캐싱하는 함수
        // 서버에서 데이터를 불러오늖 함수는 BackendData.Base.GameData의 BackendGameDataLoad() 함수를 참고해주세요
        protected override void SetServerDataToLocal(JsonData gameDataJson) {

            if (gameDataJson.ContainsKey("TreasureList") == true)
                TreasureList = LitJson.JsonMapper.ToObject<List<TreasureData>>(gameDataJson["TreasureList"].ToJson());
        }

        // 테이블 이름 설정 함수
        public override string GetTableName() {
            return "PlayerTreasure";
        }

        // 컬럼 이름 설정 함수
        public override string GetColumnName() {
            return null;
        }

        // 데이터 저장 시 저장할 데이터를 뒤끝에 맞게 파싱하는 함수
        public override Param GetParam() {
            Param param = new Param();
            param.Add("TreasureList", TreasureList);
            return param;
        }
        
        public void AddTreasure(int id, int count)
        {
            IsChangedData = true;

            TreasureData treasureData = TreasureList.Find(item => item.TreasureID == id);

            if(treasureData == null)
            {
                TreasureData newData = new TreasureData() { TreasureID = id, TreasureCount = count, TreasureLevel = 0 };
                TreasureList.Add(newData);
            }
            else
            {
                treasureData.TreasureCount += count;
            }
        }

        public void EnchantTreasure(int id)
        {
            IsChangedData = true;

            TreasureData treasureData = TreasureList.Find(item => item.TreasureID == id);

            if (treasureData != null)
            {
                treasureData.TreasureLevel += 1;
            }
        }

        public void RemoveTreasure()
        {

        }

        public TreasureData GetTreasure(int id)
        {
            TreasureData treasureData = TreasureList.Find(item => item.TreasureID == id);
            return treasureData;
        }

        public double GetTreasureRatio(Define.StatType statItemType)
        {
            // 일단 차트에 있는 ID 얻어오기 
            int id = -1;
            float itemStat = 0;
            foreach (var item in StaticManager.Backend.Chart.Treasure.Dictionary.Values)
            {
                if (item.StatType == statItemType)
                {
                    id = item.ItemID;
                    itemStat = item.ItemStat;
                }
            }
            if (id == -1 || itemStat == 0)
                return 0;

            TreasureData treasureData = TreasureList.Find(item => item.TreasureID == id);
            if (treasureData == null)
                return 0;

            double treasureRatio = treasureData.TreasureLevel * itemStat;
            return treasureRatio;
        }

    }
}