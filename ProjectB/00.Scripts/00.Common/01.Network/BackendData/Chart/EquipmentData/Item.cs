// Copyright 2013-2022 AFI, INC. All rights reserved.

using LitJson;
using UnityEngine;

namespace BackendData.Chart.Weapon {
    //===============================================================
    // Weapon 차트의 각 row 데이터 클래스
    //===============================================================
    public class Item {
        public Item(JsonData json)
        {
            ItemID = int.Parse(json["ItemID"].ToString());
            ItemName = json["ItemName"].ToString();
            ItemGrade = json["ItemGrade"].ToString();
            EquipAttack = double.Parse(json["EquipAttack"].ToString());
            GrowingEquipAttack = double.Parse(json["GrowingEquipAttack"].ToString());
            GetAttack = double.Parse(json["GetAttack"].ToString());
            GrowingGetAttact = double.Parse(json["GrowingGetAttact"].ToString());
            EnchantMaxLevel = int.Parse(json["EnchantMaxLevel"].ToString());
            EnchantNeedCount = double.Parse(json["EnchantNeedCount"].ToString());
            EnchantNeedCountRatio = float.Parse(json["EnchantNeedCountRatio"].ToString());
        }

        public int ItemID { get; private set; }
        public string ItemName { get; private set; }
        public string ItemGrade { get; private set; }
        public Define.EnumEquipmentType ItemType { get; private set; }

        public double EquipAttack { get; private set; }

        public double GrowingEquipAttack { get; private set; }
        public double GetAttack { get; private set; }
        public double GrowingGetAttact { get; private set; }
        public int EnchantMaxLevel { get; private set; }
        public double EnchantNeedCount { get; private set; }
        public float EnchantNeedCountRatio { get; private set; }

        public Sprite ItemSprite;
    }
}