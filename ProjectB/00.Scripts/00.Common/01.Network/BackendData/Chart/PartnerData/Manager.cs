// Copyright 2013-2022 AFI, INC. All rights reserved.

using System.Collections.Generic;
using LitJson;
using Unity.VisualScripting;
using UnityEngine;

namespace BackendData.Chart.Partner {
    //===============================================================
    // Weapon 차트의 데이터를 관리하는 클래스
    //===============================================================
    public class Manager : Base.Chart {
        
        // 각 차트의 row 정보를 담는 Dictionary
        readonly Dictionary<int, Item> _dictionary = new ();
        
        // 다른 클래스에서 Add, Delete등 수정이 불가능하도록 읽기 전용 Dictionary
        public IReadOnlyDictionary<int, Item> Dictionary => (IReadOnlyDictionary<int, Item>)_dictionary.AsReadOnlyCollection();
        
        //이미지 캐싱 처리용 Dictionary
        readonly Dictionary<string, Sprite> _avatarImages = new ();

        // 차트 파일 이름 설정 함수
        // 차트 불러오기를 공통적으로 처리하는 BackendChartDataLoad() 함수에서 해당 함수를 통해 차트 파일 이름을 얻는다.
        public override string GetChartFileName() {
            return "Partner";
        }

        // Backend.Chart.GetChartContents에서 각 차트 형태에 맞게 파싱하는 클래스
        // 차트 정보 불러오는 함수는 BackendData.Base.Chart의 BackendChartDataLoad를 참고해주세요
        protected override void LoadChartDataTemplate(JsonData json) {
            foreach (JsonData eachItem in json) {
                Item info = new Item(eachItem);

                _dictionary.Add(info.PartnerID, info);
                info.ItemSprite = base.AddOrGetImageDictionary(_avatarImages, "", info.PartnerID.ToString());
            }
        }
        public Item GetItem(int itemID)
        {
            Item item = null;
            Dictionary.TryGetValue(itemID, out item);
            return item;
        }
        public List<Item> GetChartALlItem()
        {
            List<Item> chartItem_All = new();

            foreach (var key in Dictionary)
            {
                chartItem_All.Add(key.Value);
            }

            return chartItem_All;
        }
        public List<Item> GetChartListItem(PartnerGradeType partnerGradeType)
        {
            List<Item> chartItem_D = new();
            List<Item> chartItem_C = new();
            List<Item> chartItem_B = new();
            List<Item> chartItem_A = new();
            List<Item> chartItem_S = new();
            foreach (var key in Dictionary)
            {
                switch (partnerGradeType)
                {
                    case PartnerGradeType.D:
                        chartItem_D.Add(key.Value);
                        break;
                    case PartnerGradeType.C:
                        chartItem_C.Add(key.Value);
                        break;
                    case PartnerGradeType.B:
                        chartItem_B.Add(key.Value);
                        break;
                    case PartnerGradeType.A:
                        chartItem_A.Add(key.Value);
                        break;
                    case PartnerGradeType.S:
                        chartItem_S.Add(key.Value);
                        break;

                }
            }

            if (partnerGradeType == PartnerGradeType.D)
                return chartItem_D;
            if (partnerGradeType == PartnerGradeType.C)
                return chartItem_C;
            if (partnerGradeType == PartnerGradeType.B)
                return chartItem_B;
            if (partnerGradeType == PartnerGradeType.A)
                return chartItem_A;
            if (partnerGradeType == PartnerGradeType.S)
                return chartItem_S;

            return null;
        }


        public List<BackendData.GameData.PartnerData> GetPlayerPartnerList(int playerID, PartnerGradeType partnerGradeType)
        {
            List<BackendData.GameData.PartnerData> playerItem_D = new();
            List<BackendData.GameData.PartnerData> playerItem_C = new();
            List<BackendData.GameData.PartnerData> playerItem_B = new();
            List<BackendData.GameData.PartnerData> playerItem_A = new();
            List<BackendData.GameData.PartnerData> playerItem_S = new();


            for (int i = 0; i < StaticManager.Backend.GameData.PlayerPartner.PartnerList.Count; i++)
            {
                int id = StaticManager.Backend.GameData.PlayerPartner.PartnerList[i].PartnerID / playerID;
                switch (id)
                {
                    case 1:
                        playerItem_D.Add(StaticManager.Backend.GameData.PlayerPartner.PartnerList[i]);
                        break;
                    case 2:
                        playerItem_C.Add(StaticManager.Backend.GameData.PlayerPartner.PartnerList[i]);
                        break;
                    case 3:
                        playerItem_B.Add(StaticManager.Backend.GameData.PlayerPartner.PartnerList[i]);
                        break;
                    case 4:
                        playerItem_A.Add(StaticManager.Backend.GameData.PlayerPartner.PartnerList[i]);
                        break;
                    case 5:
                        playerItem_S.Add(StaticManager.Backend.GameData.PlayerPartner.PartnerList[i]);
                        break;
                }
            }

            if (partnerGradeType == PartnerGradeType.D)
                return playerItem_D;
            if (partnerGradeType == PartnerGradeType.C)
                return playerItem_C;
            if (partnerGradeType == PartnerGradeType.B)
                return playerItem_B;
            if (partnerGradeType == PartnerGradeType.A)
                return playerItem_A;
            if (partnerGradeType == PartnerGradeType.S)
                return playerItem_S;


            return null;
        }

    }
}