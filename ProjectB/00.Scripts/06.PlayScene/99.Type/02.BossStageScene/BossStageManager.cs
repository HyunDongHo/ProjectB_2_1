using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Scheduler;
using UnityEngine.Video;

public class BossStageManager : GamePlayManager
{
   //protected override void Awake()
   //{
   //    base.Awake();
   //
   //    StageData stageData = BackEndServerManager.instance.GetSavedResourceData<StageData>(SceneSettingManager.instance.GetCurrentBossStage(), isCopy: true);
   //    Init(stageData);
   //
   //    enviornment.Init(Instantiate(stageData));
   //    enemyManager.Init(Instantiate(stageData));
   //
   //}
   //
   //protected override void AddEvent()
   //{
   //    base.AddEvent();
   //
   //    enemyManager.OnAnnounceEnemySpawn += HandleOnAnnounceEnemySpawn;
   //    enemyManager.OnAnnounceEnemyDie += HandleOnAnnounceEnemyDie;
   //}
   //
   //protected override void OnDestroy()
   //{
   //    base.OnDestroy();
   //
   //    enemyManager.OnAnnounceEnemySpawn -= HandleOnAnnounceEnemySpawn;
   //    enemyManager.OnAnnounceEnemyDie -= HandleOnAnnounceEnemyDie;
   //}
   //
   //private void HandleOnAnnounceEnemySpawn(EnemyControl enemyControl)
   //{
   //    switch (enemyControl)
   //    {
   //        case BossEnemyControl bossEnemyControl:
   //            (canvasManager as CanvasManager_Boss).SetBossHp(enemyControl.GetStats<Stats>());
   //            PlayIntro(bossEnemyControl);  
   //            break;
   //    }
   //}
   //
   //private void HandleOnAnnounceEnemyDie(EnemyControl enemyControl)
   //{
   //    switch (enemyControl)
   //    {
   //        case BossEnemyControl bossEnemyControl:
   //            BossDied(bossEnemyControl);
   //            break;
   //    }
   //}
   //
   //public void PlayIntro(BossEnemyControl bossEnemyControl)
   //{
   //    Action EndIntro = () => EndBossIntro(bossEnemyControl);
   //
   //    PlayerControl_BossStage bossPlayerControl = (playerControl as PlayerControl_BossStage);
   //
   //    CanvasManager_Boss bossCanvasManager = (canvasManager as CanvasManager_Boss);
   //    bossCanvasManager.AddSkipEvent(EndIntro);
   //
   //    Timer.instance.TimerStart(new TimerBuffer(Time.deltaTime),
   //        OnFrame: () =>
   //        {
   //            bossEnemyControl.StartIntro(OnCompleteBossIntroStart: EndIntro);
   //            bossPlayerControl.StartIntro();
   //        });
   //
   //    bossEnemyControl.isEnabledControl = false;
   //    bossPlayerControl.isEnabledControl = false;
   //}
   //
   //public void EndBossIntro(BossEnemyControl bossEnemyControl)
   //{
   //    PlayerControl_BossStage bossPlayerControl = (playerControl as PlayerControl_BossStage);
   //
   //    CanvasManager_Boss bossCanvasManager = (canvasManager as CanvasManager_Boss);
   //    bossCanvasManager.RemoveAllSkipEvent();
   //
   //    bossPlayerControl.EndIntro();
   //    bossEnemyControl.EndIntro();
   //
   //    bossCanvasManager.EndIntroSetUI(
   //        () =>
   //        {
   //            bossCanvasManager.ShowWarning(
   //                OnComplete:() =>
   //                {
   //                    bossEnemyControl.isEnabledControl = true;
   //                    bossPlayerControl.isEnabledControl = true;
   //                });
   //        });
   //}
   //
   //private void BossDied(BossEnemyControl bossEnemyControl)
   //{
   //    CanvasManager_Boss bossCanvasManager = (canvasManager as CanvasManager_Boss);
   //    PlayerControl_BossStage bossPlayerControl = (playerControl as PlayerControl_BossStage);
   //
   //    bossEnemyControl.isEnabledControl = false;
   //    bossPlayerControl.isEnabledControl = false;
   //
   //    bossCanvasManager.EndBoss(
   //        () =>
   //        {
   //            bossEnemyControl.EndBoss();
   //            bossPlayerControl.EndPlayer(OnComplete: () => GetStageReward(bossEnemyControl));
   //        });
   //}
   //
   //private void GetStageReward(BossEnemyControl bossEnemyControl)
   //{
   //    PopupBossClear popup = PopupManager.instance.CreatePopup<PopupBossClear>("PopupBossClear").GetPopup();
   //
   //    List<RewardDataSetting> rewardDataSettings = bossEnemyControl.enemyReward.rewardDataSettings;
   //    foreach (var rewardDataSetting in rewardDataSettings)
   //    {
   //        RewardData rewardData = rewardDataSetting.GetRewardData();
   //
   //        int amount = rewardData.GetRewardAmount();
   //        Sprite sprite = rewardData.GetRewardSprite();
   //
   //        rewardData.ReceiveReward(FindObjectOfType<PlayerControl_GamePlay>());
   //        popup.rewardUI.SetReward(sprite, amount);
   //    }
   //
   //    popup.closeButton.onClick.AddListener(() =>
   //    {
   //        SceneSettingManager.instance.LoadLobbyStageScene();
   //        popup.RemovePopup();
   //    });
   //}
}
