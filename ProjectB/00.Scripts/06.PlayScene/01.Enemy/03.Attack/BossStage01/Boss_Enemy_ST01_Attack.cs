using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Enemy_ST01_Attack : BossEnemyAttack
{
    public EnemyAttackData attack_01;
  // public EnemyAttackData attack_02;
  // public EnemyAttackData attack_03;
  // public EnemyAttackData attack_04;
    public EnemyAttackData skill_01;

    private const string CREATE_LASER = "CREATE_LASER";

    protected override void Start()
    {
        base.Start();

        attackMethods.Add(attack_01.random.GetRandomSetting(), Attack_1);
        // attackMethods.Add(attack_02.random.GetRandomSetting(), Attack_2);
        // attackMethods.Add(attack_03.random.GetRandomSetting(), Attack_3);
        // attackMethods.Add(attack_04.random.GetRandomSetting(), Attack_4);


        skillMethods.Add(skill_01.random.GetRandomSetting(), Attack_1);
    }

    protected virtual void Attack_1()
    {
       // (control as BossEnemyControl)?.eRaycast.SetRaycast(0);

        AttackExistAttackEvent(attack_01.data);
        AddAttackDataEvent(attack_01.data);

        AddAttackControlDataEvent(attack_01.data);
    }

   // protected virtual void Attack_2()
   // {
   //   //  (control as BossEnemyControl)?.eRaycast.SetRaycast(1);
   //
   //     AttackExistAttackEvent(attack_02.data);
   //     AddAttackDataEvent(attack_02.data);
   //
   //     AddAttackControlDataEvent(attack_02.data); 
   // }
   // protected virtual void Attack_3()
   // {
   //    // (control as BossEnemyControl)?.eRaycast.SetRaycast(2);
   //
   //     AttackExistAttackEvent(attack_03.data);
   //     AddAttackDataEvent(attack_03.data);
   //
   //     AddAttackControlDataEvent(attack_03.data);
   // }
   //
   // protected virtual void Attack_4()
   // {
   //   //  (control as BossEnemyControl)?.eRaycast.SetRaycast(3);
   //
   //     AttackExistAttackEvent(attack_04.data);
   //     AddAttackDataEvent(attack_04.data);
   //
   //     AddAttackControlDataEvent(attack_04.data);
   // }
   //
    protected virtual void Skill_1()
    {
        //(control as BossEnemyControl)?.eRaycast.SetRaycast(2);
    
        AttackExistAttackEvent(skill_01.data);
        AddAttackDataEvent(skill_01.data);

        AddAttackControlDataEvent(skill_01.data);
        //AddBoundDataEvent(skill_01.data);
    }

    protected override void AttackEnd()
    {
        base.AttackEnd();

        (control as EnemyControl).PlayIdleAnimation();
    }
}
