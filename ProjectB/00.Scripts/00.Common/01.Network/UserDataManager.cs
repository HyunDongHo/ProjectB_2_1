using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using UnityEngine.SceneManagement;
using CodeStage.AntiCheat.ObscuredTypes;
using CodeStage.AntiCheat.Storage;
using BackEnd;

public partial class UserDataManager : Singleton<UserDataManager>
{
    public bool isFirstLobby;
    public bool isFirstDefaultStage;

    #region PlayerGameData

    //public float hp;
    //public float sp;
    //public float exp;
    //public int level;
    //public int coin;
    //public int ruby;
    //public int core;

    //public string hangarinittime;  // 격납고 초기화 된 시간
    //public int hangarrefreshcount; // 남은 장비 새로고침 횟수
    //public list<string> hangaritemlist = new list<string>(); // 격납고 스폰 아이템 리스트
    //public list<int> hangaritembuylist = new list<int>(); // 격납고 스폰 아이템 리스트
    //public list<int> hangaritemneedlist = new list<int>(); // 격납고 스폰 아이템 가격

    //public List<PlayerQuickSlotItemData> quickSlot = new List<PlayerQuickSlotItemData>();

    //public List<PlayerBuffItemData> buffs = new List<PlayerBuffItemData>();

    /* Warrior Info */
    public int warriorLevel;
    public float warriorHp;
    public float warriorExp;

    /* Archer Info */
    public int archerLevel;
    public float archerHp;
    public float archerExp;

    /* Wizard Info */
    public int wizardLevel;
    public float wizardHp;
    public float wizardExp;

    public long clearStageLevel;
    public long nowStageLevel;

    public Dictionary<String, double> warriorGoldStatLevel;

    #endregion

    [System.Serializable]
    public class SavePlayerInventory
    {
        public string inventoryName;
        public List<PlayerInventoryData> inventoryDatas = new List<PlayerInventoryData>();

        public SavePlayerInventory(string inventoryName)
        {
            this.inventoryName = inventoryName;
        }
    }

    public List<SavePlayerInventory> saveInventories = new List<SavePlayerInventory>(){
        new SavePlayerInventory("Weapon"),
        new SavePlayerInventory("ProtectiveGear"),
        new SavePlayerInventory("Accessary"),
        new SavePlayerInventory("Etc"),
        new SavePlayerInventory("Pet")
    };

    public Dictionary<ObscuredString, QuestSaveData> quests = new Dictionary<ObscuredString, QuestSaveData>();

    public Dictionary<UpgradeTarget, int> upgrades = new Dictionary<UpgradeTarget, int>();

    public PlayerEquipData equipData = new PlayerEquipData();

    public List<StageLock> stageLocks = new List<StageLock>();

    public ObscuredDouble RandomItemRefreshTime = 60;

    private void Awake()
    {
        MoveSceneManager.instance.OnStartSceneChanged += HandleOnStartSceneChanged;
        MoveSceneManager.instance.OnEndSceneChanged += HandleOnEndSceneChanged;
    }

    private void FixedUpdate()
    {
        //CalcHangarRefreshTime();
        
       // if(RandomItemRefreshTime <= 0)
       // { 
       //     if ((StageManager.instance as LobbySceneManager) != null)
       //     {
       //         if ((StageManager.instance as LobbySceneManager).NowLobbyState == LobbyState.Right)
       //             (StageManager.instance as LobbySceneManager).RefreshHangarList(true);
       //     }
       // }
    }

    private void HandleOnStartSceneChanged(LoadSceneMode loadSceneMode)
    {
        // 씬 로드 시작하면 모든 데이터 저장.
     //  if (loadSceneMode == LoadSceneMode.Single)
     //  {
     //      string nowSceneName = SceneManager.GetActiveScene().name;
     //      if (nowSceneName == SceneSettingManager.SETTING_SCENE || nowSceneName == SceneSettingManager.INTRO_SCENE
     //          || nowSceneName == SceneSettingManager.ACCOUNT_SCENE || nowSceneName == SceneSettingManager.NICKNAME_SCENE || nowSceneName == SceneSettingManager.DEFAULT_STAGE_INTRO_SCENE)
     //          return;
     //
     //      SaveAllDataToServer();  
     //  }
    }

    private void HandleOnEndSceneChanged(LoadSceneMode loadSceneMode)
    {
      ///  if (loadSceneMode == LoadSceneMode.Single)
      ///  {
      ///      string nowSceneName = SceneManager.GetActiveScene().name;
      ///      if (nowSceneName == SceneSettingManager.SETTING_SCENE || nowSceneName == SceneSettingManager.INTRO_SCENE || nowSceneName == SceneSettingManager.ACCOUNT_SCENE
      ///          || nowSceneName == SceneSettingManager.NICKNAME_SCENE || nowSceneName == SceneSettingManager.DEFAULT_STAGE_INTRO_SCENE)
      ///          return;
      ///      // 씬 로드 시작하면 모든 데이터 저장.
      ///      InitDataEveryLoad();
      ///  }
    }

    public void InitUserData(Action OnInitEnd)
    {
        StartCoroutine(Init(OnInitEnd));
    }

    private IEnumerator Init(Action OnInitEnd)
    {
        AddEvent();

        yield return StartCoroutine(InitUserDatas());
        yield return StartCoroutine(InitSeverData());

        InitDataOnce();
     //   InitDataEveryLoad();

        OnInitEnd?.Invoke();
    }

    private void OnDestroy()
    {
        RemoveEvent();
    }

