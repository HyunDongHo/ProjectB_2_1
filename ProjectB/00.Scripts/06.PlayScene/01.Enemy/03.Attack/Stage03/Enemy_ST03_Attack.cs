using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scheduler;

public class Enemy_ST03_Attack : EnemyAttack
{
    public EnemyAttackData attack_01;
    public EnemyAttackData idleTransfom;
    public EnemyAttackData skill_01;

    public float defenseTime = 3.0f;

    protected override void Start()
    {
        base.Start();

        attackMethods.Add(attack_01.random.GetRandomSetting(), Attack_1);
        //attackMethods.Add(idleTransfom.random.GetRandomSetting(), IdleTransform);
        //skillMethods.Add(skill_01.random.GetRandomSetting(), Skill_1);
    }

    protected virtual void Attack_1()
    {
        AttackExistAttackEvent(attack_01.data);
    }

    protected virtual void IdleTransform()
    {
        AttackExistAttackEvent(idleTransfom.data);

        //꽦꽦
        //StartAttack("Enemy_ST03_001_Idle_Transform",
        //    OnAttack: (frame) =>
        //    {
        //        if (frame >= 17)
        //        {
        //            SetDefenseMode();
        //        }
        //    });
    }

    private void SetDefenseMode()
    {
        control.GetStats<EnemyStats>().hp.isAvailableReduceHp = false;
        control.SetIsPlayHitMotion(false);

        string animationName = "Enemy_ST03_001_Idle_Transform";
        Timer.instance.TimerStart(new TimerBuffer(defenseTime),
            OnFrame: () =>
            {
                control.GetModel<Model>().animationControl.PlayAnimation(animationName, startNormalizedTime: control.GetModel<Model>().animationControl.GetFrameToTime(animationName, 19));
            },
            OnComplete: () =>
            {
                control.GetStats<EnemyStats>().hp.isAvailableReduceHp = true;
                control.SetIsPlayHitMotion(true);

                AttackEnd();
            });
    }

    // 애니메이션 제작 예정
    //protected virtual void Skill_1()
    //{
    //    StartAttack("Enemy_ST03_002_Attack",
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

    protected override void AttackEnd()
    {
        base.AttackEnd();

        (control as EnemyControl).PlayIdleAnimation();
    }
}