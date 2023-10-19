// Copyright 2013-2022 AFI, INC. All rights reserved.

using System;
using System.Collections.Generic;
using LitJson;
using UnityEngine;

namespace BackendData.Chart.TreasureRandom {
    //===============================================================
    // Item 차트의 각 row 데이터 클래스
    //===============================================================
    public class Item {
        public int ItemID { get; private set; }
        public float Freq01 { get; private set; }

        public Item(JsonData json) {
            ItemID = int.Parse(json["ItemID"].ToString());
            Freq01 = float.Parse(json["Freq01"].ToString());

        }
    }
}