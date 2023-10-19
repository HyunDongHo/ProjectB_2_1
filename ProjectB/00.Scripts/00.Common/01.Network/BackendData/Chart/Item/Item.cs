// Copyright 2013-2022 AFI, INC. All rights reserved.

using System;
using System.Collections.Generic;
using LitJson;
using UnityEngine;

namespace BackendData.Chart.Item {
    //===============================================================
    // Item 차트의 각 row 데이터 클래스
    //===============================================================
    public class Item {
        public int ItemID { get; private set; }
        public string ItemName { get; private set; }
        public string ItemSprite { get; private set; }
        public string ItemTextColor { get; private set; } // 등급별 색깔

        public Define.ItemType ItemType { get; private set; }

        public Sprite ImageSprite { get; set; }
        

        public Dictionary<string, float> ItemStat { get; private set; }

        public Item(JsonData json) {
            ItemID = int.Parse(json["ItemID"].ToString());
            ItemName = json["ItemName"].ToString();
            ItemSprite = json["ItemSprite"].ToString();
            ItemTextColor = json["ItemTextColor"].ToString();

            ItemType = (Define.ItemType)Enum.Parse(typeof(Define.ItemType), json["ItemType"].ToString());
        }
    }
}