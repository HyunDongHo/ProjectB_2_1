using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiddleBossStageManager : DefaultStageManager
{
    protected override void Awake()
    {
        base.Awake();

        StageData stageData = BackEndServerManager.instance.GetSavedResourceData<StageData>(SceneSettingManager.instance.MiddleSectorType[SceneSettingManager.instance.GetNowMiddleSectorNum()], isCopy: true);

        Init(stageData);

      //  playerControl = PlayersControlManager.instance.GetNowActivePlayer();
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
        if (!enemyManager.isStageRepeat)
        {
            enemyManager.isStageEnd = true;
            OnGamePlayStageEnd?.Invoke();
        }
    }
}
