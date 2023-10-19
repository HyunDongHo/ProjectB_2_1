// Copyright 2013-2022 AFI, INC. All rights reserved.

using System.Collections.Generic;
using LitJson;

namespace BackendData.Chart.TopFloor {
    //===============================================================
    // Stage 차트의 각 row 데이터 클래스
    //===============================================================
    public class Item {
        public class TopRewardInfo
        {
            public int ItemID { get; private set; }
            public double Count { get; private set; }

            public TopRewardInfo(int itemID,  double count)
            {
                ItemID = itemID;
                Count = count;
            }
        }

        public long FloorLevel { get; private set; }
        public List<TopRewardInfo> RewardItemList { get; private set; }
        public double NeedBaseAbil { get; private set; }

        public Item(JsonData json){
            FloorLevel = long.Parse(json["FloorLevel"].ToString());
      
            string topRewardListString = json["RewardItemList"].ToString();
            JsonData topRewardListJson = JsonMapper.ToObject(topRewardListString);

            RewardItemList = new List<TopRewardInfo>();

            foreach (JsonData dropItem in topRewardListJson)
            {
                int id = int.Parse(dropItem["id"].ToString());
                double count = double.Parse(dropItem["count"].ToString());
                RewardItemList.Add(new TopRewardInfo(id,  count));
            }

            NeedBaseAbil = double.Parse(json["NeedBaseAbil"].ToString());  
        }
    }
}