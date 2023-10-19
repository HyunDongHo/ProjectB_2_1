// Copyright 2013-2022 AFI, INC. All rights reserved.

using System.Collections.Generic;
using LitJson;

namespace BackendData.Chart.StageEnemyData
{
    //===============================================================
    // Stage 차트의 각 row 데이터 클래스
    //===============================================================
    public class Item {
      
        public int ItemID { get; private set; }
        public string TargetEnemy { get; private set; }
        public int A_Normal { get; private set; }
        public int A_Middle { get; private set; }
        public int A_Boss { get; private set; }
        public int SectorNum { get; private set; }
        public Item(JsonData json){
            ItemID = int.Parse(json["ItemID"].ToString());
            TargetEnemy = json["TargetEnemy"].ToString();

            A_Normal = int.Parse(json["A_Normal"].ToString());

            A_Middle = int.Parse(json["A_Middle"].ToString());

            A_Boss = int.Parse(json["A_Boss"].ToString());

            SectorNum = int.Parse(json["SectorNum"].ToString());
        }
    }
}