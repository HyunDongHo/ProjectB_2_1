using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scheduler;

public class Enemy_ST01_H_Attack : EnemyAttack
{
    public EnemyAttackData attack_01;
    public EnemyAttackData attack_02;
    public EnemyAttackData attack_03;
    public EnemyAttackData skill_01;

    private const string CREATE_LASER = "CREATE_LASER";

    protected override void Start()
    {
        base.Start();

        attackMethods.Add(attack_01.random.GetRandomSetting(), Attack_1);
        attackMethods.Add(attack_02.random.GetRandomSetting(), Attack_2);
        attackMethods.Add(attack_03.random.GetRandomSetting(), Attack_3);
        skillMethods.Add(skill_01.random.GetRandomSetting(), Skill_1);
    }

    protected void AddCreateEffectDataEvent(AttackData attackData)
    {
        attackData.AddHandleEvent(CREATE_LASER,
            (parameter) =>
            {
                CreatedResource createdResource = CreateResourceManager.instance.CreateResource(control.GetModel<Enemy_Stage_01_H_Model>().eyeTransform.gameObject, "Skill_01_Beam");

                Timer.instance.TimerStart(new TimerBuffer(createdResource.destroyTime),
                    OnFrame: () =>
                    {
                        if (control.GetStats<Stats>().hp.GetCurrentHp() <= 0)
                            createdResource?.DestroyCreatedResource();
                    });
            });
    }

    protected virtual void Attack_1()
    {
        AttackExistAttackEvent(attack_01.data);
    }

    protected virtual void Attack_2()
    {
        AttackExistAttackEvent(attack_02.data);
    }

    protected virtual void Attack_3()
    {
        AttackExistAttackEvent(attack_03.data);
    }

    protected virtual void Skill_1()
    {
        AttackExistAttackEvent(skill_01.data);
        AddCreateEffectDataEvent(skill_01.data);
    }

    protected override void AttackEnd()
    {
        base.AttackEnd();

        (control as EnemyControl).PlayIdleAnimation();
    }
}
