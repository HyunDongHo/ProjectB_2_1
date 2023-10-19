using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scheduler;
using DG.Tweening;

public class PlayerSkill : MonoBehaviour
{
    [Header("[ Player ]")]
    public PlayerType playerType;
    //public PlayerMissile playerMissile;
    public PlayerRaycast_GamePlay pRaycast_temp;
    [Space(15f)]

    [Header("[ Player Skill Raycast Range ]")]
    public float defaultRaycastNum;
    public float skRaycastNum1;
    public float skRaycastNum2;
    public float skRaycastNum3;
    public float skRaycastNum4;
    [Space(10f)]

    [Header("[ Player Skill Z Offset ]")]
    public float defaultOffsetZ;
    public float offsetZ_1;
    public float offsetZ_2;
    public float offsetZ_3;
    public float offsetZ_4;     
    [Space(10f)]

    [Header("[ Player Archer Arrows Type]")]
    public PlayerArrow FanArrow;
    public PlayerArrow ArcherSkillArrow02;
    public PlayerArrow ExplodeArrow;
    public PlayerArrow FallingArrow;
    [Space(10f)]

    [Header("[ Falling Arrow Detail InFo ]")] 
    public int totalArrowCnt;
    public float timeDistance;
    public float MaxDistance1;
    [Space(10f)]

    [Header("[ Player Wizard Arrows Type]")]
    public PlayerArrow FireBall;
    public PlayerArrow Wind;
    public PlayerArrow LightningBolt;
    public PlayerArrow NormalAttack;
    public PlayerArrow Skill04Projectile;

    [Space(10f)]

    [Header("[ Wind Detail Info]")]
    public float MaxDistance2;
    public float WindTime;
    [Space(10f)]

    [Header("[ Lightning Bolt Detail Info]")]
    public int LB_totalCnt;
    public float LB_timeDistance;
    public float LB_maxDistance;
    [Space(10f)]

    [Header("[ Sphere Detail Info]")]
    public int S_totalCnt;
    public float S_timeDistance;
    public float S_maxDistance;

    public class SkillParameter
    {
        public Action OnStart;
        public Action OnEnd;

        public PlayerControl control;    
        public PlayerAttack attack;

        public SkillParameter(Action OnStart, Action OnEnd, PlayerControl control, PlayerAttack attack)
        {
            this.OnStart = OnStart;
            this.OnEnd = OnEnd;

            this.control = control;  
            this.attack = attack;
        }
    }

    //private void OnDisable()
    //{
    //    StopAllCoroutines();
    //}
    public void UseSkill(int index, SkillParameter skillParameter)
    {
        skillParameter.attack.StopAttack();  

        string skillCameraCam = string.Empty;    
          
        switch (index)       
        {
            case 0:
                //skillCameraCam = "Skill_01_Cam";
                UseSkill_1(skillParameter);        
                break;
            case 1:
                //skillCameraCam = "Skill_02_Cam";
                UseSkill_2(skillParameter);
                break;  
            case 2:
                //skillCameraCam = "Skill_03_Cam";
                UseSkill_3(skillParameter);
                break;
            case 3:
                //skillCameraCam = "Skill_04_Cam";
                UseSkill_4(skillParameter);   
                break;
        }

        if (!string.IsNullOrEmpty(skillCameraCam))
            skillParameter.control.utility.subCamera.PlayCameraAnimation(skillCameraCam, DefineManager.DEFAULT_CAMERA_TIME);
    }

    private void AddSkillExistAttackEvent(SkillParameter skillParameter, AttackData attackData, float cameraShakePower, float cameraShakeTime, float addSp, PlayerAttackType skillType)
    {
        //skillParameter.attack.AttackExistAttackEvent(PlayerAttackType.Skill, attackData, cameraShakePower, cameraShakeTime, addSp, weight: (int)PlayerAnimationWeight.Skill,
        //    OnStartAttack: () => skillParameter.OnStart?.Invoke(), OnComplete: () => skillParameter.OnEnd?.Invoke());
        skillParameter.attack.AttackExistAttackEvent(skillType, attackData, cameraShakePower, cameraShakeTime, addSp, weight: (int)PlayerAnimationWeight.Skill,
            OnStartAttack: () => skillParameter.OnStart?.Invoke(), OnComplete: () => skillParameter.OnEnd?.Invoke());
    }

