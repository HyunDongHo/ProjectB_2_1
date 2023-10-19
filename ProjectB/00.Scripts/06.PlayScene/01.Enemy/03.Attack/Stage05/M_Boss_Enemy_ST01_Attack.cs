using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scheduler;

public class M_Boss_Enemy_ST01_Attack : EnemyAttack
{
    public EnemyAttackData attack_01;
    public EnemyAttackData skill_01;
    public EnemyAttackData skill_02;

    protected override void Start()
    {
        base.Start();

        attackMethods.Add(attack_01.random.GetRandomSetting(), Attack_1);
        //skillMethods.Add(skill_01.random.GetRandomSetting(), Skill_1);
        //skillMethods.Add(skill_02.random.GetRandomSetting(), Skill_2);
    }

    protected virtual void Attack_1()
    {
        AttackExistAttackEvent(attack_01.data);

        //StartAttack("M_Boss_ST01_Attack_01",
        //    OnAttack: (frame) =>
        //    {
        //        if (frame == 21 || frame == 48)
        //        {
        //            AttackTargets(divideDamage: 2);
        //        }

        //        control.SetFrameIsPlayHitMotion(frame, 4, 63);
        //    });
    }

    // 애니메이션 제작 예정
    //protected virtual void Skill_1()
    //{
    //    StartAttack("M_Boss_ST01_Skill_01",
    //        OnAttack: (frame) =>
    //        {
    //            if (frame == 10 || frame == 20 || frame == 32 || frame == 37)
    //            {
    //                targetHP?.ReduceHp(gameObject, GetDamage());
    //                ShowAttackEffect(targetHP.GetComponent<Collider>().ClosestPoint(transform.position) + Vector3.up * 0.5f, "Damage_Hit_Spark_01_Y");
    //            }

    //            SetFrameNonStopAttack(frame, 11, 56);
    //        }
    //}

    // 애니메이션 제작 예정
    //protected virtual void Skill_2()
    //{
    //    StartAttack("Enemy_ST01_001_Attack_03",
    //        OnAttack: (frame) =>
    //        {
    //            if (frame == 16 || frame == 32 || frame == 37)
    //            {
    //                targetHP?.ReduceHp(gameObject, GetDamage());
    //                ShowAttackEffect(targetHP.GetComponent<Collider>().ClosestPoint(transform.position) + Vector3.up * 0.5f, "Damage_Hit_Spark_01_Y");
    //            }

    //            SetFrameNonStopAttack(frame, 11, 52);
    //        }
    //}

    protected override void AttackEnd()
    {
        base.AttackEnd();

        (control as EnemyControl).PlayIdleAnimation();
    }
}
