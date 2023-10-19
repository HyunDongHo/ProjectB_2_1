using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUtility : MonoBehaviour
{
    public Transform modelPivot;

    [Space]

    public PlayerInput input;
    public PlayerBuff buff;
    public PlayerSkillUseCheck skillUseCheck;
    public AnimationEndFrame animationEnd;    

    public PlayerModelSetter modelSetter;
    public PlayerBelongings belongings;

    public PlayerAttackDatas[] attackDatas;
    public PlayerWeaponModel weaponModel;

    public SubCamera subCamera;

    public void Init(PlayerControl playerControl)
    {
       // input.Init(playerControl);
       // buff.Init(playerControl);
        skillUseCheck.Init(playerControl);
       // metamorphosis.Init(playerControl);
        animationEnd.Init(playerControl);  

        modelSetter.Init(playerControl);
        weaponModel.Init(playerControl);
      //  petModel.Init(playerControl);  
      //  belongings.Init(playerControl);
      //  equipment.Init(playerControl);
      //  quickSlot.Init(playerControl);
    }

    public void Release()
    {
       // input.Release();
       // skillUseCheck.Release();
       // buff.Release();
      //  metamorphosis.Release();

        weaponModel.Release();
    }
}