    private void AddSkillNotExistAttackEvent(SkillParameter skillParameter, AttackData attackData)
    {
        skillParameter.attack.AddAttackNotExistAttackEvent(PlayerAttackType.Skill, attackData, weight: (int)PlayerAnimationWeight.Skill,
            OnStartAttack: () => skillParameter.OnStart?.Invoke(), OnComplete: () => skillParameter.OnEnd?.Invoke());
    }

    #region Use Skill Index

    private void UseSkill_1(SkillParameter skillParameter)
    {
        if ((StageManager.instance as GamePlayManager) == null)
            return;

        if ((StageManager.instance as GamePlayManager).enemyManager.isStageEnd == true)
            return;

        Debug.Log("UseSkill_1");

        AttackData attackData = skillParameter.control.utility.attackDatas[0].skill_01;
        pRaycast_temp.attackRangeRaycast.radius = skRaycastNum1;
        pRaycast_temp.attackRangeRaycast.offset.z = offsetZ_1;

        AddSkillExistAttackEvent(skillParameter, attackData, cameraShakePower: 0, cameraShakeTime: 0, addSp: 0, PlayerAttackType.Skill0);

        switch (playerType)
        {
            case PlayerType.Warrior:
                Skill_1_Warrior(attackData, skillParameter);
                break;
            case PlayerType.Archer:
                Skill_1_Archer(attackData, skillParameter);
                break;
            case PlayerType.Wizard:
                Skill_1_Wizard(attackData, skillParameter);
                break;
        }

    }
    private void Skill_1_Warrior(AttackData attackData, SkillParameter skillParameter)
    {
        attackData.AddHandleEvent(CommonPlayerAttackDataParameterDefine.ANIMATION_END_FRAME,
            (parameter) =>
            {
                skillParameter.control.utility.animationEnd.AnimationEnd(defaultRaycastNum);  
            });
    }
    private void Skill_1_Archer(AttackData attackData, SkillParameter skillParameter)
    {
        attackData.AddHandleEvent(CommonPlayerAttackDataParameterDefine.ANIMATION_END_FRAME,
            (parameter) =>
            {
                pRaycast_temp.attackRangeRaycast.offset.z = defaultOffsetZ;
                pRaycast_temp.attackRangeRaycast.radius = defaultRaycastNum;
                //skillParameter.control.utility.animationEnd.AnimationEnd(defaultRaycastNum);
            });

        attackData.AddHandleEvent(CommonPlayerAttackDataParameterDefine.ShootNormalTripleArrow,
               (parameter) =>
               {
                   // TODO : 파라미터로 갯수를 받아서  (3개, 5개 ,7개, 9개 ) 2개씩 붙여서 늘어가게끔
                   // -> 처음에는 옆에 나가는 화살 각도를 15도, 30도, 45도 (사이각도 15도로 균일하게)

                   Arrow centerArrow = FanArrow.CreateArrow((skillParameter.control).GetModel<Model>().ProjectileOffset.gameObject);
                   centerArrow.gameObject.layer = LayerMask.NameToLayer("Projectile");
                   Vector3 forward = skillParameter.control.GetModel<Model>().transform.forward;  
                   centerArrow.SetTargetPosition(transform.position + forward * centerArrow.speed, forward, 0.5f); 


                   centerArrow.OnAttack = (Collider) =>
                   {
                       Control control = Collider.GetComponentInParent<Control>();
                       //control.GetMove<Move>().KnockBack(1.2f, animTime, centerArrow.transform.forward);

                       if(control is BossEnemyControl == false)
                       {
                           (control as EnemyControl)?.PlayBoundAnimation(null);
                           control.GetMove<Move>().Bound(1.5f, 1.5f, 0.5f, 2, centerArrow.transform.forward);
                       }

                       CreateResourceManager.instance.CreateResource(centerArrow.gameObject, createResourceData: centerArrow.hitObj);
                       skillParameter.control.GetAttack<PlayerAttack>().DamageToAttackTargets(control, 1, PlayerHitType.NoEffect, PlayerAttackType.Skill0);
                       //DamageToAttackTargets(control, 1, PlayerHitType.Missile); 
                   };

                   AddFanArrow(skillParameter, forward, centerArrow, 15);

                   if (parameter.intValue == 5)
                   {
                       AddFanArrow(skillParameter, forward, centerArrow, 30);
                   }
                   else if (parameter.intValue == 7)
                   {
                       AddFanArrow(skillParameter, forward, centerArrow, 30);
                       AddFanArrow(skillParameter, forward, centerArrow, 45);      
                   }
                   else if (parameter.intValue == 9)
                   {
                       AddFanArrow(skillParameter, forward, centerArrow, 30);
                       AddFanArrow(skillParameter, forward, centerArrow, 45);
                       AddFanArrow(skillParameter, forward, centerArrow, 60);  
                   }
               });
    }

