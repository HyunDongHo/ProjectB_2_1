using RNG;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerHitType
{
    Default,
    Missile,
    NoEffect
}

public enum PlayerAttackType
{
    Skill0,
    Skill1,
    Skill2,
    Skill3,

    Auto,
    Combo,
    Dash,
    Skill,
    None,
}

public abstract class PlayerAttack : Attack
{
    
    public enum PlayerType
    {
        Warrior,
        Archer,
        Wizard,
        None
    }

    public PlayerSkill skill;

    public enum ComplexComboResult
    {
        NotAble,
        Able,
        Use,
    }

    public Action OnAttackForceQuit;
    public Action<PlayerAttackType> OnAttackDataStarted;
    public Action<PlayerAttackType> OnAttackHitted;
    public Action<PlayerAttackType> OnAttackForceFrameEnd;
    public Action<PlayerAttackType> OnAttackDataEnd;

    public bool isUseSkill { get; set; } = false;
    public bool isUseCombo { get; set; } = false;
    public bool isUseDashCombo { get; set; } = false;

    public PlayerType playerModelType = PlayerType.Warrior;

    public int leftCombo { get; set; } = 0;
    public PlayerArrow playerArrow;
    public PlayerArrow playerWand;

    private const int maxLeftCombo = 2;

    public override void StopAttack()
    {
        OnAttackForceQuit?.Invoke();

        base.StopAttack();  
    }  

    #region Skill

    public bool Attack_Skill(int skillIndex)
    {
        if (!IsAvailiableAttack_Skill()) 
            return false;

        skill.UseSkill(skillIndex, new PlayerSkill.SkillParameter(OnStart: StartSkill, OnEnd: EndSkill, control: control as PlayerControl, attack: this));

        return true;
    }

    #endregion

    #region DashCombo

    public void Attack_Dash(ScreenRectType screenRectType, float addSp)
    {
        if (!IsAvailableAttack_DashCombo()) return;

    }


    #endregion

    #region Combo


    private bool Attack_LeftCombo(float addSp)
    {
        if (!IsAvaliableAttack() || control.GetMove<Move>().isNowBound == true) return false;

        StopAttack();

        AttackData attackData = null;

        int leftCombo = this.leftCombo;
        switch (leftCombo)
        {
            case 0:
                attackData = AttackExistAttackEvent(PlayerAttackType.Combo, (control as PlayerControl).utility.attackDatas[0].attack_a_01,
                    cameraShakePower: 2, cameraShakeTime: 0.05f, addSp, GetAttackSpeed());
                break;
            case 1:
                attackData = AttackExistAttackEvent(PlayerAttackType.Combo, (control as PlayerControl).utility.attackDatas[0].attack_a_02,
                    cameraShakePower: 2, cameraShakeTime: 0.05f, addSp, GetAttackSpeed());
                break;
        }

        if (attackData != null)
            Invoke(nameof(ClearCombo), attackData.GetTotalLength());
        return true;
    }

    private ComplexComboResult Attack_AnotherRightToLeftCombo(float addSp)
    {
        if (!IsAvaliableAttack()) return ComplexComboResult.NotAble;

        return ComplexComboResult.Able;
    }

