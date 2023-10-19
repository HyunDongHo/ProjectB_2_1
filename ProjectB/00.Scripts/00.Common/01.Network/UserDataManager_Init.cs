using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using UnityEngine.SceneManagement;
using CodeStage.AntiCheat.ObscuredTypes;
using CodeStage.AntiCheat.Storage;
using BackEnd;

public partial class UserDataManager : Singleton<UserDataManager>
{
    
    public Param GetStartGameUserData()
    {
        Param param = new Param();
        param.Add("DLevel", 1);
        param.Add("DCoin", 10000000);
        param.Add("DRuby", 500);
        param.Add("DSp", 0.0);
        param.Add("DHp", 0.0);
        param.Add("DCore", 500);
        param.Add("DExp", 500);
        param.Add("QuickSlotItemList", new List<PlayerQuickSlotItemData>());
        param.Add("Buffs", new Dictionary<string, float>());

        param.Add("ClearStageLevel", 0);
        param.Add("NowStageLevel", 1);

        return param;
    }
}