    private void AddFanArrow(SkillParameter skillParameter, Vector3 forward, Arrow centerArrow, float degree)
    {
        Arrow addLeftArrow = FanArrow.CreateArrow((skillParameter.control).GetModel<Model>().ProjectileOffset.gameObject);
        addLeftArrow.gameObject.layer = LayerMask.NameToLayer("Projectile");
        Arrow addRightArrow = FanArrow.CreateArrow((skillParameter.control).GetModel<Model>().ProjectileOffset.gameObject);
        addRightArrow.gameObject.layer = LayerMask.NameToLayer("Projectile");

        Vector3 right = ((skillParameter.control.GetModel<Model>().transform.right + forward * Mathf.Tan((float)Math.PI / 180 * (90 - degree)))).normalized;
        Vector3 left = ((-(skillParameter.control.GetModel<Model>().transform.right) + forward * Mathf.Tan((float)Math.PI / 180 * (90 - degree)))).normalized;

        addRightArrow.SetTargetPosition(transform.position + right * addRightArrow.speed, right, 0.5f);
        addLeftArrow.SetTargetPosition(transform.position + left * addLeftArrow.speed, left, 0.5f);

        addRightArrow.OnAttack = (Collider) =>
        {
            Control control = Collider.GetComponentInParent<Control>();
            if (control is BossEnemyControl == false)
            {                
                (control as EnemyControl)?.PlayBoundAnimation(null);
                control.GetMove<Move>().Bound(1.5f, 1.5f, 0.5f, 2, centerArrow.transform.forward);
            }

            CreateResourceManager.instance.CreateResource(centerArrow.gameObject, createResourceData: centerArrow.hitObj);
            skillParameter.control.GetAttack<PlayerAttack>().DamageToAttackTargets(control, 1, PlayerHitType.NoEffect, PlayerAttackType.Skill0);            
        };

        addLeftArrow.OnAttack = (Collider) =>
        {
            Control control = Collider.GetComponentInParent<Control>();

            if (control is BossEnemyControl == false)
            {
                (control as EnemyControl)?.PlayBoundAnimation(null);
                control.GetMove<Move>().Bound(1.5f, 1.5f, 0.5f, 2, centerArrow.transform.forward);
            }

            CreateResourceManager.instance.CreateResource(centerArrow.gameObject, createResourceData: centerArrow.hitObj);
            skillParameter.control.GetAttack<PlayerAttack>().DamageToAttackTargets(control, 1, PlayerHitType.NoEffect, PlayerAttackType.Skill0);
        };
    }
    private void Skill_1_Wizard(AttackData attackData, SkillParameter skillParameter)
    {
        attackData.AddHandleEvent(CommonPlayerAttackDataParameterDefine.ANIMATION_END_FRAME,
            (parameter) =>
            {
                pRaycast_temp.attackRangeRaycast.offset.z = defaultOffsetZ;
                pRaycast_temp.attackRangeRaycast.radius = defaultRaycastNum;
            //skillParameter.control.utility.animationEnd.AnimationEnd(defaultRaycastNum);
            });

        attackData.AddHandleEvent(CommonPlayerAttackDataParameterDefine.SKILL_SHOOT_MISSILE,
            (parameter) =>
            {
                Arrow centerArrow = FireBall.CreateArrow((skillParameter.control).GetModel<Model>().ProjectileOffset.gameObject);
                centerArrow.gameObject.layer = LayerMask.NameToLayer("Projectile");
                GameObject nearMonster = (StageManager.instance as GamePlayManager).enemyManager.GetNearMonster(transform.position);
            
                if (nearMonster != null)
                    skillParameter.control.utility.modelPivot.transform.LookAt(nearMonster.transform);

                Vector3 forward = skillParameter.control.GetModel<Model>().transform.forward;
                centerArrow.SetTargetPosition(transform.position + forward * centerArrow.speed + new Vector3(0, 1, 0), forward, 0.5f);


                centerArrow.OnAttack = (Collider) =>
                {
                    Control control = Collider.GetComponentInParent<Control>();
                    //control.GetMove<Move>().KnockBack(1.2f, animTime, centerArrow.transform.forward);

                    if (control is BossEnemyControl == false)
                    {
                        (control as EnemyControl)?.PlayBoundAnimation(null);
                        control.GetMove<Move>().Bound(1.5f, 1.5f, 0.5f, 2, centerArrow.transform.forward);
                    }

                    skillParameter.control.GetAttack<PlayerAttack>().DamageToAttackTargets(control, 1, PlayerHitType.Missile , PlayerAttackType.Skill0);
                    //DamageToAttackTargets(control, 1, PlayerHitType.Missile); 
                };

            });

    }
    private void UseSkill_2(SkillParameter skillParameter)
    {
        if ((StageManager.instance as GamePlayManager) == null)
            return;
        if ((StageManager.instance as GamePlayManager).enemyManager.isStageEnd == true)
            return;

        Debug.Log("UseSkill_2");


        //AttackData attackData = skillParameter.control.utility.attackDatas[(int)skillParameter.attack.playerModelType].skill_02;
        AttackData attackData = skillParameter.control.utility.attackDatas[0].skill_02;
        pRaycast_temp.attackRangeRaycast.radius = skRaycastNum2;
        pRaycast_temp.attackRangeRaycast.offset.z = offsetZ_2;

        AddSkillExistAttackEvent(skillParameter, attackData, cameraShakePower: 0, cameraShakeTime: 0, addSp: 0, PlayerAttackType.Skill1);

        switch (playerType)
        {
            case PlayerType.Warrior:
                Skill_2_Warrior(attackData, skillParameter);
                break;
            case PlayerType.Archer:
                Skill_2_Archer(attackData, skillParameter);
                break;
            case PlayerType.Wizard:
                Skill_2_Wizard(attackData, skillParameter);
                break;
        }

    }
    private void Skill_2_Warrior(AttackData attackData, SkillParameter skillParameter)
    {
        attackData.AddHandleEvent(CommonPlayerAttackDataParameterDefine.ANIMATION_END_FRAME,
            (parameter) =>
            {
                skillParameter.control.utility.animationEnd.AnimationEnd(defaultRaycastNum);
            });
    }
    private void Skill_3_Archer(AttackData attackData, SkillParameter skillParameter)
    {
        attackData.AddHandleEvent(CommonPlayerAttackDataParameterDefine.ANIMATION_END_FRAME,
            (parameter) =>
            {
                pRaycast_temp.attackRangeRaycast.offset.z = defaultOffsetZ;
                pRaycast_temp.attackRangeRaycast.radius = defaultRaycastNum;        
                //skillParameter.control.utility.animationEnd.AnimationEnd(defaultRaycastNum);
            });

        attackData.AddHandleEvent(CommonPlayerAttackDataParameterDefine.ShootNormalArrow,
               (parameter) =>
               {
                   GameObject nearMonster = (StageManager.instance as GamePlayManager).enemyManager.GetNearMonster(transform.position);
                   if (nearMonster != null)
                   {
                       skillParameter.control.utility.modelPivot.transform.LookAt(nearMonster.transform);

                       Arrow centerArrow = ExplodeArrow.CreateArrow((skillParameter.control).GetModel<Model>().ProjectileOffset.gameObject);
                       centerArrow.gameObject.layer = LayerMask.NameToLayer("Projectile");

                       centerArrow.transform.position = transform.position;

                       ParticleSystem ps = centerArrow.efx.GetComponent<ParticleSystem>();  
                       ps.Play();

                     
                   }

               });


    }
    private void Skill_2_Wizard(AttackData attackData, SkillParameter skillParameter)
    {
        attackData.AddHandleEvent(CommonPlayerAttackDataParameterDefine.ANIMATION_END_FRAME,
            (parameter) =>
            {
                pRaycast_temp.attackRangeRaycast.offset.z = defaultOffsetZ;
                pRaycast_temp.attackRangeRaycast.radius = defaultRaycastNum;
                //skillParameter.control.utility.animationEnd.AnimationEnd(defaultRaycastNum);
            });

        attackData.AddHandleEvent(CommonPlayerAttackDataParameterDefine.SKILL_SHOOT_MISSILE,
            (parameter) =>
            {
                //if (monsterList.Count > 0)
                    StartCoroutine(CreateSphere(skillParameter, null));

                    
            });
    }

