// Copyright 2013-2022 AFI, INC. All rights reserved.

using System;
using System.Collections.Generic;
using LitJson;
using UnityEngine;

namespace BackendData.Chart.PlayerEnhancemetData
{
    //===============================================================
    // Item 차트의 각 row 데이터 클래스
    //===============================================================
    public class Item {
        public int ItemID { get; private set; }
        public string StatName { get; private set; }
        public long MaxLevel { get; private set; }
        public double NeedCount { get; private set; }

        public double NeedCountRatio { get; private set; }

        public string StatUIName { get; private set; }
       
        public double IncreasedAmount { get; private set; }
        //public Define.StatType StatType { get; private set; }

        public Item(JsonData json) {
            ItemID = int.Parse(json["ItemID"].ToString());
            StatName = (json["StatName"].ToString());
            MaxLevel = long.Parse(json["MaxLevel"].ToString());
            NeedCount = double.Parse(json["NeedCount"].ToString());
            NeedCountRatio = double.Parse(json["NeedCountRatio"].ToString());
            StatUIName = json["StatUIName"].ToString();
            IncreasedAmount = double.Parse(json["IncreasedAmount"].ToString());

            //if (!Enum.TryParse<Define.StatType>(json["StatType"].ToString(), out var statType))
            //{
            //    throw new Exception($"Q{ItemID} - 지정되지 않은 StatType 입니다.");  
            //}
            //StatType = statType;
        }
    }
}