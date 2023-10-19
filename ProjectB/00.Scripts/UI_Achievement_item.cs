using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Achievement_item : UI_Base
{

    double _level ;
    double _needCount;
    double _presentStat;
    double _afterStat;

    // chart info
    //string _chartStatName;
    int _chartItemID;
    string _chartTitleText;
    long _chartMaxLevel;
    double _chartNeedCount;
    double _chartNeedCountRatio;
    double _chartIncreasedAmount;

    // button push flag
    bool _buttonPush;

    TextMeshProUGUI titleNameText;
    TextMeshProUGUI levelText;
    TextMeshProUGUI presentStatText;
    TextMeshProUGUI afterStatText;

    TextMeshProUGUI text_Button;
    TextMeshProUGUI goldNeedCountText;
    Image button_claim;

    enum GameObjects
    {
        TitleNameText,
        LevelText,
        PresentStatText,
        AfterStatText,

        Button_Claim,
        Text_Button, // 능력치 강화
        GoldNeedCountText,
    }

    public override void init()
    {
        Bind<GameObject>(typeof(GameObjects)); //Binding 

        titleNameText = Get<GameObject>((int)GameObjects.TitleNameText).GetComponent<TextMeshProUGUI>();
        levelText = Get<GameObject>((int) GameObjects.LevelText).GetComponent<TextMeshProUGUI>();
        presentStatText= Get<GameObject>((int)GameObjects.PresentStatText).GetComponent<TextMeshProUGUI>();
        afterStatText = Get<GameObject>((int)GameObjects.AfterStatText).GetComponent<TextMeshProUGUI>();

        button_claim = Get<GameObject>((int)GameObjects.Button_Claim).GetComponent<Image>();
        text_Button = Get<GameObject>((int)GameObjects.Text_Button).GetComponent<TextMeshProUGUI>();
        goldNeedCountText = Get<GameObject>((int)GameObjects.GoldNeedCountText).GetComponent<TextMeshProUGUI>();

        titleNameText.text = _chartTitleText;
        levelText.text = $"Lv.{_level}";
        presentStatText.text = $"{_presentStat}";
        afterStatText.text = $"{_afterStat}";
        goldNeedCountText.text = $"{Math.Truncate(_needCount)}";

        _buttonPush = false;
        //Get<GameObject>((int)GameObjects.Button_Claim).gameObject.AddUIEvent(OnButtonClicked);
        Get<GameObject>((int)GameObjects.Button_Claim).gameObject.AddUIEvent(OnButtonDown, type : Define.UIEvent.PointerDown);
        Get<GameObject>((int)GameObjects.Button_Claim).gameObject.AddUIEvent(OnButtonUp, type: Define.UIEvent.PointerUp);
        SetAllText();
        SetAllColor();
    }
    private void Update()
    {
        SetAllColor();
    }
    public void SetLevelText(double level) // 여기서 Level 주면 다 세팅 
    {
        _level = level;
        _needCount = _chartNeedCount * Mathf.Pow((float)_chartNeedCountRatio, (float)level);
        _presentStat = level * _chartIncreasedAmount;
        _afterStat = _presentStat + _chartIncreasedAmount;
    }
    /* chart */
    public void SetChartItemID(int id)
    {
        _chartItemID = id;
    }
    public void SetChartTitleNameText(string titleText)
    {
        _chartTitleText = titleText;
    }
    public void SetChartGoldNeedCountText(double needCount)
    {
        _chartNeedCount = needCount;
    }

    public void SetChartMaxLevel(long chartMaxLevel)
    {
        _chartMaxLevel = chartMaxLevel;
    }
    public void SetChartNeedCountRatio(double needCountRatio)
    {
        _chartNeedCountRatio = needCountRatio;
    }
    public void SetChartIncreasedAmount(double amount)
    {
        _chartIncreasedAmount = amount;
    }

    public void SetAllText()
    {
        if (titleNameText == null || levelText == null || goldNeedCountText == null || presentStatText == null || afterStatText == null)
            return;
        titleNameText.text = _chartTitleText; // 이름 지정  
        levelText.text = $"Lv.{_level}"; // 이름 지정   
        goldNeedCountText.text = $"{Math.Truncate(_needCount)}"; // 이름 지정    
        presentStatText.text = $"{_presentStat}"; // 이름 지정  
        afterStatText.text = $"{_afterStat}"; // 이름 지정   
    }
    public void SetAllColor()
    {
        if (button_claim == null || text_Button == null || goldNeedCountText == null)
            return;  

        if(StaticManager.Backend.GameData.PlayerGameData.DCoin > _needCount)
        {
            button_claim.color = new Color(1, 1, 1, 1);
            text_Button.color = new Color(1, 1, 1, 1);
            goldNeedCountText.color = new Color(1, 1, 1, 1);
        }
        else
        {
            button_claim.color = new Color(0.5f, 0.5f, 0.5f, 1);
            text_Button.color = new Color(1, 0, 0, 1);
            goldNeedCountText.color = new Color(1, 0, 0, 1);
        }
    }

    void EnhanceMentActivate()
    {
        //Debug.Log($"Achivement button clicked ");
        //Debug.Log($"DCoin : {StaticManager.Backend.GameData.PlayerGameData.DCoin}");
        /////////////////////////////////////////////////////////////////////////
        double myCoin = StaticManager.Backend.GameData.PlayerGameData.DCoin;
        if (myCoin <= 0)  
            return;
        if(myCoin >= _needCount)
        {
            if(_level < _chartMaxLevel)
            {
                // _needCount 올라가고, my coin 줄어들고 
                StaticManager.Backend.GameData.PlayerGameData.UpdateUserData_GoldStat(_chartItemID, -_needCount);
                StageManager.instance.canvasManager.GetUIManager<UIManager_Common>().RefreshCommonUI();  

                ++_level;
                _needCount *= _chartNeedCountRatio;
                _presentStat += _chartIncreasedAmount;
                _afterStat = _presentStat + _chartIncreasedAmount;

                if (_chartItemID == 2)
                    PlayersControlManager.instance.RefreshPlayerHp();

                SetAllText();
                SetAllColor();

                if (QuestsManager.instance.questType == BackendData.Chart.Quest.QuestType.UseGold)
                    QuestsManager.instance.UpdateQuestUI();
            }
        }
        else
        {
            // 버튼 회색, 능력치 강화 검은색, goldneedcount 빨간색
            SetAllColor();  
        }
    }

    void OnButtonDown(PointerEventData data)
    {
        _buttonPush = true;
        StartCoroutine(CoLevelUpButton());
    }
    void OnButtonUp(PointerEventData data)
    {
        _buttonPush = false;

    }
    IEnumerator CoLevelUpButton()
    {
        while (_buttonPush)
        {
            EnhanceMentActivate();
            yield return new WaitForSeconds(0.1f);

        }     
    }                                                                                               
}