    IEnumerator CreateSphere(SkillParameter skillParameter, List<GameObject> Monsters)
    {
        for (int i=0;i<S_totalCnt; i++)
        {
            List<GameObject> monsterLists = (StageManager.instance as GamePlayManager).enemyManager.GetAllMonster(transform.position, S_maxDistance);    

            if (monsterLists.Count < 1)
                continue;  

            Arrow centerArrow = Skill04Projectile.CreateArrow((skillParameter.control).GetModel<Model>().ProjectileOffset.gameObject);
            centerArrow.gameObject.layer = LayerMask.NameToLayer("Projectile");
           // centerArrow.collider.enabled = false;

            centerArrow.transform.position = transform.position + new Vector3(0, 2.5f, 0f);
            int randInt = UnityEngine.Random.Range(0, monsterLists.Count);

            // 직선 
           Vector3 dir = (monsterLists[randInt].transform.position - centerArrow.transform.position).normalized;
           centerArrow.SetTargetPosition(monsterLists[randInt].transform.position + (Vector3.up * 0.5f), dir, 0.4f, () =>
           {
            //   centerArrow.collider.enabled = true;
               CreateResourceManager.instance.CreateResource(centerArrow.gameObject, createResourceData: centerArrow.hitObj);                    
           });
           

            // TODO : 곡선 
         //  Vector3 hpos = centerArrow.transform.position + ((Monsters[randInt].transform.position - centerArrow.transform.position) / 1.5f);
         // 
         //  Vector3[] Jumppath ={new Vector3(centerArrow.transform.position.x,centerArrow.transform.position.y, centerArrow.transform.position.z),
         //   new Vector3(hpos.x,hpos.y+3f,hpos.z),
         //   new Vector3(Monsters[randInt].transform.position.x,Monsters[randInt].transform.position.y,Monsters[randInt].transform.position.z)};

          //  centerArrow.SetTargetPosition_curve(Jumppath, 0.8f);

            centerArrow.OnAttack = (Collider) =>
            {
                Control control = Collider.GetComponentInParent<Control>();

                if (control is BossEnemyControl == false)
                {
                    (control as EnemyControl)?.PlayBoundAnimation(null);

                    Vector3 boundDir = new Vector3(centerArrow.transform.forward.x, 0, centerArrow.transform.forward.z);

                    control.GetMove<Move>().Bound(1.0f, 1.0f, 0.5f, 2, -control.transform.forward);  
                }

                skillParameter.control.GetAttack<PlayerAttack>().DamageToAttackTargets(control, 1, PlayerHitType.NoEffect, PlayerAttackType.Skill1);
            };

            yield return new WaitForSeconds(S_timeDistance);
        }
        

    }

