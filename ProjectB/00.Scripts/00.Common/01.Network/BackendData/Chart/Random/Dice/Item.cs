// Copyright 2013-2022 AFI, INC. All rights reserved.

using System;
using System.Collections.Generic;
using LitJson;
using UnityEngine;

namespace BackendData.Chart.DiceRandom {
    //===============================================================
    // Item 차트의 각 row 데이터 클래스
    //===============================================================
    public class Item {
        public int DiceRandomID { get; private set; }
        public string Percent { get; private set; }

        public int Grade { get; private set; }
        public Define.StatType StatType { get; private set; }
        public float MinValue { get; private set; }
        public float MaxValue { get; private set; }

        public Item(JsonData json) {
            DiceRandomID = int.Parse(json["DiceRandomID"].ToString());
            Percent = (json["Percent"].ToString());
            Grade = int.Parse(json["Grade"].ToString());
            StatType = (Define.StatType)Enum.Parse(typeof(Define.StatType), json["StatType"].ToString());
            MinValue = float.Parse(json["MinValue"].ToString());
            MaxValue = float.Parse(json["MaxValue"].ToString());
        }
    }
}