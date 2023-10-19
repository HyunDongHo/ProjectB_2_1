using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scheduler;

public class Enemy_Stage_04_Attack : EnemyAttack
{
    private const string SHOOT_PROJECTILE_LEFT = "ShootProjectileLeft";
    private const string SHOOT_PROJECTILE_RIGHT = "ShootProjectileRight";

    public float projectileSpeed = 25f;

    public EnemyAttackData attack_01;
    public EnemyAttackData attack_02;
    public EnemyAttackData skill_01;

    protected override void Start()
    {
        base.Start();

        attackMethods.Add(attack_01.random.GetRandomSetting(), Attack_1);
        attackMethods.Add(attack_02.random.GetRandomSetting(), Attack_2);
        skillMethods.Add(skill_01.random.GetRandomSetting(), Skill_1);
    }

    protected void AddProjectileDataEvent(AttackData attackData)
    {
        attackData.AddHandleEvent(SHOOT_PROJECTILE_LEFT,
            (parameter) =>
            {
                Shoot_Projectile((control.GetModel<Model>() as Enemy_Stage_04_Model).leftFinger.position);
            });

        attackData.AddHandleEvent(SHOOT_PROJECTILE_RIGHT,
            (parameter) =>
            {
                Shoot_Projectile((control.GetModel<Model>() as Enemy_Stage_04_Model).rightFinger.position);
            });
    }

    protected virtual void Attack_1()
    {
        AttackExistAttackEvent(attack_01.data);
    }

    protected virtual void Attack_2()
    {
        AttackExistAttackEvent(attack_02.data);
        AddProjectileDataEvent(attack_02.data);
    }

    private void Shoot_Projectile(Vector3 createPosition)
    {
        int randomTarget = Random.Range(0, attackTargets.Values.Count);
        int currentTarget = 0;

        Control targetControl = null;
        foreach (var attackTarget in attackTargets)
        {
            if (currentTarget == randomTarget)
            {
                targetControl = attackTarget.Value;
                break;
            }
        }

        if (targetControl != null)
        {
            Vector3 targetPosition = targetControl.GetModel<Model>().bodyOffset.position;

            CreatedResource createdResource = CreateResourceManager.instance.CreateResource(gameObject, "Attack_02_Missile", createPosition);

            createdResource.transform.forward = (targetPosition - createdResource.transform.position).normalized;
            createdResource?.MoveToPosition(projectileSpeed, targetPosition,
                OnComplete: () =>
                {
                    AttackTargets(divideDamage: 8, isPlayDefaultParticle: false);
                    ShowAttackEffect(targetPosition, "Bullet_Hit_01");

                    createdResource.DestroyCreatedResource();
                });
        }
    }

    protected virtual void Skill_1()
    {
        AttackExistAttackEvent(skill_01.data);
    }

    protected override void AttackEnd()
    {
        base.AttackEnd();

        (control as EnemyControl).PlayIdleAnimation();
    }
}