    private bool Attack_Arrow(float addSp)
    {
        if (!IsAvaliableAttack()) return false;

        StopAttack();

        AttackData attackData = null;

      //  int leftCombo = this.leftCombo;

        if (leftCombo == 2)
            leftCombo = 0;

        switch (leftCombo)
        {
            case 0:
                attackData = (control as PlayerControl).utility.attackDatas[0].attack_a_01;

                break;
            case 1:
                attackData = (control as PlayerControl).utility.attackDatas[0].attack_a_02;

               
                break;               
        }

//        AttackData attackData = (control as PlayerControl).utility.attackDatas[0].attack_a_01;

        StartJustAttack(attackData);

        //PlayerControl_DefaultStage pd = (control as PlayerControl_DefaultStage);
        //Vector3 lookTargetEulerAngles = Quaternion.FromToRotation(Vector3.forward, (pd.pRaycast.targetCollider.transform.position - transform.position).normalized).eulerAngles;
        //(control as PlayerControl).utility.modelPivot.transform.eulerAngles = new Vector3(0, lookTargetEulerAngles.y, 0);

        attackData.AddHandleEvent(CommonPlayerAttackDataParameterDefine.ShootNormalArrow,
         (parameter) =>
         {
             Arrow arrow = playerArrow.CreateArrow((control as PlayerControl).GetModel<Model>().ProjectileOffset.gameObject);
             arrow.gameObject.layer = LayerMask.NameToLayer("Projectile");

             Vector3 forward = control.GetModel<Model>().transform.forward;
             arrow.SetTargetPosition(transform.position + forward * 10, forward);

             arrow.OnAttack = (Collider) =>
             {
                 Control control = Collider.GetComponentInParent<Control>();

                 //  if(control is BossEnemyControl == false)
                 //     control.GetMove<Move>().KnockBack(1.2f, 0.3f, arrow.transform.forward); 

                 CreateResourceManager.instance.CreateResource(arrow.gameObject, createResourceData: arrow.hitObj);
                 DamageToAttackTargets(control,1, PlayerHitType.NoEffect, PlayerAttackType.None);
             };
         });

     //  attackData.AddHandleEvent(CommonPlayerAttackDataParameterDefine.ShootNormalTripleArrow,
     //         (parameter) =>
     //         {
     //             Arrow centerArrow = playerArrow.CreateArrow((control as PlayerControl).GetModel<Model>().ProjectileOffset.gameObject);
     //             centerArrow.gameObject.layer = LayerMask.NameToLayer("Projectile");
     //
     //             Arrow leftArrow = playerArrow.CreateArrow((control as PlayerControl).GetModel<Model>().ProjectileOffset.gameObject);
     //             leftArrow.gameObject.layer = LayerMask.NameToLayer("Projectile");
     //
     //             Arrow rightArrow = playerArrow.CreateArrow((control as PlayerControl).GetModel<Model>().ProjectileOffset.gameObject);
     //             rightArrow.gameObject.layer = LayerMask.NameToLayer("Projectile");
     //
     //             Vector3 forward = control.GetModel<Model>().transform.forward;
     //             Vector3 right = (control.GetModel<Model>().transform.right + forward).normalized;
     //             Vector3 left = (-(control.GetModel<Model>().transform.right) + forward).normalized;
     //
     //             centerArrow.SetTargetPosition(transform.position + forward * 8, forward);
     //             rightArrow.SetTargetPosition(transform.position + right * 8, right);
     //             leftArrow.SetTargetPosition(transform.position + left * 8, left);
     //
     //
     //             centerArrow.OnAttack = (Collider) =>
     //             {
     //                 Control control = Collider.GetComponentInParent<Control>();
     //                 float animTime = control.GetModel<Model>().animationControl.GetTotalTime((control as EnemyControl).GetHit());
     //                 control.GetMove<Move>().KnockBack(1.2f, animTime, centerArrow.transform.forward);
     //                 DamageToAttackTargets(control, 1, PlayerHitType.Missile);
     //             };
     //
     //             rightArrow.OnAttack = (Collider) =>
     //             {
     //                 Control control = Collider.GetComponentInParent<Control>();
     //                 float animTime = control.GetModel<Model>().animationControl.GetTotalTime((control as EnemyControl).GetHit());
     //                 control.GetMove<Move>().KnockBack(1.2f, animTime, rightArrow.transform.forward);
     //                 DamageToAttackTargets(control, 1, PlayerHitType.Missile);
     //             };
     //
     //             leftArrow.OnAttack = (Collider) =>
     //             {
     //                 Control control = Collider.GetComponentInParent<Control>();
     //                 float animTime = control.GetModel<Model>().animationControl.GetTotalTime((control as EnemyControl).GetHit());
     //                 control.GetMove<Move>().KnockBack(1.2f, animTime, leftArrow.transform.forward);
     //                 DamageToAttackTargets(control, 1, PlayerHitType.Missile);
     //             };
     //         });

        if (attackData != null)
            Invoke(nameof(ClearCombo), attackData.GetTotalLength());

        return true;
    }

    private bool Attack_Wand(float addSp)
    {
        if (!IsAvaliableAttack()) return false;      

        StopAttack();    

        AttackData attackData = null;

        //  int leftCombo = this.leftCombo;

        if (leftCombo == 2)
            leftCombo = 0;

        switch (leftCombo)
        {
            case 0:
                attackData = (control as PlayerControl).utility.attackDatas[0].attack_a_01;
                break;
            case 1:
                attackData = (control as PlayerControl).utility.attackDatas[0].attack_a_02;
                break;
        }

        //        AttackData attackData = (control as PlayerControl).utility.attackDatas[0].attack_a_01;

        StartJustAttack(attackData);    

        //PlayerControl_DefaultStage pd = (control as PlayerControl_DefaultStage);
        //Vector3 lookTargetEulerAngles = Quaternion.FromToRotation(Vector3.forward, (pd.pRaycast.targetCollider.transform.position - transform.position).normalized).eulerAngles;
        //(control as PlayerControl).utility.modelPivot.transform.eulerAngles = new Vector3(0, lookTargetEulerAngles.y, 0);

        attackData.AddHandleEvent(CommonPlayerAttackDataParameterDefine.ShootNormalWand,
         (parameter) =>
         {
             Arrow arrow = playerWand.CreateArrow((control as PlayerControl).GetModel<Model>().ProjectileOffset.gameObject);
             if (arrow == null) return;

             arrow.gameObject.layer = LayerMask.NameToLayer("Projectile");

             Vector3 forward = control.GetModel<Model>().transform.forward;
             forward.y = 0;
             arrow.SetTargetPosition(transform.position + forward * 10, forward);

             arrow.OnAttack = (Collider) =>
             {
                 Control control = Collider.GetComponentInParent<Control>();

                 //if (control is BossEnemyControl == false)
                 //    control.GetMove<Move>().KnockBack(1.2f, 0.3f, arrow.transform.forward);

                 DamageToAttackTargets(control, 1, PlayerHitType.NoEffect, PlayerAttackType.None);
             };
         });


        if (attackData != null)
            Invoke(nameof(ClearCombo), attackData.GetTotalLength());

        return true;
    }


