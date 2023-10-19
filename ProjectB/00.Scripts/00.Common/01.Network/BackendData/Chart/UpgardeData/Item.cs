// Copyright 2013-2022 AFI, INC. All rights reserved.

using System;
using System.Collections.Generic;
using LitJson;
using UnityEngine;

namespace BackendData.Chart.Upgrade
{
    //===============================================================
    // Item 차트의 각 row 데이터 클래스
    //===============================================================

    public class UpgradeStats
    {
        public Define.StatType StatType { get; private set; }
        public double AbilValue { get; private set; }

        public UpgradeStats(Define.StatType type, double value)
        {
            StatType = type;
            AbilValue = value;
        }
    }


    public class Item {
        public int ItemID { get; private set; }
        public PlayerType PlayerType { get; private set; }
        public List<UpgradeStats> UpgradeStats { get; private set; }
        public int Level { get; private set; }
        public double NeedCount { get; private set; }
        public string ModelName { get; private set; }

        public Item(JsonData json) {

            ItemID = int.Parse(json["ItemID"].ToString());
            Level = int.Parse(json["Level"].ToString());
            NeedCount = double.Parse(json["NeedCount"].ToString());

            if (!Enum.TryParse <PlayerType> (json["PlayerType"].ToString(), out var statType))
            {
                throw new Exception($"Q{ItemID} - 지정되지 않은 StatType 입니다.");
            }

            PlayerType = statType;

            UpgradeStats = new List<UpgradeStats>();

            string statTypes = json["StatTypes"].ToString();
            JsonData statTypesJson = JsonMapper.ToObject(statTypes);

            foreach (JsonData stat in statTypesJson)
            {
                Define.StatType type = (Define.StatType)Enum.Parse(typeof(Define.StatType), stat["StatType"].ToString());
                double value = double.Parse(stat["AbilValue"].ToString());
                UpgradeStats.Add(new UpgradeStats(type, value));
            }

            ModelName = json["ModelName"].ToString();
        }
    }
}