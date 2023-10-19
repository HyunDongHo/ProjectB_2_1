using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class StageWindow : MonoBehaviour, IOpenClose
{
    public StageWindowView stageWindowView;      
    [SerializeField]
    StageList stageList;
    [SerializeField]
    GameObject monsterCountObj;
    [SerializeField]
    GameObject stageMenuObj;

    int stageTotalCnt = 0;
    long nowStage;
    long nowSectorIndex;
    private void Start()
    {
        stageWindowView.UpdateStageText();
        if (StageManager.instance as LobbySceneManager)  
            return;  

        stageTotalCnt = (StageManager.instance as GamePlayManager).enemyManager.chartTotalEnemyCnt;
        stageWindowView.UpdateMonsterCntText((StageManager.instance as GamePlayManager).enemyManager.chartTotalEnemyCnt, stageTotalCnt, false);

        nowStage = StaticManager.Backend.GameData.PlayerGameData.NowStageLevel;
        nowSectorIndex = nowStage % 10 != 0 ? nowStage % 10 : 10;  

    }

    private void Update()
    {
        if (StageManager.instance as LobbySceneManager)
            return;

        if (PlayersControlManager.instance.nowPlayMode == PlayersControlMode.TrainingDungeon)
        {
            monsterCountObj.SetActive(false);
            return;    
        }
            

        if(nowSectorIndex == 5 || nowSectorIndex == 10)
        {
            if (stageTotalCnt > (StageManager.instance as GamePlayManager).enemyManager.dieMonsterCount)
            {
                stageWindowView.UpdateMonsterCntText(stageTotalCnt - (StageManager.instance as GamePlayManager).enemyManager.dieMonsterCount, stageTotalCnt, false);
            }
            else
            {
                monsterCountObj.SetActive(false);  
                //stageWindowView.UpdateMonsterCntText(0, 0, true);  
            }

        }
        else
        {
            monsterCountObj.SetActive(true);
            stageWindowView.UpdateMonsterCntText(stageTotalCnt - (StageManager.instance as GamePlayManager).enemyManager.dieMonsterCount, stageTotalCnt, false);    
        }
    }
    public void Open(bool isAnimation)
    {
      
        stageWindowView.OpenCloseStageList(isOpen: true, isAnimation);

        stageList.ResetStageList();
    }

    public void Close(bool isAnimation)
    {
        stageWindowView.OpenCloseStageList(isOpen: false, isAnimation);
    }
    public void SetMonsterCountActive(bool flag)
    {
        monsterCountObj.SetActive(flag);
    }
    public void SetStageMenuActive(bool flag)
    {
        stageMenuObj.SetActive(flag);
    }

    public void SetStageUI()
    {
        monsterCountObj.SetActive(true);
        stageTotalCnt = (StageManager.instance as GamePlayManager).enemyManager.chartTotalEnemyCnt;
        stageWindowView.UpdateStageText();

        UIManager_Stage stage = transform.GetComponent<UIManager_Stage>();
        if (stage.firstInitDone == true)
        {
            // nowStageLevel의 chapter 번호 알기 
            long chapterNum = stage.GetChapterLevel(StaticManager.Backend.GameData.PlayerGameData.NowStageLevel);
            if (chapterNum <= 0)
                return;
            if (stage.GetChapterNumText() == null)
                return;
            long showChapterNum = long.Parse(Regex.Replace(stage.GetChapterNumText().text, @"\D", ""));
            if (showChapterNum <= 0)
                return;
            if (showChapterNum == chapterNum ) // 현재 보고있는 챕터 번호가 nowStageLevel에 따른 챕터랑 같을 때 
            {
                stage.RefreshStageUI();  
            }
            else // 다른 챕터 보고 있을 때 
            {
                if (StaticManager.Backend.GameData.PlayerGameData.NowStageLevel % UIManager_Stage.STAGEVIEW_NUM == 0)
                {
                    StageManager.instance.canvasManager.GetUIManager<UIManager_Stage>().GetComponent<UIManager_Stage>().SelectStageNum = UIManager_Stage.STAGEVIEW_NUM;
                }
                else
                {
                    StageManager.instance.canvasManager.GetUIManager<UIManager_Stage>().GetComponent<UIManager_Stage>().SelectStageNum = StaticManager.Backend.GameData.PlayerGameData.NowStageLevel % UIManager_Stage.STAGEVIEW_NUM;

                }
                StageManager.instance.canvasManager.GetUIManager<UIManager_Stage>().GetComponent<UIManager_Stage>().SelectStageLevel = StaticManager.Backend.GameData.PlayerGameData.NowStageLevel;

                if (stage.GetStageNumContent() == null || stage.GetStageNumContent().transform.childCount != UIManager_Stage.STAGEVIEW_NUM)
                    return;
                for (int i = 0; i < UIManager_Stage.STAGEVIEW_NUM; i++)
                {
                    GameObject item = stage.GetStageNumContent().transform.GetChild(i).gameObject;    
                    if (item == null)
                        return;

                    UI_StageViewItem itemComponent = item.transform.GetComponent<UI_StageViewItem>();
                    if (itemComponent == null)
                        continue;

                    itemComponent.SetChapterNum(showChapterNum);
                    itemComponent.SetStageLock();
                    itemComponent.SetNowStageSprite();
                }
            }


        }


    }
}
