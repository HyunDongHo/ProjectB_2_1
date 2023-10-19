using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSettingManager : Singleton<SceneSettingManager>
{
    public const string SETTING_SCENE = "01.SettingScene";
    public const string INTRO_SCENE = "02.IntroScene";
    public const string ACCOUNT_SCENE = "03.AccountScene";
    public const string NICKNAME_SCENE = "04.NicknameScene";
    public const string LOBBY_SCENE = "05.LobbyScene";    
    public const string DEFAULT_STAGE_INTRO_SCENE = "06.GameStageIntroScene";

    public const string NORMAL_STAGE_SCENE = "06_1.Stage";
    public const string MIDDLE_BOSS_STAGE_SCENE = "06_2.MiddleBossStage";
    public const string FINAL_BOSS_STAGE_SCENE = "06_3.FinalBossStage";

    public const string DEFAULT_OUTSIDE_STAGE_SCENE = "06_1.GameStage_Outside_Temp"; 
    //public const string DEFAULT_INSIDE_STAGE_SCENE = "06_2.GameStage_Inside";
    public const string DEFAULT_INSIDE_STAGE_SCENE = "06_1.GameStage_OutSide_Temp2";  
    public const string BOSS_STAGE_SCENE = "07_1.BossStage";
    public const string RANDOM_LOTTERY_GENERAL = "98_1.RandomLotteryScene_General";
    public const string RANDOM_LOTTERY_PRODUCT = "98_2.RandomLotteryScene_Product";
    public const string LODING_SCENE_DEFAULT = "99_1.LoadingScene_Default";
    public const string LODING_SCENE_PROGRESS_ACCOUNT_TO_LOBBY = "99_2.LoadingScene_Progress_Account_To_Lobby";
    public const string LODING_SCENE_PROGRESS_ANYWHERE_TO_GAME = "99_2.LoadingScene_Progress_AnyWhere_To_Game";
    public const string LODING_SCENE_VIDEO_PORTAL = "99_3.LoadingScene_Video_Portal";

    public readonly Dictionary<int, Dictionary<int, string>> stageTypes = new Dictionary<int, Dictionary<int, string>>()
    {
        { 0, new Dictionary<int, string>(){ {0, "A_Sector_Outside_01" }, { 1, "A_Sector_Inside_01" }, { 2, "A_Sector_Outside_02" }, { 3, "A_Sector_Inside_02" }, { 4, "A_Sector_Outside_03" }, { 5, "A_Sector_Inside_03" }, { 6, "A_Sector_Outside_04" }, { 7, "A_Sector_Inside_04" }, { 8, "A_Sector_Outside_05" }, { 9, "A_Sector_Inside_05" }, { 10, "Hidden_Sector_A_Inside" }, { 11, "Hidden_Sector_A_Outside" } } },
        { 1, new Dictionary<int, string>(){ {0, "B_Sector_Outside_01" }, { 1, "B_Sector_Inside_01" }, { 2, "B_Sector_Outside_02" }, { 3, "B_Sector_Inside_02" }, { 4, "B_Sector_Outside_03" }, { 5, "B_Sector_Inside_03" }, { 6, "B_Sector_Outside_04" }, { 7, "B_Sector_Inside_04" }, { 8, "B_Sector_Outside_05" }, { 9, "B_Sector_Inside_05" }, { 10, "Hidden_Sector_B_Inside" }, { 11, "Hidden_Sector_B_Outside" } } },
        { 2, new Dictionary<int, string>(){ {0, "C_Sector_Outside_01" }, { 1, "C_Sector_Inside_01" }, { 2, "C_Sector_Outside_02" }, { 3, "C_Sector_Inside_02" }, { 4, "C_Sector_Outside_03" }, { 5, "C_Sector_Inside_03" }, { 6, "C_Sector_Outside_04" }, { 7, "C_Sector_Inside_04" }, { 8, "C_Sector_Outside_05" }, { 9, "C_Sector_Inside_05" }, { 10, "Hidden_Sector_C_Inside" }, { 11, "Hidden_Sector_C_Outside" } } },
        { 3, new Dictionary<int, string>(){ {0, "D_Sector_Outside_01" }, { 1, "D_Sector_Inside_01" }, { 2, "D_Sector_Outside_02" }, { 3, "D_Sector_Inside_02" }, { 4, "D_Sector_Outside_03" }, { 5, "D_Sector_Inside_03" }, { 6, "D_Sector_Outside_04" }, { 7, "D_Sector_Inside_04" }, { 8, "D_Sector_Outside_05" }, { 9, "D_Sector_Inside_05" } } }
    };

    public readonly Dictionary<int, Dictionary<int, string>> bossStageTypes = new Dictionary<int, Dictionary<int, string>>()  
    {
        { 0, new Dictionary<int, string>(){ {0, "A_Sector_Boss" } } },
        { 1, new Dictionary<int, string>(){ {0, "B_Sector_Boss" } } },
        { 2, new Dictionary<int, string>(){ {0, "C_Sector_Boss" } } },
        { 3, new Dictionary<int, string>(){ {0, "D_Sector_Boss" } } }
     };

    public readonly Dictionary<long, string> NormalSectorType = new Dictionary<long, string>()
    {
        { 1, "Normal_Sector1"},
        { 2, "Normal_Sector2"},
        { 3, "Normal_Sector3"},
        { 4, "Normal_Sector4"}
     };

    public readonly Dictionary<long, string> MiddleSectorType = new Dictionary<long, string>()
    {
        { 1, "Middle_Sector1"},
        { 2, "Middle_Sector2"},
        { 3, "Middle_Sector3"},
        { 4, "Middle_Sector4"}
     };

    public readonly Dictionary<long, string> BossSectorType = new Dictionary<long, string>()
    {
        { 1, "Boss_Sector1"},
        { 2, "Boss_Sector2"},
        { 3, "Boss_Sector3"},
        { 4, "Boss_Sector4"}
     };

    private const int MAX_SECTOR_INDEX = 10;

    public int sector { get; private set; } = 0;
    public int sectorIndex { get; private set; } = 0;

    public Action<int> OnSectorChanged;
    public Action<int> OnSectorIndexChanged;

    public int bossSector { get; private set; } = 0;
    public int bossSectorIndex { get; private set; } = 0;

    public Action<int> OnBossSectorChanged;
    public Action<int> OnBossSectorIndexChanged;

    public long NowMoveStageNum { get; private set; }
    public string GetSectorName(int sector)
    {
        string sectorName = string.Empty;
        switch (sector)
        {
            case 0:
                sectorName = "A";
                break;
            case 1:
                sectorName = "B";
                break;
            case 2:
                sectorName = "C";
                break;
            case 3:
                sectorName = "D";
                break;
        }
        return sectorName;
    }

    public string GetSectorIndexName(int sectorIndex)
    {
        string sectorIndexName = string.Empty;
        switch (sector)
        {
            case 0:
                sectorIndexName = "Outside_01";
                break;
            case 1:
                sectorIndexName = "Inside_01";
                break;
            case 2:
                sectorIndexName = "Outside_02";
                break;
            case 3:
                sectorIndexName = "Inside_02";
                break;
            case 4:
                sectorIndexName = "Outside_03";
                break;
            case 5:
                sectorIndexName = "Inside_03";
                break;
            case 6:
                sectorIndexName = "Outside_04";
                break;
            case 7:
                sectorIndexName = "Inside_04";
                break;
            case 8:
                sectorIndexName = "Outside_05";
                break;
            case 9:
                sectorIndexName = "Inside_05";
                break;
            case 10:
                sectorIndexName = "Hide_Outside_01";
                break;
            case 11:
                sectorIndexName = "Hide_Inside_01";
                break;
        }
        return sectorIndexName;
    }

    public string GetBossSectorName(long stageNum)
    {
        // 현재 스테이지 / 10 계산  후 현재 존재하는 보스 스테이지 수로 다시 나누기
        long bossSector = (stageNum / 10) / BossSectorType.Count;

        if(BossSectorType.ContainsKey(bossSector))
        {
            return BossSectorType[bossSector];
        }

        return string.Empty;
    }

    public string GetBossSectorIndexName(int sectorIndex)
    {
        string sectorIndexName = string.Empty;
        switch (sector)
        {
            case 0:
                sectorIndexName = "Difficulty_1";
                break;
            case 1:
                sectorIndexName = "Difficulty_2";
                break;
            case 2:
                sectorIndexName = "Difficulty_3";
                break;
        }
        return sectorIndexName;
    }

    public string GetSectorFullName()
    {
        string sectorName = string.Empty;
        string sectorIndexName = string.Empty;

        switch (sector)
        {
            case 0:
                sectorName = "A지구 ";
                break;
            case 1:
                sectorName = "B지구 ";
                break;
            case 2:
                sectorName = "C지구 ";
                break;
            case 3:
                sectorName = "D지구 ";
                break;
        }

        switch(sectorIndex)
        {
            case 0:
                sectorIndexName = "1구역 외부";
                break;
            case 1:
                sectorIndexName = "1구역 내부";
                break;
            case 2:
                sectorIndexName = "2구역 외부";
                break;
            case 3:
                sectorIndexName = "2구역 내부";
                break;
            case 4:
                sectorIndexName = "3구역 외부";
                break;
            case 5:
                sectorIndexName = "3구역 내부";
                break;
            case 6:
                sectorIndexName = "4구역 외부";
                break;
            case 7:
                sectorIndexName = "4구역 내부";
                break;
            case 8:
                sectorIndexName = "5구역 외부";
                break;
            case 9:
                sectorIndexName = "5구역 내부";
                break;
            case 10:
                sectorIndexName = "숨겨진 구역 외부";
                break;
            case 11:
                sectorIndexName = "숨겨진 구역 내부";
                break;
        }

        return sectorName + sectorIndexName;
    }

    public string GetCurrentStage()
    {
        return stageTypes[sector][sectorIndex];
    }

    public string GetCurrentBossStage()
    {
        return bossStageTypes[bossSector][bossSectorIndex];
    }

    public void SetNext()
    {
        //StaticManager.Backend.GameData.PlayerGameData.UpdateStageLevel();


        int setSector = sector;
        int setSectorIndex = sectorIndex + 1;

        int maxSectorIndexCount = stageTypes[setSector].Count;

        if (setSectorIndex > MAX_SECTOR_INDEX && setSectorIndex >= stageTypes[sector].Count)
        {
            setSector = Mathf.Clamp(++setSector, 0, maxSectorIndexCount);
            setSectorIndex = 0;
        }
        else if (setSectorIndex >= MAX_SECTOR_INDEX)
        {
            setSector = Mathf.Clamp(++setSector, 0, maxSectorIndexCount);
            setSectorIndex = 0;
        }

        SetStage(setSector, setSectorIndex);
    }

    public void SetStageWithName(string stageName)
    {
        if (stageName == "Lobby")
        {
            LoadLobbyStageScene();
        }
        else
        {
            for (int sector = 0; sector < stageTypes.Count; sector++)
            {
                for (int sectorIndex = 0; sectorIndex < stageTypes[sector].Count; sectorIndex++)
                {
                    if (stageTypes[sector][sectorIndex] == stageName)
                    {
                        SetStage(sector, sectorIndex);
                     //   LoadDefaultStageScene();  

                        break;  
                    }
                }
            }  

            for (int sector = 0; sector < bossStageTypes.Count; sector++)  
            {
                for (int sectorIndex = 0; sectorIndex < bossStageTypes[sector].Count; sectorIndex++)
                {
                    if (bossStageTypes[sector][sectorIndex] == stageName)
                    {
                        SetBossStage(sector, sectorIndex);
                        LoadBossStageScene();

                        break;
                    }
                }
            }  
        }
    }

    public void SetStage(int sector, int sectorIndex)
    {
        this.sector = sector;
        OnSectorChanged?.Invoke(sector);

        this.sectorIndex = sectorIndex;
        OnSectorIndexChanged?.Invoke(sectorIndex);

        UserDataManager.instance.SetUnlockStageToServer(sector, sectorIndex, true);
    }

    public void SetBossStage(int sector, int sectorIndex)
    {
        this.bossSector = sector;
        OnBossSectorChanged?.Invoke(sector);

        this.bossSectorIndex = sectorIndex;
        OnBossSectorIndexChanged?.Invoke(sectorIndex);
    }

    public void LoadAccountScene(bool isFade)
    {
        Load(ACCOUNT_SCENE, isFade: isFade, LODING_SCENE_DEFAULT);
    }
    public void LoadAccountToNickNameScene()
    {
        Load(NICKNAME_SCENE, isFade: true, LODING_SCENE_PROGRESS_ACCOUNT_TO_LOBBY);
    }
    public void LoadAccountToLobbyStageScene()
    {
        Load(LOBBY_SCENE, isFade: true, LODING_SCENE_PROGRESS_ACCOUNT_TO_LOBBY);
    }
    public void LoadNicknameToLobbyStageScene()
    {
        Load(LOBBY_SCENE, isFade: true, LODING_SCENE_PROGRESS_ACCOUNT_TO_LOBBY);
    }
    public bool haveReachedTheLobbyScene = false;
    public void LoadLobbyStageScene()
    {
        Load(LOBBY_SCENE, isFade: true, LODING_SCENE_PROGRESS_ANYWHERE_TO_GAME);
        haveReachedTheLobbyScene = true;
    }

    public bool haveReachedTheDefaultStage = false;
    public void LoadDefaultStageScene(long loadStageNum)
    {
        long moveStageLevel = loadStageNum;
        NowMoveStageNum = loadStageNum;

       // (StageManager.instance as DefaultStageManager).enemyManager.isStageStart = false;

        Load(NORMAL_STAGE_SCENE, isFade: true, LODING_SCENE_PROGRESS_ANYWHERE_TO_GAME,
              () =>
              {
                  (StageManager.instance as DefaultStageManager).enviornment.SpawnAllStageObject();
                  InitStage(moveStageLevel); ;
                  (StageManager.instance as DefaultStageManager).enemyManager.isStageStart = true;
                  // StageData stageData = (StageManager.instance as DefaultStageManager).enviornment.SetStage(moveStageLevel);                
                  // (StageManager.instance as DefaultStageManager).enemyManager.Init(stageData);
                  // PlayersControlManager.instance.AllPlayerOn();
                  // (StageManager.instance as DefaultStageManager).LoadDefault();
              });

     //   if (moveStageLevel % 10 == 0)
     //   {
     //       Load(FINAL_BOSS_STAGE_SCENE, isFade: true, LODING_SCENE_PROGRESS_ANYWHERE_TO_GAME,
     //             () =>
     //             {
     //                 (StageManager.instance as DefaultStageManager).LoadDefault();
     //                 //(StageManager.instance as DefaultStageManager).LoadIntro();
     //             });
     //   }
     //   else if (moveStageLevel % 5 == 0)
     //   {
     //       Load(MIDDLE_BOSS_STAGE_SCENE, isFade: true, LODING_SCENE_PROGRESS_ANYWHERE_TO_GAME,
     //         () =>
     //         {
     //             (StageManager.instance as DefaultStageManager).LoadDefault();
     //             //(StageManager.instance as DefaultStageManager).LoadIntro();
     //         });
     //   }
     //   else
     //   {
     //       Load(NORMAL_STAGE_SCENE, isFade: true, LODING_SCENE_PROGRESS_ANYWHERE_TO_GAME,
     //          () =>
     //          {
     //              (StageManager.instance as DefaultStageManager).LoadDefault();
     //              //(StageManager.instance as DefaultStageManager).LoadIntro();
     //          });
     //   }

        //BossStage
      //  if (moveStageLevel % 10 == 0)
      //  {
      //      Load(FINAL_BOSS_STAGE_SCENE, isFade: true, LODING_SCENE_PROGRESS_ANYWHERE_TO_GAME,
      //            () =>
      //            {
      //                (StageManager.instance as DefaultStageManager).LoadDefault();
      //                //(StageManager.instance as DefaultStageManager).LoadIntro();
      //            });
      //  }
      //  else if (moveStageLevel % 5 == 0)
      //  {
      //      Load(MIDDLE_BOSS_STAGE_SCENE, isFade: true, LODING_SCENE_PROGRESS_ANYWHERE_TO_GAME,
      //        () =>
      //        {
      //            (StageManager.instance as DefaultStageManager).LoadDefault();
      //            //(StageManager.instance as DefaultStageManager).LoadIntro();
      //        });
      //  }
      //  else
      //  {
      //      Load(NORMAL_STAGE_SCENE, isFade: true, LODING_SCENE_PROGRESS_ANYWHERE_TO_GAME,
      //         () =>
      //         {
      //             (StageManager.instance as DefaultStageManager).LoadDefault();
      //           //(StageManager.instance as DefaultStageManager).LoadIntro();
      //       });
      //  }
    }
    public void LoadDungeonScene(int trainingDungeonNum)
    {
        (StageManager.instance as DefaultStageManager).enemyManager.isStageStart = false;

        StaticManager.UI.FadeUI.FadeStart(FadeUI.FadeType.ChangeToBlack, () => {

            //(StageManager.instance as DefaultStageManager).enviornment.SpawnAllStageObject();
            InitDungeon(trainingDungeonNum); ;
            (StageManager.instance as DefaultStageManager).enemyManager.isStageStart = true;

            /*던전 인게임창 열기 */
            StageManager.instance.canvasManager.GetUIManager<UI_DungeonIngame>().SetActiveExitObj(true);  

        });

    }
    public void LoadNextStageScene()
    {
        long moveStageLevel = StaticManager.Backend.GameData.PlayerGameData.NowStageLevel + 1;
        NowMoveStageNum = moveStageLevel;

        (StageManager.instance as DefaultStageManager).enemyManager.isStageStart = false;

        StaticManager.UI.FadeUI.FadeStart(FadeUI.FadeType.ChangeToBlack, () => {

            StaticManager.Backend.GameData.PlayerGameData.UpdateNowStageLevel(moveStageLevel);
            InitStage(moveStageLevel);
            (StageManager.instance as DefaultStageManager).enemyManager.isStageStart = true;
            // StageData stageData = (StageManager.instance as DefaultStageManager).enviornment.SetStage(moveStageLevel);
            // (StageManager.instance as DefaultStageManager).enemyManager.Init(stageData);
            // PlayersControlManager.instance.AllPlayerOn();
            // (StageManager.instance as DefaultStageManager).LoadDefault();

        });


        //BossStage
      // if (moveStageLevel % 10 == 0)
      // {
      //     Load(FINAL_BOSS_STAGE_SCENE, isFade: true, LODING_SCENE_PROGRESS_ANYWHERE_TO_GAME,
      //           () =>
      //           {
      //               (StageManager.instance as DefaultStageManager).LoadDefault();
      //               StaticManager.Backend.GameData.PlayerGameData.UpdateNowStageLevel(moveStageLevel);
      //               StageManager.instance.canvasManager.GetUIManager<UIManager_Stage>().RefreshStageUI();
      //               Debug.Log($"현재 스테이지 {moveStageLevel}");
      //
      //               //(StageManager.instance as DefaultStageManager).LoadIntro();
      //           });
      // }
      // else if (moveStageLevel % 5 == 0)
      // {
      //     Load(MIDDLE_BOSS_STAGE_SCENE, isFade: true, LODING_SCENE_PROGRESS_ANYWHERE_TO_GAME,
      //       () =>
      //       {
      //           (StageManager.instance as DefaultStageManager).LoadDefault();
      //           StaticManager.Backend.GameData.PlayerGameData.UpdateNowStageLevel(moveStageLevel);
      //           StageManager.instance.canvasManager.GetUIManager<UIManager_Stage>().RefreshStageUI();
      //           Debug.Log($"현재 스테이지 {moveStageLevel}");
      //           //(StageManager.instance as DefaultStageManager).LoadIntro();
      //       });
      // }
      // else
      // {
      //     Load(NORMAL_STAGE_SCENE, isFade: true, LODING_SCENE_PROGRESS_ANYWHERE_TO_GAME,
      //        () =>
      //        {
      //            (StageManager.instance as DefaultStageManager).LoadDefault();
      //            StaticManager.Backend.GameData.PlayerGameData.UpdateNowStageLevel(moveStageLevel);
      //            StageManager.instance.canvasManager.GetUIManager<UIManager_Stage>().RefreshStageUI();
      //            Debug.Log($"현재 스테이지 {moveStageLevel}");
      //            //(StageManager.instance as DefaultStageManager).LoadIntro();
      //        });
      // }
    }

    private void InitStage(long moveStageLevel)
    {
        (StageManager.instance as DefaultStageManager).enemyManager.Clear();
        StageData stageData = (StageManager.instance as DefaultStageManager).enviornment.SetStage(moveStageLevel);
        // (StageManager.instance as DefaultStageManager).enemyManager.Init(stageData);
        (StageManager.instance as DefaultStageManager).enemyManager.SetTrainingDungeonPlayerType(PlayerType.None);
        (StageManager.instance as DefaultStageManager).enemyManager.Init(stageData);
        
        PlayersControlManager.instance.AllPlayerOn();
        (StageManager.instance as DefaultStageManager).LoadDefault();

        StageManager.instance.canvasManager.GetUIManager<UIManager_Stage>().gameObject.GetComponent<StageWindow>().SetStageUI();

    }
    private void InitDungeon(int trainingDungeonNum)
    {
        (StageManager.instance as DefaultStageManager).enemyManager.Clear();
        StageData stageData = (StageManager.instance as DefaultStageManager).enviornment.SetDungeon(trainingDungeonNum);

        (StageManager.instance as DefaultStageManager).enemyManager.SetTrainingDungeonPlayerType((PlayerType)Enum.Parse(typeof(PlayerType), Enum.GetName(typeof(PlayerType), trainingDungeonNum)));
        (StageManager.instance as DefaultStageManager).enemyManager.Init(stageData);  
        PlayersControlManager.instance.SelectPlayerOn(trainingDungeonNum);    
        PlayersControlManager.instance.SetActivePlayer((PlayerType)Enum.Parse(typeof(PlayerType), Enum.GetName(typeof(PlayerType), trainingDungeonNum)), PlayersControlMode.TrainingDungeon);
        PlayersControlManager.instance.SetNowActivePlayerPos(stageData);  
        (StageManager.instance as DefaultStageManager).LoadDefault();




    }

    public void LoadReStartStageScene()
    {
        long moveStageLevel = StaticManager.Backend.GameData.PlayerGameData.NowStageLevel;
        NowMoveStageNum = moveStageLevel;
        (StageManager.instance as DefaultStageManager).enemyManager.isStageStart = false;


        StaticManager.UI.FadeUI.FadeStart(FadeUI.FadeType.ChangeToBlack, () => {

            PlayersControlManager.instance.ResetHpAllPlayer();
            PlayersControlManager.instance.ResetAllPlayerState();
            PlayersControlManager.instance.SetNotActiveAllPlayer();

            InitStage(moveStageLevel);
            (StageManager.instance as DefaultStageManager).enemyManager.isStageStart = true;
            // StageData stageData = (StageManager.instance as DefaultStageManager).enviornment.SetStage(moveStageLevel);
            // (StageManager.instance as DefaultStageManager).enemyManager.Init(stageData);
            // PlayersControlManager.instance.AllPlayerOn();
            // (StageManager.instance as DefaultStageManager).LoadDefault();


        });



       //long moveStageLevel = StaticManager.Backend.GameData.PlayerGameData.NowStageLevel ;
       //NowMoveStageNum = moveStageLevel;
       ////BossStage
       //if (moveStageLevel % 10 == 0)
       //{
       //    Load(FINAL_BOSS_STAGE_SCENE, isFade: true, LODING_SCENE_PROGRESS_ANYWHERE_TO_GAME,
       //          () =>
       //          {
       //              (StageManager.instance as DefaultStageManager).LoadDefault();
       //              StaticManager.Backend.GameData.PlayerGameData.UpdateNowStageLevel(moveStageLevel);
       //              Debug.Log($"현재 스테이지 {moveStageLevel}");
       //
       //              //(StageManager.instance as DefaultStageManager).LoadIntro();
       //          });
       //}
       //else if (moveStageLevel % 5 == 0)
       //{
       //    Load(MIDDLE_BOSS_STAGE_SCENE, isFade: true, LODING_SCENE_PROGRESS_ANYWHERE_TO_GAME,
       //      () =>
       //      {
       //          (StageManager.instance as DefaultStageManager).LoadDefault();
       //          StaticManager.Backend.GameData.PlayerGameData.UpdateNowStageLevel(moveStageLevel);
       //          Debug.Log($"현재 스테이지 {moveStageLevel}");
       //          //(StageManager.instance as DefaultStageManager).LoadIntro();
       //      });
       //}
       //else
       //{
       //    Load(NORMAL_STAGE_SCENE, isFade: true, LODING_SCENE_PROGRESS_ANYWHERE_TO_GAME,
       //       () =>
       //       {
       //           (StageManager.instance as DefaultStageManager).LoadDefault();
       //           StaticManager.Backend.GameData.PlayerGameData.UpdateNowStageLevel(moveStageLevel);
       //           Debug.Log($"현재 스테이지 {moveStageLevel}");  
       //           //(StageManager.instance as DefaultStageManager).LoadIntro();
       //       });
       //}
    }
    public void LoadBackToStageScene() // 던전 하고 스테이지로 돌아갈 때 
    {
        long moveStageLevel = StaticManager.Backend.GameData.PlayerGameData.NowStageLevel;
        NowMoveStageNum = moveStageLevel;
        (StageManager.instance as DefaultStageManager).enemyManager.isStageStart = false;


        StaticManager.UI.FadeUI.FadeStart(FadeUI.FadeType.ChangeToBlack, () => {

            PlayersControlManager.instance.ResetHpAllPlayer();
            PlayersControlManager.instance.ResetAllPlayerState();
            PlayersControlManager.instance.SetNotActiveAllPlayer();
            if ((StageManager.instance as DefaultStageManager).enemyManager.GetNowRemainTime() == 0)
            {
                /* 보상 적용 (차트 기준 계산 )*/
                if((StageManager.instance as DefaultStageManager).enemyManager.GetStageType() == StageTypeNum.TrainingDungeon && (StageManager.instance as DefaultStageManager).enemyManager.dieMonsterCount > 0)
                {
                    Dictionary<int, double> FinalRewardItems = new();

                    BackendData.Chart.DungeonData.Item dungeonChartItem = null;
                    switch ((StageManager.instance as DefaultStageManager).enemyManager.GetTDPlayerType())
                    {
                        case PlayerType.Warrior:
                            dungeonChartItem = StaticManager.Backend.Chart.DungeonData.GetItem(BackendData.Chart.DungeonData.DungeonType.Training, BackendData.Chart.DungeonData.RewardItemType.Warrior);
                            break;
                        case PlayerType.Archer:
                            dungeonChartItem = StaticManager.Backend.Chart.DungeonData.GetItem(BackendData.Chart.DungeonData.DungeonType.Training, BackendData.Chart.DungeonData.RewardItemType.Archer);
                            break;
                        case PlayerType.Wizard:
                            dungeonChartItem = StaticManager.Backend.Chart.DungeonData.GetItem(BackendData.Chart.DungeonData.DungeonType.Training, BackendData.Chart.DungeonData.RewardItemType.Wizard);
                            break;
                    }

                    
                    List<BackendData.Chart.DungeonData.Item.RewardItemInfo> TrainingRewardIDList = dungeonChartItem.RewardItemIDList;
                    for(int i=0;i< TrainingRewardIDList.Count; i++)
                    {
                        double rewardCount = Define.Util.GetExpressionValue(Define.ExpressionType.GetTrainingReward, StaticManager.Backend.GameData.PlayerGameData.TDMaxClearLevelList[(StageManager.instance as DefaultStageManager).enemyManager.GetTDPlayerType().ToString()]);

                        if (FinalRewardItems.ContainsKey(TrainingRewardIDList[i].ItemID))
                        {
                            FinalRewardItems[TrainingRewardIDList[i].ItemID] += rewardCount;
                        }
                        else
                        {
                            FinalRewardItems.Add(TrainingRewardIDList[i].ItemID, rewardCount);
                        }

                    }

                    if (FinalRewardItems.Count != 0)
                    {
                        List<int> rewardItemIds = new();
                        List<double> rewardItemCounts = new();
                        foreach (int key in FinalRewardItems.Keys)
                        {
                            Debug.Log($"id : {key} -> {FinalRewardItems[key]}");
                            rewardItemIds.Add(key);
                            rewardItemCounts.Add(FinalRewardItems[key]);
                        }

                        if (rewardItemIds.Count != 0)
                            RewardManager.instance.ShowRewardWindow(rewardItemIds, rewardItemCounts, true);    
                    }
                }
            }

            InitStage(moveStageLevel);
            (StageManager.instance as DefaultStageManager).enemyManager.isStageStart = true;
            // StageData stageData = (StageManager.instance as DefaultStageManager).enviornment.SetStage(moveStageLevel);
            // (StageManager.instance as DefaultStageManager).enemyManager.Init(stageData);
            // PlayersControlManager.instance.AllPlayerOn();
            // (StageManager.instance as DefaultStageManager).LoadDefault();
            /*던전 인게임창 끄기 */
            StageManager.instance.canvasManager.GetUIManager<UI_DungeonIngame>().SetActiveExitObj(false);

            /* 창 다 켜기*/
            StageManager.instance.canvasManager.GetUIManager<UIManager_Common>().OpenMainMenu();
            StageManager.instance.canvasManager.GetUIManager<UIManager_Stage>().playerUI.gameObject.SetActive(true);

            StageManager.instance.canvasManager.GetUIManager<UI_DungeonPopup>().RefreshTrainingUI();
            StageManager.instance.canvasManager.GetUIManager<UI_DungeonPopup>().GetComponent<OpenClose>().SetOpen();



            // 아이템 드랍(Canvas (ItemDrop)_temp) 이것도 켜기 
            StageManager.instance.canvasManager.GetUIManager<UI_ItemDropAchivement>().SetActiveItemDrop(true);
            StageManager.instance.canvasManager.GetUIManager<UIManager_ContentsLoading>().SetActiveContentsLoading(true);

            PlayersControlManager.instance.nowPlayMode = PlayersControlMode.Normal;


        });
    }

    public void LoadMoveStageScene(long selectStageLevel)
    {
        long moveStageLevel = selectStageLevel;
        NowMoveStageNum = moveStageLevel;
        (StageManager.instance as DefaultStageManager).enemyManager.isStageStart = false;


        StaticManager.UI.FadeUI.FadeStart(FadeUI.FadeType.ChangeToBlack, () => {

            StaticManager.Backend.GameData.PlayerGameData.UpdateNowStageLevel(moveStageLevel);
            InitStage(moveStageLevel);
            (StageManager.instance as DefaultStageManager).enemyManager.isStageStart = true;

            // StageData stageData = (StageManager.instance as DefaultStageManager).enviornment.SetStage(moveStageLevel);
            // (StageManager.instance as DefaultStageManager).enemyManager.Init(stageData);
            // PlayersControlManager.instance.AllPlayerOn();
            // (StageManager.instance as DefaultStageManager).LoadDefault();

        });



        //BossStage
     //  if (moveStageLevel % 10 == 0)
     //  {
     //      Load(FINAL_BOSS_STAGE_SCENE, isFade: true, LODING_SCENE_PROGRESS_ANYWHERE_TO_GAME,
     //            () =>
     //            {
     //                (StageManager.instance as DefaultStageManager).LoadDefault();
     //                StaticManager.Backend.GameData.PlayerGameData.UpdateNowStageLevel(moveStageLevel);
     //                Debug.Log($"현재 스테이지 {moveStageLevel}");
     //
     //                //(StageManager.instance as DefaultStageManager).LoadIntro();
     //            });
     //  }
     //  else if (moveStageLevel % 5 == 0)
     //  {
     //      Load(MIDDLE_BOSS_STAGE_SCENE, isFade: true, LODING_SCENE_PROGRESS_ANYWHERE_TO_GAME,
     //        () =>
     //        {
     //            (StageManager.instance as DefaultStageManager).LoadDefault();
     //            StaticManager.Backend.GameData.PlayerGameData.UpdateNowStageLevel(moveStageLevel);
     //            Debug.Log($"현재 스테이지 {moveStageLevel}");
     //            //(StageManager.instance as DefaultStageManager).LoadIntro();
     //        });
     //  }
     //  else
     //  {
     //      Load(NORMAL_STAGE_SCENE, isFade: true, LODING_SCENE_PROGRESS_ANYWHERE_TO_GAME,
     //         () =>
     //         {
     //             (StageManager.instance as DefaultStageManager).LoadDefault();
     //             StaticManager.Backend.GameData.PlayerGameData.UpdateNowStageLevel(moveStageLevel);
     //             Debug.Log($"현재 스테이지 {moveStageLevel}");
     //             //(StageManager.instance as DefaultStageManager).LoadIntro();
     //         });
     //  }
    }
    public bool haveReachedTheBossStage = false;
    public void LoadBossStageScene()
    {
        Load(BOSS_STAGE_SCENE, isFade: true, LODING_SCENE_PROGRESS_ANYWHERE_TO_GAME);
        haveReachedTheBossStage = true;
    }

    private void Load(string scene, bool isFade, string loadingSceneName, Action OnComplete = null)
    {
        MoveSceneManager.instance.MoveSceneAsync(scene, isFade, loadingSceneName, OnCompleteLoadScene: (data) => OnComplete?.Invoke());
    }

    //private string GetDefaultStageType()
    //{
    //    StageData stageData = BackEndServerManager.instance.GetSavedResourceData<StageData>(SceneSettingManager.instance.GetCurrentStage());
    //    string sceneName = string.Empty;
    //    switch (stageData.stageType)
    //    {
    //        case StageType.InSide:
    //            sceneName = DEFAULT_INSIDE_STAGE_SCENE;
    //            break;
    //        case StageType.OutSide:
    //            sceneName = DEFAULT_OUTSIDE_STAGE_SCENE;
    //            break;
    //    }

    //    return sceneName;
    //}
    private string GetDefaultStageTypeNum()
    {
        StageData stageData = BackEndServerManager.instance.GetSavedResourceData<StageData>(SceneSettingManager.instance.GetCurrentStage());
        string sceneName = string.Empty;

        switch (stageData.stageTypeNum)
        {
            case StageTypeNum.Normal:
                sceneName = DEFAULT_OUTSIDE_STAGE_SCENE;
                break;
            case StageTypeNum.Middle:
                sceneName = DEFAULT_INSIDE_STAGE_SCENE;  
                break;
            case StageTypeNum.Final:
                sceneName = DEFAULT_INSIDE_STAGE_SCENE;  
                break;
        }

        return sceneName;
    }

    private string GetBossStageTypeNum(long stageLevel)
    {
        string bossStageName = GetBossSectorName(stageLevel);

        StageData stageData = BackEndServerManager.instance.GetSavedResourceData<StageData>(bossStageName);
        string sceneName = string.Empty;

        switch (stageData.stageTypeNum)
        {
            case StageTypeNum.Normal:
                sceneName = DEFAULT_OUTSIDE_STAGE_SCENE;
                break;
            case StageTypeNum.Middle:
                sceneName = DEFAULT_INSIDE_STAGE_SCENE;
                break;
            case StageTypeNum.Final:
                sceneName = DEFAULT_INSIDE_STAGE_SCENE;
                break;
        }
        return sceneName;
    }

    public bool IsDefaultStageTypeInside()
    {
        return SceneManager.GetActiveScene().name == DEFAULT_INSIDE_STAGE_SCENE;
    }

    public long GetSectorNum()
    {
        long sectorNum = 1;

        if (NowMoveStageNum % 10 == 0)
        {
            sectorNum = NowMoveStageNum / 10;
        }
        else
        {
            sectorNum = NowMoveStageNum / 10 + 1;
        }

        return sectorNum;
    }

    public long GetNowStageSectorNum()
    {
        long sectorNum = GetSectorNum();

        if (sectorNum % NormalSectorType.Count == 0)
            return NormalSectorType.Count;
        else
            return sectorNum % NormalSectorType.Count;
    }
    public long GetNowMiddleSectorNum()
    {
        long sectorNum = GetSectorNum();

        if (sectorNum % MiddleSectorType.Count == 0)
            return MiddleSectorType.Count;
        else
            return sectorNum % MiddleSectorType.Count;
    }
    public long GetNowBossSectorNum()
    {
        long sectorNum = GetSectorNum();

        if (sectorNum % BossSectorType.Count == 0)
            return BossSectorType.Count;
        else
            return sectorNum % BossSectorType.Count;
        
    }
}