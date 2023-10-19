// Copyright 2013-2022 AFI, INC. All rights reserved.

using System;
using System.Collections.Generic;
using System.Reflection;
using LitJson;

namespace BackendData.Chart.LevelPass
{


    //===============================================================
    // PassInfo ��Ʈ�� �� row ������ Ŭ����
    //===============================================================
    public class Item
    {
        public double Level { get; private set; }
        public BackendData.Chart.PassInfo.PassType ConditionType { get; private set; }
        public float ConditionCount { get; private set; }
        public int NormalRewardItemNum { get; private set; }
        public float NormalRewardItemCount { get; private set; }
        public int PremiumRewardItemNum { get; private set; }
        public float PremiumRewardItemCount { get; private set; }

        public Item(JsonData json)
        {
            Level = double.Parse(json["Level"].ToString());

            if (!Enum.TryParse<BackendData.Chart.PassInfo.PassType>(json["ConditionType"].ToString(), out var conditionType))
            {
                throw new Exception($"Q{Level} - �������� ���� ConditionType �Դϴ�.");
            }

            this.ConditionType = conditionType;

            ConditionCount = float.Parse(json["ConditionCount"].ToString());
            NormalRewardItemNum = int.Parse(json["NormalRewardItemNum"].ToString());
            NormalRewardItemCount = float.Parse(json["NormalRewardItemCount"].ToString());
            PremiumRewardItemNum = int.Parse(json["PremiumRewardItemNum"].ToString());
            PremiumRewardItemCount = float.Parse(json["PremiumRewardItemCount"].ToString());  
        }
    }
}
