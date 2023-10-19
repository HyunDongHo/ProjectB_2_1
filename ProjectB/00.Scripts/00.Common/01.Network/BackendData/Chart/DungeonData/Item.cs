// Copyright 2013-2022 AFI, INC. All rights reserved.

using System;
using System.Collections.Generic;
using LitJson;

namespace BackendData.Chart.DungeonData {
    //===============================================================
    // Stage 차트의 각 row 데이터 클래스
    //===============================================================
    public enum DungeonType
    {
        Training,
        Gold,
    }

    public enum RewardItemType
    {
        Warrior,
        Archer,
        Wizard,
        None,
    }

    public class Item {
        public class RewardItemInfo
        {
            public int ItemID { get; private set; }

            public RewardItemInfo(int itemID)
            {
                ItemID = itemID;
            }
        }

        public int DungeonID { get; private set; }
        public DungeonType DungeonType { get; private set; }
        public RewardItemType RewardItemType { get; private set; }

        public List<RewardItemInfo> RewardItemIDList { get; private set; }

        public string DungeonUIName { get; private set; }
        public Item(JsonData json){

            DungeonID = int.Parse(json["DungeonID"].ToString());


            if (!Enum.TryParse<DungeonType>(json["DungeonType"].ToString(), out var dungeonType))
            {
                throw new Exception($"Q{DungeonID} - 지정되지 않은 DungeonType 입니다.");
            }

            this.DungeonType = dungeonType;

            if (!Enum.TryParse<RewardItemType>(json["RewardItemType"].ToString(), out var rewardItemType))
            {
                throw new Exception($"Q{DungeonID} - 지정되지 않은 RewardItemType 입니다.");
            }

            this.RewardItemType = rewardItemType;

            string rewardListString = json["RewardItemIDList"].ToString();
            JsonData rewardListJson = JsonMapper.ToObject(rewardListString);

            RewardItemIDList = new List<RewardItemInfo>();

            foreach (JsonData Item in rewardListJson)
            {
                int id = int.Parse(Item["id"].ToString());
                RewardItemIDList.Add(new RewardItemInfo(id));
            }

            DungeonUIName = json["DungeonUIName"].ToString();  
        }
    }
}