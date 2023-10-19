using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerUI : UI_Scene
{
    // Start is called before the first frame update
    [Header("[ Player Button ]")]
    [SerializeField]
    Button warriorButton;
    [SerializeField]
    Button archerButton;
    [SerializeField]
    Button wizardButton;
    [Space(15f)]

    [Header("[ Player Icon ]")]
    [SerializeField]
    Image warriorIcon;
    [SerializeField]
    Image archerIcon;
    [SerializeField]
    Image wizardIcon;
    [Space(15f)]

    [Header("[ Player Text ]")]
    [SerializeField]
    TextMeshProUGUI warriorText, warriorHpText;
    [SerializeField]
    TextMeshProUGUI archerText, archerHpText;
    [SerializeField]
    TextMeshProUGUI wizardText, wizardHpText;

    [Header("[ Player Icon ]")]
    [SerializeField]
    Image WarriorHp;
    [SerializeField]
    Image ArcherHp;
    [SerializeField]
    Image WizardHp;

    [Space(15f)]

    [Header("[ MonsterKill ]")]
    [SerializeField]
    UI_MonsterKillText WarriorKillText;
    [SerializeField]
    UI_MonsterKillText ArcherKillText;
    [SerializeField]
    UI_MonsterKillText WizardKillText;

    [Space(15f)]

    public bool isWarriorDead;
    public bool isArcherDead;
    public bool isWizardDead;
    void Awake()
    {
        AddEvent();
    }

    private void OnDestroy()
    {
        RemoveEvent();
    }
    //private void Start()
    //{
    //    //warriorText.text = PlayersControlManager.instance.playersContol[0].GetStats<PlayerStats>().level.GetCurrentLevel().ToString();
    //    //archerText.text = PlayersControlManager.instance.playersContol[1].GetStats<PlayerStats>().level.GetCurrentLevel().ToString();
    //    //wizardText.text = PlayersControlManager.instance.playersContol[2].GetStats<PlayerStats>().level.GetCurrentLevel().ToString();

    //    warriorText.text = StaticManager.Backend.GameData.PlayerGameData.DWarriorLevel.ToString();
    //    archerText.text = StaticManager.Backend.GameData.PlayerGameData.DArcherLevel.ToString();
    //    wizardText.text = StaticManager.Backend.GameData.PlayerGameData.DWizardLevel.ToString();
    //}

    enum GameObjects
    {

    }

    public override void init()
    {
        Bind<GameObject>(typeof(GameObjects)); // Binding   

        warriorText.text = StaticManager.Backend.GameData.PlayerGameData.DWarriorLevel.ToString();
        archerText.text = StaticManager.Backend.GameData.PlayerGameData.DArcherLevel.ToString();
        wizardText.text = StaticManager.Backend.GameData.PlayerGameData.DWizardLevel.ToString();

        warriorButton.gameObject.AddUIEvent(HandleWarriorButton);
        archerButton.gameObject.AddUIEvent(HandleArcherButton);
        wizardButton.gameObject.AddUIEvent(HandleWizardButton);

        switch ((int)StaticManager.Backend.GameData.PlayerGameData.PlayerType) // 카메라 떄문에 사용 
        {
            case (int)PlayerType.Warrior:
                WarriorBtn();
                break;
            case (int)PlayerType.Archer:
                ArcherBtn();
                break;
            case (int)PlayerType.Wizard:
                WizardBtn();     
                break;
        }
    }

    void AddEvent()
    {
        //warriorButton.onClick.AddListener(HandleWarriorButton);
        //archerButton.onClick.AddListener(HandleArcherButton);
        //wizardButton.onClick.AddListener(HandleWizardButton);

        PlayersControlManager.instance.playersContol[0].GetStats<PlayerStats>().level.OnLevelUp += WarriorLevelUpText;
        PlayersControlManager.instance.playersContol[1].GetStats<PlayerStats>().level.OnLevelUp += ArcherLevelUpText;  
        PlayersControlManager.instance.playersContol[2].GetStats<PlayerStats>().level.OnLevelUp += WizardLevelUpText;

        PlayersControlManager.instance.playersContol[0].GetStats<Stats>().hp.OnHpInit += HandleHpSetWarrior;
        PlayersControlManager.instance.playersContol[1].GetStats<Stats>().hp.OnHpInit += HandleHpSetArcher;
        PlayersControlManager.instance.playersContol[2].GetStats<Stats>().hp.OnHpInit += HandleHpSetWizard;

        PlayersControlManager.instance.playersContol[0].GetStats<Stats>().hp.OnHpSet += HandleHpSetWarrior;
        PlayersControlManager.instance.playersContol[1].GetStats<Stats>().hp.OnHpSet += HandleHpSetArcher;
        PlayersControlManager.instance.playersContol[2].GetStats<Stats>().hp.OnHpSet += HandleHpSetWizard;

        PlayersControlManager.instance.playersContol[0].GetStats<Stats>().hp.OnHpAdd += HandleHpSetWarrior;
        PlayersControlManager.instance.playersContol[1].GetStats<Stats>().hp.OnHpAdd += HandleHpSetArcher;
        PlayersControlManager.instance.playersContol[2].GetStats<Stats>().hp.OnHpAdd += HandleHpSetWizard;

        PlayersControlManager.instance.playersContol[0].GetStats<Stats>().hp.OnHpReduce += HandleWarriorHpUpdate;
        PlayersControlManager.instance.playersContol[1].GetStats<Stats>().hp.OnHpReduce += HandleArcherHpUpdate;
        PlayersControlManager.instance.playersContol[2].GetStats<Stats>().hp.OnHpReduce += HandleWizardHpUpdate;

        PlayersControlManager.instance.playersContol[0].OnHpExhausted += HandleOnPlayerDie;
        PlayersControlManager.instance.playersContol[1].OnHpExhausted += HandleOnPlayerDie;
        PlayersControlManager.instance.playersContol[2].OnHpExhausted += HandleOnPlayerDie;
    }

    void RemoveEvent()
    {
        //warriorButton.onClick.RemoveListener(HandleWarriorButton);
        //archerButton.onClick.RemoveListener(HandleArcherButton);
        //wizardButton.onClick.RemoveListener(HandleWizardButton);

        if(PlayersControlManager.instance.playersContol != null && PlayersControlManager.instance.playersContol.Length > 0)
        {
            PlayersControlManager.instance.playersContol[0].GetStats<PlayerStats>().level.OnLevelUp -= WarriorLevelUpText;
            PlayersControlManager.instance.playersContol[1].GetStats<PlayerStats>().level.OnLevelUp -= ArcherLevelUpText;
            PlayersControlManager.instance.playersContol[2].GetStats<PlayerStats>().level.OnLevelUp -= WizardLevelUpText;

            PlayersControlManager.instance.playersContol[0].OnHpExhausted -= HandleOnPlayerDie;
            PlayersControlManager.instance.playersContol[1].OnHpExhausted -= HandleOnPlayerDie;
            PlayersControlManager.instance.playersContol[2].OnHpExhausted -= HandleOnPlayerDie;

            PlayersControlManager.instance.playersContol[0].GetStats<Stats>().hp.OnHpInit -= HandleHpSetWarrior;
            PlayersControlManager.instance.playersContol[1].GetStats<Stats>().hp.OnHpInit -= HandleHpSetArcher;
            PlayersControlManager.instance.playersContol[2].GetStats<Stats>().hp.OnHpInit -= HandleHpSetWizard;

            PlayersControlManager.instance.playersContol[0].GetStats<Stats>().hp.OnHpSet -= HandleHpSetWarrior;
            PlayersControlManager.instance.playersContol[1].GetStats<Stats>().hp.OnHpSet -= HandleHpSetArcher;
            PlayersControlManager.instance.playersContol[2].GetStats<Stats>().hp.OnHpSet -= HandleHpSetWizard;

            PlayersControlManager.instance.playersContol[0].GetStats<Stats>().hp.OnHpAdd -= HandleHpSetWarrior;
            PlayersControlManager.instance.playersContol[1].GetStats<Stats>().hp.OnHpAdd -= HandleHpSetArcher;
            PlayersControlManager.instance.playersContol[2].GetStats<Stats>().hp.OnHpAdd -= HandleHpSetWizard;

            PlayersControlManager.instance.playersContol[0].GetStats<Stats>().hp.OnHpReduce -= HandleWarriorHpUpdate;
            PlayersControlManager.instance.playersContol[1].GetStats<Stats>().hp.OnHpReduce -= HandleArcherHpUpdate;
            PlayersControlManager.instance.playersContol[2].GetStats<Stats>().hp.OnHpReduce -= HandleWizardHpUpdate;
        }
    }

    public void StartMonsterKillAnim(string playerName)
    {
        PlayerType nowPlayerType = PlayersControlManager.instance.nowActive;

        if(playerName.Contains("Warrior") && nowPlayerType != PlayerType.Warrior)
        {
            WarriorKillText.PlayAnim(MonsterKillTextState.KILL);
        }
        else if(playerName.Contains("Archer") && nowPlayerType != PlayerType.Archer)
        {
            ArcherKillText.PlayAnim(MonsterKillTextState.KILL);
        }
        else if(playerName.Contains("Wizard") && nowPlayerType != PlayerType.Wizard)
        {
            WizardKillText.PlayAnim(MonsterKillTextState.KILL);
        }
    }

    private void HandleHpSetWarrior(float hp)
    {
        //PlayersControlManager.instance.playersContol[0].GetStats<PlayerStats>().hp.

        HpSetPlayer(PlayerType.Warrior, hp);
        StaticManager.Backend.GameData.PlayerGameData.UpdateUserData_Hp(0, hp);
        WarriorHp.fillAmount = hp / PlayersControlManager.instance.playersContol[0].GetStats<PlayerStats>().hp.MaxHp;
        //Debug.Log($"warrior Hp : {StaticManager.Backend.GameData.PlayerGameData.DWarriorHp}");
    }

    private void HandleHpSetArcher(float hp)
    {
        HpSetPlayer(PlayerType.Archer, hp);
        StaticManager.Backend.GameData.PlayerGameData.UpdateUserData_Hp(1, hp);
        ArcherHp.fillAmount = hp / PlayersControlManager.instance.playersContol[1].GetStats<PlayerStats>().hp.MaxHp;
    }

    private void HandleHpSetWizard(float hp)
    {
        HpSetPlayer(PlayerType.Wizard, hp);
        StaticManager.Backend.GameData.PlayerGameData.UpdateUserData_Hp(2, hp);
        WizardHp.fillAmount = hp / PlayersControlManager.instance.playersContol[2].GetStats<PlayerStats>().hp.MaxHp;
    }

    private void HpSetPlayer(PlayerType playerType, float hp)
    {
        switch(playerType)
        {
            case PlayerType.Warrior:
                warriorHpText.text = hp.ToNumberStringCount();
                break;
            case PlayerType.Archer:
                archerHpText.text = hp.ToNumberStringCount();
                break;
            case PlayerType.Wizard:
                wizardHpText.text = hp.ToNumberStringCount();
                break;
        }        
    }

    private void HandleWarriorHpUpdate(float hp)
    {
        HandleHpUpdate(PlayerType.Warrior, hp);
        StaticManager.Backend.GameData.PlayerGameData.UpdateUserData_Hp(0, hp);
        //Debug.Log($"warrior Hp : {StaticManager.Backend.GameData.PlayerGameData.DWarriorHp}");

        WarriorHp.fillAmount = hp / PlayersControlManager.instance.playersContol[0].GetStats<PlayerStats>().hp.MaxHp;
    }

    private void HandleArcherHpUpdate(float hp)
    {
        HandleHpUpdate(PlayerType.Archer, hp);
        StaticManager.Backend.GameData.PlayerGameData.UpdateUserData_Hp(1, hp);

        ArcherHp.fillAmount = hp / PlayersControlManager.instance.playersContol[1].GetStats<PlayerStats>().hp.MaxHp;
    }

    private void HandleWizardHpUpdate(float hp)
    {
        HandleHpUpdate(PlayerType.Wizard, hp);
        StaticManager.Backend.GameData.PlayerGameData.UpdateUserData_Hp(2, hp);
        WizardHp.fillAmount = hp / PlayersControlManager.instance.playersContol[2].GetStats<PlayerStats>().hp.MaxHp;
    }

    private void HandleHpUpdate(PlayerType playerType, float hp)
    {
        switch (playerType)
        {
            case PlayerType.Warrior:
                warriorHpText.text = hp.ToNumberStringCount();
                break;
            case PlayerType.Archer:
                archerHpText.text = hp.ToNumberStringCount();
                break;
            case PlayerType.Wizard:
                wizardHpText.text = hp.ToNumberStringCount();
                break;
        }
    }

    private void HandleOnPlayerDie(PlayerControl playerControl)
    {
        if ((StageManager.instance as GamePlayManager).enemyManager.GetStageType() == StageTypeNum.TrainingDungeon)
        {
            // 재시작 
            //StageManager.instance.canvasManager.GetUIManager<UI_DungeonIngame>().ExitTrainingDungeon();    
            return;
        }


        switch ((int)playerControl.GetAttack<PlayerAttack>().playerModelType)  
        {
            case (int)PlayerType.Warrior:
                isWarriorDead = true;  
                break;
            case (int)PlayerType.Archer:
                isArcherDead = true;
                break;
            case (int)PlayerType.Wizard:
                isWizardDead = true;
                break;

        }

        if(isWarriorDead ==true && isArcherDead == true && isWizardDead == true)
        {
            ReStartScene();
        }
        else
        {
            PlayersControlManager.instance.ChangePlayerForDie();

            switch ((int)StaticManager.Backend.GameData.PlayerGameData.PlayerType) // 카메라 떄문에 사용 
            {
                case (int)PlayerType.Warrior:
                    WarriorBtn();
                    break;
                case (int)PlayerType.Archer:
                    ArcherBtn();
                    break;
                case (int)PlayerType.Wizard:
                    WizardBtn();
                    break;
            }
        }
    }
    void ReStartScene()
    {
        Debug.Log("Scene restart");
        SceneSettingManager.instance.LoadReStartStageScene();

        //PlayersControlManager.instance.ResetHpAllPlayer();
        //PlayersControlManager.instance.ResetAllPlayerState();
        //PlayersControlManager.instance.SetNotActiveAllPlayer();
        isWarriorDead = false;
        isArcherDead = false;
        isWizardDead = false;

    }
    void HandleWarriorButton(PointerEventData data)
    {
        StaticManager.Backend.GameData.PlayerGameData.ChangeNowPlayerType(PlayerType.Warrior);
        WarriorBtn();
    }
    void WarriorBtn()
    {
        //(StageManager.instance as GamePlayManager).playersControl.SetActivePlayer(PlayerType.Warrior);
        PlayersControlManager.instance.SetActivePlayer(PlayerType.Warrior);
        OutsideStageManager.instance.playerControl = PlayersControlManager.instance.GetNowActivePlayer();
        warriorButton.transform.SetAsFirstSibling();

        warriorButton.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(130, 130);
        archerButton.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 100);
        wizardButton.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 100);

        warriorIcon.transform.parent.GetComponent<RectTransform>().sizeDelta = new Vector2(110, 110);
        archerIcon.transform.parent.GetComponent<RectTransform>().sizeDelta = new Vector2(80, 80);
        wizardIcon.transform.parent.GetComponent<RectTransform>().sizeDelta = new Vector2(80, 80);

        warriorIcon.transform.parent.GetComponent<RectTransform>().anchoredPosition = new Vector2(65, 0);
        archerIcon.transform.parent.GetComponent<RectTransform>().anchoredPosition = new Vector2(50, 0);
        wizardIcon.transform.parent.GetComponent<RectTransform>().anchoredPosition = new Vector2(50, 0);

        warriorIcon.color = new Color(1f, 1f, 1f, 1f);
        archerIcon.color = new Color(0.5f, 0.5f, 0.5f, 1f);
        wizardIcon.color = new Color(0.5f, 0.5f, 0.5f, 1f);

        warriorText.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(-65, 8);
        archerText.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(-54, 6);
        wizardText.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(-54, 6);

        warriorText.color = new Color(1f, 1f, 1f, 1f);
        archerText.color = new Color(0.5f, 0.5f, 0.5f, 1f);
        wizardText.color = new Color(0.5f, 0.5f, 0.5f, 1f);
    }
    void HandleArcherButton(PointerEventData data)
    {
        StaticManager.Backend.GameData.PlayerGameData.ChangeNowPlayerType(PlayerType.Archer);
        ArcherBtn();

    }
    void ArcherBtn()
    {
        //(StageManager.instance as GamePlayManager).playersControl.SetActivePlayer(PlayerType.Archer);
        PlayersControlManager.instance.SetActivePlayer(PlayerType.Archer);
        OutsideStageManager.instance.playerControl = PlayersControlManager.instance.GetNowActivePlayer();
        archerButton.transform.SetAsFirstSibling();

        archerButton.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(130, 130);
        wizardButton.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 100);
        warriorButton.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 100);

        archerIcon.transform.parent.GetComponent<RectTransform>().sizeDelta = new Vector2(110, 110);
        warriorIcon.transform.parent.GetComponent<RectTransform>().sizeDelta = new Vector2(80, 80);
        wizardIcon.transform.parent.GetComponent<RectTransform>().sizeDelta = new Vector2(80, 80);

        archerIcon.transform.parent.GetComponent<RectTransform>().anchoredPosition = new Vector2(65, 0);
        warriorIcon.transform.parent.GetComponent<RectTransform>().anchoredPosition = new Vector2(50, 0);
        wizardIcon.transform.parent.GetComponent<RectTransform>().anchoredPosition = new Vector2(50, 0);

        archerIcon.color = new Color(1f, 1f, 1f, 1f);
        warriorIcon.color = new Color(0.5f, 0.5f, 0.5f, 1f);
        wizardIcon.color = new Color(0.5f, 0.5f, 0.5f, 1f);

        archerText.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(-65, 8);
        warriorText.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(-54, 6);
        wizardText.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(-54, 6);

        archerText.color = new Color(1f, 1f, 1f, 1f);
        warriorText.color = new Color(0.5f, 0.5f, 0.5f, 1f);
        wizardText.color = new Color(0.5f, 0.5f, 0.5f, 1f);
    }

    void HandleWizardButton(PointerEventData data)
    {
        StaticManager.Backend.GameData.PlayerGameData.ChangeNowPlayerType(PlayerType.Wizard);
        WizardBtn(); 
    }
    void WizardBtn()
    {        //(StageManager.instance as GamePlayManager).playersControl.SetActivePlayer(PlayerType.Wizard);
        PlayersControlManager.instance.SetActivePlayer(PlayerType.Wizard);
        OutsideStageManager.instance.playerControl = PlayersControlManager.instance.GetNowActivePlayer();
        wizardButton.transform.SetAsFirstSibling();

        wizardButton.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(130, 130);
        archerButton.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 100);
        warriorButton.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 100);

        wizardIcon.transform.parent.GetComponent<RectTransform>().sizeDelta = new Vector2(110, 110);
        warriorIcon.transform.parent.GetComponent<RectTransform>().sizeDelta = new Vector2(80, 80);
        archerIcon.transform.parent.GetComponent<RectTransform>().sizeDelta = new Vector2(80, 80);

        wizardIcon.transform.parent.GetComponent<RectTransform>().anchoredPosition = new Vector2(65, 0);
        warriorIcon.transform.parent.GetComponent<RectTransform>().anchoredPosition = new Vector2(50, 0);
        archerIcon.transform.parent.GetComponent<RectTransform>().anchoredPosition = new Vector2(50, 0);

        wizardIcon.color = new Color(1f, 1f, 1f, 1f);
        archerIcon.color = new Color(0.5f, 0.5f, 0.5f, 1f);
        warriorIcon.color = new Color(0.5f, 0.5f, 0.5f, 1f);

        wizardText.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(-65, 8);
        warriorText.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(-54, 6);
        archerText.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(-54, 6);

        wizardText.color = new Color(1f, 1f, 1f, 1f);
        warriorText.color = new Color(0.5f, 0.5f, 0.5f, 1f);
        archerText.color = new Color(0.5f, 0.5f, 0.5f, 1f);

    }
    public void WarriorLevelUpText(int warriorLevel)
    {
        warriorText.text = $"{warriorLevel}";
        StaticManager.Backend.GameData.PlayerGameData.UpdatePlayerLevel(PlayerType.Warrior);
        if(QuestsManager.instance.questType == BackendData.Chart.Quest.QuestType.LevelUp)
            QuestsManager.instance.UpdateQuestUI();

        StageManager.instance.canvasManager.GetUIManager<UI_PassPopup>().SetNowPassText(BackendData.Chart.PassInfo.PassType.Level);  
        StageManager.instance.canvasManager.GetUIManager<UI_PassPopup>().SetPassItemLockState(BackendData.Chart.PassInfo.PassType.Level);  
        //PlayerStats stats = PlayersControlManager.instance.playersContol[0].GetStats<PlayerStats>();
        //stats.UpdateLevelData(warriorLevel);
    }
    public void ArcherLevelUpText(int archerLevel)
    {
        archerText.text = $"{archerLevel}";
        StaticManager.Backend.GameData.PlayerGameData.UpdatePlayerLevel(PlayerType.Archer);

        //PlayerStats stats = PlayersControlManager.instance.playersContol[1].GetStats<PlayerStats>();
        //stats.UpdateLevelData(archerLevel);
    }
    public void WizardLevelUpText(int wizardLevel)
    {
        wizardText.text = $"{wizardLevel}";
        StaticManager.Backend.GameData.PlayerGameData.UpdatePlayerLevel(PlayerType.Wizard);

        //PlayerStats stats = PlayersControlManager.instance.playersContol[2].GetStats<PlayerStats>();  
        //stats.UpdateLevelData(wizardLevel);
    }
}
