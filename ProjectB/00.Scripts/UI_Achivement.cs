using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public enum StatItemType
{
    Attack,
    Hp,
    CriticalPercent,
    CriticalRatio,
    ExpRatio,
    GoldRatio,
    ItemRatio,
    None
}
public class UI_Achivement : UI_Scene
{

    [SerializeField]
    GameObject ui_achivement_item;

    [SerializeField]
    GameObject StatGridPanel_temp;


    [SerializeField]
    Image StatClickImage;
    [SerializeField]
    Image TreasureClickImage;
    [SerializeField]
    Image AbilClickImage;

    [SerializeField]
    Image StatPanel;
    [SerializeField]
    Image TreasurePanel;
    [SerializeField]
    Image AbilPanel;
    [SerializeField]
    UI_UpgradeDetail UpgradePanel;

    private bool _firstInitDone = false;

    enum GameObjects
    {
        StatTitleText,
        TreasureTitleText,
        AbilTitleText,
        StatGridPanel,
        TreasureGridPanel,
        UpgradeTitleText
    }

    private void OnEnable()
    {
        if (_firstInitDone == true)
        {
            int i = 0;
            Dictionary<int, BackendData.Chart.PlayerEnhancemetData.Item> EnHancementChartData = null;
            EnHancementChartData = StaticManager.Backend.Chart.PlayerEnhancemetData.GetPlayerGoldStatItem();

            foreach (var chartItem in EnHancementChartData)
            {
                GameObject item = Get<GameObject>((int)GameObjects.StatGridPanel).transform.GetChild(i).gameObject;
                UI_Achievement_item itemComponent = item.transform.GetComponent<UI_Achievement_item>();
                PlayerStatAchivement(item, chartItem.Value);
                itemComponent.SetAllText();
                itemComponent.SetAllColor();
                i++;
            }
        }
    }
    public override void init()
    {
        //base.init(); // canvas 붙여줌 
        if (StatGridPanel_temp.activeSelf == false)
            StatGridPanel_temp.SetActive(true);
        Bind<GameObject>(typeof(GameObjects)); // Binding          

        StatPanel.gameObject.SetActive(true);
        StatClickImage.gameObject.SetActive(true);
        TreasurePanel.gameObject.SetActive(false);
        AbilPanel.gameObject.SetActive(false);
        AbilClickImage.gameObject.SetActive(false);

        Get<GameObject>((int)GameObjects.StatTitleText).gameObject.AddUIEvent(HandleStatTitleButton);
        Get<GameObject>((int)GameObjects.TreasureTitleText).gameObject.AddUIEvent(HandleTreasureTitleButton);
        Get<GameObject>((int)GameObjects.AbilTitleText).gameObject.AddUIEvent(HandleAbilTitleButton);
        Get<GameObject>((int)GameObjects.UpgradeTitleText).gameObject.AddUIEvent(HandleUpgradeTitleButton);


        GameObject statGridPanel = Get<GameObject>((int)GameObjects.StatGridPanel);
        foreach (Transform child in statGridPanel.transform)
            GameObject.Destroy(child.gameObject);


        int i = 0;
        Dictionary<int, BackendData.Chart.PlayerEnhancemetData.Item> EnHancementChartData = null;
        EnHancementChartData = StaticManager.Backend.Chart.PlayerEnhancemetData.GetPlayerGoldStatItem(); 

        foreach (var chartItem in EnHancementChartData)
        {
            GameObject item = Instantiate<GameObject>(ui_achivement_item);
            UI_Achievement_item itemComponent = item.transform.GetComponent<UI_Achievement_item>();
            item.transform.SetParent(statGridPanel.transform); // 부모 지정         
            PlayerStatAchivement(item, chartItem.Value);
            itemComponent.SetAllText();
            itemComponent.SetAllColor();
            i++;
        }
        _firstInitDone = true;
    }

    void HandleStatTitleButton(PointerEventData data)
    {
        Debug.Log("HandleStatTitleButton");
        StatClickImage.gameObject.SetActive(true);
        StatPanel.gameObject.SetActive(true);
        TreasureClickImage.gameObject.SetActive(false);
        TreasurePanel.gameObject.SetActive(false);
        AbilClickImage.gameObject.SetActive(false);
        AbilPanel.gameObject.SetActive(false);
        UpgradePanel.gameObject.SetActive(false);

    }

    public void PlayerStatAchivement(GameObject item, BackendData.Chart.PlayerEnhancemetData.Item chartItem)
    {
        if (ui_achivement_item == null)
            return;

        item.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
        UI_Achievement_item itemComponent = item.transform.GetComponent<UI_Achievement_item>();
        /* chart setting */
        itemComponent.SetChartItemID(chartItem.ItemID);
        itemComponent.SetChartTitleNameText(chartItem.StatUIName);
        itemComponent.SetChartMaxLevel(chartItem.MaxLevel);
        itemComponent.SetChartGoldNeedCountText(chartItem.NeedCount);
        itemComponent.SetChartNeedCountRatio(chartItem.NeedCountRatio);
        itemComponent.SetChartIncreasedAmount(chartItem.IncreasedAmount);

        //    /* User data Setting */
        BackendData.GameData.StatData statData = null;
        statData = StaticManager.Backend.GameData.PlayerGameData.StatList.Find(item => item.ItemID == chartItem.ItemID);
        if (statData == null)
        {
            itemComponent.SetLevelText(0);
        }
        else
        {
            itemComponent.SetLevelText(statData.ItemLevel);
        }
        item.gameObject.SetActive(true);
    }

    void HandleTreasureTitleButton(PointerEventData data)
    {
        Debug.Log("HandleTreasureTitleButton");
        StatClickImage.gameObject.SetActive(false);
        StatPanel.gameObject.SetActive(false);
        TreasureClickImage.gameObject.SetActive(true);
        TreasurePanel.gameObject.SetActive(true);
        AbilClickImage.gameObject.SetActive(false);
        AbilPanel.gameObject.SetActive(false);
        UpgradePanel.gameObject.SetActive(false);
    }

    void HandleAbilTitleButton(PointerEventData data)
    {
        Debug.Log("HandleTreasureTitleButton");
        StatClickImage.gameObject.SetActive(false);
        StatPanel.gameObject.SetActive(false);
        TreasureClickImage.gameObject.SetActive(false);
        TreasurePanel.gameObject.SetActive(false);
        AbilClickImage.gameObject.SetActive(true);
        AbilPanel.gameObject.SetActive(true);
        UpgradePanel.gameObject.SetActive(false);
    }
    void HandleUpgradeTitleButton(PointerEventData data)
    {
        Debug.Log("HandleTreasureTitleButton");
        StatClickImage.gameObject.SetActive(false);
        StatPanel.gameObject.SetActive(false);
        TreasureClickImage.gameObject.SetActive(false);
        TreasurePanel.gameObject.SetActive(false);
        AbilClickImage.gameObject.SetActive(false);
        AbilPanel.gameObject.SetActive(false);
        UpgradePanel.gameObject.SetActive(true);

        UpgradePanel.ClickWarrior();
    }

    
}