    private void UseSkill_3(SkillParameter skillParameter)
    {
        if ((StageManager.instance as GamePlayManager) == null)
            return;
        if ((StageManager.instance as GamePlayManager).enemyManager.isStageEnd == true)
            return;
        Debug.Log("UseSkill_3");

        //AttackData attackData = skillParameter.control.utility.attackDatas[(int)skillParameter.attack.playerModelType].skill_03;
        AttackData attackData = skillParameter.control.utility.attackDatas[0].skill_03;
        pRaycast_temp.attackRangeRaycast.radius = skRaycastNum3;
        pRaycast_temp.attackRangeRaycast.offset.z = offsetZ_3;

        AddSkillExistAttackEvent(skillParameter, attackData, cameraShakePower: 0, cameraShakeTime: 0, addSp: 0 , PlayerAttackType.Skill2);  

        switch (playerType)
        {
            case PlayerType.Warrior:
                Skill_3_Warrior(attackData, skillParameter);    
                break;
            case PlayerType.Archer:
                Skill_3_Archer(attackData, skillParameter);
                break;
            case PlayerType.Wizard:
                Skill_3_Wizard(attackData, skillParameter);       
                break;
        }
    }
    private void Skill_3_Warrior(AttackData attackData, SkillParameter skillParameter)
    {
        attackData.AddHandleEvent(CommonPlayerAttackDataParameterDefine.ANIMATION_END_FRAME,
            (parameter) =>
            {
                skillParameter.control.utility.animationEnd.AnimationEnd(defaultRaycastNum);
            });

        attackData.AddHandleEvent(CommonPlayerAttackDataParameterDefine.JUMP_TO_NEAR_TARGET,
            (parameter) =>
            {
                skillParameter.control.GetMove<PlayerMove_Warrior>().MoveToMonsterOnTop();
            });
    }
    private void Skill_2_Archer(AttackData attackData, SkillParameter skillParameter)
    {
        attackData.AddHandleEvent(CommonPlayerAttackDataParameterDefine.ANIMATION_END_FRAME,
             (parameter) =>
             {
                 pRaycast_temp.attackRangeRaycast.offset.z = defaultOffsetZ;
                 pRaycast_temp.attackRangeRaycast.radius = defaultRaycastNum;         
            });

        attackData.AddHandleEvent(CommonPlayerAttackDataParameterDefine.ShootArrowSkill02,
            (parameter) =>
            {
                GameObject nearMonster = (StageManager.instance as GamePlayManager).enemyManager.GetNearMonster(transform.position);

                Arrow centerArrow = ArcherSkillArrow02.CreateArrow((skillParameter.control).GetModel<Model>().ProjectileOffset.gameObject);

                if (centerArrow == null || nearMonster == null)  
                    return;

                ParticleSystem ps = centerArrow.efx.GetComponent<ParticleSystem>();
                if (ps == null)
                    return;

                centerArrow.gameObject.layer = LayerMask.NameToLayer("Projectile");  
                centerArrow.transform.position = nearMonster.transform.position;
                ps.Play();
                centerArrow.SetDelayArrowCollider(0.15f);        

                centerArrow.OnAttack = ((targetObj) =>
               {
                   Control enemyControl = nearMonster.GetComponentInParent<Control>();
                   if (enemyControl is BossEnemyControl == false)
                   {
                       (enemyControl as EnemyControl)?.PlayBoundAnimation(null);
                       enemyControl?.GetMove<Move>().Bound(2, 2, 0.5f, 2, -enemyControl.transform.forward);
                   }
                   
                   skillParameter.control.GetAttack<PlayerAttack>().DamageToAttackTargets(enemyControl, 1, PlayerHitType.Missile, PlayerAttackType.Skill1);
               });              

            });
    }
    private void Skill_3_Wizard(AttackData attackData, SkillParameter skillParameter)
    {
        Debug.Log("Skill_3_Wizard");

        attackData.AddHandleEvent(CommonPlayerAttackDataParameterDefine.ANIMATION_END_FRAME,
            (parameter) =>
            {
                pRaycast_temp.attackRangeRaycast.offset.z = defaultOffsetZ;
                pRaycast_temp.attackRangeRaycast.radius = defaultRaycastNum;
                //skillParameter.control.utility.animationEnd.AnimationEnd(defaultRaycastNum);            
            });

        attackData.AddHandleEvent(CommonPlayerAttackDataParameterDefine.FALLING_TARGET,
              (parameter) =>
                {
                /* # TODO : 랜덤 몬스터 선택해서 위에 떠있다가 사라지면 되는 기능  구현*/
                /* # 이팩트 : effect_skill_green_09_hit1  */
                /* # 쏘는 간격을 0.2초 정도(총 몇발이 떨어지는데 떨어지는 시간간격 조절 가능하게  */
                List<GameObject> monsterList = (StageManager.instance as GamePlayManager).enemyManager.GetAllMonster(transform.position, LB_maxDistance);
                if (monsterList.Count > 0)
                    StartCoroutine(CreateLightningBolt(skillParameter, monsterList, skillParameter.control));

            });  

    }
    IEnumerator CreateLightningBolt(SkillParameter skillParameter, List<GameObject> Monsters, PlayerControl control)
    {
        // 화살 떨어지는 수 조절가능
        // 캐릭터 주변 반경 거리 조절 가능하게

        for (int i = 0; i < LB_totalCnt; i++)
        {
            Arrow lightningBolt = LightningBolt.CreateArrow((skillParameter.control).GetModel<Model>().ProjectileOffset.gameObject);
            if (lightningBolt == null)
                continue;

            lightningBolt.gameObject.layer = LayerMask.NameToLayer("Projectile");
            int randInt = UnityEngine.Random.Range(0, Monsters.Count);  
            lightningBolt.transform.position = Monsters[randInt].transform.position;

            lightningBolt.OnAttack = (Collider) =>
            {
                Control control = Collider.GetComponentInParent<Control>();
                //control.GetMove<Move>().KnockBack(1.2f, animTime, centerArrow.transform.forward);

                if (control is BossEnemyControl == false)
                {
                    (control as EnemyControl)?.PlayBoundAnimation(null);
                    control.GetMove<Move>().Bound(2, 2, 0.5f, 2, -Monsters[randInt].transform.forward);          
                }     

                skillParameter.control.GetAttack<PlayerAttack>().DamageToAttackTargets(control, 1, PlayerHitType.Missile, PlayerAttackType.Skill2);
            };

            yield return new WaitForSeconds(LB_timeDistance);

        }

    }
    private void UseSkill_4(SkillParameter skillParameter)
    {
        if ((StageManager.instance as GamePlayManager) == null)
            return;

        if ((StageManager.instance as GamePlayManager).enemyManager.isStageEnd == true)
            return;

        Debug.Log("UseSkill_4");

        AttackData attackData = skillParameter.control.utility.attackDatas[0].skill_04;
        pRaycast_temp.attackRangeRaycast.radius = skRaycastNum4;
        pRaycast_temp.attackRangeRaycast.offset.z = offsetZ_4;
        //AttackData attackData = skillParameter.control.utility.attackDatas[(int)skillParameter.attack.playerModelType].skill_04;
        //AddSkillNotExistAttackEvent(skillParameter, attackData);

        AddSkillExistAttackEvent(skillParameter, attackData, cameraShakePower: 0, cameraShakeTime: 0, addSp: 0, PlayerAttackType.Skill3);

        switch (playerType)
        {
            case PlayerType.Warrior:
                Skill_4_Warrior(attackData, skillParameter);
                break;
            case PlayerType.Archer:
                Skill_4_Archer(attackData, skillParameter);
                break;
            case PlayerType.Wizard:
                Skill_4_Wizard(attackData, skillParameter);
                break;
        }
    }
    private void Skill_4_Warrior(AttackData attackData, SkillParameter skillParameter)
    {
        attackData.AddHandleEvent(CommonPlayerAttackDataParameterDefine.ANIMATION_END_FRAME,
            (parameter) =>
            {
                skillParameter.control.utility.animationEnd.AnimationEnd(defaultRaycastNum);      
            });
    }
    private void Skill_4_Archer(AttackData attackData, SkillParameter skillParameter)
    {
        attackData.AddHandleEvent(CommonPlayerAttackDataParameterDefine.ANIMATION_END_FRAME,
            (parameter) =>
            {
                pRaycast_temp.attackRangeRaycast.offset.z = defaultOffsetZ;
                pRaycast_temp.attackRangeRaycast.radius = defaultRaycastNum;      
                //skillParameter.control.utility.animationEnd.AnimationEnd(defaultRaycastNum);      
            });

        attackData.AddHandleEvent(CommonPlayerAttackDataParameterDefine.ShootNormalFallingArrow,
            (parameter) =>
            {
                /* # TODO : 랜덤 몬스터 선택해서 위에 떠있다가 사라지면 되는 기능  구현*/
                /* # 이팩트 : effect_skill_green_09_hit1  */
                /* # 쏘는 간격을 0.2초 정도(총 몇발이 떨어지는데 떨어지는 시간간격 조절 가능하게  */
                //Vector3 nearMonsterPosition = skillParameter.control.GetComponent<PlayerControl_Archer>().FindNearMonsterPosition();    // 제일 가까운 몬스터 어택
                //Vector3 nearMonsterPosition = skillParameter.control.GetComponent<PlayerControl_Archer>().FindRandomMonsterPosition();      // 랜덤 몬스터 어택 
                //StartCoroutine(CreateFallingArrows(skillParameter, nearMonsterPosition));    

                /* # ver2 : 몬스터 전체 어택*/
                //List<GameObject> monsterList = skillParameter.control.GetComponent<PlayerControl_Archer>().FindAllMonsterPosition(MaxDistance1);
                List<GameObject> monsterList = (StageManager.instance as GamePlayManager).enemyManager.GetAllMonster(transform.position, MaxDistance1);      
                if(monsterList.Count > 0)
                    StartCoroutine(CreateFallingArrows_ver2(skillParameter, monsterList, skillParameter.control ));       

            });


    }
    IEnumerator CreateFallingArrows_ver2(SkillParameter skillParameter, List<GameObject> Monsters, PlayerControl control)
    {
        // 화살 떨어지는 수 조절가능
        // 캐릭터 주변 반경 거리 조절 가능하게
        
        for (int i = 0; i < totalArrowCnt; i++)
        {
            Arrow centerArrow = FallingArrow.CreateArrow((skillParameter.control).GetModel<Model>().ProjectileOffset.gameObject);
            if (centerArrow == null)
                continue;

            ParticleSystem ps = centerArrow.efx.GetComponent<ParticleSystem>();
            if (ps == null)
                continue;

            centerArrow.gameObject.layer = LayerMask.NameToLayer("Projectile");
            int randInt = UnityEngine.Random.Range(0, Monsters.Count);
            centerArrow.transform.position = Monsters[randInt].transform.position ;
            ps.Play();

            Control enemyControl = Monsters[randInt].GetComponentInParent<Control>();

            if (enemyControl is BossEnemyControl == false)
            {
                //enemyControl?.GetMove<Move>().KnockBack(1.5f, 0.5f, -Monsters[randInt].transform.forward);
                enemyControl?.GetMove<Move>().KnockBack(3, 0.5f, -Monsters[randInt].transform.forward);
            }

            control.GetAttack<PlayerAttack>().DamageToAttackTargets(enemyControl, 1, PlayerHitType.Missile, PlayerAttackType.Skill3);   

            yield return new WaitForSeconds(timeDistance);    

        }

    }

