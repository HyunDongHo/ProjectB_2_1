// Copyright 2013-2022 AFI, INC. All rights reserved.

using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using BackendData.Chart.Quest;
using DG.Tweening;

namespace InGameScene.UI
{
    //===========================================================
    // 퀘스트 아이템 UI
    //===========================================================
    public class InGameUI_QuestItem : MonoBehaviour
    {
        [SerializeField] private Image _questRewardItemImage;
        //[SerializeField] private TMP_Text _questReqeatTypeText;
        [SerializeField] private TMP_Text _questContentText;
        [SerializeField] private TMP_Text _questRewardText;
        [SerializeField] private TMP_Text _myRequestAchieveText;

        [SerializeField] private Button _requestAchieveButton;
        //[SerializeField] private TMP_Text _isAchieveText;  

        private BackendData.Chart.Quest.Item _questItemInfo;

        [Space]

        public Image highlight;
        public float highlightBlinkingDuration = 0.6f;
        private float highlightOriginAlpha = 0.0f;
        bool isHightlight = false;

        [Space]

        public GameObject toggleCircleImage;
        public Image sliderImage; 

        // 퀘스트 타입 리턴하는 함수
        public QuestRepeatType GetRepeatType()
        {
            return _questItemInfo.QuestRepeatType;
        }
        public int GetQuestItemID()
        {
            return _questItemInfo.QuestID;
        }
        public string GetQuestContent()
        {
            return _questItemInfo.QuestContent;

        }
        // 퀘스트 보상 리스트
        private List<string> _rewardList = new();

        private void Awake()
        {
            highlightOriginAlpha = highlight.color.a;
            _requestAchieveButton.onClick.AddListener(Achieve);
        }

        private void OnDestroy()
        {
            _requestAchieveButton.onClick.RemoveAllListeners();  
        }

        public void Init(Item questItemInfo)
        {
            SetHighlightActive(false);
            _questItemInfo = questItemInfo;

            //  switch (_questItemInfo.QuestRepeatType) {
            //      case QuestRepeatType.Day:
            //          _questReqeatTypeText.text = "일일";
            //          break;
            //      case QuestRepeatType.Week:
            //          _questReqeatTypeText.text = "주간";
            //          break;
            //      case QuestRepeatType.Month:
            //          _questReqeatTypeText.text = "월간";
            //          break;
            //      case QuestRepeatType.Once:
            //          _questReqeatTypeText.text = "업적";
            //          break;
            //  }

            _questRewardItemImage.sprite = StaticManager.Backend.Chart.Item.GetItem(_questItemInfo.RewardItem[0].Id).ImageSprite;
            _questContentText.text = _questItemInfo.QuestContent;
            _questRewardText.text = $"{_questItemInfo.RewardItem[0].Count}";
            _myRequestAchieveText.text = $"{StaticManager.Backend.GameData.PlayerQuest.QuestNowStep.ToString()} / {_questItemInfo.RequestCount}";
            sliderImage.fillAmount = StaticManager.Backend.GameData.PlayerQuest.QuestNowStep / _questItemInfo.RequestCount;

            if(sliderImage.fillAmount == 1)
            {
                highlight.gameObject.SetActive(true);
                toggleCircleImage.gameObject.SetActive(true);
            }
            else
            {
                highlight.gameObject.SetActive(false);
                toggleCircleImage.gameObject.SetActive(false);
            }


            /* ========이건 필요 없음 ======*/
            // Exp, Money를 주는 RewardStat이 존재할 경우, 보상을 알려주는 text에 삽입
            if (_questItemInfo.RewardStat != null) {
                foreach (var item in _questItemInfo.RewardStat) {
                    // exp가 보상일 경우
                    if (item.Exp > 0) {
                        _rewardList.Add($"{item.Exp} Exp");
                    }
                    // money가 보상일 경우
                    if (item.Money > 0) {
                        _rewardList.Add($"{item.Money} Gold");
                    }
                }
            }

                /* ========보상 리스트 추가 ========*/
            // 아이템, 무기를 주는 RewardItem이 존재할 경우, 보상을 알려주는 text에 삽입
            if (_questItemInfo.RewardItem != null) {
                foreach (var item in _questItemInfo.RewardItem) {
                    switch (item.RewardItemType) {
                        case RewardItemType.Item: // 보상이 아이템일 경우 아이템 이름
                            _rewardList.Add(StaticManager.Backend.Chart.Item.Dictionary[item.Id].ItemName);
                            break;
                        case RewardItemType.Weapon:// 보상이 무기일 경우 무기 이름
                            _rewardList.Add(StaticManager.Backend.Chart.Weapon.Dictionary[item.Id].ItemName);
                            break;
                    }
                }
            }


            // 보상을 담은 list 전부 한줄로 표현
            StringBuilder rewardString = new StringBuilder();
            for (int i = 0; i < _rewardList.Count; i++) {
                if (i > 0) {
                    rewardString.Append(" | ");
                }

                rewardString.Append(_rewardList[i]);
            }

         // _questRewardText.text = rewardString.ToString();
         // _questRequestText.text = _questItemInfo.RequestCount.ToString();
         // _myRequestAchieveText.text = 0.ToString();

        }

