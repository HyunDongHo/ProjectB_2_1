// Copyright 2013-2022 AFI, INC. All rights reserved.

using LitJson;
using System;
using UnityEngine;

namespace BackendData.Chart.AdsBuff {
    //===============================================================
    // Weapon 차트의 각 row 데이터 클래스
    //===============================================================
    public enum AdsType
    {
        Attack,
        Hp,
        Gold,
        Exp,
        None,
    }
    public class Item {
        public Item(JsonData json)
        {
            AdsID = int.Parse(json["AdsID"].ToString());
            AdsName = json["AdsName"].ToString();

            if (!Enum.TryParse<AdsType>(json["AdsType"].ToString(), out var adsType))
            {
                throw new Exception($"Q{AdsID} - 지정되지 않은 MissionType 입니다.");
            }

            this.AdsType = adsType;

            BaseBuffStatRatio = double.Parse(json["BaseBuffStatRatio"].ToString());
            StatGrowingRatio = double.Parse(json["StatGrowingRatio"].ToString());
            NeedNextCount = double.Parse(json["NeedNextCount"].ToString());
            NeedGrowingRatio = double.Parse(json["NeedGrowingRatio"].ToString());

        }

        public int AdsID { get; private set; }
        public string AdsName { get; private set; }
        public AdsType AdsType { get; private set; }
        public double BaseBuffStatRatio { get; private set; }
        public double StatGrowingRatio { get; private set; }
        public double NeedNextCount { get; private set; }
        public double NeedGrowingRatio { get; private set; }

    }
}