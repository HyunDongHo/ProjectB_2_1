using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager_Common : UIManagers
{
    //public GameObject bossButton;
    //public QuestWindow questWindow;
    //public StageWindow stageWindow;
    //public ShopView shopWindow;

    [SerializeField]
    Button _spawnButton;
    [SerializeField]
    Button _menuButton;
    [SerializeField]
    GameObject subMenuObj;
    [SerializeField]
    GameObject commonMenuObj;  
    //public PlayerSelectView playerSelectWindow;  
    [SerializeField]
    TextMeshProUGUI _myCoin;

    [SerializeField]
    TextMeshProUGUI _myDiamondCoin;

    Canvas commonCanvas;
    InGameScene.UI.InGameUI_Quest questItem;
    StageWindow stageWindow;
    private void Awake()
    {
        if (_spawnButton != null)
            _spawnButton.onClick.AddListener(() => StageManager.instance.canvasManager.GetUIManager<UIManager_Spawn>().ShowUI());

        if(_menuButton != null)
            _menuButton.onClick.AddListener(() => MenuOpenOrClose());

        if (subMenuObj != null)
            subMenuObj.SetActive(false);    

        RefreshCommonUI();

        commonCanvas = transform.GetComponent<Canvas>();
        questItem = StageManager.instance.canvasManager.GetUIManager<UIManager_Stage>().gameObject.GetComponent<InGameScene.UI.InGameUI_Quest>();
        stageWindow = StageManager.instance.canvasManager.GetUIManager<UIManager_Stage>().gameObject.GetComponent<StageWindow>();
    }

    private void OnDestroy()
    {
        _spawnButton.onClick.RemoveAllListeners();
    }

    //private void Update()
    //{
    //    _myCoin.text = $"{Math.Truncate(StaticManager.Backend.GameData.PlayerGameData.DCoin)}";  

    //}
    public void PushPSaveButton()
    {
        if (GameObject.Find("PopupPSave") != null)
            return;

         Popup<PopupPSave> popupPSave = PopupManager.instance.CreatePopup<PopupPSave>("PopupPSave");  
        DontDestroyOnLoad(popupPSave);      
    }
    public void RefreshCommonUI()
    {
        if (_myCoin == null || _myDiamondCoin == null)
            return;
        _myCoin.text = $"{Math.Truncate(StaticManager.Backend.GameData.PlayerGameData.DCoin)}";
        _myDiamondCoin.text = $"{Math.Truncate(StaticManager.Backend.GameData.PlayerGameData.DDiamondCoin)}";
    }

    void MenuOpenOrClose() // 메뉴 끄고 키고 
    {
        if(subMenuObj.activeSelf == true) // 켜진 상태라먄 
        {
            subMenuObj.SetActive(false); // 꺼주기
            commonCanvas.sortingOrder = 100;
            questItem.SetQuestItemActive(true);
            stageWindow.SetMonsterCountActive(true);
        }
        else
        {
            subMenuObj.SetActive(true);
            commonCanvas.sortingOrder = 190;
            questItem.SetQuestItemActive(false);
            stageWindow.SetMonsterCountActive(false);
        }
    }
    public void CloseAllMenu()
    {
        commonMenuObj.SetActive(false);
        questItem.SetQuestItemActive(false);
        stageWindow.SetMonsterCountActive(false);
        stageWindow.SetStageMenuActive(false);
    }
    public void OpenMainMenu()
    {
        commonMenuObj.SetActive(true);
        subMenuObj.SetActive(false); // 꺼주기
        commonCanvas.sortingOrder = 100;
        questItem.SetQuestItemActive(true);
        stageWindow.SetMonsterCountActive(true);
        stageWindow.SetStageMenuActive(true);  

    }
}
