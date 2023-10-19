using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerType
{
    Warrior,
    Archer,
    Wizard,
    None
}

public enum PlayersControlMode
{
    Normal,
    Boss,
    TrainingDungeon,
}

public class PlayersControlManager : Singleton<PlayersControlManager>
{
    public PlayerControl[] playersContol;    

    public PlayersControlMode nowPlayMode = PlayersControlMode.Normal;
    public PlayerType nowActive = PlayerType.None;  

    //private PlayerControl nowActivePlayer = null;
    public PlayerControl nowActivePlayer = null;

    // public void Init()
    // {
    //
    //
    //     for (int i = 0; i < playersContol.Length; ++i)
    //     {
    //         playersContol[i].utility.Init(playersContol[i]);
    //     }  
    //
    //
    // }

    public void DontDestroyControls()
    {
      //  Debug.Log("DontDestroyControls");
        for(int i=0; i < playersContol.Length; ++i)
            DontDestroyOnLoad(playersContol[i]);  
    }

    public void SetPlayerFromServer()
    {
        // switch(StaticManager.Backend.GameData.PlayerGameData.PlayerType)
        // {
        //     case PlayerType.Warrior:
        //
        // }

        UserDataManager.instance.InitDataEveryLoad();
        InitPlayerGameData();
        nowActivePlayer = playersContol[(int)StaticManager.Backend.GameData.PlayerGameData.PlayerType];
        SetActivePlayer(StaticManager.Backend.GameData.PlayerGameData.PlayerType);

       //for (int i = 0; i < playersContol.Length; ++i)
       //{
       //    playersContol[i].gameObject.SetActive(true);
       //}
    }

    // 캐릭터 사망 시 남은 캐릭터로 카메라 전환
    public void ChangePlayerForDie()
    {
        for(int i=0; i < playersContol.Length; ++i)
        {
            if(playersContol[i].GetStats<Stats>().hp.isAlive == true)
            {
             //   nowActivePlayer = playersContol[i];
           //     SetActivePlayer((PlayerType)i);
                StaticManager.Backend.GameData.PlayerGameData.ChangeNowPlayerType((PlayerType)i);
            }
        }
    }

