using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using BackendData.GameData;

public class UI_DiceItem : MonoBehaviour
{
    [SerializeField]
    TMP_Text _statRankText ,_statAbilText, _lockPanelText;

    [SerializeField]
    GameObject _lockPanelObj;

    [SerializeField]
    int _diceIndex = 0;

    [SerializeField]
    Button _slotLockButton;

    bool _isButtonClicked = false;

    private void Awake()
    {
        AddEvent();
    }

    private void OnDestroy()
    {
        RemoveEvent();
    }

    void AddEvent()
    {
        _slotLockButton.onClick.AddListener(ToggleSlotLock);
    }
    void RemoveEvent()
    {
        _slotLockButton.onClick.RemoveListener(ToggleSlotLock);
    }

    private void ToggleSlotLock()
    {
        Debug.Log("¿·±›");

        int nowIndex = StaticManager.Backend.GameData.PlayerDice.NowSlotNum;

        List<DiceData> diceDatas = null;
        StaticManager.Backend.GameData.PlayerDice.DiceDict.TryGetValue(nowIndex.ToString(), out diceDatas);

        if (diceDatas == null)
        {
            return;
        }
        else
        {
            if (diceDatas.Count < _diceIndex)
            {
                _lockPanelObj.gameObject.SetActive(true);
                _lockPanelText.text = "ΩΩ∑‘ πÃ∞≥πÊ";
            }
            else
            {
                bool isLock = !diceDatas[_diceIndex].IsLock;
                StaticManager.Backend.GameData.PlayerDice.SetSlotLock(_diceIndex, isLock);

                if (isLock == true)
                {
                    _lockPanelObj.gameObject.SetActive(true);
                    _lockPanelText.text = "ΩΩ∑‘ ¿·±›";
                }
                else
                {
                    _lockPanelObj.gameObject.SetActive(false);
                }

            }
        }
    }

    public void SetUI()
    {
        List<DiceData> diceDatas = null;

        int index = StaticManager.Backend.GameData.PlayerDice.NowSlotNum;

        StaticManager.Backend.GameData.PlayerDice.DiceDict.TryGetValue(index.ToString(), out diceDatas);

        if (diceDatas == null)
        {
            _lockPanelObj.gameObject.SetActive(true);
            _lockPanelText.text = "ΩΩ∑‘ ¿·±›";
            return;
        }

        bool isLockSlot = diceDatas[_diceIndex].IsLock;
        int diceNum = diceDatas[_diceIndex].DiceNum;

        BackendData.Chart.DiceRandom.Item chartItem = null;
        StaticManager.Backend.Chart.DiceRandom.Dictionary.TryGetValue(diceNum, out chartItem);

        // ∫Ûƒ≠
        if (diceNum == 0)
        {
            _statRankText.text = "";
            _statAbilText.text = "";

        } // 
        else if (diceNum % 7 != 0)
        {
            _statRankText.text = Define.DiceGradeStat[diceNum % 7];
            _statRankText.color = Define.DiceGradeColor[diceNum % 7];
        }
        else
        {
            _statRankText.text = Define.DiceGradeStat[7];
            _statRankText.color = Define.DiceGradeColor[7];
        }

        if (chartItem != null)
        {
            string statTitle = Define.StatStatTitle[chartItem.StatType];
            _statAbilText.text = $"{statTitle} + {(diceDatas[_diceIndex].DiceValue * 100).ToString("F1")}%";
        }
        else
            _statAbilText.text = "";

        if (isLockSlot == true)
        {
            _lockPanelObj.gameObject.SetActive(true);
            _lockPanelText.text = "ΩΩ∑‘ ¿·±›";
        }
        else
        {
            _lockPanelObj.gameObject.SetActive(false);
        }

    }


    public void RefreshUI()
    {
        // SetUI();
    }
}
