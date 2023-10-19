// Copyright 2013-2022 AFI, INC. All rights reserved.

using System;
using System.Collections.Generic;
using System.Globalization;
using BackEnd;
using LitJson;
using UnityEngine;

namespace BackendData.GameData
{
    //===============================================================
    // UserData 테이블의 데이터를 담당하는 클래스(변수)
    //===============================================================

    public partial class PlayerEquipment
    {
        public List<ItemData> WeaponList = new List<ItemData>();

        public int SwordSpawnLevel { get; private set; }
        public int BowSpawnLevel { get; private set; }
        public int StaffSpawnLevel { get; private set; }
        public long SwordSpawnCount { get; private set; }
        public long BowSpawnCount { get; private set; }
        public long StafSpawnCount { get; private set; }

        public double EquipGem { get; private set; }
    }

    public class ItemData
    {
        public int ItemID { get; set; }
        public int ItemLevel { get; set; }
        public int ItemCount { get; set; }
        public bool ItemIsEquip { get; set; }
        public Define.EnumEquipmentType EquipmentType { get; set; }
    }

    //===============================================================
    // UserData 테이블의 데이터를 담당하는 클래스(함수)
    //===============================================================
    public partial class PlayerEquipment : Base.GameData
    {

        private const int StartSwordID = 101;
        // 데이터가 존재하지 않을 경우, 초기값 설정
        protected override void InitializeData()
        {
            WeaponList.Add(new ItemData() { ItemID = StartSwordID, ItemCount = 0, ItemLevel = 1, ItemIsEquip = true, EquipmentType = Define.EnumEquipmentType.Sword });

            SwordSpawnLevel = 1;
            SwordSpawnCount = 0;

            BowSpawnLevel = 1;
            BowSpawnCount = 0;

            StaffSpawnLevel = 1;
            StafSpawnCount = 0;

            EquipGem = 0;
        }

        // Backend.GameData.GetMyData 호출 이후 리턴된 값을 파싱하여 캐싱하는 함수
        // 서버에서 데이터를 불러오늖 함수는 BackendData.Base.GameData의 BackendGameDataLoad() 함수를 참고해주세요
        protected override void SetServerDataToLocal(JsonData gameDataJson)
        {

            if (gameDataJson.ContainsKey("WeaponList") == true)
                WeaponList = LitJson.JsonMapper.ToObject<List<ItemData>>(gameDataJson["WeaponList"].ToJson());
            else
                WeaponList.Add(new ItemData() { ItemID = StartSwordID, ItemCount = 0, ItemLevel = 1, ItemIsEquip = true, EquipmentType = Define.EnumEquipmentType.Sword });

            SwordSpawnLevel = int.Parse(gameDataJson["SwordSpawnLevel"].ToString());
            BowSpawnLevel = int.Parse(gameDataJson["BowSpawnLevel"].ToString());
            StaffSpawnLevel = int.Parse(gameDataJson["StaffSpawnLevel"].ToString());

            SwordSpawnCount = long.Parse(gameDataJson["SwordSpawnCount"].ToString());
            BowSpawnCount = long.Parse(gameDataJson["BowSpawnCount"].ToString());
            StafSpawnCount = long.Parse(gameDataJson["StafSpawnCount"].ToString());

            EquipGem = gameDataJson.ContainsKey("EquipGem") ? double.Parse(gameDataJson["EquipGem"].ToString()) : 0;  

        }

        // 테이블 이름 설정 함수
        public override string GetTableName()
        {
            return "PlayerEquipment";
        }

        // 컬럼 이름 설정 함수
        public override string GetColumnName()
        {
            return null;
        }

        // 데이터 저장 시 저장할 데이터를 뒤끝에 맞게 파싱하는 함수
        public override Param GetParam()
        {
            Param param = new Param();

            param.Add("WeaponList", WeaponList);

            param.Add("SwordSpawnLevel", SwordSpawnLevel);
            param.Add("BowSpawnLevel", BowSpawnLevel);
            param.Add("StaffSpawnLevel", StaffSpawnLevel);

            param.Add("SwordSpawnCount", SwordSpawnCount);
            param.Add("BowSpawnCount", BowSpawnCount);
            param.Add("StafSpawnCount", StafSpawnCount);

            param.Add("EquipGem", EquipGem);
            return param;
        }

        // 장비 교체 함수
        public void ChangeEquipment(int weaponID , double needCount)
        {
            IsChangedData = true;
            EquipGem -= needCount;

            BackendData.GameData.ItemData itemData = null;
            itemData = WeaponList.Find(item => item.ItemID == weaponID);
            itemData.ItemLevel++;

            //if (itemData == null)
            //{
            //    WeaponList.Add(new ItemData() { ItemID = weaponID, ItemLevel = 1, ItemIsEquip = false, ItemCount = 0 });
            //}
            //else
            //{
            //    itemData.ItemLevel++;
            //}  
        }
        public void ChangeSpawnCount(int weapon, int armor, int helmet)
        {
            IsChangedData = true;

            SwordSpawnCount += weapon;
            BowSpawnCount += armor;
            StafSpawnCount += helmet;
        }

        public void UpdateCompose(List<ItemData> list)
        {
            IsChangedData = true;
            WeaponList = list;
        }
        public void UpdateItemIsEquip(int weaponID, bool isItemEquip)
        {
            IsChangedData = true;

            BackendData.GameData.ItemData itemData = null;
            itemData = WeaponList.Find(item => item.ItemID == weaponID);
            itemData.ItemIsEquip = isItemEquip;
        }

        // 유저의 정보를 변경하는 함수
        public void UpdateUserData()
        {
            IsChangedData = true;

        }
        public void UpdateEquipGem(double count)
        {
            IsChangedData = true;
            EquipGem += count;
        }
        public ItemData GetItem(int itemID)
        {
            for (int i = 0; i < WeaponList.Count; ++i)
            {
                if (WeaponList[i].ItemID == itemID)
                    return WeaponList[i];
            }               

            return null;
        }

        public bool IsHaveItem(int itemID)
        {          
            for (int i = 0; i < WeaponList.Count; ++i)
            {
                if (WeaponList[i].ItemID == itemID)
                    return true;
            }
             
            return false;
        }

        public int GetEquipID(Define.EnumEquipmentType equipmentType)
        {
            for (int i = 0; i < WeaponList.Count; ++i)
            {
                if (WeaponList[i].ItemIsEquip == true && equipmentType == WeaponList[i].EquipmentType)
                    return WeaponList[i].ItemID;
            }

            return 0;
        }

    }
}