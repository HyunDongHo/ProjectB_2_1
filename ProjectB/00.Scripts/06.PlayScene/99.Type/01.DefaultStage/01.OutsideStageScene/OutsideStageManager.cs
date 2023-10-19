using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutsideStageManager : DefaultStageManager
{
    protected override void Awake()
    {
        base.Awake();

        StageData stageData = BackEndServerManager.instance.GetSavedResourceData<StageData>(SceneSettingManager.instance.GetCurrentStage(), isCopy: true);

        Init(stageData);

        //TODO 현재 선택중인 플레이어 
        if(stageData.stageTypeNum != StageTypeNum.Final)
        {
            PlayersControlManager.instance.SetActivePlayer(PlayerType.Wizard);
        }
        else
        {
            PlayersControlManager.instance.SetActivePlayer(PlayerType.Warrior, PlayersControlMode.Boss);        
        }  

        playerControl = PlayersControlManager.instance.GetNowActivePlayer();      
        playerControl.gameObject.transform.position = new Vector3(30, 0, 30);

        //if(stageData.stageTypeNum != StageTypeNum.Final)       
        //    playersControl.SetActivePlayer(PlayerType.Wizard);                                            
        //else
        //    playersControl.SetActivePlayer(PlayerType.Warrior, PlayersControlMode.Boss);

        //playerControl = playersControl.GetNowActivePlayer();  

        enviornment.Init(Instantiate(stageData));    
        enemyManager.Init(Instantiate(stageData));      


    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void AddEvent()
    {
        base.AddEvent();

        enemyManager.OnRemovedAllEnemy += HandleOnAllEnemyDied;
    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();

        enemyManager.OnRemovedAllEnemy -= HandleOnAllEnemyDied;
    }

    private void HandleOnAllEnemyDied()
    {
        if(!enemyManager.isStageRepeat)
        {
            enemyManager.isStageEnd = true;
            OnGamePlayStageEnd?.Invoke();
        }
    }
}
