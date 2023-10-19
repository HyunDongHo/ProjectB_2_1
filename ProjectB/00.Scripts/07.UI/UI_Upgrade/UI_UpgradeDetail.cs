using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_UpgradeDetail : UI_Base
{
    private const int UpgradeMaxLevel = 4;

    [SerializeField]
    UI_PlayerModel _uiPlayerModel;

    [SerializeField]
    TextMeshProUGUI _upgradeLevelText, _upgradeStatValue01, _upgradeStatValue02, _upgradeNeedCount;

    [SerializeField]
    Button _prevGradeButton, _nextGradeButton, _warriorButton, _archerButton, _wizardButton, _upgradeButton;

    UI_ToggleColor[] _warriorToggleColors, _archerToggleColors, _wizardToggleColors;

    private int nowIndex = 0;
    private PlayerType nowPlayerType = PlayerType.Warrior;

    public override void init()
    {
        //nowPlayerType = PlayersControlManager.instance.nowActive;
    }

    private void OnEnable()
    {
        nowPlayerType = PlayersControlManager.instance.nowActive;

        switch (nowPlayerType)
        {
            case PlayerType.Warrior:
                nowIndex = StaticManager.Backend.GameData.PlayerGameData.WarriorUpgradeLevel - 1;
                break;
            case PlayerType.Archer:
                nowIndex = StaticManager.Backend.GameData.PlayerGameData.ArcherUpgradeLevel - 1;
                break;
            case PlayerType.Wizard:
                nowIndex = StaticManager.Backend.GameData.PlayerGameData.WizardUpgradeLevel - 1;
                break;
        }

        if (nowIndex >= UpgradeMaxLevel)
            nowIndex = UpgradeMaxLevel - 1;
    }

    public void Awake()
    {
        AddEvent();
        _warriorToggleColors = _warriorButton.GetComponentsInChildren<UI_ToggleColor>();
        _archerToggleColors = _archerButton.GetComponentsInChildren<UI_ToggleColor>();
        _wizardToggleColors = _wizardButton.GetComponentsInChildren<UI_ToggleColor>();
  
    }

    private void OnDestroy()
    {
        RemoveEvent();
    }

    void AddEvent()
    {
        _warriorButton.onClick.AddListener(ClickWarrior);
        _archerButton.onClick.AddListener(ClickArcher);
        _wizardButton.onClick.AddListener(ClickWizard); 
        _prevGradeButton.onClick.AddListener(OnPrevGradeButton);
        _nextGradeButton.onClick.AddListener(OnNextGradeButton);
        _upgradeButton.onClick.AddListener(ClickUpgradeButton);
    }

    void RemoveEvent()
    {
        _warriorButton.onClick.RemoveListener(ClickWarrior);
        _archerButton.onClick.RemoveListener(ClickArcher);
        _wizardButton.onClick.RemoveListener(ClickWizard);
        _prevGradeButton.onClick.RemoveListener(OnPrevGradeButton);
        _nextGradeButton.onClick.RemoveListener(OnNextGradeButton);
        _upgradeButton.onClick.RemoveListener(ClickUpgradeButton);
    }

    public void ClickWarrior()
    {
        SetToggleColor(_warriorToggleColors, true);
        SetToggleColor(_archerToggleColors, false);
        SetToggleColor(_wizardToggleColors, false);

        int index = StaticManager.Backend.GameData.PlayerGameData.WarriorUpgradeLevel - 1;

        if (index >= UpgradeMaxLevel)
            index = UpgradeMaxLevel - 1;

        _uiPlayerModel.ChangeModelSetter(PlayerType.Warrior, index);
        nowPlayerType = PlayerType.Warrior;
        nowIndex = index;

        SetUpgradeDetailUI();
    }
    public void ClickArcher()
    {
        SetToggleColor(_warriorToggleColors, false);
        SetToggleColor(_archerToggleColors, true);
        SetToggleColor(_wizardToggleColors, false);

        int index = StaticManager.Backend.GameData.PlayerGameData.ArcherUpgradeLevel - 1;

        _uiPlayerModel.ChangeModelSetter(PlayerType.Archer, index);

        if (index >= UpgradeMaxLevel)
            index = UpgradeMaxLevel - 1;

        nowPlayerType = PlayerType.Archer;
        nowIndex = index;

        SetUpgradeDetailUI();
    }
    public void ClickWizard()
    {
        SetToggleColor(_warriorToggleColors, false);
        SetToggleColor(_archerToggleColors, false);
        SetToggleColor(_wizardToggleColors, true);

        int index = StaticManager.Backend.GameData.PlayerGameData.WizardUpgradeLevel - 1;

        if (index >= UpgradeMaxLevel)
            index = UpgradeMaxLevel - 1;

        _uiPlayerModel.ChangeModelSetter(PlayerType.Wizard, index);
        nowPlayerType = PlayerType.Wizard;
        nowIndex = index;
        SetUpgradeDetailUI();
    }

    bool CheckAvailableUpgradeNowLevel()
    {
        switch (nowPlayerType)
        {
            case PlayerType.Warrior:
                if (StaticManager.Backend.GameData.PlayerGameData.WarriorUpgradeLevel + 1 == nowIndex + 1 && StaticManager.Backend.GameData.PlayerGameData.WarriorUpgradeLevel < UpgradeMaxLevel)
                    return true;
                else
                    return false;
            case PlayerType.Archer:
                if (StaticManager.Backend.GameData.PlayerGameData.ArcherUpgradeLevel + 1 == nowIndex + 1 && StaticManager.Backend.GameData.PlayerGameData.ArcherUpgradeLevel < UpgradeMaxLevel)
                    return true;
                else
                    return false;
            case PlayerType.Wizard:
                if (StaticManager.Backend.GameData.PlayerGameData.WizardUpgradeLevel + 1 == nowIndex + 1 && StaticManager.Backend.GameData.PlayerGameData.WizardUpgradeLevel < UpgradeMaxLevel)
                    return true;
                else
                    return false;
        }

        return false;
    }

    void ClickUpgradeButton()
    {
        int nextLevel = 2;

        switch (nowPlayerType)
        {
            case PlayerType.Warrior:
                nextLevel = StaticManager.Backend.GameData.PlayerGameData.WarriorUpgradeLevel + 1;
                break;
            case PlayerType.Archer:
                nextLevel = StaticManager.Backend.GameData.PlayerGameData.ArcherUpgradeLevel + 1;
                break;
            case PlayerType.Wizard:
                nextLevel = StaticManager.Backend.GameData.PlayerGameData.WizardUpgradeLevel + 1;
                break;
        }

        if (nextLevel > UpgradeMaxLevel)
            return;       
        
        if(nowIndex + 1 == nextLevel)
        {
            StaticManager.Backend.GameData.PlayerGameData.UpdatePlayerUpgradeLevel(nowPlayerType, nextLevel);

            PlayersControlManager.instance.playersContol[(int)nowPlayerType].utility.modelSetter.ChangeModel((PlayerChangeTag)nowIndex);

            _uiPlayerModel.ChangeModelSetter(nowPlayerType, nowIndex);
            SetUpgradeDetailUI();
        }
    }

    void SetToggleColor(UI_ToggleColor [] toggleColors, bool isOn)
    {
        for(int i=0; i < toggleColors.Length; ++i)
        {
            toggleColors[i].ToggleSet(isOn);      
        }
    }

    void OnNextGradeButton()
    {
        if (nowIndex == 3)
            return;

        _uiPlayerModel.ChangeModelSetter(nowPlayerType, nowIndex += 1);
        SetUpgradeDetailUI();
    }

    void OnPrevGradeButton()
    {
        if (nowIndex == 0)
            return;

        _uiPlayerModel.ChangeModelSetter(nowPlayerType, nowIndex -= 1);
        SetUpgradeDetailUI();
    }

    void SetUpgradeDetailUI()
    {
        if (CheckAvailableUpgradeNowLevel())
            _upgradeButton.gameObject.SetActive(true);
        else
            _upgradeButton.gameObject.SetActive(false);

        bool isNowActive = false;

        switch(nowPlayerType)
        {
            case PlayerType.Warrior:
                if (nowIndex + 1 == StaticManager.Backend.GameData.PlayerGameData.WarriorUpgradeLevel)
                    isNowActive = true;
                break;
            case PlayerType.Archer:
                if (nowIndex + 1 == StaticManager.Backend.GameData.PlayerGameData.ArcherUpgradeLevel)
                    isNowActive = true;
                break;
            case PlayerType.Wizard:
                if (nowIndex + 1 == StaticManager.Backend.GameData.PlayerGameData.WizardUpgradeLevel)
                    isNowActive = true;
                break;
        }

        if (isNowActive)
            _upgradeLevelText.text = $"{nowIndex + 1} 단계 (적용중)";
        else
            _upgradeLevelText.text = $"{nowIndex + 1} 단계";

        BackendData.Chart.Upgrade.Item item = StaticManager.Backend.Chart.Upgrade.GetUpgradeItem(nowPlayerType, nowIndex + 1);


      //  for(int i=0; i < item.UpgradeStats.Count; ++i)
      //  {
      //      string name = $"{Define.StatStatTitle[item.UpgradeStats[i].StatType]} +{item.UpgradeStats[i].AbilValue * 100}%";            
      //  }

        _upgradeStatValue01.text = $"{Define.StatStatTitle[item.UpgradeStats[0].StatType]} {item.UpgradeStats[0].AbilValue * 100}% 증가";
        _upgradeStatValue02.text = $"{Define.StatStatTitle[item.UpgradeStats[1].StatType]} {item.UpgradeStats[1].AbilValue * 100}% 증가";

        _upgradeNeedCount.text = $"x {item.NeedCount}";
    }


}