        public void UpdateUI()
        {
            //bool isAchieve = StaticManager.Backend.GameData.QuestAchievement.Dictionary[_questItemInfo.QuestID].IsAchieve;

            BackendData.Chart.Quest.Item questChartData = null;
            StaticManager.Backend.Chart.Quest.Dictionary.TryGetValue(StaticManager.Backend.GameData.PlayerQuest.QuestID, out questChartData);

            if (questChartData == null)
                return;

            { // 달성이 되었다면
                if (questChartData.RequestCount <= StaticManager.Backend.GameData.PlayerQuest.QuestNowStep)
                {
                    //_isAchieveText.text = "달성";
                    _requestAchieveButton.interactable = true;
                 //   _requestAchieveButton.GetComponent<Image>().color = new Color32(255, 236, 144, 255);
                    SetHighlightBlinking(true);
                }
                else
                { // 아직 count가 부족하다면
                    //_isAchieveText.text = "미달성";
                    _requestAchieveButton.interactable = false;
                   // _requestAchieveButton.GetComponent<Image>().color = Color.gray;
                }
            }

            // 현재 진행중인 횟수
            _myRequestAchieveText.text = StaticManager.Backend.GameData.PlayerQuest.QuestNowStep.ToString();
        }
        // 퀘스트 차트에 있는 도달 횟수가 넘었는지 확인.
        public void UpdateUI(float count)
        {
            //bool isAchieve = StaticManager.Backend.GameData.QuestAchievement.Dictionary[_questItemInfo.QuestID].IsAchieve;

            BackendData.Chart.Quest.Item questChartData = null;
            StaticManager.Backend.Chart.Quest.Dictionary.TryGetValue(StaticManager.Backend.GameData.PlayerQuest.QuestID, out questChartData);

            if (questChartData == null)
                return;

            { // 달성이 되었다면
                if (questChartData.RequestCount <= count)
                {
                    _requestAchieveButton.interactable = true;
                    //  _requestAchieveButton.GetComponent<Image>().color = new Color32(255, 236, 144, 255);
                    highlight.gameObject.SetActive(true);
                    toggleCircleImage.gameObject.SetActive(true);    

                    SetHighlightBlinking(true);
                }
                else
                { // 아직 count가 부족하다면
                    highlight.gameObject.SetActive(false);
                    toggleCircleImage.gameObject.SetActive(false);
                    _requestAchieveButton.interactable = false;
                   // _requestAchieveButton.GetComponent<Image>().color = Color.gray;
                }
            }

            // 현재 진행중인 횟수
            StaticManager.Backend.GameData.PlayerQuest.SetQuestData((long)count);
            _myRequestAchieveText.text = $"{count} / {_questItemInfo.RequestCount}";
            sliderImage.fillAmount = StaticManager.Backend.GameData.PlayerQuest.QuestNowStep / _questItemInfo.RequestCount;  

        }