    private void Skill_4_Wizard(AttackData attackData, SkillParameter skillParameter)
    {
        attackData.AddHandleEvent(CommonPlayerAttackDataParameterDefine.ANIMATION_END_FRAME,
            (parameter) =>
            {
                pRaycast_temp.attackRangeRaycast.offset.z = defaultOffsetZ;
                pRaycast_temp.attackRangeRaycast.radius = defaultRaycastNum;
                //skillParameter.control.utility.animationEnd.AnimationEnd(defaultRaycastNum);
            });


        attackData.AddHandleEvent(CommonPlayerAttackDataParameterDefine.FOLLOWING_TARGET,
            (parameter) =>
            {
                StartCoroutine(CoWindSkillCreate(skillParameter, parameter.intValue, 0.3f));  

                // 120도 방향 나가기 
                //Arrow LeftWindArrow = Wind.CreateArrow((skillParameter.control).GetModel<Model>().ProjectileOffset.gameObject);
                //LeftWindArrow.gameObject.layer = LayerMask.NameToLayer("Projectile");
                //Arrow RightWindArrow = Wind.CreateArrow((skillParameter.control).GetModel<Model>().ProjectileOffset.gameObject);
                //RightWindArrow.gameObject.layer = LayerMask.NameToLayer("Projectile");

                //Vector3 forward = skillParameter.control.GetModel<Model>().transform.forward;
                //CenterWindArrow.SetTargetPosition(transform.position + forward * CenterWindArrow.speed, forward, 1);
                //Vector3 right = ((skillParameter.control.GetModel<Model>().transform.right + (-1) * forward * Mathf.Tan((float)Math.PI / 180 * (90 - 60)))).normalized;
                //Vector3 left = ((-(skillParameter.control.GetModel<Model>().transform.right) + (-1) * forward * Mathf.Tan((float)Math.PI / 180 * (90 - 60)))).normalized;

                //RightWindArrow.SetTargetPosition(transform.position + right * RightWindArrow.speed, right, 1);
                //LeftWindArrow.SetTargetPosition(transform.position + left * LeftWindArrow.speed, left, 1);  

                //DamageArrow(skillParameter, CenterWindArrow);
                //DamageArrow(skillParameter, RightWindArrow);
                //DamageArrow(skillParameter, LeftWindArrow);  

            });
    }

