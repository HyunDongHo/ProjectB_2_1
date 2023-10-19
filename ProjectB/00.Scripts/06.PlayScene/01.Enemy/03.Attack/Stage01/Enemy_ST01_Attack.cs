using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scheduler;

public class Enemy_ST01_Attack : EnemyAttack
{
    public EnemyAttackData attack_01;
    public EnemyAttackData attack_02;
    public EnemyAttackData attack_03;
    public EnemyAttackData skill_01;

    public PlayerArrow arrowCreater;

    private const string CREATE_LASER = "CREATE_LASER";
    private const string SHOOT_PROJECTILE = "ShootProjectile";
    private const string Start_PROJECTILE_Setting = "StartProjectileSetting";
    public float projectileSpeed = 8f;

    protected override void Start()
    {
        base.Start();

        attackMethods.Add(attack_01.random.GetRandomSetting(), Attack_1);
        attackMethods.Add(attack_02.random.GetRandomSetting(), Attack_2);
        attackMethods.Add(attack_03.random.GetRandomSetting(), Attack_3);
        skillMethods.Add(skill_01.random.GetRandomSetting(), Skill_1);

    }
    protected override void AddAttackDataEvent(AttackData attackData)
    {
        base.AddAttackDataEvent(attackData);
        AddProjectileDataEvent(attack_01.data);
        //attackData.AddHandleEvent(CommonEnemyAttackDataParameterDefine.)
    }

    protected void AddCreateEffectDataEvent(AttackData attackData)
    {
        attackData.AddHandleEvent(CREATE_LASER,
            (parameter) =>
            {
                CreatedResource createdResource = CreateResourceManager.instance.CreateResource(control.GetModel<Enemy_Stage_01_Model>().eyeTransform.gameObject, "Skill_01_Beam");

                Timer.instance.TimerStart(new TimerBuffer(createdResource.destroyTime),
                    OnFrame: () =>
                    {
                        if (control.GetStats<Stats>().hp.GetCurrentHp() <= 0)
                            createdResource.DestroyCreatedResource();
                    });
            });
    }
    protected void AddProjectileDataEvent(AttackData attackData)
    {
        attackData.AddHandleEvent(Start_PROJECTILE_Setting,
            (parameter) =>
            {
                //control.GetAttack<EnemyAttack>().CompleteAttackWait();
                shootDone = false;
            });
        attackData.AddHandleEvent(SHOOT_PROJECTILE,
            (parameter) =>
            {
                Shoot_Projectile();
                //shootDone = true;          
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
    private void Shoot_Projectile()
    {
        //TODO : control 
        if (shootDone == true)
            return;
        if (control.GetMove<EnemyMove>().isAvailableMove == false || control.GetMove<EnemyMove>().isNowNukbackMove == true || control.GetMove<EnemyMove>().isNowBound == true)
            return;    

        //Debug.Log($"order : {(control.transform.gameObject.name.Substring(22))}");            
        Arrow arrow = arrowCreater.CreateArrow((control as EnemyControl).GetModel<Model>().ProjectileOffset.gameObject);  
        arrow.gameObject.GetComponent<ObjectFollowControl>().target = (control as EnemyControl).GetModel<Model>().ProjectileOffset.gameObject.transform;    
        arrow.gameObject.name += "_"+(control.transform.gameObject.name.Substring(control.transform.gameObject.name.Length-1));  
        if (arrow.gameObject.name.Length > 40)
        {
            arrow.gameObject.name = "ArcherArrow_test(Clone)" + (control.transform.gameObject.name.Substring(control.transform.gameObject.name.Length - 1));
        }

        //arrow.transform.position = transform.position;
        arrow.gameObject.layer = LayerMask.NameToLayer("EnemyProjectile");  
        //arrow.transform.position = (control as EnemyControl).GetModel<Model>().ProjectileOffset.gameObject.transform.position;  
        
        Vector3 forward = (control as EnemyControl).GetModel<Model>().transform.forward;
        arrow.SetTargetPosition(transform.position + forward * 10, forward, (control as EnemyControl));
        shootDone = true;  
        arrow.OnAttack = (Collider) =>
        {
            Control control = Collider.GetComponentInParent<Control>();          
            //DamageDefault(control, DamageType.Default);
            DamageDefault(control, control.ReduceHp(control, GetAttackDamage(), GetCriticalRatio(), 1));
            shootDone = true;
            //   DamageToAttackTargets(control, 1, PlayerHitType.Missile);
        };
        //Arrow arrow = arrowCreater.CreateArrow((control as EnemyControl).GetModel<Model>().ProjectileOffset.gameObject);          
        //arrow.gameObject.layer = LayerMask.NameToLayer("EnemyProjectile");  

        //Vector3 forward = control.GetModel<Model>().transform.forward;
        //arrow.SetTargetPosition(transform.position + forward * 10, forward);

        //arrow.OnAttack = (Collider) =>
        //{
        //    Control control = Collider.GetComponentInParent<Control>();
        //    DamageDefault(control, DamageType.Default);
        // //   DamageToAttackTargets(control, 1, PlayerHitType.Missile);
        //};  
    }
    IEnumerator Shoot_Projectile2()
    {
        //Arrow arrow = arrowCreater.CreateArrow((control as EnemyControl).GetModel<Model>().ProjectileOffset.gameObject);
        Debug.Log($"{control.transform.position.x} {control.transform.position.y} {control.transform.position.z}");
        Arrow arrow = arrowCreater.CreateArrow((control as EnemyControl).GetModel<Model>().ProjectileOffset.gameObject);     
        arrow.gameObject.GetComponent<ObjectFollowControl>().target = (control as EnemyControl).GetModel<Model>().ProjectileOffset.gameObject.transform;    

        //arrow.transform.position = transform.position;
        arrow.gameObject.layer = LayerMask.NameToLayer("EnemyProjectile");  
        //arrow.transform.position = (control as EnemyControl).GetModel<Model>().ProjectileOffset.gameObject.transform.position;  

        Vector3 forward = (control as EnemyControl).GetModel<Model>().transform.forward;
        arrow.SetTargetPosition(transform.position + forward * 10, forward);

        arrow.OnAttack = (Collider) =>
        {
            Control control = Collider.GetComponentInParent<Control>();
            DamageDefault(control, DamageType.Default);
            //   DamageToAttackTargets(control, 1, PlayerHitType.Missile);
        };

        //Arrow arrow = arrowCreater.CreateArrow((control as EnemyControl).GetModel<Model>().ProjectileOffset.gameObject);          
        //arrow.gameObject.layer = LayerMask.NameToLayer("EnemyProjectile");  

        //Vector3 forward = control.GetModel<Model>().transform.forward;
        //arrow.SetTargetPosition(transform.position + forward * 10, forward);

        //arrow.OnAttack = (Collider) =>
        //{
        //    Control control = Collider.GetComponentInParent<Control>();
        //    DamageDefault(control, DamageType.Default);
        // //   DamageToAttackTargets(control, 1, PlayerHitType.Missile);
        //};  
        yield return new WaitForSeconds(0.2f);  
    } 
}