    #endregion

    #region BaseAttack

    public void Attack_Auto(float addSp)
    {
        if (control.GetMove<Move>().isNowBound == true)
            return;

        switch(playerModelType)
        {
            case PlayerType.Warrior:
                Attack_LeftCombo(addSp);
                break;
            case PlayerType.Archer:
                Attack_Arrow(addSp);
                break;
            case PlayerType.Wizard:
                Attack_Wand(addSp);
                break;
        }

        

        //if (!IsAvailableAttack_Auto()) return;

        //if (!IsAvaliableAttack()) return;

        //AttackExistAttackEvent(PlayerAttackType.Auto, (control as PlayerControl).utility.attackDatas[(int)playerModelType].base_attack_combo_01,
        //    cameraShakePower: 5, cameraShakeTime: 0.1f, addSp, GetAttackSpeed(), OnComplete: EndBaseAttack);
    }

    #endregion


    #region Attack To Target

    public void AttackTargets(PlayerAttackType playerAttackType ,float cameraShakePower, float cameraShakeTime, int divideDamage, float addSp, PlayerHitType attackType = PlayerHitType.Default)
    {
        if (DamageToAttackTargets(divideDamage, attackType, playerAttackType))
        {
            PlayerStats stats = (control as PlayerControl).GetStats<PlayerStats>();

            float convertAddHp = stats.manager.GetValue(PlayerStatsValueDefine.HpDrainRatioPerHit);
            float convertAddSp = addSp + stats.manager.GetValue(PlayerStatsValueDefine.SpDrainRatioPerHit);

            //stats.hp.AddHp(convertAddHp);
            //stats.hp.AddHp(convertAddHp, (int)playerModelType);
            stats.sp.AddSp(convertAddSp);  

            StageManager.instance.cameraManager.StartShakeAllCamera_Position(cameraShakePower, cameraShakeTime);
        }
    }

    private bool DamageToAttackTargets(int divideDamage, PlayerHitType attackType, PlayerAttackType playerAttakType)
    {
        int attackCount = 0;

        foreach (var target in attackTargets)
        {
            if (target.Value == null) continue;

            /* 스탯 공격력 추가 및 스킬공격력 계산 */
            double defaultAttackDamage = GetAttackDamage(playerModelType); // 기본 공격력(100) + 스탯공격력 + 인벤 무기 공격력 + 파트너 공격력 
            double finalAttackDamage = 0;
            double skillDamageRatio = 0;
            finalAttackDamage = defaultAttackDamage;

            switch (playerAttakType)
            {
                case PlayerAttackType.Auto:
                    break;
                case PlayerAttackType.Combo:
                    break;
                case PlayerAttackType.Dash:
                    break;
                case PlayerAttackType.Skill0:   
                    skillDamageRatio = GetActiveDamageRatio((int)playerModelType , PlayerAttackType.Skill0) + GetPassiveDamageRatio((int)playerModelType, PlayerAttackType.Skill0);
                    finalAttackDamage *= skillDamageRatio;
                    //Debug.Log($"skill 0  * defaultDamage : {finalAttackDamage}");  
                    break;
                case PlayerAttackType.Skill1:
                    skillDamageRatio = GetActiveDamageRatio((int)playerModelType, PlayerAttackType.Skill1) + GetPassiveDamageRatio((int)playerModelType, PlayerAttackType.Skill1);    
                    finalAttackDamage *= skillDamageRatio;
                    //Debug.Log($"skill 1  * defaultDamage : {finalAttackDamage}");
                    break;
                case PlayerAttackType.Skill2:
                    skillDamageRatio = GetActiveDamageRatio((int)playerModelType, PlayerAttackType.Skill2) + GetPassiveDamageRatio((int)playerModelType, PlayerAttackType.Skill2);
                    finalAttackDamage *= skillDamageRatio;
                    //Debug.Log($"skill 2  * defaultDamage : {finalAttackDamage}");
                    break;
                case PlayerAttackType.Skill3:
                    skillDamageRatio = GetActiveDamageRatio((int)playerModelType, PlayerAttackType.Skill3) + GetPassiveDamageRatio((int)playerModelType, PlayerAttackType.Skill3);  
                    finalAttackDamage *= skillDamageRatio;
                    //Debug.Log($"skill 3  * defaultDamage : {finalAttackDamage}");  
                    break;
            }

            DamageType damageType = target.Value.ReduceHp(control, (float)finalAttackDamage, GetCriticalRatio(), divideDamage);                
            //DamageType damageType = target.Value.ReduceHp(control, GetAttackDamage(), GetCriticalRatio(), divideDamage);

            attackCount++;

            switch (attackType)
            {
                case PlayerHitType.Default:
                    DamageDefault(target.Value, damageType);
                    break;
                case PlayerHitType.Missile:
                   // DamageMissile(target, damageType);
                    break;
            }
        }

        return attackCount > 0;
    }
    double GetStatAttack(Dictionary<string, double> PlayerStatLevel) // 스탯 공격력 
    {
        double outValue;
        if (PlayerStatLevel.TryGetValue("AttackLevel", out outValue) == false)
            return 0;
        return PlayerStatLevel["AttackLevel"];
    }