    private void OnApplicationPause(bool pause)
    {
        // SaveAllDataToServer();
    }

    private void OnApplicationQuit()
    {
        //SaveAllDataToServer();
        BackEnd.SendQueue.StopSendQueue();
    }

    private void AddEvent()
    {
        SoundManager.instance.OnVolumeChanged += HandleOnVolumeChanged;

        SceneSettingManager.instance.OnSectorChanged += HandleOnSectorChanged;
        SceneSettingManager.instance.OnSectorIndexChanged += HandleOnSectorIndexChanged;

        QuestManager.instance.OnAdded += AddQuest;
        QuestManager.instance.OnUpdated += UpdateQuest;
        QuestManager.instance.OnRemoved += RemoveQuest;
    }

    private void RemoveEvent()
    {
        SoundManager.instance.OnVolumeChanged -= HandleOnVolumeChanged;

        SceneSettingManager.instance.OnSectorChanged -= HandleOnSectorChanged;
        SceneSettingManager.instance.OnSectorIndexChanged -= HandleOnSectorIndexChanged;

        QuestManager.instance.OnAdded -= AddQuest;
        QuestManager.instance.OnUpdated -= UpdateQuest;
        QuestManager.instance.OnRemoved -= RemoveQuest;
    }

    #region Save

    public void SaveAllDataToServer()
    {
        string nowSceneName = SceneManager.GetActiveScene().name;
        if (nowSceneName == SceneSettingManager.SETTING_SCENE || nowSceneName == SceneSettingManager.INTRO_SCENE
            || nowSceneName == SceneSettingManager.ACCOUNT_SCENE || nowSceneName == SceneSettingManager.NICKNAME_SCENE
            || nowSceneName == SceneSettingManager.DEFAULT_STAGE_INTRO_SCENE)
            return;

        PlayerDataManager.instance.SaveAllPlayerDataToUserData();
        SaveGameData();
        SaveQuestValue();
        SaveUpgrades();
        SaveInventoryDatas();
    }

    public void SaveGameData(Action<BackendReturnObject> OnSuccess = null)
    {
        Param param = new Param();
        //float convertHp = hp;
        //param.Add("DHp", convertHp);

        //float convertSp = sp;
        //param.Add("DSp", convertSp);

        //float convertExp = exp;
        //param.Add("DExp", convertExp);

        //int convertLevel = level;
        //param.Add("DLevel", convertLevel);

        //int convertCoin = coin;
        //param.Add("DCoin", convertCoin);

        //int convertRuby = ruby;
        //param.Add("DRuby", convertRuby);

        //int convertCore = core;
        //param.Add("DCore", convertCore);

        //param.Add("QuickSlotItemList", quickSlot);

        //param.Add("Buffs", buffs);

        int convertWarriorLevel = warriorLevel;
        param.Add("DWarriorLevel", convertWarriorLevel);

        float convertWarriorHp = warriorHp;
        param.Add("DWarriorHp", convertWarriorHp);

        float convertWarriorExp = warriorExp;
        param.Add("DWarriorExp", convertWarriorExp);

        int convertArcherLevel = archerLevel;
        param.Add("DArcherLevel", convertArcherLevel);

        float convertArcherHp = archerHp;
        param.Add("DArcherHp", convertArcherHp);

        float convertArcherExp = archerExp;
        param.Add("DArcherExp", convertArcherExp);

        int convertWizardLevel = wizardLevel;
        param.Add("DWizardLevel", convertWizardLevel);

        float convertWizardHp = wizardHp;
        param.Add("DWizardHp", convertWizardHp);

        float convertWizardExp = wizardExp;
        param.Add("DWizardExp", convertArcherExp);
        
        long csl = clearStageLevel;
        param.Add("ClearStageLevel", csl);

        long nsl = nowStageLevel;
        param.Add("NowStageLevel", nsl);

        BackEndFunctions.instance.UpdateData(ServerGameDefine.PLAYER_GAME_DATA, new Where(), param, OnSuccess);
    }

    private void SaveUpgrades()
    {
        foreach (var upgrade in upgrades)
        {
            Param param = new Param();

            switch (upgrade.Key)
            {
                case UpgradeTarget.Skill_01:
                    param.Add("skill01", upgrades[UpgradeTarget.Skill_01]);
                    break;
                case UpgradeTarget.Skill_02:
                    param.Add("skill02", upgrades[UpgradeTarget.Skill_02]);
                    break;
                case UpgradeTarget.Skill_03:
                    param.Add("skill03", upgrades[UpgradeTarget.Skill_03]);
                    break;
                case UpgradeTarget.Skill_04:
                    param.Add("skill04", upgrades[UpgradeTarget.Skill_04]);
                    break;
                case UpgradeTarget.Upgrade_01:
                    param.Add("upgrade01", upgrades[UpgradeTarget.Upgrade_01]);
                    break;
                case UpgradeTarget.Upgrade_02:
                    param.Add("upgrade02", upgrades[UpgradeTarget.Upgrade_02]);
                    break;
                case UpgradeTarget.Upgrade_03:
                    param.Add("upgrade03", upgrades[UpgradeTarget.Upgrade_03]);
                    break;
                case UpgradeTarget.Upgrade_04:
                    param.Add("upgrade04", upgrades[UpgradeTarget.Upgrade_04]);
                    break;
                case UpgradeTarget.Upgrade_05:
                    param.Add("upgrade05", upgrades[UpgradeTarget.Upgrade_05]);
                    break;
                case UpgradeTarget.Upgrade_06:
                    param.Add("upgrade06", upgrades[UpgradeTarget.Upgrade_06]);
                    break;
                case UpgradeTarget.Upgrade_07:
                    param.Add("upgrade07", upgrades[UpgradeTarget.Upgrade_07]);
                    break;
                case UpgradeTarget.Upgrade_08:
                    param.Add("upgrade08", upgrades[UpgradeTarget.Upgrade_08]);
                    break;
                case UpgradeTarget.Upgrade_09:
                    param.Add("upgrade09", upgrades[UpgradeTarget.Upgrade_09]);
                    break;
            }

            BackEndFunctions.instance.UpdateData(ServerGameDefine.PLAYER_UPGRADE, new Where(), param);
        }
    }

