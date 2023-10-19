// Copyright 2013-2022 AFI, INC. All rights reserved.

using System.Collections.Generic;
using LitJson;
using UnityEngine;

namespace BackendData.Chart.PlayerData {
    //===============================================================
    // Item 차트의 각 row 데이터 클래스
    //===============================================================
    public class Item {
        public int Level { get; private set; }
        public float MaxExp { get; private set; }
        public float MaxHp { get; private set; }
        public float MaxSp { get; private set; }

        public float HpRecovery { get; private set; }

        public float HpRecoveryTime { get; private set; }
        public float DamageRatio { get; private set; }

        public float CriticalRatio { get; private set; }

        public float CriticalPercentage { get; private set; }
        public float AttackSpeedRatio { get; private set; }

        public float DodgePercentage { get; private set; }

        public float MoveSpeedRatio { get; private set; }

        public Item(JsonData json) {
            Level = int.Parse(json["Level"].ToString());
            MaxExp = float.Parse(json["MaxExp"].ToString());
            MaxHp = float.Parse(json["MaxHp"].ToString());
            MaxSp = float.Parse(json["MaxSp"].ToString());
            HpRecovery = float.Parse(json["HpRecovery"].ToString());
            HpRecoveryTime = float.Parse(json["HpRecoveryTime"].ToString());
            DamageRatio = float.Parse(json["DamageRatio"].ToString());
            CriticalRatio = float.Parse(json["CriticalRatio"].ToString());
            CriticalPercentage = float.Parse(json["CriticalPercentage"].ToString());
            AttackSpeedRatio = float.Parse(json["AttackSpeedRatio"].ToString());
            DodgePercentage = float.Parse(json["DodgePercentage"].ToString());
            MoveSpeedRatio = float.Parse(json["MoveSpeedRatio"].ToString());
        }
    }
}