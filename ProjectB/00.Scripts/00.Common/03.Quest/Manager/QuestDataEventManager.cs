using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestDataEventManager : Singleton<QuestDataEventManager>
{
    public void PlayerKilledEnemy(EnemyControl enemyControl)
    {
       // QuestManager.instance.AddQuestVariable("EnemyKill", 1);
       // QuestManager.instance.AddQuestVariable($"{SceneSettingManager.instance.GetCurrentStage()}_EnemyKill", 1);
       // QuestManager.instance.AddQuestVariable($"{Logic.DeleteCloneText(enemyControl.name)}_EnemyKill", 1);

       // QuestManager.instance.AddQuestVariable($"{SceneSettingManager.instance.GetCurrentStage()}_CollectItem", 1);
    }

    public void PlayerMoveScene(string sceneName)
    {
        QuestManager.instance.AddQuestVariable($"{sceneName}_LocalMove", 1);
    }
}
