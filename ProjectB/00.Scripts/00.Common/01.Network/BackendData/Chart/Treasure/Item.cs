// Copyright 2013-2022 AFI, INC. All rights reserved.

using System;
using System.Collections.Generic;
using LitJson;
using UnityEngine;

namespace BackendData.Chart.Treasure {
    //===============================================================
    // Item 차트의 각 row 데이터 클래스
    //===============================================================
    public class Item {
        public int ItemID { get; private set; }
        public string ItemName { get; private set; }
        public string ItemSprite { get; private set; }
        public float ItemStat { get; private set; }
        public Define.StatType StatType { get; private set; }
        public int MaxLevel { get; private set; }

        public Sprite Sprite;

        public Item(JsonData json) {

            ItemID = int.Parse(json["ItemID"].ToString());
            ItemName = json["ItemName"].ToString();

            if (!Enum.TryParse <Define.StatType> (json["StatType"].ToString(), out var statType))
            {
                throw new Exception($"Q{ItemID} - 지정되지 않은 StatType 입니다.");
            }
            StatType = statType;
            ItemStat = float.Parse(json["ItemStat"].ToString());
            MaxLevel = int.Parse(json["MaxLevel"].ToString());
            ItemSprite = json["ItemSprite"].ToString();
        }
    }
}