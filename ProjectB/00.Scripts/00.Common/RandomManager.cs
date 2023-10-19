using BackendData.GameData;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class RandomManager : MonoBehaviour
{
    RandomEquipSpawnData _randomEquipSpawnData;
    RandomEquipSpawnData _randomPartnerSpawnData;
    RandomDiceData _randomDiceData;
    RandomTreasureEnchantData _treasureEnhancementRandomData;
    RandomTreasureSpawnData _randomTreasureSpawnData;

   // private void Awake()
   // {
   //     SetTreasureEnhancementRandomData();
   // }

    #region 장비 랜덤 확률
    public List<int> GetEquipRandomList(Define.EnumSpawnItemType enumEquipmentType, int spawnLevel, int count)
    {
        if (_randomEquipSpawnData == null)
        {
            _randomEquipSpawnData = Resources.Load<RandomEquipSpawnData>("RandomEquipSpawnData");
            _randomEquipSpawnData.containers.Clear();

            GetRandomEquipSpawnContainer(enumEquipmentType, spawnLevel);
        }
        else
        {
            _randomEquipSpawnData.containers.Clear();
            GetRandomEquipSpawnContainer(enumEquipmentType, spawnLevel);
        }

        List<int> resultID = new List<int>();

        for (int i = 0; i < count; ++i)
        {
            RNG.RandomSetting randomSetting = RNG.RNGManager.instance.GetRandom(_randomEquipSpawnData.GetRandomSettings());
            int spawnId = int.Parse(randomSetting.name);

            resultID.Add(spawnId);

            BackendData.GameData.ItemData itemData = null;

            itemData = StaticManager.Backend.GameData.PlayerEquipment.WeaponList.Find(item => item.ItemID == spawnId); 

            if (itemData != null)
            {
                itemData.ItemCount += 1;
            }
            else
            {
                 StaticManager.Backend.GameData.PlayerEquipment.WeaponList.Add(new BackendData.GameData.ItemData() { ItemID = spawnId, ItemLevel = 1, ItemIsEquip = false, ItemCount = 0 });
            }
        }

        if (enumEquipmentType == Define.EnumSpawnItemType.Sword)
            StaticManager.Backend.GameData.PlayerEquipment.ChangeSpawnCount(count, 0, 0);
        else if (enumEquipmentType == Define.EnumSpawnItemType.Bow)
            StaticManager.Backend.GameData.PlayerEquipment.ChangeSpawnCount(0, count, 0);
        else if (enumEquipmentType == Define.EnumSpawnItemType.Staff)
            StaticManager.Backend.GameData.PlayerEquipment.ChangeSpawnCount(0, 0, count);

        StaticManager.Backend.GameData.PlayerEquipment.UpdateUserData();

        return resultID;
    }
    private void GetRandomEquipSpawnContainer(Define.EnumSpawnItemType enumEquipmentType, int spawnLevel)
    {
        if (enumEquipmentType == Define.EnumSpawnItemType.Sword)
        {
            foreach (var item in StaticManager.Backend.Chart.SwordRandom.Dictionary)
            {
                RandomEquipSpawnContainer randomEquipSpawnContainer = new RandomEquipSpawnContainer();

                switch (spawnLevel)
                {
                    case 1:
                        randomEquipSpawnContainer.itemID = item.Value.ItemID;
                        randomEquipSpawnContainer.percentage = item.Value.Freq01;
                        break;
                    case 2:
                        break;
                    case 3:
                        break;
                }

                _randomEquipSpawnData.containers.Add(randomEquipSpawnContainer);
            }
        }
        else if (enumEquipmentType == Define.EnumSpawnItemType.Bow)
        {
            foreach (var item in StaticManager.Backend.Chart.BowRandom.Dictionary)
            {
                RandomEquipSpawnContainer randomEquipSpawnContainer = new RandomEquipSpawnContainer();

                switch (spawnLevel)
                {
                    case 1:
                        randomEquipSpawnContainer.itemID = item.Value.ItemID;
                        randomEquipSpawnContainer.percentage = item.Value.Freq01;
                        break;
                    case 2:
                        break;
                    case 3:
                        break;
                }

                _randomEquipSpawnData.containers.Add(randomEquipSpawnContainer);
            }
        }
        else if (enumEquipmentType == Define.EnumSpawnItemType.Staff)
        {
            foreach (var item in StaticManager.Backend.Chart.StaffRandom.Dictionary)
            {
                RandomEquipSpawnContainer randomEquipSpawnContainer = new RandomEquipSpawnContainer();

                switch (spawnLevel)
                {
                    case 1:
                        randomEquipSpawnContainer.itemID = item.Value.ItemID;
                        randomEquipSpawnContainer.percentage = item.Value.Freq01;
                        break;
                    case 2:
                        break;
                    case 3:
                        break;
                }

                _randomEquipSpawnData.containers.Add(randomEquipSpawnContainer);
            }
        }
    }
    #endregion

    #region 용병 랜덤 확률
    public List<int> GetPartnerRandomList(int spawnLevel, int count)
    {
        if (_randomPartnerSpawnData == null)
        {
            _randomPartnerSpawnData = Resources.Load<RandomEquipSpawnData>("RandomPartnerSpawnData");
            _randomPartnerSpawnData.containers.Clear();

            GetRandomPartnerSpawnContainer(spawnLevel);
        }
        else
        {
            _randomPartnerSpawnData.containers.Clear();
            GetRandomPartnerSpawnContainer(spawnLevel);
        }

        List<int> resultID = new List<int>();

        for (int i = 0; i < count; ++i)
        {
            RNG.RandomSetting randomSetting = RNG.RNGManager.instance.GetRandom(_randomPartnerSpawnData.GetRandomSettings());
            int spawnId = int.Parse(randomSetting.name);

            resultID.Add(spawnId);

            BackendData.GameData.PartnerData partnerData = null;

             partnerData = StaticManager.Backend.GameData.PlayerPartner.PartnerList.Find(item => item.PartnerID == spawnId);
            
             if (partnerData != null)
             {
                 partnerData.PartnerCount += 1;
             }
              else
            {
               StaticManager.Backend.GameData.PlayerPartner.PartnerList.Add(new BackendData.GameData.PartnerData() { PartnerID = spawnId, PartnerLevel = 1, PartnerCount = 0 });
            }
        }

        //if (enumEquipmentType == Define.EnumEquipmentType.Sword)
        //    StaticManager.Backend.GameData.PlayerEquipment.ChangeSpawnCount(count, 0, 0);

        StaticManager.Backend.GameData.PlayerPartner.UpdateUserData();

        return resultID;
    }
    private void GetRandomPartnerSpawnContainer(int spawnLevel)
    {
        foreach (var item in StaticManager.Backend.Chart.PartnerRandom.Dictionary)
        {
            RandomEquipSpawnContainer randomEquipSpawnContainer = new RandomEquipSpawnContainer();

            switch (spawnLevel)
            {
                case 1:
                    randomEquipSpawnContainer.itemID = item.Value.ItemID;
                    randomEquipSpawnContainer.percentage = item.Value.Freq01;
                    break;
                case 2:
                    break;
                case 3:
                    break;
            }

            _randomPartnerSpawnData.containers.Add(randomEquipSpawnContainer);
        }

    }
    #endregion

    #region 주사위 확률
    public bool GetDiceRandomList(int index, Action OnComplete = null)
    {
        if (_randomDiceData == null)
        {
            _randomDiceData = Resources.Load<RandomDiceData>("RandomDiceData");
            _randomDiceData.containers.Clear();

            GetRandomDiceSpawnContainer();
        }
        else
        {
            _randomDiceData.containers.Clear();
            GetRandomDiceSpawnContainer();
        }

        List<DiceData> diceDatas = null;
        StaticManager.Backend.GameData.PlayerDice.DiceDict.TryGetValue(index.ToString(), out diceDatas);

        if (diceDatas == null)
            return false;

        int count = 0;
        int lockCount = 0;

        for (int i = 0; i < diceDatas.Count; ++i)
        {
            if (diceDatas[i].IsLock == false)
                count++;
            else
                lockCount++;
        }

        //TODO 모두 잠금인 경우
        if (count < 1)
        {
            Debug.Log("모두 잠금상태");
            return false;
        }

        List<int> resultID = new List<int>();

        for (int i = 0; i < count; ++i)
        {
            RNG.RandomSetting randomSetting = RNG.RNGManager.instance.GetRandom(_randomDiceData.GetRandomSettings());
            int spawnId = int.Parse(randomSetting.name);

            resultID.Add(spawnId);
        }

        int resultIndex = 0;

        bool isStopAuto = false;

        for (int i = 0; i < diceDatas.Count; ++i)
        {
            if (diceDatas[i].IsLock == false)
            {
                diceDatas[i].DiceNum = resultID[resultIndex];

                BackendData.Chart.DiceRandom.Item item = null;
                StaticManager.Backend.Chart.DiceRandom.Dictionary.TryGetValue(diceDatas[i].DiceNum, out item);

                if (item == null)
                {
                    // 로비로 이동
                    continue;
                }

                if (CheckStopDice(item) == true)
                    isStopAuto = true;

                float randomValue = UnityEngine.Random.Range(item.MinValue, item.MaxValue);
                diceDatas[i].DiceValue = randomValue;
                resultIndex++;
            }
        }

        StaticManager.Backend.GameData.PlayerDice.SetDiceList(index, diceDatas);
        OnComplete?.Invoke();

        if (isStopAuto)
            return false;
        else
            return true;
    }

    private bool CheckStopDice(BackendData.Chart.DiceRandom.Item item)
    {
        int stopGradeValue = PlayerOptionManager.instance.GetDiceStartOption();

        if (stopGradeValue <= item.Grade)
        {
            if (PlayerOptionManager.instance.GetDiceOption(item.StatType) == true)
                return true;
        }

        return false;
    }

    private void GetRandomDiceSpawnContainer()
    {
        foreach (var item in StaticManager.Backend.Chart.DiceRandom.Dictionary)
        {
            RandomDiceSpawnContainer randomEquipSpawnContainer = new RandomDiceSpawnContainer();

            randomEquipSpawnContainer.itemID = item.Value.DiceRandomID;
            randomEquipSpawnContainer.percentage = float.Parse(item.Value.Percent);

            _randomDiceData.containers.Add(randomEquipSpawnContainer);
        }
    }
    #endregion

    #region 보물 강화

    public List<int> GetTreasureRandomList(int count)
    {
        if (_randomTreasureSpawnData == null)
        {
            _randomTreasureSpawnData = Resources.Load<RandomTreasureSpawnData>("RandomTreasureSpawnData");
            _randomTreasureSpawnData.containers.Clear();

            GetRandomTreasureSpawnContainer();
        }
        else
        {
            _randomTreasureSpawnData.containers.Clear();
            GetRandomTreasureSpawnContainer();
        }

        List<int> resultItem = new List<int>();

        for (int i = 0; i < count; ++i)
        {
            RNG.RandomSetting randomSetting = RNG.RNGManager.instance.GetRandom(_randomTreasureSpawnData.GetRandomSettings());
            int spawnId = int.Parse(randomSetting.name);

            resultItem.Add(spawnId);
            StaticManager.Backend.GameData.PlayerTreasure.AddTreasure(spawnId, 1);
        }

        return resultItem;
    }

    private void GetRandomTreasureSpawnContainer()
    {
        foreach (var item in StaticManager.Backend.Chart.TreasureRandom.Dictionary)
        {
            RandomTreasureSpawnContainer randomSpawnContainer = new RandomTreasureSpawnContainer();
            randomSpawnContainer.itemID = item.Value.ItemID;
            randomSpawnContainer.percentage = item.Value.Freq01;
            _randomTreasureSpawnData.containers.Add(randomSpawnContainer);
        }
    }

    private void SetTreasureEnhancementRandomData()
    {
        if (_treasureEnhancementRandomData == null)
        {
            _treasureEnhancementRandomData = Resources.Load<RandomTreasureEnchantData>("RandomTreasureEnchantData");
        }

        _treasureEnhancementRandomData.containers.Clear();

        for (int i = 1; i <= 100; ++i)
        {
            BackendData.Chart.TreasureEnchant.Item treasureEnchant = StaticManager.Backend.Chart.TreasureEnchant.GetEnchantItem(i);

            TreasureEnhancementSetting equipmentEnhancementRandomDataBuffer = new TreasureEnhancementSetting();
            equipmentEnhancementRandomDataBuffer.level = treasureEnchant.Level;

            TreasureEnhancementSetting.TreasureSettingContainer upgrade = new TreasureEnhancementSetting.TreasureSettingContainer();
            upgrade.enhancementResult = EnhancementResult.Upgrade;
            upgrade.percentage = treasureEnchant.Success;
            equipmentEnhancementRandomDataBuffer.containers.Add(upgrade);

            TreasureEnhancementSetting.TreasureSettingContainer destroy = new TreasureEnhancementSetting.TreasureSettingContainer();
            destroy.enhancementResult = EnhancementResult.Destroy;
            destroy.percentage = treasureEnchant.Destroy;
            equipmentEnhancementRandomDataBuffer.containers.Add(destroy);

            _treasureEnhancementRandomData.containers.Add(equipmentEnhancementRandomDataBuffer);
        }
    }

    public bool GetTreasureEnchantResult(int level)
    {
        SetTreasureEnhancementRandomData();
        TreasureEnhancementSetting.TreasureSettingContainer container = _treasureEnhancementRandomData.GetRandomContainer
            (_treasureEnhancementRandomData.GetRandomSettings(level - 1)
            , level - 1);

        switch (container.enhancementResult)
        {
            case EnhancementResult.Upgrade:
                return true;
            case EnhancementResult.Destroy:
                return false;
        }

        return false;
    }

    public float GetTreasureEnchantSuccessPer(int level)
    {
        SetTreasureEnhancementRandomData();
        return _treasureEnhancementRandomData.containers[level].containers[0].percentage;
    }

    #endregion
}
