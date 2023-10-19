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

    public partial class PlayerDice
    {
        public Dictionary<string, List<DiceData>> DiceDict = new Dictionary<string, List<DiceData>>();
        public int NowSlotNum;
        public double DiceGem { get; private set; }

    }

    public class DiceData
    {
        public int DiceNum { get; set; }
        public bool IsLock { get; set; }
        public float DiceValue { get; set; }
    }

    //===============================================================
    // UserData 테이블의 데이터를 담당하는 클래스(함수)
    //===============================================================
    public partial class PlayerDice : Base.GameData {

        private const int DiceSlotNumber = 6;
        private const int SlotCount = 4;

        // 데이터가 존재하지 않을 경우, 초기값 설정
        protected override void InitializeData() 
        {
            NowSlotNum = 0;
            DiceGem = 0;
            InitDiceDict();
        }

        // Backend.GameData.GetMyData 호출 이후 리턴된 값을 파싱하여 캐싱하는 함수
        // 서버에서 데이터를 불러오늖 함수는 BackendData.Base.GameData의 BackendGameDataLoad() 함수를 참고해주세요
        protected override void SetServerDataToLocal(JsonData gameDataJson) {

            if (gameDataJson.ContainsKey("DiceDict") == true)
                DiceDict = LitJson.JsonMapper.ToObject<Dictionary<string, List<DiceData>>>(gameDataJson["DiceDict"].ToJson());

            if (gameDataJson.ContainsKey("NowSlotNum") == true)
                NowSlotNum = int.Parse(gameDataJson["NowSlotNum"].ToString());

            DiceGem = gameDataJson.ContainsKey("DiceGem") ? double.Parse(gameDataJson["DiceGem"].ToString()) : 0;

        }

        // 테이블 이름 설정 함수
        public override string GetTableName() {
            return "PlayerDice";
        }

        // 컬럼 이름 설정 함수
        public override string GetColumnName() {
            return null;
        }

        // 데이터 저장 시 저장할 데이터를 뒤끝에 맞게 파싱하는 함수
        public override Param GetParam() {
            Param param = new Param();
            param.Add("DiceDict", DiceDict);
            param.Add("NowSlotNum", NowSlotNum);
            param.Add("DiceGem", DiceGem);

            return param;
        }
        
        // 주사위 인덱스
        public List<DiceData> GetDiceList(int index)
        {
            List<DiceData> diceDatas = null;
            DiceDict.TryGetValue(index.ToString(), out diceDatas);
            return diceDatas;
        }

        public void InitDiceDict()
        {
            if (DiceDict.Count >= SlotCount)
                return;

            IsChangedData = true;

            for (int i = 0; i < SlotCount; ++i)
            {
                List<DiceData> newList = new List<DiceData>();

                for(int j= 0; j < DiceSlotNumber; ++ j)
                    newList.Add(new DiceData() { DiceNum = 0, IsLock = false });

                DiceDict.Add(i.ToString(), newList);
            }
        }

        public void AddDiceData()
        {
            IsChangedData = true;

            for (int i = 0; i < SlotCount; ++i)
            {
                List<DiceData> newList = new List<DiceData>();
                newList.Add(new DiceData() { DiceNum = 0, IsLock = false });
                DiceDict.Add(i.ToString(), newList);
            }
        }

        public void SetDiceList(int index, List<DiceData> diceDatas)
        {
            IsChangedData = true;
            List<DiceData> diceData = null;
            DiceDict.TryGetValue(index.ToString(), out diceData);
            
            for(int i = 0; i < diceDatas.Count; ++i)
            {
                diceData[i].DiceNum = diceDatas[i].DiceNum;
                diceData[i].DiceValue = diceDatas[i].DiceValue;
                diceData[i].IsLock = diceDatas[i].IsLock;
            }
        }

        public void SetChangeSlotNum(int slotNum)
        {
            IsChangedData = true;

            NowSlotNum = slotNum;
        }

        public void SetSlotLock(int slotIndexNum, bool isLock)
        {
            IsChangedData = true;

            List<DiceData> diceData = null;
            DiceDict.TryGetValue(NowSlotNum.ToString(), out diceData);

            if (diceData == null)
                return;
            else
            {
                if (diceData.Count <= slotIndexNum)
                    return;

                diceData[slotIndexNum].IsLock = isLock;
            }
        }
        public double GetDiceRatio(Define.StatType statItemType)
        {
            List<DiceData> diceData = null;
            DiceDict.TryGetValue(NowSlotNum.ToString(), out diceData);

            if (diceData == null)
                return 0;

            List<int> chartItemNums = new();
            foreach (var item in StaticManager.Backend.Chart.DiceRandom.Dictionary.Values)
            {
                if (item.StatType == statItemType)
                    chartItemNums.Add(item.DiceRandomID);

            }

            double totalDiceRatio = 0;
            // list 에 data가 존재하면 계산 
            foreach(DiceData data in diceData)
            {
                int dataDiceNum = data.DiceNum;
                if (chartItemNums.Contains(dataDiceNum))
                    totalDiceRatio += data.DiceValue;
            }
            return totalDiceRatio;
        }
        public void UpdateDiceGem(double count)
        {
            IsChangedData = true;
            DiceGem += count;
        }
    }
}