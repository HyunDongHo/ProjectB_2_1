// Copyright 2013-2022 AFI, INC. All rights reserved.

using System;
using System.Collections.Generic;
using LitJson;
using UnityEngine;

namespace BackendData.Chart.TreasureEnchant {
    //===============================================================
    // Item 차트의 각 row 데이터 클래스
    //===============================================================
    public class Item {
        public int Level { get; private set; }
        public float Success { get; private set; }
        public float Destroy { get; private set; }

        public Item(JsonData json) {
            Level = int.Parse(json["Level"].ToString());
            Success = float.Parse(json["Success"].ToString());
            Destroy = float.Parse(json["Destroy"].ToString());
        }
    }
}