    double InvenGetAttack(PlayerType playerType) // 보유 효과 (Get)
    {
        List<BackendData.GameData.ItemData> playerDatas = null;
        double GetAttacks = 0;

        playerDatas = StaticManager.Backend.Chart.Weapon.GetPlayerEquipment(100, (int)playerType);
        if (playerDatas != null)
        {
            for (int i = 0; i < playerDatas.Count; i++)
            {
                BackendData.Chart.Weapon.Item chartItem = null;
                chartItem = StaticManager.Backend.Chart.Weapon.GetListItem(100, (int)playerType).Find(item => item.ItemID == playerDatas[i].ItemID); // chart 
                double _chartGetAttack = chartItem.GetAttack;
                double _chartGrowingGetAttack = chartItem.GrowingGetAttact;
                double level = playerDatas[i].ItemLevel;
                double _getAttack = _chartGetAttack + (_chartGrowingGetAttack * (level - 1));
                //Debug.Log($"id : {playerDatas[i].ItemID} =>  GetAttack : {_getAttack}");
                GetAttacks += _getAttack;  
            }
        }
        else
        {
            return 0;
        }

        return GetAttacks;
    }
    double InvenEquipAttack(PlayerType playerType) // 장착 효과 (Equip)
    {
        BackendData.GameData.ItemData playerData = null;
        BackendData.Chart.Weapon.Item chartItem = null;

        playerData = StaticManager.Backend.Chart.Weapon.GetPlayerEquipment(100, (int)playerType).Find(item => item.ItemIsEquip == true);
        if (playerData != null)
            chartItem = StaticManager.Backend.Chart.Weapon.GetListItem(100, (int)playerType).Find(item => item.ItemID == playerData.ItemID); // chart 

        if (playerData == null || chartItem == null)
            return 0;

        double _chartEquipAttack = chartItem.EquipAttack;
        double _chartGrowingEquipAttack = chartItem.GrowingEquipAttack;
        double level = playerData.ItemLevel;
        double _equipAttack = _chartEquipAttack + (_chartGrowingEquipAttack * (level - 1));

        return _equipAttack;
    }


    double GetActiveDamageRatio(int playerType, PlayerAttackType skillNum) // 액티브 스킬 
    {
        Dictionary<string, double> PlayerActiveSkillLevel = null;
        switch (playerType)
        {
            case (int)PlayerType.Warrior:
                PlayerActiveSkillLevel = StaticManager.Backend.GameData.PlayerGameData.DWarriorActiveSkillLevel;
                break;
            case (int)PlayerType.Archer:
                PlayerActiveSkillLevel = StaticManager.Backend.GameData.PlayerGameData.DArcherActiveSkillLevel;
                break;
            case (int)PlayerType.Wizard:
                PlayerActiveSkillLevel = StaticManager.Backend.GameData.PlayerGameData.DWizardActiveSkillLevel;
                break;
        }
        Dictionary<int, BackendData.Chart.PlayerActiveSkillData.Item> ActiveSkillChartData = null;
        ActiveSkillChartData = StaticManager.Backend.Chart.PlayerActiveSkillData.GetPlayerActiveSkillItem(playerType);

        int chartItemIndex = 0;
        foreach(var item in ActiveSkillChartData)
        {
            if (item.Value.SkillNum == (int)skillNum)
                chartItemIndex = item.Value.ItemID;
        }

        if (chartItemIndex == 0)
            return 0;

        double outValue1;
        if (PlayerActiveSkillLevel.TryGetValue(Enum.GetName(typeof(SkillTypeNum), 0), out outValue1) == false)
            return ActiveSkillChartData[chartItemIndex].DamageInfo;

        double final_active_damageRaio = 0;
        final_active_damageRaio = ActiveSkillChartData[chartItemIndex].DamageInfo + (ActiveSkillChartData[chartItemIndex].DamageRatio * PlayerActiveSkillLevel[$"SkillLevel_{(int)skillNum}"]);

        if (final_active_damageRaio == 0)
        {
            return 0;
        }
        else
        {
            //Debug.Log($"active ratio : {final_active_damageRaio}");
            return final_active_damageRaio;
        }
            
    }

