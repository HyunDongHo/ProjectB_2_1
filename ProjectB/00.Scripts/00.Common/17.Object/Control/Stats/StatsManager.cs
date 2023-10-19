using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Operation
{
    Plus,
    Multiply,
    Ratio,
    BaseSet
}

[System.Serializable]
public class AdditionValue
{
    public string additionName;
    public Operation operation;
    public float value;

    public AdditionValue(string additionName, Operation operation, float value)
    {
        this.additionName = additionName;
        this.operation = operation;
        this.value = value;
    }
}

public class StatsValue
{
    public Action<float> OnAdditionValueChanged;
    public Action<float> OnBaseValueChanged;

    private List<AdditionValue> additionValues = new List<AdditionValue>();
    private float baseValue;

    public StatsValue(float value)
    {
        SetValue(value);
    }

    // 같은 Class의 AdditionValue가 없으면 Set.
    public void SetAdditionValue(AdditionValue additionValue)
    {
        AdditionValue findValue = additionValues.Find(data => data == additionValue);
        if (findValue == null)
        {
            if(additionValue.operation == Operation.BaseSet)
            {
                Debug.LogError($"[StatsValue] Operation.Set은 Addition에 사용할 수 없습니다.");
            }
            else
            {
                additionValues.Add(additionValue);

                OnAdditionValueChanged?.Invoke(GetValue());
            }
        }
    }

    // Class의 모든 값이 같으면 Remove.
    public void RemoveAdditionValue(string additionName, Operation operation, float value)
    {
        float remainValue = value;

        AdditionValue findValue = additionValues.Find(data => data.additionName == additionName && data.operation == operation);
        while (findValue != null)
        {
            if (findValue.value - remainValue <= 0)
            {
                additionValues.Remove(findValue);
                remainValue -= findValue.value;

                findValue = additionValues.Find(data => data.additionName == additionName && data.operation == operation);
            }
            else
            {
                findValue.value -= remainValue;
                break;
            }
        }

        OnAdditionValueChanged?.Invoke(GetValue());
    }

    public void SetValue(float setValue)
    {
        baseValue = setValue;

        OnBaseValueChanged?.Invoke(GetValue());
    }

    public float GetValue()
    {
        float returnBaseValue = baseValue;

        if(additionValues.Count > 0)
        {
            List<AdditionValue> ratioAddtionValues = additionValues.FindAll(data => data.operation == Operation.Ratio);
            float totalRatioValue = 1;
            foreach (var ratioAddtionValue in ratioAddtionValues)
                totalRatioValue += ratioAddtionValue.value;
            returnBaseValue *= totalRatioValue;

            List<AdditionValue> plusAddtionValues = additionValues.FindAll(data => data.operation == Operation.Plus);
            float totalPlusValue = 0;
            foreach (var plusAddtionValue in plusAddtionValues)
                totalPlusValue += plusAddtionValue.value;
            returnBaseValue += totalPlusValue;

            List<AdditionValue> multiplyAddtionValues = additionValues.FindAll(data => data.operation == Operation.Multiply);
            float totalMultiplyValue = 1;
            foreach (var multiplyAddtionValue in multiplyAddtionValues)
                totalMultiplyValue *= multiplyAddtionValue.value;
            returnBaseValue *= totalMultiplyValue;
        }

        return returnBaseValue;
    }
}

public class StatsManager : MonoBehaviour
{
    public Dictionary<string, StatsValue> statsValues = new Dictionary<string, StatsValue>();

    private void AddNewStatsValue(string valueName, float value)
    {
        statsValues.Add(valueName, new StatsValue(value));
    }

    public void SetAdditionValue(string valueName, AdditionValue additionValue)
    {
        if (statsValues.ContainsKey(valueName))
            statsValues[valueName].SetAdditionValue(additionValue);
        else
            ShowValueExistError(valueName);
    }

    public void RemoveAdditionValue(string valueName, string additionName, Operation operation, float value)
    {
        if (statsValues.ContainsKey(valueName))
            statsValues[valueName].RemoveAdditionValue(additionName, operation, value);
        else
            ShowValueExistError(valueName);
    }

    public void SetValue(string valueName, float value)
    {
        if (statsValues.ContainsKey(valueName))
        {
            statsValues[valueName].SetValue(value);
        }
        else
            AddNewStatsValue(valueName, value);
    }

    public float GetValue(string valueName)
    {
        float value = default;

        if (statsValues.ContainsKey(valueName))
        {
            value = statsValues[valueName].GetValue();
        }
        else
            ShowValueExistError(valueName);

        return value;
    }

    public StatsValue GetStatsValue(string valueName)
    {
        StatsValue value = null;

        if (statsValues.ContainsKey(valueName))
        {
            value = statsValues[valueName];
        }
        else
            ShowValueExistError(valueName);

        return value;
    }

    private void ShowValueExistError(string valueName)
    {
        Debug.LogError($"[Stats] 스탯중에 {valueName}은 존재하지 않습니다.");
    }
}
