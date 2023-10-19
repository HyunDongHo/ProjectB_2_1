using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scheduler;
using DG.Tweening;

public class NextStageManager : MonoBehaviour
{
    private void Awake()
    {
        AddEvent();
    }

    private void OnDestroy()
    {
        RemoveEvent();
    }

    private void AddEvent()
    {
        (StageManager.instance as GamePlayManager).OnGamePlayStageEnd += HandleOnStageEnd;
    }

    private void RemoveEvent()
    {
        (StageManager.instance as GamePlayManager).OnGamePlayStageEnd -= HandleOnStageEnd;
    }

    private void HandleOnStageEnd()
    {
        //SceneSettingManager.instance.SetNext();

        PlayerControl_DefaultStage control = (StageManager.instance.playerControl as PlayerControl_DefaultStage);
        control.EndGamePlay();
        if((StaticManager.Backend.GameData.PlayerGameData.NowStageLevel - StaticManager.Backend.GameData.PlayerGameData.ClearStageLevel) == 1) 
            StaticManager.Backend.GameData.PlayerGameData.UpdateStageClearLevel();
        Debug.Log("스테이지 클리어");
        StageManager.instance.canvasManager.GetUIManager<UI_PassPopup>().SetNowPassText(BackendData.Chart.PassInfo.PassType.Stage);
        StageManager.instance.canvasManager.GetUIManager<UI_PassPopup>().SetPassItemLockState(BackendData.Chart.PassInfo.PassType.Stage);    
        Timer.instance.TimerStart(new TimerBuffer(1.5f), 
            OnFrame: () =>
            {
                control.UpdateStageEndBefore();
            },
            OnComplete: () =>
            {
                control.StartStageEndMotion(motionSpeed: 0.5f, endMotionFrame: 45,  OnEnd: () =>
                {
                    PlayersControlManager.instance.ResetAllPlayerState();
                    PlayersControlManager.instance.ResetHpAllPlayer();                   

                    SceneSettingManager.instance.LoadNextStageScene();

                }
                );

            });

    }
}
