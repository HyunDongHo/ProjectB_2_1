using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scheduler;

public class PlayerSkillUseCheck : MonoBehaviour
{
    public const int TOTAL_USE_SKILL_COUNT = 4;

    private PlayerControl playerControl;
    private int skillLength;

    public Action<int, TimerBuffer> OnChangedSkillTimer;
    public Action<int> OnSkillButtonClicked;
    public Action<int, bool> OnUsedSkill;

    [System.Serializable]
    public class PlayerSkillSetting
    {
        public float skillCoolTime;
        public TimerBuffer skillBuffer = new TimerBuffer(0);
    }
    public PlayerSkillSetting[] playerSkillSettings = new PlayerSkillSetting[TOTAL_USE_SKILL_COUNT];

    public void Init(PlayerControl playerControl)
    {
        this.playerControl = playerControl;
        playerLevelSkillSetting(playerControl.GetStats<PlayerStats>().level.GetCurrentLevel());  

        for (int i = 0; i < playerSkillSettings.Length; i++)
        { 
           // playerSkillSettings[i] = new PlayerSkillSetting();
            ConvertSkillTime(i);  
        }

        AddEvent();
    }

    public void FixedUpdate()
    {
        if (StageManager.instance as LobbySceneManager)
            return;

        //for (int i = 0; i < playerSkillSettings.Length; ++i)
        //{
        //    UseSkill(i, true);  
        //}
        //Debug.Log($"skillLength : {skillLength}");
        for (int i = 0; i < skillLength; ++i)
        {
            UseSkill(i, true);    
        }
    }
    public void Release()
    {
        RemoveEvent();
    }

    private void AddEvent()
    {
        playerControl.GetStats<PlayerStats>().level.OnLevelUp += playerLevelSkillSetting;
        //  StatsManager statsManager = playerControl.GetStats<Stats>().manager;
        //  for (int i = 0; i < TOTAL_USE_SKILL_COUNT; i++)
        //  {
        //      int index = i;
        //      statsManager.GetStatsValue(PlayerStatsValueDefine.skillStats.GetSkillCoolTime(i)).OnBaseValueChanged += (value) => HandlOnSkillCoolTime(value, index);
        //      statsManager.GetStatsValue(PlayerStatsValueDefine.skillStats.GetSkillCoolTime(i)).OnAdditionValueChanged += (value) => HandlOnSkillCoolTime(value, index);
        //  }
        //
        //  statsManager.GetStatsValue(PlayerStatsValueDefine.SkillCoolDecreaseRatio).OnAdditionValueChanged += HandleOnSkillCoolDecreaseRatio;
    }

    private void RemoveEvent()
    {
        playerControl.GetStats<PlayerStats>().level.OnLevelUp -= playerLevelSkillSetting;

        //  StatsManager statsManager = playerControl.GetStats<Stats>().manager;
        //  for (int i = 0; i < TOTAL_USE_SKILL_COUNT; i++)
        //  {
        //      int index = i;
        //      statsManager.GetStatsValue(PlayerStatsValueDefine.skillStats.GetSkillCoolTime(i)).OnBaseValueChanged -= (value) => HandlOnSkillCoolTime(value, index);
        //      statsManager.GetStatsValue(PlayerStatsValueDefine.skillStats.GetSkillCoolTime(i)).OnAdditionValueChanged -= (value) => HandlOnSkillCoolTime(value, index);
        //  }
        //
        //  statsManager.GetStatsValue(PlayerStatsValueDefine.SkillCoolDecreaseRatio).OnAdditionValueChanged -= HandleOnSkillCoolDecreaseRatio;
    }
    private void playerLevelSkillSetting(int level)
    {
        if (level < 10)
        {
            skillLength = 1;
        }
        else if (level >= 10 && level < 20)
        {
            skillLength = 2;
        }
        else if (level >= 20 && level < 30)
        {
            skillLength = 3;
        }
        else if (level >= 30)
        {
            skillLength = 4;    
        }
    }

    private void HandlOnSkillCoolTime(float value, int index)
    {
        PlayerSkillSetting playerSkillSetting = playerSkillSettings[index];
        playerSkillSetting.skillCoolTime = value;

        ConvertSkillTime(index);

        if (!playerSkillSetting.skillBuffer.isRunningTimer || playerSkillSetting.skillBuffer.timer > playerSkillSetting.skillBuffer.time)
            playerSkillSetting.skillBuffer.timer = playerSkillSetting.skillBuffer.time;
    }

    private void HandleOnSkillCoolDecreaseRatio(float value)
    {
        for (int i = 0; i < playerSkillSettings.Length; i++)
        {
            ConvertSkillTime(i);
        }
    }

    private void ConvertSkillTime(int index)
    {
        float originSkillCoolTime = playerSkillSettings[index].skillCoolTime;
        playerSkillSettings[index].skillBuffer.time = originSkillCoolTime;
    }

    public bool IsAvaliableUseSkillButton(int index)
    {
        return playerSkillSettings[index].skillBuffer.isTimerEnd;
    }

    public void UseSkill(int skillIndex, bool isAuto)
    {
        OnSkillButtonClicked?.Invoke(skillIndex);

        if (IsAvaliableUseSkillButton(skillIndex))      
        {
            OnUsedSkill?.Invoke(skillIndex, isAuto);
        }
        else
        {
            if (!playerSkillSettings[skillIndex].skillBuffer.isRunningTimer)
                RunSkillCoolTime(skillIndex);

        }
    }

    public void UsedSkill(int skillIndex)
    {
        RunSkillCoolTime(skillIndex);
    }

    private void RunSkillCoolTime(int skillIndex)
    {
        Timer.instance.TimerStart(playerSkillSettings[skillIndex].skillBuffer,
            OnFrame: () =>
            {
                OnChangedSkillTimer?.Invoke(skillIndex, playerSkillSettings[skillIndex].skillBuffer);
            });
    }
}
