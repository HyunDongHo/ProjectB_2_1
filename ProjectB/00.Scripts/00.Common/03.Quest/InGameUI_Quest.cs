// Copyright 2013-2022 AFI, INC. All rights reserved.

using System;
using System.Collections.Generic;
using UnityEngine;
using BackendData.Chart.Quest;

namespace InGameScene.UI {
    //===========================================================
    // 퀘스트 UI
    //===========================================================
    public class InGameUI_Quest : MonoBehaviour {
        [SerializeField] private GameObject _QuestParentObject;
        [SerializeField] private GameObject _questItemPrefab;

        [SerializeField] private InGameUI_QuestItem _questItem;

        // 퀘스트 아이템 리스트(key값은 questId)
        private Dictionary<int, InGameUI_QuestItem> _questItemDic = new();

        // 퀘스트 유형별 아이템 리스트(key값은 questType)
        private Dictionary<int, List<InGameUI_QuestItem>> _questItemDicByQuestType = new();

        BackendData.Chart.Quest.Item _nowQuestItem = null;

        [SerializeField] private long defeatEnemyCount = 0;  
        public void Awake() {

            // 퀘스트 타입별로 업데이트(골드 관련 퀘스트는 UserData, 무기 관련은 WeaponInventory등)

            StaticManager.Backend.Chart.Quest.Dictionary.TryGetValue(StaticManager.Backend.GameData.PlayerQuest.QuestID, out _nowQuestItem);

            if(_nowQuestItem != null)
            {
                _questItem.Init(_nowQuestItem);

                QuestsManager.instance.questType = _nowQuestItem.QuestType;
                if (_nowQuestItem.QuestType == QuestType.DefeatEnemy)
                    defeatEnemyCount = StaticManager.Backend.GameData.PlayerQuest.QuestNowStep;
                UpdateUI();
            }
            else
            {
                QuestsManager.instance.questType = QuestType.None;
                _questItem.gameObject.SetActive(false);      
            }
        }

        public InGameUI_QuestItem GetQuestItem()
        {
            return _questItem;
        }
        void SetDefeatEnemyCount(long count)  
        {
            defeatEnemyCount = count;  
        }
        public void ResetQuestState()
        {
            QuestsManager.instance.questType = BackendData.Chart.Quest.QuestType.None;
            _nowQuestItem = null;
            SetDefeatEnemyCount(0);
        }
        public void ChangeQuestItem(BackendData.Chart.Quest.Item item)
        {
            if (item != null)
            {
                _nowQuestItem = item;
                _questItem.Init(_nowQuestItem);
                QuestsManager.instance.questType = item.QuestType;
                defeatEnemyCount = 0;  
                //BackendData.Chart.Quest.Item lastQuestItem = null;
                //StaticManager.Backend.Chart.Quest.Dictionary.TryGetValue(StaticManager.Backend.GameData.PlayerQuest.QuestID-1, out lastQuestItem);
                //if(lastQuestItem != null )
                //{
                //    if(lastQuestItem.QuestType == QuestType.DefeatEnemy)
                //    {
                //        if (_nowQuestItem.QuestType == QuestType.DefeatEnemy)
                //        {
                //            defeatEnemyCount -= ((long)lastQuestItem.RequestCount+1);
                //        }
                //        else
                //        {
                //            defeatEnemyCount = 0;
                //        }
                //    }
                //    else
                //    {
                //        defeatEnemyCount = 0;  
                //    }
                //}
                UpdateUI();
            }
        }