    double GetPassiveDamageRatio(int playerType,  PlayerAttackType skillNum) // 패시브 스킬 
    {

        Dictionary<string, double> PlayerPassiveSkillLevel = null;
        switch (playerType)
        {
            case (int)PlayerType.Warrior:
                PlayerPassiveSkillLevel = StaticManager.Backend.GameData.PlayerGameData.DWarriorPassiveSkillLevel;
                break;
            case (int)PlayerType.Archer:
                PlayerPassiveSkillLevel = StaticManager.Backend.GameData.PlayerGameData.DArcherPassiveSkillLevel;
                break;
            case (int)PlayerType.Wizard:
                PlayerPassiveSkillLevel = StaticManager.Backend.GameData.PlayerGameData.DWizardPassiveSkillLevel;
                break;
        }

        Dictionary<int, BackendData.Chart.PlayerPassiveSkillData.Item> PassiveSkillChartData = null;
        PassiveSkillChartData = StaticManager.Backend.Chart.PlayerPassiveSkillData.GetPlayerPassiveSkillItem(playerType);

        int chartItemIndex = 0;
        foreach (var item in PassiveSkillChartData)
        {
            if (item.Value.SkillNum == (int)skillNum)
                chartItemIndex = item.Value.ItemID;
        }

        if (chartItemIndex == 0)
            return 0;

        double outValue1;
        if (PlayerPassiveSkillLevel.TryGetValue(Enum.GetName(typeof(SkillTypeNum), 0), out outValue1) == false) // 데이터가 없으면 
            return PassiveSkillChartData[chartItemIndex].DamageInfo;

        double final_passive_damageRaio = 0;
        final_passive_damageRaio = PassiveSkillChartData[chartItemIndex].DamageInfo + (PassiveSkillChartData[chartItemIndex].DamageRatio * PlayerPassiveSkillLevel[$"SkillLevel_{(int)skillNum}"]);

        if (final_passive_damageRaio == 0)
        {
            return 0;
        }
        else
        {
            //Debug.Log($"passive ratio : {final_passive_damageRaio}");
            return final_passive_damageRaio;
            
        }
    }
    public void DamageToAttackTargets(Control target, int divideDamage, PlayerHitType attackType, PlayerAttackType playerAttakType)
    {
        if (target == null)
            return;

        /* 스탯 공격력 추가 및 스킬공격력 계산 */
        double defaultAttackDamage = GetAttackDamage(playerModelType); // 기본 공격력(100) + 스탯공격력 + 인벤 무기 공격력 + 파트너 공격력 
        double finalAttackDamage = 0;
        double skillDamageRatio = 0;
        finalAttackDamage = defaultAttackDamage;

        switch (playerAttakType)
        {
            case PlayerAttackType.Auto:
                break;
            case PlayerAttackType.Combo:
                break;
            case PlayerAttackType.Dash:
                break;
            case PlayerAttackType.Skill0:
                skillDamageRatio = GetActiveDamageRatio((int)playerModelType, PlayerAttackType.Skill0) + GetPassiveDamageRatio((int)playerModelType, PlayerAttackType.Skill0);
                finalAttackDamage *= skillDamageRatio;
                //Debug.Log($"skill 0  * defaultDamage : {finalAttackDamage}");
                break;
            case PlayerAttackType.Skill1:
                skillDamageRatio = GetActiveDamageRatio((int)playerModelType, PlayerAttackType.Skill1) + GetPassiveDamageRatio((int)playerModelType, PlayerAttackType.Skill1);
                finalAttackDamage *= skillDamageRatio;
                //Debug.Log($"skill 1  * defaultDamage : {finalAttackDamage}");  
                break;
            case PlayerAttackType.Skill2:
                skillDamageRatio = GetActiveDamageRatio((int)playerModelType, PlayerAttackType.Skill2) + GetPassiveDamageRatio((int)playerModelType, PlayerAttackType.Skill2);
                finalAttackDamage *= skillDamageRatio;
                //Debug.Log($"skill 2  * defaultDamage : {finalAttackDamage}");
                break;
            case PlayerAttackType.Skill3:
                skillDamageRatio = GetActiveDamageRatio((int)playerModelType, PlayerAttackType.Skill3) + GetPassiveDamageRatio((int)playerModelType, PlayerAttackType.Skill3);
                finalAttackDamage *= skillDamageRatio;
                //Debug.Log($"skill 3  * defaultDamage : {finalAttackDamage}");
                break;
        }

        //DamageType damageType = target.ReduceHp(control, GetAttackDamage(), GetCriticalRatio(), divideDamage);        
        //DamageType damageType = target.ReduceHp(control, GetAttackDamage(playerModelType), GetCriticalRatio(), divideDamage);        
        DamageType damageType = target.ReduceHp(control, (float)finalAttackDamage, GetCriticalRatio(), divideDamage);        

        switch (attackType)
        {
            case PlayerHitType.Default:
                DamageDefault(control, damageType);
                break;
            case PlayerHitType.Missile:
                DamageMissile(target, damageType);
                break;
            case PlayerHitType.NoEffect:
                break;
        }        
    }

