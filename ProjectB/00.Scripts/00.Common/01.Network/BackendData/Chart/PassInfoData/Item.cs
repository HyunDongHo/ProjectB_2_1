// Copyright 2013-2022 AFI, INC. All rights reserved.

using System;
using System.Collections.Generic;
using System.Reflection;
using LitJson;

namespace BackendData.Chart.PassInfo
{
    public enum PassType
    {
        Level,
        Stage,
        Attend,
        None,
    }

    public enum PremiumType
    {
        None,
        Buy,
        Ads,
    }

    //===============================================================
    // PassInfo ��Ʈ�� �� row ������ Ŭ����
    //===============================================================
    public class Item
    {
        public int PassID { get; private set; }
        public string EndTime { get; private set; }
        public PassType PassType { get; private set; }
        public PremiumType PremiumType { get; private set; }
        public string PassTitleName { get; private set; }
        public string PassContentTitleName { get; private set; }

        public Item(JsonData json)
        {
            PassID = int.Parse(json["PassID"].ToString());
            EndTime = json["EndTime"].ToString();

            if (!Enum.TryParse<PassType>(json["PassType"].ToString(), out var passType))
            {
                throw new Exception($"Q{PassID} - �������� ���� PassType �Դϴ�.");
            }

            this.PassType = passType;

            if (!Enum.TryParse<PremiumType>(json["PremiumType"].ToString(), out var premiumType))
            {
                throw new Exception($"Q{PassID} - �������� ���� PremiumType �Դϴ�.");
            }

            this.PremiumType = premiumType;
            PassTitleName = json["PassTitleName"].ToString();
            PassContentTitleName = json["PassContentTitleName"].ToString();

        }
    }
}
