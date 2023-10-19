// Copyright 2013-2022 AFI, INC. All rights reserved.

using System.Collections.Generic;
using LitJson;
using UnityEngine;

namespace BackendData.Chart.EnemyData
{
    //===============================================================
    // Enemy 차트의 각 row 데이터 클래스
    //===============================================================
    public class Item {       
        public int EnemyID { get; private set; }
        public string EnemyName { get; private set; }
        public double MaxHp { get; private set; }
        public double MinDamage { get; private set; }
        public double MaxDamage { get; private set; }
        public double CriticalRatio { get; private set; }
        public double CriticalPercentage { get; private set; }
        public double AttackSpeedRatio { get; private set; }
        public double DodgePercentage { get; private set; }
        public double RewardMin { get; private set; }
        public double RewardMax { get; private set; }
        public double CoreAmount { get; private set; }
        public double GoldAmount { get; private set; }
        public double ExpAmount { get; private set; }
        public Item(JsonData json)
        {
            EnemyID = int.Parse(json["EnemyID"].ToString());
            EnemyName = json["EnemyName"].ToString();
            MaxHp = double.Parse(json["MaxHp"].ToString());
            MinDamage = double.Parse(json["MinDamage"].ToString());
            MaxDamage = double.Parse(json["MaxDamage"].ToString());
            CriticalRatio = double.Parse(json["CriticalRatio"].ToString());
            CriticalPercentage = double.Parse(json["CriticalPercentage"].ToString());
            AttackSpeedRatio = double.Parse(json["AttackSpeedRatio"].ToString());
            DodgePercentage = double.Parse(json["DodgePercentage"].ToString());
            RewardMin = double.Parse(json["RewardMin"].ToString());
            RewardMax = double.Parse(json["RewardMax"].ToString());
            CoreAmount = double.Parse(json["CoreAmount"].ToString());
            GoldAmount = double.Parse(json["GoldAmount"].ToString());
            ExpAmount = double.Parse(json["ExpAmount"].ToString());
        }
    }
}