    private void InitPlayerGameData()
    {
        for (int i = 0; i < (int)PlayerType.None; ++i)
        {
            PlayerStats playerStats = playersContol[i].GetStats<PlayerStats>();

            if (playerStats == null)
                continue;

            playerStats.hp.SetHpToMax(i);
            switch ((PlayerType)i)
            {
                case PlayerType.Warrior:
                    playerStats.level.InitLevel(StaticManager.Backend.GameData.PlayerGameData.DWarriorLevel);
                    playerStats.exp.InitExp(StaticManager.Backend.GameData.PlayerGameData.DWarriorExp);
                    playerStats.UpdateLevelData(StaticManager.Backend.GameData.PlayerGameData.DWarriorLevel);
                    break;
                case PlayerType.Archer:
                    playerStats.level.InitLevel(StaticManager.Backend.GameData.PlayerGameData.DArcherLevel);
                    //playerStats.hp.InitHp(StaticManager.Backend.GameData.PlayerGameData.DArcherHp);
                    playerStats.exp.InitExp(StaticManager.Backend.GameData.PlayerGameData.DArcherExp);
                    playerStats.UpdateLevelData(StaticManager.Backend.GameData.PlayerGameData.DArcherLevel);
                    break;
                case PlayerType.Wizard:
                    playerStats.level.InitLevel(StaticManager.Backend.GameData.PlayerGameData.DWizardLevel);
                    //playerStats.hp.InitHp(StaticManager.Backend.GameData.PlayerGameData.DWizardHp);
                    playerStats.exp.InitExp(StaticManager.Backend.GameData.PlayerGameData.DWizardExp);
                    playerStats.UpdateLevelData(StaticManager.Backend.GameData.PlayerGameData.DWizardLevel);
                    break;
            }
        }
    }
    public void SetActivePlayer(PlayerType switchPlayerType, PlayersControlMode playMode = PlayersControlMode.Normal)  
    {
        //임시
        //if (switchPlayerType == PlayerType.Wizard)
        //    return;

        // 교체하려는 플레이어가 다른 클래스일때
          
        if (playMode == PlayersControlMode.Normal)
        { 
            if (nowActivePlayer != null && nowActive != switchPlayerType)
            {
                if (playersContol[(int)switchPlayerType] != null)
                {
                    Vector3 movePosition = nowActivePlayer.transform.position;

                    nowActivePlayer.gameObject.SetActive(false);
                    nowActivePlayer = playersContol[(int)switchPlayerType];  
                    nowActivePlayer.gameObject.SetActive(true);
                    nowActive = switchPlayerType;
                //    nowActivePlayer.gameObject.transform.position = movePosition;
                }
            }
            else
            {
                if (playersContol[(int)switchPlayerType] != null)
                {
                    nowActivePlayer = playersContol[(int)switchPlayerType];
                    nowActivePlayer.gameObject.SetActive(true);
                    nowActive = switchPlayerType;
                }
            }
        }
        else if(playMode == PlayersControlMode.Boss)
        {
            for (int i = 0; i < playersContol.Length; ++i)
            {
                playersContol[i].gameObject.SetActive(true);
            }

            if (nowActivePlayer != null && nowActive != switchPlayerType)
            {
                if (playersContol[(int)switchPlayerType] != null)
                {
                    nowActivePlayer = playersContol[(int)switchPlayerType];  
                    nowActive = switchPlayerType;
                }
            }
            else
            {
                if (playersContol[(int)switchPlayerType] != null)
                {
                    nowActivePlayer = playersContol[(int)switchPlayerType];
                    nowActive = switchPlayerType;
                }
            }
        }else if(playMode == PlayersControlMode.TrainingDungeon)
        {
            nowPlayMode = playMode;
            if (playersContol[(int)switchPlayerType] != null)
            {
                nowActivePlayer = playersContol[(int)switchPlayerType];
                //nowActivePlayer.gameObject.transform.localPosition = new Vector3(5,0,-10);
                nowActive = switchPlayerType;
            }
        }

        if(nowPlayMode != PlayersControlMode.TrainingDungeon)
        {
            for (int i = 0; i < playersContol.Length; ++i)
            {
                playersContol[i].gameObject.SetActive(true);
            }
        }


        if (StageManager.instance != null)
            StageManager.instance.cameraManager.SetNowPlayer(nowActivePlayer);
    }
    public void SetNowActivePlayerPos(StageData stageData) // 던전에서 플레이어 위치 세팅
    {
        GameObject trainingDungeon = stageData.environmentPrefab;
        if (trainingDungeon == null)
            return;

        TrainingDungeon td = trainingDungeon.GetComponent<TrainingDungeon>();
        //nowActivePlayer.gameObject.transform.localPosition = new Vector3(5, 0, -10);
        if (td.GetPlayerPosition() == null)
            return;
        Transform playerPos = td.GetPlayerPosition();
        //nowActivePlayer.gameObject.transform.localPosition = td.GetPlayerPosition().position;    
        nowActivePlayer.gameObject.transform.position = td.GetPlayerPosition().position;    
    }
    public PlayerControl GetNowActivePlayer()
    {
        if (playersContol[(int)nowActive] != null)
            return playersContol[(int)nowActive];

        return null;
    }

    public PlayerControl GetNearActivePlayer(Vector3 position)
    {
        float minLen = float.MaxValue;
        PlayerControl returnValue = null;
        for(int i=0; i < playersContol.Length; ++i)
        {
            if (playersContol[i].isAvailableControl == false
                || playersContol[i].isEnabledControl == false
                || playersContol[i].GetStats<Stats>().hp.isAlive == false)
                continue;

            Vector3 dif = position - playersContol[i].transform.position;

            if(dif.magnitude < minLen)
            {
                minLen = dif.magnitude;
                returnValue = playersContol[i];
            }
        }

        return returnValue;
    }


    public void SetNotActiveAllPlayer() // 로비씬 끝나고 
    {
        for(int i = 0; i < (int)PlayerType.None; i++)
        {
            playersContol[i].gameObject.SetActive(false);  
        }
    }
    public void ResetRotationAllPlayer() // lobby 로 돌아갈 때나 로비 끝날때 
    {
        for(int i = 0; i < (int)PlayerType.None; i++)
        {
            playersContol[i].gameObject.transform.eulerAngles = Vector3.zero;
            playersContol[i].utility.modelPivot.localRotation = Quaternion.identity;
            playersContol[i].GetModel<PlayerModel>().gameObject.transform.eulerAngles = Vector3.zero;
        }
    }

    public void AllPlayerOn()
    {
        for (int i = 0; i < (int)PlayerType.None; i++)
        {
            playersContol[i].gameObject.SetActive(true);            
        }
    }
    public void SelectPlayerOn(int playerNum)
    {
        for (int i = 0; i < (int)PlayerType.None; i++)
        {
            playersContol[i].gameObject.SetActive(false);
        }
        playersContol[playerNum].gameObject.SetActive(true);
    }

