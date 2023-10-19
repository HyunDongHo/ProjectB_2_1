// Copyright 2013-2022 AFI, INC. All rights reserved.

using System.Collections.Generic;
using LitJson;

namespace BackendData.Chart.StageDrop {
    //===============================================================
    // Stage 차트의 각 row 데이터 클래스
    //===============================================================
    public class Item {
        public class DropItemInfo
        {
            public int ItemID { get; private set; }
            public float Percent { get; private set; }
            public double Count { get; private set; }

            public DropItemInfo(int itemID, float percent, double count)
            {
                ItemID = itemID;
                Percent = percent;
                Count = count;
            }
        }

        public long StageLevel { get; private set; }
        public List<DropItemInfo> DropItemList { get; private set; }
      
        public Item(JsonData json){
            StageLevel = long.Parse(json["StageLevel"].ToString());
      
            string stageDropListString = json["DropItemList"].ToString();
            JsonData stageDropListJson = JsonMapper.ToObject(stageDropListString);

            DropItemList = new List<DropItemInfo>();

            foreach (JsonData dropItem in stageDropListJson)
            {
                int id = int.Parse(dropItem["id"].ToString());
                float percent = float.Parse(dropItem["percent"].ToString());
                double count = double.Parse(dropItem["count"].ToString());
                DropItemList.Add(new DropItemInfo(id, percent, count));
            }


        }
    }
}