    private void SaveQuestValue()
    {
        foreach (var quest in quests)
        {
            Where where = new Where();
            where.Equal("questName", quest.Key);

            Param param = new Param();
            param.Add("questDataJson", JsonUtility.ToJson(quest.Value));

            BackEndFunctions.instance.UpdateData(ServerGameDefine.PLAYER_QUEST_DATA, where, param);
        }
    }

    public void SaveInventoryDatas()
    {
        for (int i = 0; i < saveInventories.Count; i++)
        {
            Param param = new Param();
            Where where = new Where();

            param.Add("inventoryDatas", JsonMapper.ToJson(saveInventories[i].inventoryDatas));
            where.Contains("inventoryName", saveInventories[i].inventoryName);

            BackEndFunctions.instance.UpdateData(ServerGameDefine.PLAYER_INVENTORY, where, param);
        }
    }

    private void SaveEquipDatas()
    {
        Param param = new Param();

        param.Add("equipWeaponItemIndex", equipData.equipWeaponItemIndex);
        param.Add("equipHelmetItemIndex", equipData.equipHelmetItemIndex);
        param.Add("equipArmorItemIndex", equipData.equipArmorItemIndex);
        param.Add("equipGloveItemIndex", equipData.equipGloveItemIndex);
        param.Add("equipBeltItemIndex", equipData.equipBeltItemIndex);
        param.Add("equipGaiterItemIndex", equipData.equipGaiterItemIndex);
        param.Add("equipBootsItemIndex", equipData.equipBootsItemIndex);
        param.Add("equipEarringItemIndex", equipData.equipEarringItemIndex);
        param.Add("equipNecklaceItemIndex", equipData.equipNecklaceItemIndex);
        param.Add("equipRingItemIndex", equipData.equipRingItemIndex);
        param.Add("equipPetItemIndex", equipData.equipPetItemIndex);

        BackEndFunctions.instance.UpdateData(ServerGameDefine.PLAYER_EQUIPMENT, new Where(), param);
    }

    #endregion

    #region Init

    private IEnumerator InitUserDatas()
    {
        Action<Action>[] asyncs =
        {
            (OnComplete) => CheckFirstData(ServerGameDefine.PLAYER_INFO, OnComplete),
            (OnComplete) => CheckFirstData(ServerGameDefine.PLAYER_GAME_DATA, OnComplete, param:GetStartGameUserData()),
            (OnComplete) => CheckFirstData(ServerGameDefine.PLAYER_EQUIPMENT, OnComplete),
            (OnComplete) => CheckFirstData(ServerGameDefine.PLAYER_STAGE_DATA, OnComplete),
            (OnComplete) => CheckFirstData(ServerGameDefine.PLAYER_UPGRADE, OnComplete),
            (OnComplete) =>
            {
                for (int i = 0; i < saveInventories.Count; i++)
                {
                    Where where = new Where();
                    where.Contains("inventoryName", saveInventories[i].inventoryName);

                    Param param = new Param();
                    param.Add("inventoryName", saveInventories[i].inventoryName);

                    CheckFirstData(ServerGameDefine.PLAYER_INVENTORY, i != saveInventories.Count - 1 ? null : OnComplete, where, param);
                }
            }
        };

        yield return StartCoroutine(Logic.WaitAsync(asyncs));
    }

    private void CheckFirstData(string chartName, Action OnComplete, Where where = null, Param param = null)
    {
        if (where == null)
            where = new Where();

        if (param == null)
            param = new Param();

        BackEndFunctions.instance.GetMyData(chartName, where,
            OnSuccess: (data) =>
            {
                if (data.GetReturnValuetoJSON()["rows"].Count <= 0)
                {
                    BackEndFunctions.instance.InsertData(chartName, param,
                        OnSuccess: (result) => OnComplete?.Invoke());
                }
                else
                    OnComplete?.Invoke();
            });
    }

    private IEnumerator InitSeverData()
    {
        int firstRow = 0;

        Action<Action>[] asyncs =
        {
            (OnComplete) =>
            {
                GetData<PlayerInfo>(ServerGameDefine.PLAYER_INFO,
                    (datas) =>
                    {
                        SetPlayerInfo(datas[firstRow]);

                        OnComplete?.Invoke();
                    }, new Where());
            },
            (OnComplete) =>
            {
                GetData<PlayerGameData>(ServerGameDefine.PLAYER_GAME_DATA,
                (datas) =>
                {
                    SetPlayerGameData(datas[firstRow]);

                    OnComplete?.Invoke();
                }, new Where());
            },
            (OnComplete) =>
            {
                GetData<PlayerInventory>(ServerGameDefine.PLAYER_INVENTORY,
                    (datas) =>
                    {
                        SetPlayerInventory(datas);

                        OnComplete?.Invoke();
                    }, new Where(), limit: saveInventories.Count);
            },
            (OnComplete) =>
            {
                GetData<PlayerEquipData>(ServerGameDefine.PLAYER_EQUIPMENT,
                    (datas) =>
                    {
                        SetPlayerEquipData(datas[firstRow]);

                        OnComplete?.Invoke();
                    }, new Where());
            },
            (OnComplete) =>
            {
                GetData<PlayerQuestData>(ServerGameDefine.PLAYER_QUEST_DATA,
                    (datas) =>
                    {
                        SetPlayerQuestData(datas);

                        OnComplete?.Invoke();
                    }, new Where(), limit: 25);
            },
            (OnComplete) =>
            {
                GetData<PlayerUpgradeData>(ServerGameDefine.PLAYER_UPGRADE,
                    (datas) =>
                    {
                        SetUpgrade(datas[firstRow]);

                        OnComplete?.Invoke();
                    }, new Where());
            },
            (OnComplete) =>
            {
                SetStage();

                OnComplete?.Invoke();
            },
        };

        yield return StartCoroutine(Logic.WaitAsync(asyncs));
    }