        // 퀘스트 달성 버튼 클릭시 호출되는 함수
        public void Achieve()
        {
            /* TODO : Setting Scene에서 싱글톤으로 QuestManager , RewardManager 만들어서 체크 */
            // QuestsManager 클래스로 현재  퀘스트를 클리어 했는지 체크 후  보상
            if (QuestsManager.instance.CheckNowQuestClear() == false)
                return;

            Debug.Log("퀘스트 완료 체크 ");
            // 보상
            Reward();

            // QuestsManager 클래스로 현재 퀘스트 클리어 처리 -> 다음퀘스트로 
            StaticManager.Backend.GameData.PlayerQuest.ClearQuestData();
            QuestsManager.instance.ClearQuestValue();
            //InGameScene.Managers.Quest.ClearQuestValue();


            // 퀘스트 UI를 완료로 변경하고 버튼 변경
            //  _isAchieveText.text = "완료";
            //_requestAchieveButton.interactable = false;  


          //  _requestAchieveButton.GetComponent<Image>().color = Color.gray;
        }
             

        // 각 보상 아이템 별로 보상을 지급하는 함수
        private void Reward()
        {
            //TODO : RewardManager 클래스로 보상 획득
            //SetItem(new List<int>() { 10002, 10002 }, new List<double>() { 300, 500 });
            // 1. Reward Popup 창 뜨기 
            Debug.Log("Quest Reward");
            int id = _questItemInfo.RewardItem[0].Id;
            double count = _questItemInfo.RewardItem[0].Count;
            RewardManager.instance.ShowRewardWindow(new List<int>() { id }, new List<double>() { count }, true);  

            
            //InGameScene.Managers._uiManager.RewardUI.StartAciton();

            //Managers.Reward.GetQuestReward(_questItemInfo.RewardItem);

            //if (_questItemInfo.RewardStat != null)
            //{
            //    // 차트 정보에  money, exp를 주는 rewardStat이 존재한다면
            //    foreach (var item in _questItemInfo.RewardStat)
            //    {
            //        InGameScene.Managers.Game.UpdateUserData(item.Money, item.Exp);
            //    }
            //}
            //
            //// 차트 정보에 아이템, 무기를 주는 RewardItem이 존재한다면
            //if (_questItemInfo.RewardItem != null)
            //{
            //    foreach (var item in _questItemInfo.RewardItem)
            //    {
            //        switch (item.RewardItemType)
            //        {
            //            case RewardItemType.Item: // 아이템일 경우 아이템의 id를 가져와 업데이트
            //                InGameScene.Managers.Game.UpdateItemInventory(item.Id, (int)item.Count);
            //
            //                break;
            //            case RewardItemType.Weapon: // 무기일 경우, 무기의 id를 가져와 업데이트
            //                InGameScene.Managers.Game.UpdateWeaponInventory(item.Id);
            //                break;
            //        }
            //    }
            //}
            //
            //// 받은 보상을 UI로 표현
            //StringBuilder rewardString = new StringBuilder();
            //rewardString.Append("다음 보상을 획득했습니다.\n");
            //for (int i = 0; i < _rewardList.Count; i++)
            //{
            //    if (i > 0)
            //    {
            //        rewardString.Append("\n");
            //    }
            //
            //    rewardString.Append(_rewardList[i]);
            //}
            //
            //StaticManager.UI.AlertUI.OpenAlertUI("퀘스트 완료", rewardString.ToString());
        }
        public void SetHighlightBlinking(bool isActive)
        {
            if (isHightlight == true)
                return;

            SetHighlightActive(isActive);

            highlight.DOFade(0f, highlightBlinkingDuration).SetLoops(-1, LoopType.Yoyo);
        }

        public void SetHighlightActive(bool isActive)
        {
            isHightlight = isActive;

            highlight.DOKill();

            highlight.DOFade(highlightOriginAlpha, 0);
            highlight.gameObject.SetActive(isActive);
        }

    }

}