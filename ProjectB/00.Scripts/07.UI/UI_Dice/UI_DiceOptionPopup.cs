using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_DiceOptionPopup : MonoBehaviour
{
    [SerializeField]
    ToggleGroup _toggleGroup;

    [SerializeField]
    Toggle _toggleS, _toggleSS, _toggleSSS;

    [SerializeField]
    List<UI_DiceOptionItem> _diceOptionItems = new List<UI_DiceOptionItem>();

    [SerializeField]
    Button _closeButton, _resetButton, _startButton;

    private void Awake()
    {
        SetDiceOption();

        AddEvent();
    }

    private void OnDestroy()
    {
        RemoveEvent();
    }


    void AddEvent()
    {
        _closeButton.onClick.AddListener(ClosePopup);

        _toggleS.onValueChanged.AddListener(OnChangeToggleS);
        _toggleSS.onValueChanged.AddListener(OnChangeToggleSS);
        _toggleSSS.onValueChanged.AddListener(OnChangeToggleSSS);

        _resetButton.onClick.AddListener(ResetOption);
        _startButton.onClick.AddListener(AutoSpawn);
    }

    void RemoveEvent()
    {
        _closeButton.onClick.RemoveListener(ClosePopup);
        _resetButton.onClick.RemoveListener(ResetOption);
        _startButton.onClick.RemoveListener(AutoSpawn);
    }

    public void AutoSpawn()
    {
        UI_DiceDetail uI_DiceDetail = GameObject.FindObjectOfType<UI_DiceDetail>();

        if (uI_DiceDetail != null)
        {
            uI_DiceDetail.SpawnDiceAuto();
            ClosePopup();
        }
    }

    public void ClosePopup()
    {
        transform.gameObject.SetActive(false);
    }

    void SetDiceOption()
    {
        int savedStartOption = PlayerOptionManager.instance.GetDiceStartOption();

        if (savedStartOption == 5)
            _toggleS.isOn = true;
        else if (savedStartOption == 6)
            _toggleSS.isOn = true;
        else if (savedStartOption == 7)
            _toggleSSS.isOn = true;


        for (int i = 0; i < _diceOptionItems.Count; ++i)
        {
            _diceOptionItems[i].SetUI();
        }
    }

    void OnChangeToggleS(bool isOn)
    {
        if (isOn == true)
        {
            StarChangeToggleOption(5, true);
            OnChangeToggleSS(false);
            OnChangeToggleSSS(false);
        }
    }
    void OnChangeToggleSS(bool isOn)
    {
        if (isOn == true)
        {
            StarChangeToggleOption(6, true);
            OnChangeToggleS(false);
            OnChangeToggleSSS(false);
        }
    }
    void OnChangeToggleSSS(bool isOn)
    {
        if (isOn == true)
        {
            StarChangeToggleOption(7, true);
            OnChangeToggleS(false);
            OnChangeToggleSS(false);
        }
    }

    void StarChangeToggleOption(int startCount, bool isOn)
    {
        if (startCount == 5 && isOn)
        {
            PlayerOptionManager.instance.SetDiceStarOption(5);
        }
        else if (startCount == 6 && isOn)
        {
            PlayerOptionManager.instance.SetDiceStarOption(6);
        }
        else if (startCount == 7 && isOn)
        {
            PlayerOptionManager.instance.SetDiceStarOption(7);
        }
    }

    void ResetOption()
    {
        _toggleS.isOn = true;

        for (int i = 0; i < _diceOptionItems.Count; ++i)
        {
            _diceOptionItems[i].SetToggle(false);
        }
    }
}