    private void GetData<T>(string tableName, Action<T[]> OnComplete, Where where, int limit = 10)
    {
        BackEndFunctions.instance.GetMyData(tableName, where, limit,
            OnSuccess: (data) =>
            {
                Debug.Log($"[UserDataManager] {tableName} 의 데이터를 가져왔습니다.");
                var jsonDatas = BackendReturnObject.Flatten(data.Rows());

                T[] convertDatas = new T[jsonDatas.Count];
                for (int i = 0; i < convertDatas.Length; i++)
                {
                    Debug.Log($"[UserDataManager] {tableName} : {jsonDatas[i].ToJson()}");
                    convertDatas[i] = JsonMapper.ToObject<T>(jsonDatas[i].ToJson());
                }

                OnComplete?.Invoke(convertDatas);
            });
    }

    public void InitGameStartData()
    {
        SceneSettingManager.instance.SetStage(GetSector(), GetSectorIndex());

        SoundManager.instance.bgmVolume = GetBgmVolume();
        SoundManager.instance.sfxVolume = GetSfxVolume();
        SoundManager.instance.voiceVolume = GetVoiceVolume();
    }

    private void InitDataOnce()
    {
        List<QuestData> questDatas = new List<QuestData>();
        foreach (var quest in quests)
        {
            QuestData questData = BackEndServerManager.instance.GetSavedResourceData<QuestData>(quest.Key, isCopy: true);  
            questData.saveData = quest.Value;

            questDatas.Add(questData);
        }
        QuestManager.instance.Init(questDatas);

        UpgradeManager.instance.InitUpgrade(upgrades);

        //PlayersControlManager.instance.Init();
    }

    public void InitDataEveryLoad()
    {
     //   QuestManager.instance.ExecuteAfterSceneLoad();
      //  UpgradeManager.instance.ExecuteAfterSceneLoad();

        //PlayersControl playersControl = (StageManager.instance as GamePlayManager)?.playersControl;
        PlayerControl[] playersControl = PlayersControlManager.instance.playersContol;

        if (playersControl == null) return;

        InitPlayerGameData(playersControl);
      //  InitPlayerBelongings(playersControl[0]);
       // InitPlayerEquip(playersControl[0]);      

        //StageManager.instance.canvasManager.GetUIManager<UIManager_Common>().stageWindow.stageList.Init(stageLocks);
    }

    private void InitPlayerGameData(PlayerControl[] playersControl)
    {
        return;

        for (int i = 0; i < (int)PlayerType.None; ++i)
        {
            PlayerStats playerStats = playersControl[i].GetStats<PlayerStats>();

            if (playerStats == null)
                continue;

            switch ((PlayerType)i)
            {
                case PlayerType.Warrior:
                    playerStats.level.InitLevel(StaticManager.Backend.GameData.PlayerGameData.DWarriorLevel);
                    playerStats.hp.InitHp(9000000);
                    playerStats.exp.InitExp(StaticManager.Backend.GameData.PlayerGameData.DWarriorExp);
                    playerStats.UpdateLevelData(StaticManager.Backend.GameData.PlayerGameData.DWarriorLevel);
                    break;
                case PlayerType.Archer:
                    playerStats.level.InitLevel(StaticManager.Backend.GameData.PlayerGameData.DArcherLevel);
                    playerStats.hp.InitHp(1000000);
                    playerStats.exp.InitExp(StaticManager.Backend.GameData.PlayerGameData.DArcherExp);
                    playerStats.UpdateLevelData(StaticManager.Backend.GameData.PlayerGameData.DArcherLevel);
                    break;
                case PlayerType.Wizard:
                    playerStats.level.InitLevel(StaticManager.Backend.GameData.PlayerGameData.DWizardLevel);
                    playerStats.hp.InitHp(1000000);
                    playerStats.exp.InitExp(StaticManager.Backend.GameData.PlayerGameData.DWizardExp);
                    playerStats.UpdateLevelData(StaticManager.Backend.GameData.PlayerGameData.DWizardLevel);
                    break;
            }
        }
    }