        public void UpdateUI()
        {
            switch (_nowQuestItem.QuestType)
            {
                case QuestType.LevelUp: // 레벨업 관련 퀘스트일 경우에는 유저 레벨을 이용하여 업데이트
                    _questItem.UpdateUI(StaticManager.Backend.GameData.PlayerGameData.DWarriorLevel);
                    break;
                case QuestType.UseGold: // 골드 사용 퀘스트일 경우에는 UsingGold 변수들을 이용하여 업데이트
                                        //_questItem.UpdateUI(StaticManager.Backend.GameData.PlayerGameData.StatList);
                    int questContentStatID = SetStatItemLevel();
                    if(questContentStatID == 0)
                    {
                        Debug.Log("stat의 itemID 0 은 없습니다.");
                        return;
                    }

                    BackendData.GameData.StatData statData = null;
                    statData = StaticManager.Backend.GameData.PlayerGameData.StatList.Find(item => item.ItemID == questContentStatID);
                    if (statData == null)
                        return;

                    _questItem.UpdateUI(statData.ItemLevel);
  
                    break;
                case QuestType.DefeatEnemy:// 적 처리 퀘스트는 DefeatEenmyCount를 계산  
                    _questItem.UpdateUI(defeatEnemyCount++);    
                    break;
                case QuestType.GetItem: // 아이테 관련 함수엘 경우에는 아이템이 존재하는지 확인
                    // UpdateUIForGetItem으로 대체
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(_nowQuestItem.QuestType), _nowQuestItem.QuestType, null);
            }
        }
        public int SetStatItemLevel()
        {
            Dictionary<int, BackendData.Chart.PlayerEnhancemetData.Item> EnHancementChartData = null;
            EnHancementChartData = StaticManager.Backend.Chart.PlayerEnhancemetData.GetPlayerGoldStatItem();

            int itemID = 0;
            foreach (var chartItem in EnHancementChartData)
            {
                if (_questItem.GetQuestContent().Contains(chartItem.Value.StatUIName))
                {
                    itemID = chartItem.Value.ItemID;
                }
            }
            if (itemID == 0)
                return 0;

            return itemID;
        }
        // 각 퀘스트 타입별 업데이트하는 함수
        public void UpdateUI(QuestType questType) {
            switch (questType) {
                case QuestType.LevelUp: // 레벨업 관련 퀘스트일 경우에는 유저 레벨을 이용하여 업데이트
                    foreach (var list in _questItemDicByQuestType[(int)questType]) {
                        _questItem.UpdateUI(StaticManager.Backend.GameData.PlayerGameData.DWarriorLevel);
                    }
                    break;
                case QuestType.UseGold: // 골드 사용 퀘스트일 경우에는 UsingGold 변수들을 이용하여 업데이트
                    foreach (var list in _questItemDicByQuestType[(int)questType]) {
                        if (list.GetRepeatType() == QuestRepeatType.Day) {
                        //    list.UpdateUI((float)StaticManager.Backend.GameData.PlayerGameData.);
                        }
                        else if (list.GetRepeatType() == QuestRepeatType.Week) {
                          //  list.UpdateUI((float)StaticManager.Backend.GameData.PlayerGameData.WeekUsingGold);
                        }
                        else if (list.GetRepeatType() == QuestRepeatType.Month) {
                         //   list.UpdateUI((float)StaticManager.Backend.GameData.PlayerGameData.MonthUsingGold);
                        }
                        else {
                            throw new Exception("확인되지 않은 에러입니다.");
                        }
                    }

                    break;
                case QuestType.DefeatEnemy:// 적 처리 퀘스트는 DefeatEenmyCount를 계산
                    foreach (var list in _questItemDicByQuestType[(int)questType]) {
                        if (list.GetRepeatType() == QuestRepeatType.Day) {
                       //     list.UpdateUI(StaticManager.Backend.GameData.PlayerGameData.DayDefeatEnemyCount);
                        }
                        else if (list.GetRepeatType() == QuestRepeatType.Week) {
                       //     list.UpdateUI(StaticManager.Backend.GameData.PlayerGameData.WeekDefeatEnemyCount);
                        }
                        else if (list.GetRepeatType() == QuestRepeatType.Month) {
                       //     list.UpdateUI(StaticManager.Backend.GameData.PlayerGameData.MonthDefeatEnemyCount);
                        }
                        else {
                            throw new Exception("확인되지 않은 에러입니다.");
                        }
                    }

                    break;
                case QuestType.GetItem: // 아이테 관련 함수엘 경우에는 아이템이 존재하는지 확인
                    foreach (var list in _questItemDicByQuestType[(int)questType]) {
                        list.UpdateUI(0);
                    }

                    // UpdateUIForGetItem으로 대체
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(questType), questType, null);
            }
        }


        public void SetQuestItemActive(bool flag)
        {
            _questItem.gameObject.SetActive(flag);
        }
    }
}