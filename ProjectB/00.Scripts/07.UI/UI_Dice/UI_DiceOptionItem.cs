using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_DiceOptionItem : MonoBehaviour
{
    [SerializeField]
    Define.StatType _diceStatType;

    [SerializeField]
    Toggle _toggle;

    [SerializeField]
    TextMeshProUGUI statText;

    private void Awake()
    {
        SetUI();
        AddEvent();
    }

    private void OnDestroy()
    {
        RemoveEvent();
    }

    void AddEvent()
    {
        _toggle.onValueChanged.AddListener(ToggleOnValueChange);
    }

    void RemoveEvent()
    {

        _toggle.onValueChanged.RemoveListener(ToggleOnValueChange);
    }

    void SetIsOnToggle()
    {
        _toggle.isOn = PlayerOptionManager.instance.GetDiceOption(_diceStatType);
    }
    public void SetToggle(bool isOn)
    {
        _toggle.isOn = isOn;
    }

    void ToggleOnValueChange(bool isOn)
    {
        PlayerOptionManager.instance.SetDiceOption(_diceStatType, isOn);
    }

    public void SetUI()
    {
        statText.text = Define.StatStatTitle[_diceStatType];
        SetIsOnToggle();
    }
}
