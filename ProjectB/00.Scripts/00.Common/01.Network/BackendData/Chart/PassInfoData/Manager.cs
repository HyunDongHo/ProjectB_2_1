// Copyright 2013-2022 AFI, INC. All rights reserved.

using System;
using System.Collections.Generic;
using LitJson;
using Unity.VisualScripting;

namespace BackendData.Chart.PassInfo
{
    //===============================================================
    // Quest 차트의 데이터를 관리하는 클래스
    //===============================================================
    public class Manager : Base.Chart
    {

        // 각 차트의 Item(row) 정보를 담는 Dictionary
        readonly Dictionary<int, Item> _dictionary = new();

        // 다른 클래스에서 Add, Delete등 수정이 불가능하도록 읽기 전용 Dictionary
        public IReadOnlyDictionary<int, Item> Dictionary => (IReadOnlyDictionary<int, Item>)_dictionary.AsReadOnlyCollection();


        // 차트 파일 이름 설정 함수
        // 차트 불러오기를 공통적으로 처리하는 BackendChartDataLoad() 함수에서 해당 함수를 통해 차트 파일 이름을 얻는다.
        public override string GetChartFileName()
        {
            return "PassInfo";
        }

        // Backend.Chart.GetChartContents에서 각 차트 형태에 맞게 파싱하는 클래스
        // 차트 정보 불러오는 함수는 BackendData.Base.Chart의 BackendChartDataLoad를 참고해주세요
        protected override void LoadChartDataTemplate(JsonData json)
        {
            foreach (JsonData eachItem in json)
            {
                Item info = new Item(eachItem);
                _dictionary.Add(info.PassID, info);  
            }
        }
        public Dictionary<int, Item> GetChartPassInfoData()
        {
            Dictionary<int, Item> PassInfoData = new();

            foreach (var data in _dictionary)
            {
                PassInfoData.Add(data.Value.PassID, data.Value);
            }
            return PassInfoData;
        }
        public PremiumType GetPremiumType(PassType passType)
        {
            PremiumType premiumType = PremiumType.None;
            foreach (var data in _dictionary)
            {
                if (data.Value.PassType == passType)
                    premiumType = data.Value.PremiumType;
            }
            return premiumType;
        }
        public int GetPassID(PassType passType)
        {
            int id = 0;
            foreach (var data in _dictionary)
            {
                if (data.Value.PassType == passType)
                    id = data.Value.PassID;
            }
            return id;
        }
        public PassType GetFirstDataPassType()
        {
            return _dictionary[1].PassType;
        }
    }
}