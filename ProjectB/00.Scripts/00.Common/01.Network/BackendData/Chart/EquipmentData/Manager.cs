// Copyright 2013-2022 AFI, INC. All rights reserved.

using System.Collections.Generic;
using LitJson;
using Unity.VisualScripting;
using UnityEngine;

namespace BackendData.Chart.Weapon {
    //===============================================================
    // Weapon 차트의 데이터를 관리하는 클래스
    //===============================================================
    public class Manager : Base.Chart {
        
        // 각 차트의 row 정보를 담는 Dictionary
        readonly Dictionary<int, Item> _dictionary = new ();
        
        // 다른 클래스에서 Add, Delete등 수정이 불가능하도록 읽기 전용 Dictionary
        public IReadOnlyDictionary<int, Item> Dictionary => (IReadOnlyDictionary<int, Item>)_dictionary.AsReadOnlyCollection();
        
        //이미지 캐싱 처리용 Dictionary
        readonly Dictionary<string, Sprite> _weaponImages = new ();

        // 차트 파일 이름 설정 함수
        // 차트 불러오기를 공통적으로 처리하는 BackendChartDataLoad() 함수에서 해당 함수를 통해 차트 파일 이름을 얻는다.
        public override string GetChartFileName() {
            return "Weapon";
        }

        // Backend.Chart.GetChartContents에서 각 차트 형태에 맞게 파싱하는 클래스
        // 차트 정보 불러오는 함수는 BackendData.Base.Chart의 BackendChartDataLoad를 참고해주세요
        protected override void LoadChartDataTemplate(JsonData json) {
            foreach (JsonData eachItem in json) {
                Item info = new Item(eachItem);

                _dictionary.Add(info.ItemID, info);
                info.ItemSprite = base.AddOrGetImageDictionary(_weaponImages, "", info.ItemID.ToString());
            }
        }
        public Item GetItem(int itemID)
        {
            Item item = null;
            Dictionary.TryGetValue(itemID, out item);
            return item;
        }
        public List<Item> GetListItem(int playerID, int playerType)
        {
            List<Item> chartItem_0 = new();
            List<Item> chartItem_1 = new();
            List<Item> chartItem_2 = new();  
            foreach (var key in Dictionary)
            {
                int id = key.Value.ItemID / playerID;
                switch (id)
                {
                    case 1 :
                        chartItem_0.Add(key.Value);  
                        break;
                    case 2:
                        chartItem_1.Add(key.Value);
                        break;
                    case 3:
                        chartItem_2.Add(key.Value);    
                        break;

                }
            }
            if (playerType == 0)
                return chartItem_0;
            if (playerType == 1)
                return chartItem_1;
            if (playerType == 2)
                return chartItem_2;

            return null;
        }

        public List<BackendData.GameData.ItemData> GetPlayerEquipment(int playerID, int playerType)
        {
            List<BackendData.GameData.ItemData> _playerItem_0 = new();
            List<BackendData.GameData.ItemData> _playerItem_1 = new();
            List<BackendData.GameData.ItemData> _playerItem_2 = new();

            for (int i = 0; i < StaticManager.Backend.GameData.PlayerEquipment.WeaponList.Count; i++)
            {
                int id = StaticManager.Backend.GameData.PlayerEquipment.WeaponList[i].ItemID / playerID;
                switch (id)
                {
                    case 1:
                        _playerItem_0.Add(StaticManager.Backend.GameData.PlayerEquipment.WeaponList[i]);
                        break;
                    case 2:
                        _playerItem_1.Add(StaticManager.Backend.GameData.PlayerEquipment.WeaponList[i]);
                        break;
                    case 3:
                        _playerItem_2.Add(StaticManager.Backend.GameData.PlayerEquipment.WeaponList[i]);
                        break;
                }
            }
            if (playerType == 0)
                return _playerItem_0;
            if (playerType == 1)
                return _playerItem_1;
            if (playerType == 2)
                return _playerItem_2;  

            return null;
        }

    }
}