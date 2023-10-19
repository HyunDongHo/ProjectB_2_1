using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheatManager : MonoBehaviour
{
    private int currentClickCount = 0;
    public GameObject QuestItemObj;
    public GridLayoutGroup QuestItemLists;
    public GameObject QuestListParent;

    public enum CheatItemType
    {
        None,
        HpPotion,
        Pw_HpPotion,
        SpPotion,
        Pw_SpPotion,
        GpPotion,
        Pw_GpPotion,
        GrowPotion,
        Pw_GrowPotion,
        EnchantStone,
        PwEnchantStone,
        Crystal,
        PwCrystal,
        Gold,
        Ruby,
        FameCoin,
        Core,
        Weapon1,
        Weapon11,
        PwWeapon1,
        PwWeapon11,
        Protect1,
        Protect11,
        PwProtect1,
        PwProtect11,
        Acc1,
        Acc11,
        PwAcc1,
        PwAcc11,
        Pet1,
        Pet11,
        PwPet1,
        PwPet11,
        QuestNum,
        LevelNum,
        Weapon,
        Helmet,
        Armor,
        Glove,
        Belt,
        Gaiter,
        Boots,
        Earing,
        Necklace,
        Ring
    }

    public GameObject parent;

    [Space]
    public GameObject itemCountPanel;
    public InputField countInputText;

    [Space]

    public Button closeButton;

    [Space]

    public Button stageAllOpen;
    public Button stageAllClose;

    public Button hpFull;
    public Button spFull;

    public Button mainQuest1;

    public Button hpPotion, p_hpPotion, spPotion, p_spPotion, gpPotion, p_gpPotion, growPotion, p_growPotion, enchant, pwEnchant, crystal, pwCrystal, gold, ruby, fame, core;
    public Button weapon1, weapon11, pwweapon1, pwweapon11, protect1, protect11, pwprotect1, pwprotect11, acc1, acc11, pwacc1, pwacc11, pet1, pet11, pwpet1, pwpet11;
    public Button CountConfirmButton, CountCancleButton;

    private void Awake()
    {
        AddEvent();
    }

    private void Start()
    {
        StartCoroutine(CheckCheatOpen());
        InitQuestItem();
    }

    private void Update()
    {
        if (InputManager.instance.GetMouseButtonDown(0, ScreenRectType.All, isCheckOverlapCanvas: true))
            currentClickCount += 1;
    }

    private void OnDestroy()
    {
        RemoveEvent();
    }

    private void AddEvent()
    {
        stageAllOpen.onClick.AddListener(HandleOnStageAllOpen);
        stageAllClose.onClick.AddListener(HandleOnStageAllClose);

        hpFull.onClick.AddListener(HandleOnHpFull);
        spFull.onClick.AddListener(HandleOnSpFull);

        mainQuest1.onClick.AddListener(HandleOnMainQuest1);

        closeButton.onClick.AddListener(HandleOnCloseButton);
    }

    private void RemoveEvent()
    {
        stageAllOpen.onClick.RemoveListener(HandleOnStageAllOpen);
        stageAllClose.onClick.RemoveListener(HandleOnStageAllClose);

        hpFull.onClick.RemoveListener(HandleOnHpFull);
        spFull.onClick.RemoveListener(HandleOnSpFull);

        mainQuest1.onClick.RemoveListener(HandleOnMainQuest1);

        closeButton.onClick.RemoveListener(HandleOnCloseButton);
    }

    private void HandleOnStageAllOpen()
    {
        var stageTypes = SceneSettingManager.instance.stageTypes;

        for (int sector = 0; sector < stageTypes.Count; sector++)
        {
            for (int sectorIndex = 0; sectorIndex < stageTypes[sector].Count; sectorIndex++)
            {
                UserDataManager.instance.SetUnlockStageToServer(sector, sectorIndex, true);
            }
        }
    }

    private void HandleOnStageAllClose()
    {
        var stageTypes = SceneSettingManager.instance.stageTypes;

        for (int sector = 0; sector < stageTypes.Count; sector++)
        {
            for (int sectorIndex = 0; sectorIndex < stageTypes[sector].Count; sectorIndex++)
            {
                UserDataManager.instance.SetUnlockStageToServer(sector, sectorIndex, sector == 0 && sectorIndex == 0);
            }
        }
    }

    private void HandleOnHpFull()
    {
        StageManager.instance.playerControl.GetStats<PlayerStats>().hp.SetHpToMax();
    }

    private void HandleOnSpFull()
    {
        StageManager.instance.playerControl.GetStats<PlayerStats>().sp.SetSpToMax();
    }

    private void HandleOnMainQuest1()
    {
        QuestManager.instance.AddQuest("Main_Quest_1");
    }

    private void HandleOnCloseButton()
    {
        CheatWindowActive(false);
    }

    private IEnumerator CheckCheatOpen()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(1.0f);
        while (true)
        {
            if(currentClickCount >= 5)
            {
                CheatWindowActive(true);
            }

            currentClickCount = 0;

            yield return waitForSeconds;
        }
    }

    private void CheatWindowActive(bool active)
    {
        parent.SetActive(active);
    }

    CheatItemType nowSelectItemType = CheatItemType.None;
    public void PushItemButton(int itemType)
    {
        nowSelectItemType = (CheatItemType)itemType;
        itemCountPanel.SetActive(true);
    }

    private void InitQuestItem()
    {
        string chartName = ChartDefine.QUEST_DATA;

        int columnCount = BackEndServerManager.instance.GetChartColumnCount(chartName);
        for (int i = 0; i < columnCount; i++)
        {
            QuestData questData = ResourceManager.instance.Load<QuestData>($"{BackEndServerManager.instance.GetChartJsonDataToValue<string>(chartName, "QuestName", i)}");

            if (questData != null)
            {
                GameObject questObject = ResourceManager.Instantiate<GameObject>(QuestItemObj, QuestItemLists.transform);
                QuestCheatItem questCheatItem = questObject.GetComponent<QuestCheatItem>();
                questCheatItem.SetQuest(questData.questName);
            }
        }
    }

    public void PushCountConfirmButton()
    {
        int count = 0;
        string itemName = string.Empty;

        if ((int)nowSelectItemType < (int)CheatItemType.Weapon || (int)nowSelectItemType > (int)CheatItemType.Ring)
            count = int.Parse(countInputText.text);
        else
            itemName = countInputText.text;

        switch (nowSelectItemType)
        {
          
        }


        nowSelectItemType = CheatItemType.None;
        itemCountPanel.SetActive(false);
    }

    public void PushCountCancleButton()
    {
        nowSelectItemType = CheatItemType.None;
        itemCountPanel.SetActive(false);
    }

    public void PushQuestListOpenButton()
    {
        QuestListParent.SetActive(true);
    }

    public void PushQuestListCloseButton()
    {
        QuestListParent.SetActive(false);
    }
}
