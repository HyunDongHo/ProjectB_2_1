using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOptionManager : Singleton<PlayerOptionManager>
{
    public const string DiceOptionStartName = "DiceStartOption";


    // 주사위 옵션 값 설정
    public void SetDiceStarOption(int optionValue)
    {
        PlayerPrefs.SetInt(DiceOptionStartName, optionValue);
    }

    public int GetDiceStartOption()
    {
        int value = 5;

        if (PlayerPrefs.HasKey(DiceOptionStartName))
        {
            value = PlayerPrefs.GetInt(DiceOptionStartName);
        }

        return value;
    }


    public void SetDiceOption(Define.StatType diceStatType, bool isOn)
    {
        PlayerPrefs.SetInt(diceStatType.ToString(), isOn == true ? 1 : 0);
    }

    public bool GetDiceOption(Define.StatType diceStatType)
    {
        int value = 0;

        if (PlayerPrefs.HasKey(diceStatType.ToString()))
        {
            value = PlayerPrefs.GetInt(diceStatType.ToString());
        }

        return value == 1 ? true : false;
    }


}
