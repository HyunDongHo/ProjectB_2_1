using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestsManager : Singleton<QuestsManager>
{
    public BackendData.Chart.Quest.QuestType questType;

    public void MissionGamePlayStart()// �α��� �Ϸ�Ǹ� �ڷ�ƾ ���� 
    {
        Debug.Log("Game Play Start");
        StartCoroutine(CoStartGamePlayTime(0));    
    }

    IEnumerator CoStartGamePlayTime(double nRemain)
    {
        //WaitForSeconds waitForSeconds = new WaitForSeconds(1f);
        WaitForSecondsRealtime waitForSeconds = new WaitForSecondsRealtime(60f);
        while (true) // 24�ð�   
        {
            yield return waitForSeconds;
           //nRemain++;
           //if (nRemain % 30 == 0)  
            {
                /* �̼� : ���� �÷��� �ð� */
                QuestsManager.instance.UpdateMissionUI(BackendData.Chart.Mission.MissionContentType.GamePlayTime, BackendData.Chart.Mission.MissionType.Daily, 1);
                QuestsManager.instance.UpdateMissionUI(BackendData.Chart.Mission.MissionContentType.GamePlayTime, BackendData.Chart.Mission.MissionType.Repeat, 1);    
            }

        }

    }

    /* Quest */
    public bool CheckNowQuestClear() // üũ�ϴ� �Լ� 
    {
        /* ������ �ִ� QuestID �� ���� _questItem�� QuestID�� �������� */
        InGameScene.UI.InGameUI_QuestItem questItem = StageManager.instance.canvasManager.GetUIManager<UIManager_Stage>().gameObject.GetComponent<InGameScene.UI.InGameUI_Quest>().GetQuestItem();
        if (questItem == null)
            return false;

        if (StaticManager.Backend.GameData.PlayerQuest.QuestID != questItem.GetQuestItemID())
            return false;

        // ID �� �����ϸ� ���� ����Ʈ Ŭ���� �ߴ��� üũ 
        BackendData.Chart.Quest.Item questChartData = null;
        StaticManager.Backend.Chart.Quest.Dictionary.TryGetValue(StaticManager.Backend.GameData.PlayerQuest.QuestID, out questChartData);
        if (questChartData == null)
            return false;

        if (questChartData.RequestCount <= StaticManager.Backend.GameData.PlayerQuest.QuestNowStep)
            return true;

        return false;  
    }
    public void UpdateQuestUI() // UI refresh
    {
        InGameScene.UI.InGameUI_Quest inGameQuest = StageManager.instance.canvasManager.GetUIManager<UIManager_Stage>().gameObject.GetComponent<InGameScene.UI.InGameUI_Quest>();
        if (inGameQuest == null)
            return;

        inGameQuest.UpdateUI();   
    }

    public void ClearQuestValue()
    {
        InGameScene.UI.InGameUI_Quest inGameQuest = StageManager.instance.canvasManager.GetUIManager<UIManager_Stage>().gameObject.GetComponent<InGameScene.UI.InGameUI_Quest>();
        if (inGameQuest == null)
            return;

        BackendData.Chart.Quest.Item _nextQuestItem = null;
        StaticManager.Backend.Chart.Quest.Dictionary.TryGetValue(StaticManager.Backend.GameData.PlayerQuest.QuestID, out _nextQuestItem);

        if(_nextQuestItem == null) // ������ ��Ʈ �� �������� 
        {
            inGameQuest.ResetQuestState();
            inGameQuest.GetQuestItem().gameObject.SetActive(false);
            return;
        }
        inGameQuest.ChangeQuestItem(_nextQuestItem);
    }

    /* Mission */
    public void UpdateMissionUI(BackendData.Chart.Mission.MissionContentType missionContentType, BackendData.Chart.Mission.MissionType missionType, double count)
    {
        if (missionContentType == BackendData.Chart.Mission.MissionContentType.None) 
        {
            if(missionType == BackendData.Chart.Mission.MissionType.DailyTotal) // Daily Total Mission
            {
                int dailyMissionDoneCount = 0;
                for (int i = 1; i < StaticManager.Backend.Chart.Mission.Dictionary.Count + 1; i++)
                {
                    BackendData.GameData.MissionData missionData = null;
                    missionData = StaticManager.Backend.GameData.PlayerQuest.MissionList.Find(item => item.MissionID == i);
                    if (missionData == null)
                        continue;
                    if (missionData.MissionIsDone == true)
                        dailyMissionDoneCount += 1;
                }

                StageManager.instance.canvasManager.GetUIManager<UI_MissionPopup>().UpdateContentUI(missionType, 0, dailyMissionDoneCount);
                return;
            }
        }

        Dictionary<int, BackendData.Chart.Mission.Item> MissionChartData = null;
        MissionChartData = StaticManager.Backend.Chart.Mission.GetChartMissionData(missionType);

        int missionId = 0;
        foreach(var data in MissionChartData)
        {
            if (data.Value.MissionContentType == missionContentType) // �̰� �ٲٱ� 
                missionId = data.Value.MissionID;  
        }
        if (missionId == 0)
            return;

        BackendData.GameData.MissionData missionServerData = null;
        missionServerData = StaticManager.Backend.GameData.PlayerQuest.MissionList.Find(item => item.MissionID == missionId);
        if (missionServerData == null)
            return;

        //Debug.Log($"server data id: {missionId} , count : {missionServerData.MissionNowStep}");  
        StaticManager.Backend.GameData.PlayerQuest.AddMission(missionId, count);  // ���� ������Ʈ    

        //List<UI_MissionItem> missionList = StageManager.instance.canvasManager.GetUIManager<UI_MissionPopup>().GetMissionList();
        UI_MissionPopup missionPopup = StageManager.instance.canvasManager.GetUIManager<UI_MissionPopup>();
        List<UI_MissionItem> missionItemList = missionPopup.GetMissionList();
        //if (missionItemList == null || missionItemList.Count == 0)
        //    return;
        UI_MissionItem missionItem = missionItemList.Find(item => item.GetMissionId() == missionId);
        //if (missionItem == null)
        //    return;

        missionItem.SetNowStep((float)missionServerData.MissionNowStep); // �̰Ŵ� uiMissionItems ������Ʈ �ϰ� 
        missionPopup.UpdateContentUI(missionType, missionId, (float)missionServerData.MissionNowStep); // 5�� UI item ������Ʈ            

    }
    //public void SetDailyMissionNowDateState() // ���� ���� ������ �޾Ҵ���,, �ȹ޾����� ��ư�� Ȱ��ȭ �ؾߵǰ� ���Ϸ� �Ѿ�ٸ� �ʱ�ȭ �ؾߵ�  
    //{
    //    Debug.Log("SetDailyMissionNowDateState");
    //    Dictionary<int, BackendData.Chart.Mission.Item> MissionRepeatChartData = null;
    //    MissionRepeatChartData = StaticManager.Backend.Chart.Mission.GetChartMissionData(BackendData.Chart.Mission.MissionType.Repeat);

    //    if (StaticManager.Backend.GameData.PlayerQuest.MissionList == null || StaticManager.Backend.GameData.PlayerQuest.MissionList.Count == 0)
    //        return;

    //    if (string.IsNullOrEmpty(StaticManager.Backend.GameData.PlayerGameData.LastLoginTime)) // �ű����� 
    //    {
    //        // ���� ���� ��ư �� Ȱ��ȭ 
    //        foreach (var item in StaticManager.Backend.GameData.PlayerQuest.MissionList)
    //        {
    //            StaticManager.Backend.GameData.PlayerQuest.UpdateMissionIsDone(item.MissionID, false);
    //            StaticManager.Backend.GameData.PlayerQuest.UpdateMissionAdsIsDone(item.MissionID, false);  
    //            item.MissionNowStep = 0;
    //        }
    //        StaticManager.Backend.GameData.PlayerQuest.UpdateDailyTotalMissionDone(false);  
    //        StaticManager.Backend.GameData.PlayerQuest.UpdateDailyTotalMissionAdsIsDone(false);
    //    }
    //    else // �������� 
    //    {
    //        DateTime nowTime = DateTime.Now; // ���� �ð� 
    //        //DateTime nowTime = new DateTime(2023,04,20); // ���� �ð� 
    //        DateTime serverTime = DateTime.Parse(StaticManager.Backend.GameData.PlayerGameData.LastLoginTime); // ���� �ð�       
    //        if (serverTime == null)
    //            return;
    //        TimeSpan timeSpan = nowTime - serverTime;
    //        double timeCal = timeSpan.Days;
    //        Debug.Log($"timeCal : {timeCal}");    
    //        if (timeCal >= 1) // 1�� �̻� ���� ���ߴٰ� ������   
    //        {
    //            // ���� ���� ��ư �� Ȱ��ȭ 
    //            foreach (var item in StaticManager.Backend.GameData.PlayerQuest.MissionList)
    //            {
    //                if (item.MissionIsDone == true)
    //                {
    //                    StaticManager.Backend.GameData.PlayerQuest.UpdateMissionIsDone(item.MissionID, false);
    //                }
    //                if(item.MissionAdsIsDone == true)
    //                {
    //                    StaticManager.Backend.GameData.PlayerQuest.UpdateMissionAdsIsDone(item.MissionID, false);
    //                }

    //                if (MissionRepeatChartData.ContainsKey(item.MissionID))
    //                    continue;  
    //                item.MissionNowStep = 0;
    //            }
    //            StaticManager.Backend.GameData.PlayerQuest.UpdateDailyTotalMissionDone(false);
    //            StaticManager.Backend.GameData.PlayerQuest.UpdateDailyTotalMissionAdsIsDone(false);

    //        }
    //        else
    //        {
    //            int result = DateTime.Compare(DateTime.Parse(nowTime.ToString("yyyy-MM-dd")), DateTime.Parse(serverTime.ToString("yyyy-MM-dd"))); // ����ð� , �����ð� ��
    //            if (result == 0) // ������ 
    //            {
    //                // ���� ���� �����Ŵ� ���� 
    //            }
    //            else if (result > 0) // �ٷ� ������
    //            {
    //                // �� Ȱ��ȭ 
    //                Debug.Log("�������̿���");
    //                foreach (var item in StaticManager.Backend.GameData.PlayerQuest.MissionList)
    //                {
    //                    if (item.MissionIsDone == true)
    //                    {
    //                        StaticManager.Backend.GameData.PlayerQuest.UpdateMissionIsDone(item.MissionID, false);
    //                    }
    //                    if (item.MissionAdsIsDone == true)
    //                    {
    //                        StaticManager.Backend.GameData.PlayerQuest.UpdateMissionAdsIsDone(item.MissionID, false);
    //                    }
    //                    if (MissionRepeatChartData.ContainsKey(item.MissionID))
    //                        continue;
    //                    item.MissionNowStep = 0;    
    //                }
    //                StaticManager.Backend.GameData.PlayerQuest.UpdateDailyTotalMissionDone(false);
    //                StaticManager.Backend.GameData.PlayerQuest.UpdateDailyTotalMissionAdsIsDone(false);  

    //            }
    //        }
    //    }

    //}
    public void SetDailyMissionNowDateState() // ���� ���� ������ �޾Ҵ���,, �ȹ޾����� ��ư�� Ȱ��ȭ �ؾߵǰ� ���Ϸ� �Ѿ�ٸ� �ʱ�ȭ �ؾߵ�  
    {
        Debug.Log("SetDailyMissionNowDateState");
        Dictionary<int, BackendData.Chart.Mission.Item> MissionRepeatChartData = null;
        MissionRepeatChartData = StaticManager.Backend.Chart.Mission.GetChartMissionData(BackendData.Chart.Mission.MissionType.Repeat);

        if (StaticManager.Backend.GameData.PlayerQuest.MissionList == null || StaticManager.Backend.GameData.PlayerQuest.MissionList.Count == 0)
            return;

        // ���� ���� ��ư �� Ȱ��ȭ 
        foreach (var item in StaticManager.Backend.GameData.PlayerQuest.MissionList)
        {
            if (item.MissionIsDone == true)
            {
                StaticManager.Backend.GameData.PlayerQuest.UpdateMissionIsDone(item.MissionID, false);
            }
            if (item.MissionAdsIsDone == true)
            {
                StaticManager.Backend.GameData.PlayerQuest.UpdateMissionAdsIsDone(item.MissionID, false);
            }

            if (MissionRepeatChartData.ContainsKey(item.MissionID))
                continue;
            item.MissionNowStep = 0;
        }
        StaticManager.Backend.GameData.PlayerQuest.UpdateDailyTotalMissionDone(false);
        StaticManager.Backend.GameData.PlayerQuest.UpdateDailyTotalMissionAdsIsDone(false);  
    }

}
