using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSp : MonoBehaviour
{
    public StatsManager manager;

    private float currentSp = 0;

    public Action<float> OnSpInit;
    public Action<float> OnSpSet;

    public Action<float> OnSpAdd;

    public void InitSp(float sp)
    {
        currentSp = sp;

        OnSpInit?.Invoke(currentSp);
    }

    public void SetSpToMax()
    {
        float maxSp = manager.GetValue(PlayerStatsValueDefine.MaxSp);

        AddSp(maxSp - currentSp);
    }

    public void SetSp(float sp)
    {
        currentSp = sp;

        OnSpSet?.Invoke(currentSp);
    }

    public void AddSp(float sp)
    {
        float maxSp = manager.GetValue(PlayerStatsValueDefine.MaxSp);

        if (currentSp + sp >= maxSp)
        {
            SetSp(maxSp);
        }
        else if (currentSp + sp <= 0)
        {
            SetSp(0);
        }
        else
        {
            SetSp(currentSp + sp);
        }

        OnSpAdd?.Invoke(currentSp);
    }

    public bool IsAvailiableRemoveCombo(float removeAmount)
    {
        return currentSp - removeAmount >= 0.0f;
    }

    public float GetCurrentSp()
    {
        return currentSp;
    }

    public float GetCurrentSpRate()
    {
        return currentSp / manager.GetValue(PlayerStatsValueDefine.MaxSp);
    }
}