  //  private void InitPlayerGameData(PlayerControl[] playersControl)
  //  {
  //      for(int i=0; i < (int)PlayerType.None; ++i)
  //      {
  //          PlayerStats playerStats = playersControl[i].GetStats<PlayerStats>();
  //
  //          if (playerStats == null)
  //              continue;
  //
  //          switch ((PlayerType)i)
  //          {
  //              case PlayerType.Warrior:
  //                  playerStats.level.InitLevel(warriorLevel);
  //                  //playerStats.hp.InitHp(warriorHp);
  //                  playerStats.hp.InitHp(9000000);
  //                  playerStats.exp.InitExp(warriorExp);
  //                  break;
  //              case PlayerType.Archer:
  //                  playerStats.level.InitLevel(archerLevel);
  //                  //playerStats.hp.InitHp(archerHp);
  //                  playerStats.hp.InitHp(1000000);
  //                  playerStats.exp.InitExp(archerExp);  
  //                  break;
  //              case PlayerType.Wizard:
  //                  playerStats.level.InitLevel(wizardLevel);
  //                  //playerStats.hp.InitHp(wizardHp);
  //                  playerStats.hp.InitHp(1000000);
  //                  playerStats.exp.InitExp(wizardExp);
  //                  break;
  //          }
  //      }
  //
  //      //playerStats.hp.InitHp(hp);
  //      //playerStats.sp.InitSp(sp);
  //      //playerStats.exp.InitExp(exp);
  //      //playerStats.level.InitLevel(level);       
  //
  //      //PlayerWallet playerWallet = playerControl.utility.belongings.playerWallet;
  //      //playerWallet.SetGold(coin);
  //      //playerWallet.SetRuby(ruby);
  //      //playerWallet.SetCore(core);
  //
  //      //PlayerQuickSlot playerQuickSlot = playerControl.utility.quickSlot;
  //      //playerQuickSlot.SetQuickSlot(quickSlot);
  //  }

    private void InitPlayerBelongings(PlayerControl playerControl)
    {
        PlayerBelongings playerBelongings = playerControl.utility.belongings;

       // for (int inventoryIndex = 0; inventoryIndex < saveInventories.Count; inventoryIndex++)
       // {
       //     List<PlayerInventoryData> inventoryDatas = saveInventories[inventoryIndex].inventoryDatas;
       //
       //     for (int i = 0; i < inventoryDatas.Count; i++)
       //     {
       //         PlayerInventoryType playerInventoryType = (PlayerInventoryType)Enum.Parse(typeof(PlayerInventoryType), inventoryDatas[i].itemStorage);
       //
       //         PlayerInventoryData playerInventoryData = inventoryDatas[i];
       //
       //         Item item = ResourceManager.instance.Load<ItemData>(playerInventoryData.itemName, isCopy: true).Wrap<Item>();
       //
       //         JsonUtility.FromJsonOverwrite(playerInventoryData.saveItem, item.saveItem);
       //         JsonUtility.FromJsonOverwrite(playerInventoryData.saveItemData, item.GetData<ItemData>().saveItemData);
       //
       //         playerBelongings.GetInventory(playerInventoryType).AddInitItem(item, playerInventoryData.itemIndex);
       //     }
       // }
       //
       // for (int i = 0; i < (int)PlayerInventoryType.Max; i++)
       //     playerBelongings.GetInventory((PlayerInventoryType)i).EndInitItem();
    }

    private void InitPlayerEquip(PlayerControl playerControl)
    {
     //  PlayerBelongings playerBelongings = playerControl.utility.belongings;
     //
     //  Inventory weaponInventory = playerBelongings.GetInventory(PlayerInventoryType.Weapon);
     //  Inventory protectiveGearInventory = playerBelongings.GetInventory(PlayerInventoryType.ProtectiveGear);
     //  Inventory accessaryInventory = playerBelongings.GetInventory(PlayerInventoryType.Accessary);
     //  Inventory petInventory = playerBelongings.GetInventory(PlayerInventoryType.Pet);
     //
     //  if (equipData.equipWeaponItemIndex != -1)
     //      weaponInventory.ExecuteItem(equipData.equipWeaponItemIndex);
     //
     //  if (equipData.equipHelmetItemIndex != -1)
     //      protectiveGearInventory.ExecuteItem(equipData.equipHelmetItemIndex);
     //  if (equipData.equipArmorItemIndex != -1)
     //      protectiveGearInventory.ExecuteItem(equipData.equipArmorItemIndex);
     //  if (equipData.equipGloveItemIndex != -1)
     //      protectiveGearInventory.ExecuteItem(equipData.equipGloveItemIndex);
     //  if (equipData.equipBeltItemIndex != -1)
     //      protectiveGearInventory.ExecuteItem(equipData.equipBeltItemIndex);
     //  if (equipData.equipGaiterItemIndex != -1)
     //      protectiveGearInventory.ExecuteItem(equipData.equipGaiterItemIndex);
     //  if (equipData.equipBootsItemIndex != -1)
     //      protectiveGearInventory.ExecuteItem(equipData.equipBootsItemIndex);
     //
     //  if (equipData.equipEarringItemIndex != -1)
     //      accessaryInventory.ExecuteItem(equipData.equipEarringItemIndex);
     //  if (equipData.equipNecklaceItemIndex != -1)
     //      accessaryInventory.ExecuteItem(equipData.equipNecklaceItemIndex);
     //  if (equipData.equipRingItemIndex != -1)
     //      accessaryInventory.ExecuteItem(equipData.equipRingItemIndex);
     //
     //  if (equipData.equipPetItemIndex != -1)
     //      petInventory.ExecuteItem(equipData.equipPetItemIndex);
    }

    #endregion