    IEnumerator CoWindSkillCreate(SkillParameter parameter, int count, float time)
    {
        int createCount = 0;
        while(createCount < count)
        {
            Arrow CenterWindArrow = Wind.CreateArrow((parameter.control).GetModel<Model>().ProjectileOffset.gameObject);
            CenterWindArrow.gameObject.layer = LayerMask.NameToLayer("Projectile");
            WindSkill skill4 = CenterWindArrow.gameObject.GetComponent<WindSkill>();
            //타겟 랜덤, 거리제한
            List<GameObject> _targetObjs = (StageManager.instance as GamePlayManager).enemyManager.GetRandomMonster(transform.position , 10);
            if (_targetObjs.Count == 0)
                break;

            int randInt = UnityEngine.Random.Range(0, _targetObjs.Count);    
            skill4._targetObj = _targetObjs[randInt];
            skill4._control = parameter.control;
            ++createCount;
            yield return new WaitForSeconds(time);
        }
    }

    private void DamageArrow(SkillParameter sp,Arrow arrow)
    {
        arrow.OnAttack = (Collider) =>
        {
            Control control = Collider.GetComponentInParent<Control>();
            //control.GetMove<Move>().KnockBack(1.2f, animTime, centerArrow.transform.forward);
         
            //  (control as EnemyControl)?.PlayBoundAnimation(null);
         //   control.GetMove<Move>().Bound(1.5f, 1.5f, 0.5f, 2, arrow.transform.forward);

            //sp.control.GetAttack<PlayerAttack>().DamageToAttackTargets(control, 1, PlayerHitType.Missile);  
            //DamageToAttackTargets(control, 1, PlayerHitType.Missile); 
        };
    }

 
    #endregion
}
