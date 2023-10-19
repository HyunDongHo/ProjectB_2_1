using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerInfo
{
    public bool DIsFirstDefaultStage;
    public bool DIsFirstLobby;
}

[System.Serializable]
public class PlayerGameData
{
    //public float DHp;
    //public float DSp;
    //public float DExp;
    //public int DLevel;
    //public int DCoin;
    //public int DRuby;
    //public int DCore;


    //public string HangarInitTime;  // 격납고 초기화 된 시간
    //public int HangarRefreshCount; // 남은 장비 새로고침 횟수
    //public List<string> HangarItemList; // 격납고 스폰 아이템 리스트
    //public List<int> HangarItemBuyList; // 격납고 스폰 아이템 리스트
    //public List<int> HangarItemNeedList; // 격납고 스폰 아이템 리스트
    //public List<PlayerQuickSlotItemData> QuickSlotItemList;
    //public List<PlayerBuffItemData> Buffs;

    /* Warrior Game Data */
    public int DWarriorLevel;
    public float DWarriorHp;
    public float DWarriorExp;

    /* Archer Game Data */
    public int DArcherLevel;
    public float DArcherHp;
    public float DArcherExp;

    /* Wizard Game Data*/
    public int DWizardLevel;
    public float DWizardHp;
    public float DWizardExp;

    public long ClearStageLevel;
    public long NowStageLevel;
}

[System.Serializable]
public class PlayerInventory
{
    public string inventoryName;
    public string inventoryDatas;
}

[System.Serializable]
public class PlayerInventoryData
{
    public int itemIndex;
    public string itemStorage;
    public string itemName;

    public string saveItem;
    public string saveItemData;
}

[System.Serializable]
public class PlayerEquipData
{
    public int equipWeaponItemIndex;
    public int equipHelmetItemIndex;
    public int equipArmorItemIndex;
    public int equipGloveItemIndex;
    public int equipBeltItemIndex;
    public int equipGaiterItemIndex;
    public int equipBootsItemIndex;
    public int equipEarringItemIndex;
    public int equipNecklaceItemIndex;
    public int equipRingItemIndex;
    public int equipPetItemIndex;
}

[System.Serializable]
public class PlayerQuestData
{
    public string questName;
    public string questDataJson;
}

[System.Serializable]
public class PlayerUpgradeData
{
    public int skill01;
    public int skill02;
    public int skill03;
    public int skill04;
    public int upgrade01;
    public int upgrade02;
    public int upgrade03;
    public int upgrade04;
    public int upgrade05;
    public int upgrade06;
    public int upgrade07;
    public int upgrade08;
    public int upgrade09;
}

[System.Serializable]
public class PlayerQuickSlotItemData
{
    public int slotNum;
    public string itemName;
    public bool isAuto;
}

[System.Serializable]
public class PlayerBuffItemData
{
    public string buffName;
    public int buffTime;
}

[System.Serializable]
public class HangarData
{
    public string InitTime;  // 격납고 초기화 된 시간
    public int RefreshCount; // 남은 장비 새로고침 횟수
    public List<string> ItemList; // 격납고 스폰 아이템 리스트
    public List<int> ItemBuyList; // 격납고 스폰 아이템 리스트
}