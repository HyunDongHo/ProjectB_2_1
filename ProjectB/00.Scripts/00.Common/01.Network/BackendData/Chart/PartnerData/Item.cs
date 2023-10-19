// Copyright 2013-2022 AFI, INC. All rights reserved.

using LitJson;
using UnityEngine;

namespace BackendData.Chart.Partner {
    //===============================================================
    // Weapon 차트의 각 row 데이터 클래스
    //===============================================================
    public class Item {
        public Item(JsonData json)
        {
            PartnerID = int.Parse(json["PartnerID"].ToString());
            PartnerName = json["PartnerName"].ToString();
            Grade = json["Grade"].ToString();
            BaseTopAbil = double.Parse(json["BaseTopAbil"].ToString());
            GrowingTopAbil = double.Parse(json["GrowingTopAbil"].ToString());
            GrowingGetAttack = double.Parse(json["GrowingGetAttack"].ToString());
            GrowingGetHp = double.Parse(json["GrowingGetHp"].ToString());
            MaxLevel = int.Parse(json["MaxLevel"].ToString());
            UpgradePrice = double.Parse(json["UpgradePrice"].ToString());
            PartnerName = json["PartnerName"].ToString();
            ImagePath = json["ImagePath"].ToString();
            TopAbilGap = double.Parse(json["TopAbilGap"].ToString());
        }

        public int PartnerID { get; private set; }
        public string PartnerName { get; private set; }
        public string Grade { get; private set; }

        public double BaseTopAbil { get; private set; }

        public double GrowingTopAbil { get; private set; }

        public double GrowingGetAttack { get; private set; }
        public double GrowingGetHp { get; private set; }
        public int MaxLevel { get; private set; }
        public double UpgradePrice { get; private set; }
        public string ImagePath { get; private set; }
        public double TopAbilGap { get; private set; }

        public Sprite ItemSprite;
    }
}