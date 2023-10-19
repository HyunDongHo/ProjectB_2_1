using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scheduler;
using RNG;

[System.Serializable]
public class EnemyAttackData
{
    public AttackData data;
    public RandomSettingContainer random;
}

public class EnemyAttack : Attack
{
    protected Dictionary<RandomSetting, Action> attackMethods = new Dictionary<RandomSetting, Action>();
    protected Dictionary<RandomSetting, Action> skillMethods = new Dictionary<RandomSetting, Action>();
    public bool shootDone = false;
    float defaultTDEnemyAttack = 100;
    protected virtual void Start()
    {
    }

    // Attack Data에 Attack Event가 존재할 시 이 함수 사용. (Attack Event 존재.) 
    protected AttackData AttackExistAttackEvent(AttackData attackData, float attackSpeed = 1, int weight = 0, Action OnStartAttack = null, Action OnComplete = null)
    {
        StartAttack(attackData, attackSpeed, weight,
            () =>
            {
                OnStartAttack?.Invoke();
            },
            () =>
            {
                OnComplete?.Invoke();
            });
        AddAttackDataEvent(attackData);

        return attackData;
    }

    // Attack Data에 Attack Event가 존재하지 않을 때 이 함수 사용. (Attack Event 존재하지 않음.) 
    protected AttackData AttackNotExistAttackEvent(AttackData attackData, float attackSpeed = 1, int weight = 0, Action OnStartAttack = null, Action OnComplete = null)
    {
        StartAttack(attackData, attackSpeed, weight,
            () =>
            {
                OnStartAttack?.Invoke();
            },
            () =>
            {
                OnComplete?.Invoke();
            });

        return attackData;
    }

    protected override void AddDefaultAttackDataEvent(AttackData attackData)
    {
        base.AddDefaultAttackDataEvent(attackData);
    }

    protected virtual void AddAttackDataEvent(AttackData attackData)
    {
        attackData.AddHandleEvent(CommonEnemyAttackDataParameterDefine.ATTACK,
            (parameter) =>
            {
                AttackTargets(GetAttackDataEventCount(attackData));
            });

        //attackData.AddHandleEvent(CommonEnemyAttackDataParameterDefine.)
    }

    protected virtual int GetAttackDataEventCount(AttackData attackData)
    {
        return attackData.GetAttackMethodsCount(CommonEnemyAttackDataParameterDefine.ATTACK);
    }

    public void StartRandomAttack()
    {
        if (!IsAvaliableAttack()) 
            return;

        // 넉백 바운드 판별
        if (control.GetMove<Move>().isNowBound || control.GetMove<Move>().isNowNukbackMove)
            return;

        List<RandomSetting> randomSettings = new List<RandomSetting>();

        // 임시로 스킬과 일반공격의 확률을 합침. (이후 스킬과 일반공격의 형태가 정해지면 변경 해야함.)
      //  if (skillMethods.Count > 0)
      //  {
      //      foreach (var skillMethod in skillMethods)
      //          randomSettings.Add(skillMethod.Key);
      //  }

        if (attackMethods.Count > 0)
        {
            foreach (var attackMethod in attackMethods)
                randomSettings.Add(attackMethod.Key);
        }

        if (randomSettings.Count > 0)
        {
            RandomSetting result = RNGManager.instance.GetRandom(randomSettings.ToArray());

            if (attackMethods.ContainsKey(result))
                attackMethods[result]?.Invoke();
           // else if (skillMethods.ContainsKey(result))
           //     skillMethods[result]?.Invoke();
        }
    }

    public void StartSkillAttack()
    {
        foreach (var skillMethod in skillMethods)
            skillMethod.Value.Invoke();
    }

    protected void AttackTargets(int divideDamage, bool isPlayDefaultParticle = true)
    {
        DamageToTarget(divideDamage, isPlayDefaultParticle);
    }

    private bool DamageToTarget(int divideDamage, bool isPlayDefaultParticle = true)  
    {
        int attackCount = 0;

        foreach (var target in attackTargets)
        {
            if (target.Value == null) continue;

            DamageType damageType = target.Value.ReduceHp(control, GetAttackDamage(), GetCriticalRatio(), divideDamage);

            attackCount++; 

            DamageDefault(target.Value, damageType, isPlayDefaultParticle);
        }

        return attackCount > 0;
    }

