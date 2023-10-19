using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Scheduler;

public class PlayerControl_Wizard : PlayerControl_DefaultStage
{
   // private Dictionary<ScreenRectType, bool> dashAttackPreAttackCheck = new Dictionary<ScreenRectType, bool>() { { ScreenRectType.Left, false }, { ScreenRectType.Right, false } };
  //  private bool movePreCheck = false;

    protected override void Start()
    {
        base.Start();
    }

    // protected override void Update()
    // {
    //     if (!isEnabledControl || !isAvailableControl || isEndGamePlay) return;
    //
    //     if (!GetStats<PlayerStats>().hp.isAlive) return;
    //
    //     GetAttack<PlayerAttack>().ResetAttackTargets();
    //     (pRaycast as PlayerRaycast_DefaultStage).UpdateRaycast();
    //
    //     base.Update();
    // }
    protected override bool CheckAvailableUseSkill(int skillIndex)
    {
        PlayerRaycast_DefaultStage raycast = pRaycast as PlayerRaycast_DefaultStage;

        bool available = true;

        // 스킬 1번과 2번은 근접이기 때문에 Attack에 들어오지 않으면 사용 불가.
        Collider[] attack = raycast.attackRaycast.GetRaycastHit();
        if ((skillIndex == 0 || skillIndex == 1 || skillIndex == 2 || skillIndex == 3) && attack.Length <= 0)
            available = false;

      //  Collider[] attackRange = raycast.attackRangeRaycast.GetRaycastHit();
      //  if (skillIndex == 3 && attackRange.Length <= 0)
      //      available = false;

        return available;
    }
 //  protected override void AddEvent()
 //  {
 //      base.AddEvent();
 //
 //      PlayerRaycast_DefaultStage raycast = pRaycast as PlayerRaycast_DefaultStage;
 //   //   raycast.OnDashAttackCheckHit += HandleOnDashAttackCheckHit;
 //      raycast.OnAttackRangeHit += HandleOnAttackRangeHit;
 //      raycast.OnAttackHit += HandleOnAttackHit;
 //      raycast.OnDashToTarget += HandleOnDashToTarget;
 //  }
 //
 //  protected override void RemoveEvent()
 //  {
 //      base.RemoveEvent();
 //
 //      PlayerRaycast_DefaultStage raycast = pRaycast as PlayerRaycast_DefaultStage;
 ////      raycast.OnDashAttackCheckHit -= HandleOnDashAttackCheckHit;
 //      raycast.OnAttackRangeHit -= HandleOnAttackRangeHit;
 //      raycast.OnAttackHit -= HandleOnAttackHit;
 //      raycast.OnDashToTarget -= HandleOnDashToTarget;
 //  }
 //
 //  #region 적 처치
 //
 //  protected override void KilledEnemy(EnemyControl enemyControl)
 //  {
 //      base.KilledEnemy(enemyControl);
 //
 //      Collider[] dashToTargetRange = (pRaycast as PlayerRaycast_DefaultStage).dashToTargetRaycast.GetRaycastHit();
 //      if (dashToTargetRange.Length <= 0)
 //          GetAttack<PlayerAttack_GamePlay>().ResetCombo();
 //
 //      InvokeRepeating(nameof(RotateAfterKillEnemy), Time.deltaTime, Time.deltaTime);
 //  }
 //
 //  private void RotateAfterKillEnemy()
 //  {
 //      PlayerMove_DefaultStage move = GetMove<PlayerMove_DefaultStage>();
 //      if (move.isAvaliableUpdateMove && move.isAvailableMove)
 //      {
 //          ResetRotationToEnemy();
 //          CancelInvoke(nameof(RotateAfterKillEnemy));
 //      }
 //  }
 //
 //  private void ResetRotationToEnemy()
 //  {
 //      // 적 처치 후 AttackRange에 오브젝트들이 있으면 그 중에 하나에게로 회전함.
 //      // 오브젝트가 하나도 없으면 회전 값 초기화.
 //     //Collider[] attackRange = (pRaycast as PlayerRaycast_DefaultStage).attackRangeRaycast.GetRaycastHit();
 //     //
 //     //if (attackRange.Length > 0)
 //     //{
 //     //    Transform modelPivot = utility.modelPivot.transform;
 //     //
 //     //    Vector3 lookPosition = attackRange[UnityEngine.Random.Range(0, attackRange.Length)].transform.position;
 //     //
 //     //    if (lookPosition != Vector3.zero)
 //     //    {
 //     //        Vector3 lookTargetEulerAngles = Quaternion.FromToRotation(Vector3.forward, lookPosition - modelPivot.position).eulerAngles;
 //     //        modelPivot.DORotate(new Vector3(0, lookTargetEulerAngles.y, 0), GetMove<PlayerMove_DefaultStage>().rotationSpeed).SetEase(Ease.Linear);
 //     //    }
 //     //    else
 //     //    {
 //     //        modelPivot.DOLocalRotateQuaternion(Quaternion.identity, GetMove<PlayerMove_DefaultStage>().rotationSpeed).SetEase(Ease.Linear);
 //     //    }
 //     //}
 //  }
 //
 //  #endregion
 //
 //  #region 플레이어 공격 컨트롤
 //
 //  protected override void HandleOnAttackForceQuit()
 //  {
 //      base.HandleOnAttackForceQuit();
 //
 //      movePreCheck = false;
 //  }
 //
 //  protected override void HandleOnAttackStarted(PlayerAttackType attackType)
 //  {
 //      base.HandleOnAttackStarted(attackType);
 //
 //      movePreCheck = false;
 //  }
 //
 //  protected override void HandleOnComboForceFrameEnd(PlayerAttackType attackType)
 //  {
 //      base.HandleOnComboForceFrameEnd(attackType);
 //
 //      Collider[] dashToTarget = (pRaycast as PlayerRaycast_DefaultStage).dashToTargetRaycast.GetRaycastHit();
 //
 //      if (dashToTarget.Length > 0)
 //      {
 //          GetModel<Model>().animationControl.ResetAnimationState();
 //          SetMove(true);
 //      }
 //  }
 //
 //  #endregion
 //
 //  #region 탐지하여 작동하는 기능
 //
 //  private void HandleOnAttackRangeHit(Collider[] cols)
 //  {
 //      PlayerAttack_GamePlay attack = GetAttack<PlayerAttack_GamePlay>();
 //
 //      attack.AddAttackTargets(cols);
 //
 //      //if (dashAttackPreAttackCheck[ScreenRectType.Left])
 //      //    DashAttack(ScreenRectType.Left);
 //      //else if (dashAttackPreAttackCheck[ScreenRectType.Right])
 //      //    DashAttack(ScreenRectType.Right);
 //  }
 //
 //  private void HandleOnAttackHit(Collider[] cols)
 //  {
 //      PlayerMove_DefaultStage move = GetMove<PlayerMove_DefaultStage>();
 //      PlayerAttack_GamePlay attack = GetAttack<PlayerAttack_GamePlay>();
 //
 //      int index = Logic.GetNearestObjectIndex(transform.position, cols, "Enemy");
 //
 //      if (index < 0)
 //          return;
 //
 //      if (!move.CheckState(MoveState.STOP))
 //      {
 //          move.ResetMove();
 //      }
 //
 //      if (cols[index] != null && pRaycast.targetCollider == null)
 //      {
 //          pRaycast.targetCollider = cols[index];
 //      }
 //
 //      move.RotateToTarget(pRaycast.targetCollider.transform.position);
 //
 //      StartAttackCheck();
 //  }
 //
 //  private void HandleOnDashToTarget(Collider[] cols)
 //  {
 //      if (isPlayHitMotion) return;
 //
 //      PlayerMove_DefaultStage move = GetMove<PlayerMove_DefaultStage>();
 //      PlayerAttack attack = GetAttack<PlayerAttack>();
 //
 //      if (cols.Length > 0)
 //      {
 //          int index = Logic.GetNearestObjectIndex(transform.position, cols, "Enemy");
 //
 //          if (index < 0)
 //              return;
 //
 //          move.DashToTargetUpdate(cols[index]);
 //      }
 //      else
 //      {
 //          if ((StageManager.instance as GamePlayManager) == null)
 //              return;
 //          EnemyManager enemyManager = (StageManager.instance as GamePlayManager).enemyManager;
 //
 //          if (enemyManager.IsLargerThanOneSpawnMonster() == true)
 //              move.DefaultMoveUpdate();
 //          else
 //              move.DefaultIdle();
 //      }
 //  }
 //
 //  //private void DashAttack(ScreenRectType screenRectType)
 //  //{
 //  //    dashAttackPreAttackCheck[screenRectType] = false;
 //
 //  //    PlayerAttack_GamePlay attack = GetAttack<PlayerAttack_GamePlay>();
 //  //    attack.Attack_Dash(screenRectType, -GetSpRemoveAmount(isAuto: false));
 //  //}
 //
 //  #endregion
 //
 //  #region 플레이어 인트로
 //
 //  public void StartIntro(Action OnEnd)
 //  {
 //      AnimationControl animationControl = GetModel<Model>().animationControl;
 //
 //      if (isAvailableControl)
 //      {
 //          isAvailableControl = false;
 //
 //         // utility.subCamera.PlayCameraAnimation("Idle01", DefineManager.DEFAULT_CAMERA_TIME);
 //
 //          animationControl.PlayAnimation("Idle01");
 //          EndIntro(OnEnd);
 //          //float totalTime = animationControl.GetTotalTime(PlayerAnimationType.PC_APPEAR_LANDING);
 //          //float totalFrame = animationControl.GetTotalFrame(PlayerAnimationType.PC_APPEAR_LANDING);
 //
 //          //float oneFrameTime = totalTime / totalFrame;
 //
 //          //TimerFunctions.instance.TimerFrameToFixedOneTime(new TimerBuffer(totalTime), oneFrameTime,
 //          //    OnFrame: (frame) =>
 //          //    {
 //          //        if(frame == 0)
 //          //        {
 //          //            utility.weaponModel.SetWeaponModelType(PlayerWeaponModelType.Base);
 //          //        }
 //
 //          //        if(frame == 70)
 //          //        {
 //          //            utility.weaponModel.SetWeaponModelType(PlayerWeaponModelType.Model);
 //          //        }
 //          //    },
 //          //    OnComplete: () =>
 //          //    {
 //          //        EndIntro(OnEnd);
 //          //    });
 //      }
 //      else
 //      {
 //          EndIntro(OnEnd);
 //      }
 //  }
 //
 //  public void StarDefault(Action OnEnd)
 //  {
 //      utility.weaponModel.SetWeaponModelType(PlayerWeaponModelType.Model);
 //
 //      if (isAvailableControl)
 //      {
 //          isAvailableControl = false;
 //
 //          CreateResourceManager.instance.CreateResource(gameObject, ResourceManager.instance.Load<CreateResourceData>("Teleport"));
 //          EndIntro(OnEnd);
 //      }
 //      else
 //          EndIntro(OnEnd);
 //  }
 //
 //  public void EndIntro(Action OnEnd)
 //  {
 //      PlayerModel model = GetModel<PlayerModel>();
 //
 //      model.PlayIdleAnimation(PlayerIdleType.Weapon_On);
 //
 //      CheckFirstMove();
 //
 //      OnEnd?.Invoke();
 //  }
 //
 //  #endregion
 //
 //  #region 처음 움직임 설정.
 //
 //  private void CheckFirstMove()
 //  {
 //      PlayerModel model = GetModel<PlayerModel>();
 //
 //      InitFirstMoveCheck();
 //
 //      model.PlayIdleAnimation(PlayerIdleType.Weapon_On);
 //
 //      // 10초 동안 입력 대기 후 이동 변경. (10초 안에 화면을 클릭하면 이동 변경.)
 //      TimerBuffer firstMovecheck = new TimerBuffer(2);
 //      Timer.instance.TimerStart(firstMovecheck,
 //          OnFrame: () =>
 //          {
 //              if (InputManager.instance.GetMouseButtonDown(0))
 //              {
 //                  Timer.instance.TimerStart(new TimerBuffer(Time.deltaTime), OnComplete: EndFirstMoveCheck);
 //                  Timer.instance.TimerStop(firstMovecheck);
 //              }
 //          },
 //          OnComplete: EndFirstMoveCheck);
 //  }
 //
 //  private void InitFirstMoveCheck()
 //  {
 //      isAvailableControl = false;
 //  }
 //
 //  private void EndFirstMoveCheck()
 //  {
 //      isAvailableControl = true;
 //  }
 //
 //  #endregion
 //
 //  protected override bool CheckAvailableUseSkill(int skillIndex)
 //  {
 //      PlayerRaycast_DefaultStage raycast = pRaycast as PlayerRaycast_DefaultStage;
 //
 //      bool available = true;
 //
 //      // 스킬 1번과 2번은 근접이기 때문에 Attack에 들어오지 않으면 사용 불가.
 //      Collider[] attack = raycast.attackRaycast.GetRaycastHit();
 //      if ((skillIndex == 0 || skillIndex == 1) && attack.Length <= 0)
 //          available = false;
 //
 //      Collider[] attackRange = raycast.attackRangeRaycast.GetRaycastHit();  
 //      if (skillIndex == 2 && attackRange.Length <= 0)
 //          available = false;
 //
 //      return available;
 //  }
 //
 //  public void UpdateStageEndBefore()
 //  {
 //      if (!GetAttack<PlayerAttack>().currentAttack.isAttack)
 //          GetModel<PlayerModel>().PlayIdleAnimation(PlayerIdleType.Weapon_On);
 //  }
 //
 //  public void StartStageEndMotion(float motionSpeed, int endMotionFrame, Action OnEnd)
 //  {
 //      PlayerModel model = GetModel<PlayerModel>();
 //
 //      model.PlayTeleportAnimation(motionSpeed);
 //
 //      float teleportTotalTime = model.animationControl.GetTotalTime(PlayerAnimationType.TELEPORT) / motionSpeed;
 //      float teleportTotalFrame = model.animationControl.GetTotalFrame(PlayerAnimationType.TELEPORT);
 //
 //      float oneFrameTime = teleportTotalTime / teleportTotalFrame;
 //
 //      TimerFunctions.instance.TimerFrameToFixedOneTime(new TimerBuffer(teleportTotalTime), oneFrameTime,
 //          OnFrame: (frame) =>
 //          {
 //              if (frame == endMotionFrame)
 //                  OnEnd?.Invoke();
 //          });
 //  }
 //

}
