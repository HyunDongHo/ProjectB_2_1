using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Scheduler;

public class SkillUI : MonoBehaviour
{
    public Action<int> OnSkillButtonClicked;

    [System.Serializable]
    public class SkillSetting
    {
        public DragOnOff dragOnOff;
        public TimerBuffer dragOnOffBuffer = new TimerBuffer(0.1f);

        public Button button;
        public Image timerImage;
    }

    public SkillSetting[] skillSettings;

    private void Awake()
    {
    }

    private void Start()
    {
        AddEvent();

        for (int i = 0; i < skillSettings.Length; i++)
            skillSettings[i].dragOnOff.SetState(UserDataManager.instance.GetAutoSkill(i));
    }

    private void OnDestroy()
    {
        RemoveEvent();
    }

    private void AddEvent()
    {
        for (int i = 0; i < skillSettings.Length; i++)
        {
            int index = i;

            skillSettings[index].button.onClick.AddListener(() => HandleOnSkillButtonClicked(index, isAuto: false));
            skillSettings[index].dragOnOff.OnChanged += (isOn) => HandleOnSkillButtonDrag(index, isOn);
        }

        StageManager.instance.playerControl.utility.skillUseCheck.OnChangedSkillTimer += HandleOnChangedSkillTimer;
    }

    private void RemoveEvent()
    {
        for (int i = 0; i < skillSettings.Length; i++)
        {
            int index = i;

            skillSettings[index].button.onClick.RemoveAllListeners();
            skillSettings[index].dragOnOff.OnChanged = null;
        }

        StageManager.instance.playerControl.utility.skillUseCheck.OnChangedSkillTimer -= HandleOnChangedSkillTimer;
    }

    private void HandleOnChangedSkillTimer(int index, TimerBuffer buffer)
    {
        if (skillSettings[index].timerImage == null)
            return;
        skillSettings[index].timerImage.fillAmount = 1 - (buffer.timer / buffer.time);
    }

    private void HandleOnSkillButtonDrag(int index, bool isOn)
    {
        UserDataManager.instance.SetAutoSkill(index, isOn);

        //if (isOn)
        if(true)
            StartCheckUseAutoSkill(index);
        else
            EndCheckUseAutoSkill(index);
    }

    private void StartCheckUseAutoSkill(int index)
    {
        Timer.instance.TimerStart(skillSettings[index].dragOnOffBuffer,
            OnComplete: () =>
            {
                HandleOnSkillButtonClicked(index, isAuto: true);

                StartCheckUseAutoSkill(index);
            });
    }

    private void EndCheckUseAutoSkill(int index)
    {
        Timer.instance.TimerStop(skillSettings[index].dragOnOffBuffer);
    }

    private void HandleOnSkillButtonClicked(int index, bool isAuto)
    {
        //StageManager.instance.playerControl.utility.skillUseCheck.UseSkill(index, isAuto);

    }
}
