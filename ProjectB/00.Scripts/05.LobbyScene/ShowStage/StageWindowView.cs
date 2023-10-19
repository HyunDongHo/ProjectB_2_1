using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageWindowView : MonoBehaviour
{
    public GameObject showStageParent;
    public TextMeshProUGUI stageText;
    public TextMeshProUGUI monsterCntText;
    public TextMeshProUGUI chapterNumText;

    public void OpenCloseStageList(bool isOpen, bool isAnimation)
    {
        showStageParent.SetActive(isOpen);
        if(isOpen == true) // 딱 켜지면 nowStage에 따른 챕터랑 스테이지들 보여지기 
        {
            UIManager_Stage stage = transform.GetComponent<UIManager_Stage>();
            stage.RefreshStageUI();

        }

    }

    public void UpdateStageText()
    {
        //stageText.text = $"{sector + 1} - {sectorIndex + 1}F";

        long nowStage = StaticManager.Backend.GameData.PlayerGameData.NowStageLevel;
        long nowSector = 1;
        long nowChapter = 1;
        long nowSectorIndex = nowStage % 10 != 0 ? nowStage % 10 : 10;

        if (nowStage % 10 == 0)
        {
            nowSector = nowStage / 10;
        }
        else
        {
            nowSector = nowStage / 10 + 1;
        }
        if (nowStage % 50 == 0)
        {
            nowChapter = nowStage / 50;
        }
        else
        {
            nowChapter = nowStage / 50 + 1;
        }

        stageText.text = $"스테이지 {nowSector} - {nowSectorIndex}";
        if(transform.GetComponent<OpenClose>().isOpen == false)
            chapterNumText.text = $"Chapter {nowChapter}";  
    }

    public void UpdateMonsterCntText(long presentCnt, long totCnt, bool isBossSpawn)
    {
        if(isBossSpawn == false)
        {
            monsterCntText.text = $"{presentCnt} / {totCnt}";
        }
        else
        {
            monsterCntText.text = $"";

        }
    }
}
