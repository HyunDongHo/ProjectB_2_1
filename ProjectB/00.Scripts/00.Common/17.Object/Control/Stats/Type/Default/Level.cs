using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public StatsManager manager;

    public Exp targetExp;

    public int maxLevel = -1;
    private int currentLevel = 1;

    public Action<int> OnLevelInit;

    public Action<int> OnLevelSet;

    public Action<int> OnLevelUp;
    private void Awake()
    {
        AddEvent();
    }

    private void OnDestroy()
    {
        RemoveEvent();
    }
    /* Add Event*/
    private void AddEvent()
    {
        targetExp.OnMaxExpOver += AddLevel;
    }

    
    /* Remove Event*/
    private void RemoveEvent()
    {
        targetExp.OnMaxExpOver -= AddLevel;
    }


    /* Init */
    public void InitLevel(int level)
    {
        currentLevel = level;

        OnLevelInit?.Invoke(currentLevel);
    }

    /* Set */
    public void SetLevel(int level)
    {
        currentLevel = level;

        OnLevelSet?.Invoke(currentLevel);
    }

    /* ADD */
    public void AddLevel()
    {
        SetLevel(currentLevel + 1);

        OnLevelUp?.Invoke(currentLevel);
    }


    /* Get */
    public int GetCurrentLevel()
    {
        return currentLevel;
    }

}
