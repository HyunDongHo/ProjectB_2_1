using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scheduler;

public class DefaultStageManager : GamePlayManager
{
    protected override void Start()
    {
        base.Start();
    }

    private void CheckIsFirst()
    {
        if (UserDataManager.instance.GetIsFirstDefaultStage())
        {
           // PopupManager.instance.CreatePopup<PopupGameTutorial>("PopupGameTutorial");

            UserDataManager.instance.UpdateIsFirstDefaultStage();
        }
    }

    public virtual void LoadIntro()
    {
        (playerControl as PlayerControl_DefaultStage).StartIntro(CheckIsFirst);  
    }

    public virtual void LoadDefault()
    {
        (playerControl as PlayerControl_DefaultStage).StarDefault(CheckIsFirst);
    }
}
