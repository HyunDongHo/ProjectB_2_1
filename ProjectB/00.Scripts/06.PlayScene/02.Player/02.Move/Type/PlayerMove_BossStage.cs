using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Scheduler;

public class PlayerMoveState_BossStage : PlayerMoveState_GamePlay
{
    public const string MOVE_LEFT = "MOVE_LEFT";
    public const string MOVE_RIGHT = "MOVE_RIGHT";
}

public class PlayerMove_BossStage : PlayerMove_GamePlay
{
    public const int FIRST_START_MOVE_AREA_INDEX = 2;

    public MoveArea moveArea;
    public int currentMoveAreaIndex { get; private set; } = FIRST_START_MOVE_AREA_INDEX;

    private TimerBuffer moveStartDelayBuffer = new TimerBuffer(0);
    public float moveSpeed = 2.5f;

    private void Start()
    {
        Move_Center();
    }

    public void Move_Center()
    {
        ResetMove();

        SetMoveAreaPosition(FIRST_START_MOVE_AREA_INDEX);
    }

    public void Move_Left()
    {
        ChangeState(PlayerMoveState_BossStage.MOVE_LEFT);

        Move(-1, moveStartFrame: 8, moveEndFrame: 41, animation: "Dodge_Left");
    }

    public void Move_Right()
    {
        ChangeState(PlayerMoveState_BossStage.MOVE_RIGHT);

        Move(1, moveStartFrame: 8, moveEndFrame: 41, animation: "Dodge_Right");
    }

    private void ChangePlayerValue(bool _bool)
    {
        playerControl.SetIsPlayHitMotion(_bool);

        (playerControl as PlayerControl_GamePlay).isAvailableControl = _bool;
        playerControl.GetAttack<PlayerAttack>().isEnableAttack = _bool;
        playerControl.GetStats<PlayerStats>().hp.isAvailableReduceHp = _bool;

        isAvailableMove = _bool;
    }

    private void SetMoveAreaPosition(int moveAreaIndex)
    {
        currentMoveAreaIndex = moveAreaIndex;

        transform.position = moveArea.GetMoveAreaPosition(currentMoveAreaIndex);
    }

    private void Move(int dir, int moveStartFrame = 0, int moveEndFrame = 0, string animation = "")
    {
        if (!isAvailableMove) return;

        ResetMove();

        if (moveArea.CheckMove(currentMoveAreaIndex + dir))
        {
            currentMoveAreaIndex += dir;

            if (!string.IsNullOrEmpty(animation))
                PlayMoveAnimation(animation, moveArea.GetMoveAreaPosition(currentMoveAreaIndex), moveStartFrame, moveEndFrame);
            else
                SetMoveAreaPosition(currentMoveAreaIndex);
        }
    }

    private void PlayMoveAnimation(string animation, Vector3 movePosition, int moveStartFrame, int moveEndFrame)
    {
        AnimationControl animationControl = playerControl.GetModel<Model>().animationControl;

        float moveDistanceDuration = Vector3.Distance(transform.position, movePosition) / moveSpeed;
        float moveAnimationSpeed = animationControl.GetTotalTime(animation) / 1.0f / (moveDistanceDuration + animationControl.GetFrameToTime(animation, moveStartFrame));

        animationControl.PlayAnimation(animation, speed: moveAnimationSpeed, weight: (int)PlayerAnimationWeight.DodgeMove);

        ChangePlayerValue(false);

        Timer.instance.TimerStop(moveStartDelayBuffer);
        moveStartDelayBuffer.time = animationControl.GetFrameToTime(animation, moveStartFrame);
        Timer.instance.TimerStart(moveStartDelayBuffer,
            OnComplete: () =>
            {
                if (!playerControl.GetStats<Stats>().hp.isAlive) return;

                float moveEndDuration = moveDistanceDuration - moveStartDelayBuffer.time - (animationControl.GetTotalTime(animation) - animationControl.GetFrameToTime(animation, moveEndFrame));
                transform.DOMove(movePosition, moveEndDuration)
                .SetEase(Ease.Linear)
                .OnComplete(
                    () =>
                    {
                        ChangePlayerValue(true);
                    });
            });
    }

    public int GetCurrentMoveAreaIndex()
    {
        return currentMoveAreaIndex;
    }
}