    #region GamePermission
    public bool GetPermission()
    {
        return ObscuredPrefs.GetBool("Permission", defaultValue: false);
    }
    public void SetPermission(bool permission)
    {
        ObscuredPrefs.SetBool("Permission", permission);
    }
    public bool GetFirstTerms()
    {
        return ObscuredPrefs.GetBool("FirstTerms", defaultValue: false);
    }
    public void SetFirstTerms(bool terms)
    {
        ObscuredPrefs.SetBool("FirstTerms", terms);
    }
    public bool GetSecondTerms()
    {
        return ObscuredPrefs.GetBool("SecondTerms", defaultValue: false);
    }
    public void SetSecondTerms(bool terms)
    {
        ObscuredPrefs.SetBool("SecondTerms", terms);
    }
    public bool GetThirdTerms()
    {
        return ObscuredPrefs.GetBool("ThirdTerms", defaultValue: false);
    }
    public void SetThirdTerms(bool terms)
    {
        ObscuredPrefs.SetBool("ThirdTerms", terms);
    }
    public bool GetForthTerms()
    {
        return ObscuredPrefs.GetBool("ForthTerms", defaultValue: false);
    }
    public void SetForthTerms(bool terms)
    {
        ObscuredPrefs.SetBool("ForthTerms", terms);
    }

    #endregion

    #region PlayerInfo

    public void SetPlayerInfo(PlayerInfo playerInfo)
    {
        isFirstLobby = playerInfo.DIsFirstLobby;
        isFirstDefaultStage = playerInfo.DIsFirstDefaultStage;
    }

    public void UpdateIsFirstLobby()
    {
        if (isFirstLobby)
        {
            isFirstLobby = false;

            Param param = new Param();
            bool convertIsFirstLobby = isFirstLobby;
            param.Add("DIsFirstLobby", convertIsFirstLobby);

            BackEndFunctions.instance.UpdateData(ServerGameDefine.PLAYER_INFO, new Where(), param);
        }
    }

    public bool GetIsFirstLobby()
    {
        return isFirstLobby;
    }

    public void UpdateIsFirstDefaultStage()
    {
        if (isFirstDefaultStage)
        {
            isFirstDefaultStage = false;

            Param param = new Param();
            bool convertIsFirstDefaultStage = isFirstDefaultStage;
            param.Add("DIsFirstDefaultStage", convertIsFirstDefaultStage);

            BackEndFunctions.instance.UpdateData(ServerGameDefine.PLAYER_INFO, new BackEnd.Where(), param);
        }
    }

    public bool GetIsFirstDefaultStage()
    {
        return isFirstDefaultStage;
    }

    #endregion

    #region PlayerGameData

    public void SetPlayerGameData(PlayerGameData playerGameData)
    {
        warriorLevel = playerGameData.DWarriorLevel;
        warriorHp = playerGameData.DWarriorHp;
        warriorExp = playerGameData.DWarriorExp;

        archerLevel = playerGameData.DArcherLevel;
        archerHp = playerGameData.DArcherHp;
        archerExp = playerGameData.DArcherExp;

        wizardLevel = playerGameData.DWizardLevel;
        wizardHp = playerGameData.DWizardHp;
        wizardExp = playerGameData.DWizardExp;

        clearStageLevel = playerGameData.ClearStageLevel;
        nowStageLevel = playerGameData.NowStageLevel;

    }
    public void SetPlayerGameData()
    {   
        warriorLevel = StaticManager.Backend.GameData.PlayerGameData.DWarriorLevel;
        warriorHp = StaticManager.Backend.GameData.PlayerGameData.DWarriorHp;
        warriorExp = StaticManager.Backend.GameData.PlayerGameData.DWarriorExp;

        archerLevel = StaticManager.Backend.GameData.PlayerGameData.DArcherLevel;
        archerHp = StaticManager.Backend.GameData.PlayerGameData.DArcherHp;
        archerExp = StaticManager.Backend.GameData.PlayerGameData.DArcherExp;

        wizardLevel = StaticManager.Backend.GameData.PlayerGameData.DWizardLevel;
        wizardHp = StaticManager.Backend.GameData.PlayerGameData.DWizardHp;
        wizardExp = StaticManager.Backend.GameData.PlayerGameData.DWizardExp;

        clearStageLevel = StaticManager.Backend.GameData.PlayerGameData.ClearStageLevel;
        nowStageLevel = StaticManager.Backend.GameData.PlayerGameData.NowStageLevel;

    }

    //public void UpdateHp(float value)
    //{
    //    hp = value;
    //}

    //public void UpdateSp(float value)
    //{
    //    sp = value;
    //}

    //public void UpdateExp(float value)
    //{
    //    exp = value;
    //}

    //public void UpdateLevel(int value)
    //{
    //    level = value;
    //}

    //public void UpdateCoin(int value)
    //{
    //    coin = value;
    //}

    //public void UpdateRuby(int value)
    //{
    //    ruby = value;
    //}

    //public void UpdateCore(int value)
    //{
    //    core = value;
    //}
    //public void UpdateQuickSlot(List<PlayerQuickSlotItemData> quickSlotInGame)
    //{
    //    quickSlot = quickSlotInGame;
    //}

    //public void UpdateBuff(List<PlayerBuffGameData> gameBuff)
    //{
    //    buffs.Clear();
    //    for(int i=0; i < gameBuff.Count; ++i)
    //    {
    //        PlayerBuffItemData playerBuffItemData = new PlayerBuffItemData();
    //        playerBuffItemData.buffName = gameBuff[i].buffName;
    //        playerBuffItemData.buffTime = (int)gameBuff[i].buffTime;
    //        buffs.Add(playerBuffItemData);
    //    }        
    //}

    public void UpdateWarriorLevel(int value)
    {
        warriorLevel = value;
    }

    public void UpdateWarriorHp(float value)
    {
        warriorHp = value;
    }

