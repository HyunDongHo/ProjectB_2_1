using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scheduler;

public class Enemy_ST02_Attack : EnemyAttack
{
    public EnemyAttackData attack_01;
    public EnemyAttackData attack_02;
    public EnemyAttackData attack_03;

    protected override void Start()
    {
        base.Start();

        attackMethods.Add(attack_01.random.GetRandomSetting(), Attack_1);
        attackMethods.Add(attack_02.random.GetRandomSetting(), Attack_2);
        skillMethods.Add(attack_03.random.GetRandomSetting(), Attack_3);
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

    protected override void AttackEnd()
    {
        base.AttackEnd();

        (control as EnemyControl).PlayIdleAnimation();
    }
}
