using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scheduler;

public class InsideStageManager : DefaultStageManager
{
    protected override void Awake()
    {
        base.Awake();

        StageData stageData = BackEndServerManager.instance.GetSavedResourceData<StageData>(SceneSettingManager.instance.GetCurrentStage(), isCopy: true);

        Init(stageData);

        enviornment.Init(Instantiate(stageData));
        SetCreateEnemyPosition(stageData);

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

    private void SetCreateEnemyPosition(StageData stageData)
    {
        EnemyPositionCreate_Custom enemyPositionCreate = enemyManager.enemyPositionCreate as EnemyPositionCreate_Custom;

        enemyPositionCreate.createTransformPositions = FindObjectOfType<InsideEnvironment>().GetEnemyCreateTransforms();
    }
}
