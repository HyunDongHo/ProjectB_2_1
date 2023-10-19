// Copyright 2013-2022 AFI, INC. All rights reserved.

using System.Collections.Generic;
using LitJson;
using UnityEngine;

namespace BackendData.Chart.PlayerActiveSkillData
{
    //===============================================================
    // Item 차트의 각 row 데이터 클래스
    //===============================================================
    public class Item {
        public int ItemID { get; private set; }
        public string SkillName { get; private set; }
        public int MaxLevel { get; private set; }
        public double DamageInfo { get; private set; }
        public double DamageRatio { get; private set; }
        public double NeedCount { get; private set; }

        public double NeedCountRatio { get; private set; }

        public string SkillUIName { get; private set; }
        public int PlayerType { get; private set; }
        public int SkillNum { get; private set; }
       

        public Item(JsonData json) {
            ItemID = int.Parse(json["ItemID"].ToString());
            SkillName = (json["SkillName"].ToString());
            MaxLevel = int.Parse(json["MaxLevel"].ToString());
            DamageInfo = double.Parse(json["DamageInfo"].ToString());
            DamageRatio = double.Parse(json["DamageRatio"].ToString());
            NeedCount = double.Parse(json["NeedCount"].ToString());
            NeedCountRatio = double.Parse(json["NeedCountRatio"].ToString());
            SkillUIName = json["SkillUIName"].ToString();
            PlayerType = int.Parse(json["PlayerType"].ToString());
            SkillNum = int.Parse(json["SkillNum"].ToString());            
        }
    }
}