    private void DamageDefault(Control target, DamageType damageType)
    {
        SoundManager.instance.PlaySound("Sword_Slash_Hit_01");

        Model model = target.GetModel<Model>();
        Bounds bounds = new Bounds(model.bodyOffset.position, model.bodyOffset.localScale);
        Vector3 createPosition = bounds.ClosestPoint((control as PlayerControl).utility.weaponModel.GetCurrentActiveWeaponSocket().position);

        ShowAttackEffect(createPosition, "WarriorAttackHit");

      //  if (damageType == DamageType.Critical)
        //    ShowDamageManager.instance.ShowCritical(model.headOffset.position);
    }
    private void DamageMissile(Control target, DamageType damageType)
    {
        //SoundManager.instance.PlaySound("Sword_Slash_Hit_01");

        //Model model = target.GetModel<Model>();


        ShowAttackEffect(target.transform.position + Vector3.up, "Damage_Hit_Spark_01_B");

        //  if (damageType == DamageType.Critical)
        //    ShowDamageManager.instance.ShowCritical(model.headOffset.position);
    }

    #endregion

    #region Skill Attack 관련

    private bool IsAvailiableAttack_Skill()
    {
        if (!isUseSkill)
        {
            CompleteAttackWait();

            return true;
        }

        return false;  
    }

    private void StartSkill()
    {
        isUseSkill = true;

        control.GetStats<PlayerStats>().hp.isAvailableReduceHp = false;
    }

    private void EndSkill()
    {
        isUseSkill = false;

        control.GetStats<PlayerStats>().hp.isAvailableReduceHp = true;

        OnAttackForceFrameEnd?.Invoke(PlayerAttackType.Skill);
    }

    #endregion

    #region Auto Attack 관련

    private bool IsAvailableAttack_Auto()
    {
        if (!(isUseCombo || isUseSkill || isUseDashCombo))
        {
            return true;
        }

        return false;
    }

    public void EndBaseAttack()
    {
        OnAttackForceFrameEnd?.Invoke(PlayerAttackType.Auto);
    }

    #endregion

    #region Combo Attack 관련

    private bool IsAvailableAttack_Combo()
    {
        if (!(isUseSkill || isUseDashCombo))
        {
            if (!isUseCombo)
            {
                isUseCombo = true;
                CompleteAttackWait();
            }

            if (attackWaitBuffer.isTimerEnd)
            {
                CancelInvoke(nameof(ClearCombo));
            }

            return true;
        }

        return false;
    }

    public void EndCombo()
    {
        CompleteAttackWait();

        OnAttackForceFrameEnd?.Invoke(PlayerAttackType.Combo);
    }

    public void ClearCombo()
    {
        isUseCombo = false;

        ResetCombo();
    }

    public void ResetCombo()
    {
        leftCombo = 0;
    }

    public int GetLeftComboLength()
    {
        return maxLeftCombo;
    }

    #endregion

    #region Dash Attack 관련

    private bool IsAvailableAttack_DashCombo()
    {
        if (!isUseSkill)
        {
            if (!isUseDashCombo)
            {
                isUseDashCombo = true;

                StopAttack();
                CompleteAttackWait();
            }

            return true;
        }

        return false;
    }

    public void EndDashCombo()
    {
        isUseDashCombo = false;

        OnAttackForceFrameEnd?.Invoke(PlayerAttackType.Dash);
    }

    #endregion

    protected override void AttackEnd()
    {
        base.AttackEnd();

        if (!isUseDashCombo || !isUseCombo || !isUseSkill || !control.GetMove<Move>().isNowBound)
            control.GetModel<PlayerModel>().PlayCurrentSetIdleAnimation();
    }

    public void ResetAttackValue()
    {
        isUseCombo = false;
        isUseDashCombo = false; 
        isUseSkill = false;
    }