    public void UpdateWarriorExp(float value)
    {
        warriorExp = value;
    }

    public void UpdateArcherLevel(int value)
    {
        archerLevel = value;
    }

    public void UpdateArcherHp(float value)
    {
        archerHp = value;
    }

    public void UpdateArcherExp(float value)
    {
        archerExp = value;
    }

    public void UpdateWizardLevel(int value)
    {
        wizardLevel = value;
    }

    public void UpdateWizardHp(float value)
    {
        wizardHp = value;
    }

    public void UpdateWizardExp(float value)
    {
        wizardExp = value;
    }
    public void UpdateClearStageLevel()
    {
        clearStageLevel += 1;
    }
    public void UpdateNowStageLevel()
    {
        nowStageLevel += 1;
    }

    #endregion

    #region PlayerInventory

    private void SetPlayerInventory(PlayerInventory[] savePlayerInventories)
    {
        foreach (var savePlayerInventory in savePlayerInventories)
        {
            SavePlayerInventory saveInventory = saveInventories.Find(data => data.inventoryName == savePlayerInventory.inventoryName);

            if (savePlayerInventory.inventoryDatas.Length <= 0) continue;

            JsonData inventoryDatasJsonData = JsonMapper.ToObject(savePlayerInventory.inventoryDatas);
            for (int i = 0; i < inventoryDatasJsonData.Count; i++)
                saveInventory.inventoryDatas.Add(JsonMapper.ToObject<PlayerInventoryData>(inventoryDatasJsonData[i].ToJson()));
        }
    }


    #endregion

    #region PlayerEquip

    public void SetPlayerEquipData(PlayerEquipData playerEquipData)
    {
        equipData = playerEquipData;
    }

    public void SavePlayerEquipData(PlayerEquipData changedPlayerEquipData)
    {
        equipData = changedPlayerEquipData;
    }

    #endregion

    #region PlayerQuestData

    private void SetPlayerQuestData(PlayerQuestData[] playerQuestDatas)
    {
        foreach (var playerQuestData in playerQuestDatas)
        {
            quests.Add(playerQuestData.questName, JsonUtility.FromJson<QuestSaveData>(playerQuestData.questDataJson));
        }
    }

    public void AddQuest(QuestData questData)
    {
        if (quests.ContainsKey(questData.questName))
            return;

        quests.Add(questData.questName, questData.saveData);

        Param param = new Param();
        param.Add("questName", questData.questName);
        param.Add("questDataJson", JsonUtility.ToJson(questData.saveData));

        BackEndFunctions.instance.InsertData(ServerGameDefine.PLAYER_QUEST_DATA, param);
    }

    public void UpdateQuest(QuestData questData)
    {
        if (quests.ContainsKey(questData.questName))
        {
            QuestSaveData questSaveData = quests[questData.questName];
            questSaveData = questData.saveData;
        }
    }

    public void RemoveQuest(QuestData questData)
    {
        if (quests.ContainsKey(questData.questName))
        {
            quests.Remove(questData.questName);
        }

        Where where = new Where();
        where.Equal("questName", questData.questName);

        BackEndFunctions.instance.DeleteData(ServerGameDefine.PLAYER_QUEST_DATA, where);
    }

    #endregion

    #region Stage

    public void SetStage()
    {
        var stageTypes = SceneSettingManager.instance.stageTypes;
        for (int sector = 0; sector < stageTypes.Count; sector++)
        {
            for (int sectorIndex = 0; sectorIndex < stageTypes[sector].Count; sectorIndex++)
            {
                string stage = stageTypes[sector][sectorIndex];
                if (!ObscuredPrefs.HasKey(stage))
                    ObscuredPrefs.SetBool(stage, (sector == 0 && sectorIndex == 0));

                stageLocks.Add(new StageLock(sector, sectorIndex, ObscuredPrefs.GetBool(stage)));
            }
        }
    }

    public void SetUnlockStageToServer(int sector, int sectorIndex, bool isOpened)
    {
        var stageTypes = SceneSettingManager.instance.stageTypes;

        StageLock stageLock = stageLocks.Find(data => data.sector == sector && data.sectorIndex == sectorIndex);
        if (stageLock != null)
            stageLock.isOpened = isOpened;

        ObscuredPrefs.SetBool(stageTypes[sector][sectorIndex], isOpened);
    }

    #endregion

    #region DefaultStage

    public void SetStageRepeat(bool isRepeat)
    {
        ObscuredPrefs.SetBool("IsStageRepeat", isRepeat);
    }

    public bool GetStageRepeat()
    {
        return ObscuredPrefs.GetBool("IsStageRepeat", defaultValue: false);
    }

    public void SetAutoAttack(bool isAutoCombo)
    {
        ObscuredPrefs.SetBool("IsAutoCombo", isAutoCombo);
    }

    public bool GetAutoCombo()
    {
        return ObscuredPrefs.GetBool("IsAutoCombo", defaultValue: false);
    }

    public void SetAutoPotion(bool isOn)
    {
        ObscuredPrefs.SetBool("IsAutoPotion", isOn);
    }

    public bool GetAutoPotion()
    {
        return ObscuredPrefs.GetBool("IsAutoPotion", defaultValue: false);
    }

    public void SetAutoSkill(int skillIndex, bool isOn)
    {
        ObscuredPrefs.SetBool($"IsAutoSkill_{skillIndex}", isOn);
    }

    public bool GetAutoSkill(int skillIndex)
    {
        return ObscuredPrefs.GetBool($"IsAutoSkill_{skillIndex}", defaultValue: false);
    }

