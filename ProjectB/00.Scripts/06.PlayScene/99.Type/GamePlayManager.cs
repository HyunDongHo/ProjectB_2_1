using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayManager : StageManager
{
    public StageData stageData { get; private set; }

    public Action OnGamePlayStageEnd;

    public EnemyManager enemyManager;
    public Environment enviornment;
    //public PlayersControl playersControl;    
    //public PlayersControlManager playersControl;
    protected void Init(StageData stageData)
    {
        this.stageData = stageData;

       // SoundManager.instance.PlaySound(stageData.soundData);
    //    QuestDataEventManager.instance.PlayerMoveScene(stageData.name);

       // playersControl?.Init();
    }

    protected override void Start()
    {
        base.Start();

        //canvasManager.GetUIManager<UIManager_Game>().consumableUI.SetConsumableItemData(ResourceManager.instance.Load<ConsumableItemData>("Hp_Potion"));
    }
}
