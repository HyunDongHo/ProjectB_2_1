using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_DiceDetail : UI_Base
{
    [SerializeField]
    UI_DiceOptionPopup _diceOptionPopup;

    [SerializeField]
    Button _spawnButton, _autoButton, _stopAutoSpawn;

   [SerializeField]
   List<UI_DiceItem> _diceItemList = new List<UI_DiceItem>();

    [SerializeField]
    List<Button> _slotButtons = new List<Button>();

    private int _nowIndex = 0;

    [SerializeField]
    List<ToggleSelectImage> _toggleImageList = new List<ToggleSelectImage>();
  
    [SerializeField]
    List<ToggleSelectText> _toggleTextList = new List<ToggleSelectText>();

    [SerializeField]
    GameObject _spawnButtonPanel, autoSpawnPanel;

    //[SerializeField]
    //TextMeshProUGUI _DiceGemText, _NeedCountText;

    bool _isAutoSpawnStopClick = false;

    public override void init()
    {
        _spawnButton.onClick.AddListener(SpawnDiceOnce);
       
        _autoButton.onClick.AddListener(() => _diceOptionPopup.gameObject.SetActive(true));

        _stopAutoSpawn.onClick.AddListener(() => _isAutoSpawnStopClick = true);

        SetSlotItemAction();
        SetNowIndex(StaticManager.Backend.GameData.PlayerDice.NowSlotNum);
        autoSpawnPanel.SetActive(false);
    }
    //private void Update()
    //{
    //    _DiceGemText.text = $"{Math.Truncate(StaticManager.Backend.GameData.PlayerDice.DiceGem)}";  
    //}

    private void OnDestroy()
    {
        _spawnButton.onClick.RemoveListener(SpawnDiceOnce);
        _autoButton.onClick.RemoveAllListeners();
        _stopAutoSpawn.onClick.RemoveAllListeners();
        RemoveSlotItemAction();
    }

    private void SetSlotItemAction()
    {
        for (int i = 0; i < _slotButtons.Count; ++i)
        {
            int index = i;
            _slotButtons[i].onClick.AddListener(() =>
            {
                SetNowIndex(index);
            });
        }
    }

    private void RemoveSlotItemAction()
    {
        for (int i = 0; i < _slotButtons.Count; ++i)
            _slotButtons[i].onClick.RemoveAllListeners();
    }

    private void SetNowIndex(int index)
    {
        _nowIndex = index;
        for (int i = 0; i < _toggleImageList.Count; ++i)
        {
            _toggleImageList[i].Toggle(_nowIndex == i);
            _toggleTextList[i].Toggle(_nowIndex == i);
        }

        StaticManager.Backend.GameData.PlayerDice.SetChangeSlotNum(_nowIndex);
        SetDiceItem();
        StaticManager.Backend.UpdateAllGameData((callback) => { });
    }

    public void SetDiceItem()
    {
        for (int i = 0; i < _diceItemList.Count; ++i)
        {
            _diceItemList[i].SetUI();
        }
    }

    private void SpawnDiceOnce()
    {
        //if(StaticManager.Backend.GameData.PlayerDice.DiceGem)
        double myCoin = StaticManager.Backend.GameData.PlayerDice.DiceGem;
        if (myCoin <= 0)
            return;

        if (myCoin >= 10)
        {
            StaticManager.Backend.GameData.PlayerDice.UpdateDiceGem(-10);
            StageManager.instance.canvasManager.GetUIManager<UIManager_Stat>().RefreshStatUI();

            autoSpawnPanel.SetActive(false);
            _spawnButtonPanel.SetActive(true);

            StaticManager.Random.GetDiceRandomList(_nowIndex, () => RefreshUI());
        }

    }

    public void SpawnDiceAuto()
    {
        StartCoroutine(CoSpawnDice());
    }

    IEnumerator CoSpawnDice()
    {
        yield return null;
        WaitForSeconds waitForSeconds = new WaitForSeconds(0.1f);

        autoSpawnPanel.SetActive(true);
        _spawnButtonPanel.SetActive(false);

        while (true)
        {
            bool isReTry = StaticManager.Random.GetDiceRandomList(_nowIndex, () => RefreshUI());
            
            if (isReTry == true && _isAutoSpawnStopClick == false)
            {
                yield return waitForSeconds;
            }
            else
            {
                _isAutoSpawnStopClick = false;
                autoSpawnPanel.SetActive(false);
                _spawnButtonPanel.SetActive(true);
                yield break;
            }
        }

    }

    public void RefreshUI()
    {
        SetDiceItem();
    }

}