    private void HandleOnSectorChanged(int sector)
    {
        ObscuredPrefs.SetInt("Sector", sector);
    }

    public int GetSector()
    {
        return ObscuredPrefs.GetInt($"Sector", defaultValue: 0);
    }

    private void HandleOnSectorIndexChanged(int sectorIndex)
    {
        ObscuredPrefs.SetInt("SectorIndex", sectorIndex);
    }

    public int GetSectorIndex()
    {
        return ObscuredPrefs.GetInt($"SectorIndex", defaultValue: 0);
    }

    #endregion

    #region Upgrade

    private void SetUpgrade(PlayerUpgradeData playerUpgradeData)
    {
        if (upgrades.Count > 0)
            return;

        upgrades.Add(UpgradeTarget.Skill_01, playerUpgradeData.skill01);
        upgrades.Add(UpgradeTarget.Skill_02, playerUpgradeData.skill02);
        upgrades.Add(UpgradeTarget.Skill_03, playerUpgradeData.skill03);
        upgrades.Add(UpgradeTarget.Skill_04, playerUpgradeData.skill04);

        upgrades.Add(UpgradeTarget.Upgrade_01, playerUpgradeData.upgrade01);
        upgrades.Add(UpgradeTarget.Upgrade_02, playerUpgradeData.upgrade02);
        upgrades.Add(UpgradeTarget.Upgrade_03, playerUpgradeData.upgrade03);
        upgrades.Add(UpgradeTarget.Upgrade_04, playerUpgradeData.upgrade04);
        upgrades.Add(UpgradeTarget.Upgrade_05, playerUpgradeData.upgrade05);
        upgrades.Add(UpgradeTarget.Upgrade_06, playerUpgradeData.upgrade06);
        upgrades.Add(UpgradeTarget.Upgrade_07, playerUpgradeData.upgrade07);
        upgrades.Add(UpgradeTarget.Upgrade_08, playerUpgradeData.upgrade08);
        upgrades.Add(UpgradeTarget.Upgrade_09, playerUpgradeData.upgrade09);
    }

    public void UpdateUpgrade(UpgradeTarget target, int level)
    {
        Param param = new Param();

        switch (target)
        {
            case UpgradeTarget.Skill_01:
                param.Add("skill01", level);
                break;
            case UpgradeTarget.Skill_02:
                param.Add("skill02", level);
                break;
            case UpgradeTarget.Skill_03:
                param.Add("skill03", level);
                break;
            case UpgradeTarget.Skill_04:
                param.Add("skill04", level);
                break;
            case UpgradeTarget.Upgrade_01:
                param.Add("upgrade01", level);
                break;
            case UpgradeTarget.Upgrade_02:
                param.Add("upgrade02", level);
                break;
            case UpgradeTarget.Upgrade_03:
                param.Add("upgrade03", level);
                break;
            case UpgradeTarget.Upgrade_04:
                param.Add("upgrade04", level);
                break;
            case UpgradeTarget.Upgrade_05:
                param.Add("upgrade05", level);
                break;
            case UpgradeTarget.Upgrade_06:
                param.Add("upgrade06", level);
                break;
            case UpgradeTarget.Upgrade_07:
                param.Add("upgrade07", level);
                break;
            case UpgradeTarget.Upgrade_08:
                param.Add("upgrade08", level);
                break;
            case UpgradeTarget.Upgrade_09:
                param.Add("upgrade09", level);
                break;
        }
    }

    #endregion

    #region Option

    private void HandleOnVolumeChanged(SoundType soundType, float volume)
    {
        switch (soundType)
        {
            case SoundType.BGM:
                SetBgmVolume(volume);
                break;
            case SoundType.SFX:
                SetSfxVolume(volume);
                break;
            case SoundType.VOICE:
                SetVoiceVolume(volume);
                break;
        }
    }

    private void SetBgmVolume(float volume)
    {
        ObscuredPrefs.SetFloat("BgmVolume", volume);
    }

    private float GetBgmVolume()
    {
        return ObscuredPrefs.GetFloat("BgmVolume", defaultValue: 0.5f);
    }

    private void SetSfxVolume(float volume)
    {
        ObscuredPrefs.SetFloat("SfxVolume", volume);
    }

    private float GetSfxVolume()
    {
        return ObscuredPrefs.GetFloat("SfxVolume", defaultValue: 0.5f);
    }

    private void SetVoiceVolume(float volume)
    {
        ObscuredPrefs.SetFloat("VoiceVolume", volume);
    }

    private float GetVoiceVolume()
    {
        return ObscuredPrefs.GetFloat("VoiceVolume", defaultValue: 0.5f);
    }
    public bool GetIsKorean()
    {
        return ObscuredPrefs.GetBool("IsKorean", defaultValue: true);
    }
    public void SetIsKorean(bool isKoran)
    {
        ObscuredPrefs.SetBool("IsKorean", isKoran);
    }
    #endregion

    #region HangarRegion
    //public void CalcHangarRefreshTime()
    //{
    //    if (HangarInitTime == null)
    //        return;

    //    DateTime hangarInitTime = DateTime.Parse(HangarInitTime);
    //    DateTime nowTime = DateTime.Now;

    //    TimeSpan timeSpan = nowTime - hangarInitTime;

    //    double time = Define.RandomShopRefreshTime - timeSpan.TotalSeconds;
    
    //    if (time < 0)
    //        RandomItemRefreshTime = 0;        
    //    else
    //        RandomItemRefreshTime = time;
    //}

    #endregion
}