    public void DamageDefault(Control target, DamageType damageType, bool isPlayDefaultParticle = true)
    {
        Model model = target.GetModel<Model>();
        Bounds bounds = new Bounds(model.bodyOffset.position, model.bodyOffset.localScale);

        //if (damageType == DamageType.Miss)
        //    ShowDamageManager.instance.ShowMiss(model.headOffset.position);

        if (isPlayDefaultParticle && target.GetStats<Stats>().hp.isAvailableReduceHp)
            ShowAttackEffect(bounds.ClosestPoint(control.GetModel<Model>().bodyOffset.position), "Damage_Hit_Spark_01_Y");

    }

    public void DamageRangeAttack(Control target, bool isPlayDefaultParticle = true)
    {
            if (target == null) return;

        target.ReduceHp(control, GetAttackDamage(), GetCriticalRatio(), 1);
        Model model = target.GetModel<Model>();
        Bounds bounds = new Bounds(model.bodyOffset.position, model.bodyOffset.localScale);

        if (isPlayDefaultParticle && target.GetStats<Stats>().hp.isAvailableReduceHp)
            ShowAttackEffect(bounds.ClosestPoint(control.GetModel<Model>().bodyOffset.position), "Damage_Hit_Spark_01_Y");

    }
  
    public override float GetAttackDamage()
    {
        //Debug.Log($"control name : {control.name}");
        //float damage = base.GetAttackDamage();

        //StatsManager statsManager = control.GetStats<Stats>().manager;

        //damage = UnityEngine.Random.Range(statsManager.GetValue(EnemyStatsValueDefine.BaseAttackDamageMin), statsManager.GetValue(EnemyStatsValueDefine.BaseAttackDamageMax));

        float damage = 0;
        if ((StageManager.instance as GamePlayManager).enemyManager.GetStageType() == StageTypeNum.TrainingDungeon) // 던전용 
        {
            if((StageManager.instance as GamePlayManager).enemyManager.GetTDPlayerType() != PlayerType.None)
            {
                switch ((StageManager.instance as GamePlayManager).enemyManager.GetTDPlayerType())
                {
                    case PlayerType.Warrior:
                        damage = Define.Util.GetExpressionValue(Define.ExpressionType.WarriorTrainingATK, (StageManager.instance as GamePlayManager).enemyManager.dieMonsterCount);
                        break;
                    case PlayerType.Archer:
                        damage = Define.Util.GetExpressionValue(Define.ExpressionType.ArcherTrainingATK, (StageManager.instance as GamePlayManager).enemyManager.dieMonsterCount);
                        break;
                    case PlayerType.Wizard:
                        damage = Define.Util.GetExpressionValue(Define.ExpressionType.WizardTrainingATK, (StageManager.instance as GamePlayManager).enemyManager.dieMonsterCount);
                        break;
                }

                if (damage == 0)
                    damage = defaultTDEnemyAttack;
            }
            Debug.Log($"던전 damage : {damage}");
        }
        else
        {
            switch ((control as EnemyControl).enemyType)
            {
                case EnemyType.Normal:
                    damage = Define.Util.GetExpressionValue(Define.ExpressionType.NormalATK, StaticManager.Backend.GameData.PlayerGameData.NowStageLevel);
                    break;
                case EnemyType.Boss:
                    damage = Define.Util.GetExpressionValue(Define.ExpressionType.BossATK, StaticManager.Backend.GameData.PlayerGameData.NowStageLevel);
                    break;
            }
        }


        //switch ((control as EnemyControl).enemyType)
        //{
        //    case EnemyType.Normal:
        //        damage = Define.Util.GetExpressionValue(Define.ExpressionType.NormalATK, StaticManager.Backend.GameData.PlayerGameData.NowStageLevel);
        //        break;
        //    case EnemyType.Boss:
        //        damage = Define.Util.GetExpressionValue(Define.ExpressionType.BossATK, StaticManager.Backend.GameData.PlayerGameData.NowStageLevel);
        //        break;
        //}

        //Debug.Log($"{control.name} damage : {damage}");  
        return damage;
    }
}
