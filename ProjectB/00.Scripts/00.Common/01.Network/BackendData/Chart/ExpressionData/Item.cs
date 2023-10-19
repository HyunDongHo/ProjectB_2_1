// Copyright 2013-2022 AFI, INC. All rights reserved.

using System;
using System.Collections.Generic;
using LitJson;
using UnityEngine;


namespace BackendData.Chart.Expression {
    //===============================================================
    // Enemy 차트의 각 row 데이터 클래스
    //===============================================================
    public class Item {
        public Define.ExpressionType ExpressionType { get; private set; }
        public float A { get; private set; }
        public float B { get; private set; }
        public float C { get; private set; }

        public Item(JsonData json) {
            ExpressionType = (Define.ExpressionType)Enum.Parse(typeof(Define.ExpressionType), json["ExpressionType"].ToString());
            A = float.Parse(json["A"].ToString());
            B = float.Parse(json["B"].ToString());
            C = float.Parse(json["C"].ToString());
        }
    }
}