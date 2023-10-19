using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class UI_TreasureItem : MonoBehaviour
{
    [SerializeField]
    TMP_Text _statTitle, _statAbil, _levelUpText, _lockPanelText, _successPerText;

    [SerializeField]
    Image _treasureImage;

    [SerializeField]
    GameObject _lockPanelObj;

    [SerializeField]
    Button _levelUpButton;

    [SerializeField]
    int _treasureID = 0;

    bool _isButtonClicked = false;

    private void Awake()
    {
    }

    public void PushLevelUpButton()
    {
        _isButtonClicked = true;

        StartCoroutine(CoPushLevelUp());
    }

    public void PointUP()
    {
        _isButtonClicked = false;

    }

    IEnumerator CoPushLevelUp()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(0.1f);

        while (_isButtonClicked)
        {
            if (IsCheckOkLevelUp() == false)
                break;

            BackendData.GameData.TreasureData userTreasureData = StaticManager.Backend.GameData.PlayerTreasure.GetTreasure(_treasureID);

            if (StaticManager.Random.GetTreasureEnchantResult(userTreasureData.TreasureLevel + 1))
            {
                userTreasureData.TreasureLevel += 1;
                if (_treasureID == 2002)
                    PlayersControlManager.instance.RefreshPlayerHp();
            }
            
            userTreasureData.TreasureCount -= 1;

            SetUI(_treasureID);

            yield return waitForSeconds;
        }

        _isButtonClicked = false;
    }

    public void SetUI(int treasureID)
    {
        _treasureID = treasureID;
        BackendData.Chart.Treasure.Item item = StaticManager.Backend.Chart.Treasure.GetTreasureItem(_treasureID);

        if (item == null)
            return;

        //  _statImage.sprite = item.ItemSprite;
        _treasureImage.sprite = item.Sprite;

        BackendData.GameData.TreasureData userTreasureData = StaticManager.Backend.GameData.PlayerTreasure.GetTreasure(treasureID);
        float nowStatValue = 0;
        int level = 0;
        int nowCount = 0;

        if (userTreasureData == null)
        {
            nowStatValue =0;
            level = 0;
            nowCount = 0;
        }
        else
        {
            nowStatValue = (userTreasureData.TreasureLevel) * item.ItemStat;
            level = userTreasureData.TreasureLevel;
            nowCount = userTreasureData.TreasureCount;
        }

        _statTitle.text = $"{item.ItemName} LV.{level}";

        _statAbil.text = $"<#00ff00>{Define.StatStatTitle[item.StatType]} +{(nowStatValue * 100).ToString("F2")}%</color>";
        _successPerText.text = $"성공확률 {(StaticManager.Random.GetTreasureEnchantSuccessPer(level)).ToString()}%";

        if (item.MaxLevel > 0 && level >= item.MaxLevel)
        {
            _lockPanelObj.gameObject.SetActive(true);
            _lockPanelText.text = "최대 레벨 도달";
        }
        else
            _lockPanelObj.gameObject.SetActive(false);

        if(userTreasureData == null)
        {
            _lockPanelObj.gameObject.SetActive(true);
            _lockPanelText.text = "유물 미획득";
        }

        _levelUpText.text = $"강화 시도\n( {nowCount} / 1 )";
    }

    bool IsCheckOkLevelUp()
    {
        BackendData.GameData.TreasureData item = StaticManager.Backend.GameData.PlayerTreasure.GetTreasure(_treasureID);

        if (item == null)
            return false;

        BackendData.Chart.Treasure.Item chartItem = StaticManager.Backend.Chart.Treasure.GetTreasureItem(_treasureID);

         long nowLevel = item.TreasureLevel;
        
         if (nowLevel >= chartItem.MaxLevel)
         {
             return false;
         }
        
         if (item.TreasureCount > 0)
             return true;
         else
             return false;
    }

    double GetNeedCount()
    {
        return 1;
    }

    public void RefreshUI()
    {
        SetUI(_treasureID);
    }
}
