using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_TreasureDetail : MonoBehaviour
{
    [SerializeField]
    GameObject _treasureItemObj;

    [SerializeField]
    Transform _createdItemParents;

    [SerializeField]
    Button _spawnButton; 

    List<UI_TreasureItem> _createdTrainingItems = new List<UI_TreasureItem>();

    public void Start()
    {
        CreateItems();

        _spawnButton.onClick.AddListener(SpawnTreasure);
    }

    private void OnDestroy()
    {
        _spawnButton.onClick.RemoveListener(SpawnTreasure);
    }

    public void CreateItems()
    {
        foreach (var item in StaticManager.Backend.Chart.Treasure.Dictionary.Values)
        {
            GameObject uI_TrainingItemObj = GameObject.Instantiate(_treasureItemObj, _createdItemParents);
            UI_TreasureItem uI_TrainingItem = uI_TrainingItemObj.GetComponent<UI_TreasureItem>();
            uI_TrainingItem.transform.localScale = Vector3.one;
            uI_TrainingItem.SetUI(item.ItemID);
            _createdTrainingItems.Add(uI_TrainingItem);
        }
    }
    
    private void SpawnTreasure()
    {
        if(StaticManager.Backend.GameData.PlayerGameData.DRuby < 0)
        {
            return;
        }

      //  StaticManager.Backend.GameData.PlayerGameData.UpdateUserData(StaticManager.Backend.GameData.PlayerGameData.DRuby - 0);
        
        List<int> resultID = StaticManager.Random.GetTreasureRandomList(10);

       // TODO 
        UIManager_SpawnResult uI_SpawnResultPopup = StageManager.instance.canvasManager.GetUIManager<UIManager_SpawnResult>();
        uI_SpawnResultPopup.gameObject.SetActive(true);
        uI_SpawnResultPopup.SpawnTreasureRandomItem(resultID);

        RefreshUI();
    }

    public void RefreshUI()
    {
        for (int i = 0; i < _createdTrainingItems.Count; ++i)
            _createdTrainingItems[i].RefreshUI();

    }
}