    public void ResetHpAllPlayer()
    {
        for(int i = 0; i < (int)PlayerType.None; i++)
        {
            PlayerStats stats = playersContol[i].GetStats<PlayerStats>();    
            stats.hp.SetHpToMax(i);
            stats.sp.SetSpToMax();
        }
    }
    public void ResetAllPlayerState() 
    {
        for (int i = 0; i < (int)PlayerType.None; i++)
        {
            (playersContol[i] as PlayerControl_GamePlay).pRaycast.targetCollider = null;

            playersContol[i].GetModel<PlayerModel>().animationControl.ResetAnimationState();
            playersContol[i].GetAttack<Attack>().RefreshAttackWaitBuffer();
            playersContol[i].GetAttack<PlayerAttack>().CompleteAttackWait();
            for (int j = 0; j < playersContol[i].utility.skillUseCheck.playerSkillSettings.Length; j++)
            {
                playersContol[i].utility.skillUseCheck.playerSkillSettings[j].skillBuffer.timer = playersContol[i].utility.skillUseCheck.playerSkillSettings[j].skillBuffer.time;
            }

            playersContol[i].GetAttack<Attack>().isEnableAttack = true;
            playersContol[i].GetAttack<PlayerAttack>().isUseSkill = false;
            playersContol[i].GetAttack<PlayerAttack>().isUseCombo = false;
            playersContol[i].GetAttack<PlayerAttack>().isUseDashCombo = false;
            playersContol[i].GetMove<PlayerMove>().isAvailableMove = true;
            (playersContol[i] as PlayerControl_GamePlay).ResetGamePlay();    
            playersContol[i].ResetState(true);
            playersContol[i].gameObject.SetActive(false);
            switch (i)
            {
                case (int)PlayerType.Warrior:
                    playersContol[i].GetStats<PlayerStats>().exp.InitExp(StaticManager.Backend.GameData.PlayerGameData.DWarriorExp);
                    break;
                case (int)PlayerType.Archer:
                    playersContol[i].GetStats<PlayerStats>().exp.InitExp(StaticManager.Backend.GameData.PlayerGameData.DArcherExp);
                    break;
                case (int)PlayerType.Wizard:
                    playersContol[i].GetStats<PlayerStats>().exp.InitExp(StaticManager.Backend.GameData.PlayerGameData.DWizardExp);
                    break;
            }
        }
        Debug.Log("Done reset");

    }
    public void SetPlayersExp(int playerModelType, EnemyControl enemyControl, int activeExpRatio, int notActiveExpRatio)
    {
        StatsManager enemyStatsManager = enemyControl.GetStats<Stats>().manager;

        //float defaultExpValue = enemyStatsManager.GetValue(EnemyStat
        float defaultExpValue = (float)Define.Util.GetExpressionValue(Define.ExpressionType.GetExp, StaticManager.Backend.GameData.PlayerGameData.NowStageLevel);

        float finalExpValue = defaultExpValue;

        finalExpValue *= (1 + (float)StaticManager.Backend.GameData.PlayerGameData.GetStatRatio(Define.StatType.ExpGetRatio) 
                            + (float)StaticManager.Backend.GameData.PlayerDice.GetDiceRatio(Define.StatType.ExpGetRatio)
                            + (float)StaticManager.Backend.GameData.PlayerAdsBuff.GetAdsBuffRatio(BackendData.Chart.AdsBuff.AdsType.Exp));

        //Debug.Log($"finalExpValue : {finalExpValue}");  

        //if (playerStats == null)
        //    return;
        //playerStats.exp.AddExp(playerModelType, finalExpValue);

        for (int i = 0; i < (int)PlayerType.None; i++)   
        {
            // 활성화 되어 있는 애들은 다 activeExpRatio로 활성화 되지 않은 애들은 notActiveExpRatio만큼 exp 얻음 
            if (playersContol[i].gameObject.activeSelf == true)
            {
                playersContol[i].GetStats<PlayerStats>().exp.AddExp(i, finalExpValue);// * activeExpRatio/100);       
            }
            else
            {
                //     playersContol[i].GetStats<PlayerStats>().exp.AddExp(expValue * notActiveExpRatio / 100);    
            }
        }
    }

    public void RefreshPlayerHp()
    {
        for (int i = 0; i < (int)PlayerType.None; i++)
        {
            PlayerStats stats = playersContol[i].GetStats<PlayerStats>();
            stats.hp.SetHpMax(i);  
        }
    }

}
