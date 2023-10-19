using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exp : MonoBehaviour
{
    public StatsManager manager;

    public Level targetLevel;

    private float currentExp ;


    public Action OnMaxExpOver;

    public Action<float> OnExpInit;

    public Action<float> OnExpSet;

    public Action<float> OnExpUp;


    /* init Exp */
    public void InitExp(float exp)
    {
        currentExp = exp;

        OnExpInit?.Invoke(currentExp);
    }

    /* Set Exp */
    public void SetExp(float exp)
    {
        currentExp = exp;

        OnExpSet?.Invoke(currentExp);
    }

    public void AddExp(int playerType, float exp)
    {
        if (targetLevel.maxLevel == -1 || targetLevel.GetCurrentLevel() >= targetLevel.maxLevel) return;
        StaticManager.Backend.GameData.PlayerGameData.UpdateUserData_Exp(playerType, currentExp + exp);
        SetExp(currentExp + exp);


        //  float nextExp = manager.GetValue(StatsValueDefine.MaxExp) * Mathf.Pow(1.2f, targetLevel.GetCurrentLevel());  

        //float nextExp = (float)Define.Util.GetExpressionValue(Define.ExpressionType.LevelExp, StaticManager.Backend.GameData.PlayerGameData.NowStageLevel);
        float nextExp = (float)Define.Util.GetExpressionValue(Define.ExpressionType.LevelExp, targetLevel.GetCurrentLevel()+1);
        //if (playerType == 0)
        //{
        //    Debug.Log($"nextExp : {nextExp}");
        //    Debug.Log($"current exp : {currentExp}");
        //    //Debug.Log($"Dwarrior exp : {StaticManager.Backend.GameData.PlayerGameData.DWarriorExp} ");
        //}

        while (currentExp >= nextExp)
        {

            float remainExp = currentExp - nextExp;
            StaticManager.Backend.GameData.PlayerGameData.UpdateUserData_Exp(playerType, remainExp);    
            SetExp(remainExp);
            //Debug.Log($"Dwarrior exp : {StaticManager.Backend.GameData.PlayerGameData.DWarriorExp} ");


            OnMaxExpOver?.Invoke();  
        }
        //while (currentExp >= manager.GetValue(StatsValueDefine.MaxExp))
        //{
        //    float remainExp = currentExp - manager.GetValue(StatsValueDefine.MaxExp);
        //    SetExp(remainExp);

        //    StaticManager.Backend.GameData.PlayerGameData.UpdateUserData(PlayerType.None, 0, remainExp);

        //    OnMaxExpOver?.Invoke();  
        //}

        OnExpUp?.Invoke(currentExp);
    }

    /* Get Hp */
    public float GetCurrentExp()
    {
        return currentExp;
    }


    /* Get Target Exp Per */
    public float GetTargetExpPer()
    {
        return currentExp / manager.GetValue(StatsValueDefine.MaxExp);
    }


}