    protected override void AddDefaultAttackDataEvent(AttackData attackData)
    {
        base.AddDefaultAttackDataEvent(attackData);  

        attackData.AddHandleEvent(CommonPlayerAttackDataParameterDefine.SET_COMBO_LEFT, 
            (parameter) => { leftCombo = parameter.intValue; });

        attackData.AddHandleEvent(CommonPlayerAttackDataParameterDefine.END_COMBO,
            (parameter) => 
            { 

                EndCombo(); 
            });

        attackData.AddHandleEvent(CommonPlayerAttackDataParameterDefine.END_DASH_COMBO,
            (parameter) => { EndDashCombo(); });

        attackData.AddHandleEvent(CommonPlayerAttackDataParameterDefine.END_BASE_ATTACK,
            (parameter) => { EndBaseAttack(); });

        attackData.AddHandleEvent(CommonPlayerAttackDataParameterDefine.SET_WEAPON_MODEL,
            (parameter) =>
            {
                if (Enum.TryParse(parameter.stringValue, out PlayerWeaponModelType weaponModelType))
                {
                    (control as PlayerControl_GamePlay).utility.weaponModel.SetWeaponModelType(weaponModelType);
                }
            });

        attackData.AddHandleEvent(CommonPlayerAttackDataParameterDefine.ACTIVE_WEAPON_MODEL,
            (parameter) =>
            {
                if (parameter.boolValue)
                {
                    (control as PlayerControl_GamePlay).utility.weaponModel.SetCurrentWeaponModelType();
                }
                else
                {
                    (control as PlayerControl_GamePlay).utility.weaponModel.SetAllWeaponModelActive(false);
                }
            });

        attackData.AddHandleEvent(CommonPlayerAttackDataParameterDefine.SET_IDLE_DEFAULT_POSE,
            (parameter) =>
            {
                if (Enum.TryParse(parameter.stringValue, out PlayerIdleType playerIdleType))
                    control.GetModel<PlayerModel>().SetCurrentPlayerIdleType(playerIdleType);
            });

        attackData.AddHandleEvent(CommonPlayerAttackDataParameterDefine.FRONT_BOUND,
             (parameter) =>
             {
                 foreach (var target in attackTargets.Values)
                 {
                     if (target as EnemyControl == null || (target as BossEnemyControl) == true)
                         continue;                     

                     (target as EnemyControl)?.PlayBoundAnimation(null);
                     Move targetMove = target.GetMove<Move>();
                     targetMove.GetComponent<Move>().Bound(parameter.floatValue, parameter.floatValue1, parameter.floatValue2, parameter.floatValue3, -target.transform.forward);
                 }

             });

        attackData.AddHandleEvent(CommonPlayerAttackDataParameterDefine.KNOCK_BACK,
             (parameter) =>
             {
                 foreach (var target in attackTargets.Values)
                 {
                     if (target as EnemyControl == null || (target as BossEnemyControl) == true)
                         continue;

                     Move targetMove = target.GetMove<Move>();
                     targetMove.GetComponent<Move>().KnockBack(parameter.floatValue, parameter.floatValue1, -target.transform.forward);
                 }

             });

        attackData.AddHandleEvent(CommonPlayerAttackDataParameterDefine.TIME_SCALE_CHANGE,
          (parameter) =>
          {
              TimeManager.instance.SetTimeScale(parameter.floatValue, parameter.floatValue1);
          });
    }

    // Attack Data에 Attack Event가 존재하지 않을 때 이 함수 사용. (Camera Shake, AddSp, Attack Event 존재 하지 않음.) 
    public AttackData AddAttackNotExistAttackEvent(PlayerAttackType attackType, AttackData attackData, float attackSpeed = 1, int weight = 0, Action OnStartAttack = null, Action OnComplete = null)
    {
        StartAttack(attackData, attackSpeed, weight,
            () =>
            {
                OnStartAttack?.Invoke();
                OnAttackDataStarted?.Invoke(attackType);
            },
            () =>
            {
                OnComplete?.Invoke();
                OnAttackDataEnd?.Invoke(attackType);
            });

        return attackData;
    }

    // Attack Data에 Attack Event가 존재할 시 이 함수 사용. (Camera Shake, AddSp, Attack Event 존재) 
    public AttackData AttackExistAttackEvent(PlayerAttackType attackType, AttackData attackData, float cameraShakePower, float cameraShakeTime, float addSp, float attackSpeed = 1, int weight = 0, Action OnStartAttack = null, Action OnComplete = null)
    {
        StartAttack(attackData, attackSpeed, weight,
            () =>
            {
                OnStartAttack?.Invoke();
                OnAttackDataStarted?.Invoke(attackType);
            },
            () =>
            {
                OnComplete?.Invoke();
                OnAttackDataEnd?.Invoke(attackType);
            });
        AddAttackEvent(attackType, attackData, cameraShakePower, cameraShakeTime, addSp); 

        return attackData;
    }

    private void AddAttackEvent(PlayerAttackType attackType, AttackData attackData, float cameraShakePower, float cameraShakeTime, float addSp)
    {
        attackData.AddHandleEvent(CommonPlayerAttackDataParameterDefine.ATTACK,
            (parameter) =>
            {
                OnAttackHitted?.Invoke(attackType);
                //AttackTargets(cameraShakePower, cameraShakeTime, attackData.GetAttackMethodsCount(CommonPlayerAttackDataParameterDefine.ATTACK), addSp);
                /**/
                if(attackType == PlayerAttackType.Skill0 || attackType == PlayerAttackType.Skill1 || attackType == PlayerAttackType.Skill2 || attackType == PlayerAttackType.Skill3)
                    AttackTargets(attackType, parameter.floatValue, parameter.floatValue1, 1, addSp);

                AttackTargets(attackType, parameter.floatValue, parameter.floatValue1, attackData.GetAttackMethodsCount(CommonPlayerAttackDataParameterDefine.ATTACK), addSp);
            });

        // TODO : BOUND 
       // attackData.AddHandleEvent(CommonPlayerAttackDataParameterDefine.FRONT_BOUND,
       //     (parameter) =>
       //     {
       //         foreach (var target in attackTargets.Values)
       //         {
       //             if (target as EnemyControl == null)    
       //                 continue;
       //
       //             (target as EnemyControl)?.PlayBoundAnimation(null);
       //             Move targetMove = target.GetMove<Move>();
       //             //targetMove.GetComponent<Move>().isNowBound = false;
       //             targetMove.GetComponent<Move>().Bound(1.5f, 1.5f, 0.5f, 2, -target.transform.forward);      
       //         }
       //     });
    }

