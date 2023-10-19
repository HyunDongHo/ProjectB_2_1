using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Scheduler;

public class StageLock
{
    public int sector;
    public int sectorIndex;

    public bool isOpened;

    public StageLock(int sector, int sectorIndex, bool isLocked)
    {
        this.sector = sector;
        this.sectorIndex = sectorIndex;

        this.isOpened = isLocked;
    }
}

public class StageList : MonoBehaviour
{
    public Button stageMoveButton;

    [Space]

    public StageSector stageSector;
    public StageSectorIndex[] stageSectorIndexes;

    private int sector = 0;
    private int sectorIndex = 0;

    public void Init(List<StageLock> stageLocks)
    {
        foreach (var stageLock in stageLocks)
            stageSectorIndexes[stageLock.sector].SetSectorIndexLock(stageLock.sectorIndex, stageLock.isOpened);

        stageSector.buttonToggle.firstClickButtonType = SceneSettingManager.instance.sector;
        stageSectorIndexes[SceneSettingManager.instance.sector].buttonToggle.firstClickButtonType = SceneSettingManager.instance.sectorIndex;  
    }

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
        stageMoveButton.onClick.AddListener(() => HandleOnStageMove());

        stageSector.OnSectorClicked += HandleOnSectorClicked;
        for (int i = 0; i < stageSectorIndexes.Length; i++)
            stageSectorIndexes[i].OnSectorIndexClicked += HandleOnSectorIndexClicked;
    }

    private void RemoveEvent()
    {
        stageMoveButton.onClick.RemoveListener(() => HandleOnStageMove());

        stageSector.OnSectorClicked -= HandleOnSectorClicked;
        for (int i = 0; i < stageSectorIndexes.Length; i++)
            stageSectorIndexes[i].OnSectorIndexClicked -= HandleOnSectorIndexClicked;
    }

    private void HandleOnStageMove()
    {
        SceneSettingManager.instance.SetStage(sector, sectorIndex);
        //Debug.Log($"sector : {sector}");
        //Debug.Log($"sectorIndex : {sectorIndex}");

        PlayersControlManager.instance.ResetAllPlayerState();
        PlayersControlManager.instance.ResetHpAllPlayer();
        //(StageManager.instance as GamePlayManager).enemyManager.ResetAllMonsterState();
        //SceneSettingManager.instance.LoadDefaultStageScene();

    }

    private void HandleOnSectorClicked(int buttonType)  
    {
        if (sector != buttonType)
        {
            sector = buttonType;
            for (int i = 0; i < stageSectorIndexes.Length; i++)
                stageSectorIndexes[i].gameObject.SetActive(i == buttonType);

            stageSectorIndexes[sector].buttonToggle.Click(stageSectorIndexes[sector].GetIsOpened(sectorIndex: 0) ? 0 : -1);
        }
    }

    private void HandleOnSectorIndexClicked(int buttonType)
    {
        sectorIndex = buttonType;
        //Debug.Log($"sectorIndex : {buttonType}");
    }

    public void ResetStageList()
    {
        stageSector.buttonToggle.Click(SceneSettingManager.instance.sector);
        stageSectorIndexes[SceneSettingManager.instance.sector].buttonToggle.Click(SceneSettingManager.instance.sectorIndex);
    }
}