    public override float GetAttackSpeed() 
    {
        return base.GetAttackSpeed();// * (control as PlayerControl_GamePlay).utility.equipment.GetBaseAttackSpeedRatio();
    }

    public override float GetAttackDamage()
    {
        float damage = base.GetAttackDamage();

        //float defaultDamage = (float)(damage + GetStatAttack(StaticManager.Backend.GameData.PlayerGameData.DWarriorGoldStatLevel) + InvenGetAttack(PlayerType.Warrior) + InvenEquipAttack(PlayerType.Warrior));
        ////  damage = (control as PlayerControl_GamePlay).utility.equipment.GetBaseWeaponDamage() * damage + 50;

        //// 변하지 않는 기본 공격 여기다 추가 
        ///* TODO : 동료 공격력 추가 */
        //float finalDefaultDamage = defaultDamage * (1 + (float)GetPartnerAttack());

        return damage;
    }
    public float GetAttackDamage(PlayerType playerModelType)
    {
        float damage = base.GetAttackDamage();
        float defaultDamage = 0;
        defaultDamage = (float)(damage + StaticManager.Backend.GameData.PlayerGameData.GetStatRatio(Define.StatType.AttackRatio) + InvenGetAttack(playerModelType) + InvenEquipAttack(playerModelType));

        // 변하지 않는 기본 공격 여기다 추가 
        /* TODO : 동료 공격력, 유물, 특성 추가 */
        float defaultPlayerRatio = 1 + (float)(GetPartnerAttack() 
                                        + StaticManager.Backend.GameData.PlayerTreasure.GetTreasureRatio(Define.StatType.AttackRatio) 
                                        + StaticManager.Backend.GameData.PlayerDice.GetDiceRatio(Define.StatType.AttackRatio)
                                        + (float)StaticManager.Backend.GameData.PlayerAdsBuff.GetAdsBuffRatio(BackendData.Chart.AdsBuff.AdsType.Attack)
                                        );

        float upgradePlayerRatio = (float)StaticManager.Backend.GameData.PlayerGameData.GetPlayerUpgradeRatio((int)playerModelType, Define.StatType.AttackRatio);
        float finalDefaultDamage = defaultDamage * (defaultPlayerRatio + upgradePlayerRatio);  

        /* 치명타 확률 및 피해량 계산 */
        double totalCriticalPer = StaticManager.Backend.GameData.PlayerGameData.GetStatRatio(Define.StatType.CriticalPer) + StaticManager.Backend.GameData.PlayerTreasure.GetTreasureRatio(Define.StatType.CriticalPer) + StaticManager.Backend.GameData.PlayerDice.GetDiceRatio(Define.StatType.CriticalPer);
        double totalCriticalRatio = StaticManager.Backend.GameData.PlayerGameData.GetStatRatio(Define.StatType.CriticalRatio) + StaticManager.Backend.GameData.PlayerTreasure.GetTreasureRatio(Define.StatType.CriticalRatio) + StaticManager.Backend.GameData.PlayerDice.GetDiceRatio(Define.StatType.CriticalRatio);

        if(totalCriticalPer != 0)  
        {
            //if(CheckAvailableCritical(100))
            if (CheckAvailableCritical((float)totalCriticalPer * 100))      
            {
                // 크리티컬 발동 
                if (totalCriticalRatio == 0)
                {
                    //Debug.Log($"크리티컬 : {finalDefaultDamage * 2}");
                    return finalDefaultDamage * 2; // 기본 2배 
                }
                else
                {
                    return (finalDefaultDamage * 2) * (1 + (float)totalCriticalRatio);  
                }
            }
        }

        return finalDefaultDamage;
    }

    double GetPartnerAttack() // 보유 효과 (Get)
    {
        List<BackendData.GameData.PartnerData> partnerDatas = null;
        partnerDatas = StaticManager.Backend.GameData.PlayerPartner.PartnerList;
        if (partnerDatas.Count == 0)  
            return 0;

        double GetAttacks = 0;  

        for (int i = 0; i < partnerDatas.Count; i++)
        {
            BackendData.Chart.Partner.Item chartItem = null;
            chartItem = StaticManager.Backend.Chart.Partner.GetChartALlItem().Find(item => item.PartnerID == partnerDatas[i].PartnerID);
            double _chartGrowingGetAttack = chartItem.GrowingGetAttack;
            double level = partnerDatas[i].PartnerLevel;
            double _getAttack = _chartGrowingGetAttack * level;
            GetAttacks += _getAttack;
        }

        return GetAttacks;
    }

}
