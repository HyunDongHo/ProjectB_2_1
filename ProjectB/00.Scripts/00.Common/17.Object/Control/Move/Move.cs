using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoveState
{
    public const string STOP = "STOP";
}

public abstract class Move : MonoBehaviour
{
    private string currentMoveState = MoveState.STOP;

    public bool isAvaliableUpdateMove = true;
    public bool isAvailableMove = true;

    public bool isNowNukbackMove = false;
    public bool isNowBound = false;

    public void OnEnable()
    {
        isNowNukbackMove = false;
        isNowBound = false;
    }

    public void ResetMove()
    {
        transform.DOKill();
        currentMoveState = MoveState.STOP;
    }

    protected void ChangeState(string state)
    {
        currentMoveState = state;
    }

    public bool CheckState(string moveState)
    {
        return this.currentMoveState == moveState;
    }

    public void KnockBack(float mag, float time, Vector3 dir)
    {
        if (isNowNukbackMove == true)
            return;

        isNowNukbackMove = true;
        ResetMove();

        if(dir == Vector3.zero)
        {
            transform.DOMove(transform.position - transform.forward * mag, time).SetEase(Ease.OutCubic).OnComplete(() => isNowNukbackMove = false);
        }
        else
        {
            transform.DOMove(transform.position + dir * mag, time).SetEase(Ease.OutCubic).OnComplete(() => isNowNukbackMove = false);
        }

    }

    public void Bound(float boundMag, float knockBackMag, float time, float boundTime, Vector3 dir) 
    {
        if (isNowBound == true)
            return;

        isNowBound = true;

        ResetMove();

        dir.y = 0;

        transform.DOJump(transform.position + dir * knockBackMag, boundMag, numJumps: 1, time).SetEase(Ease.OutCubic);
        StartCoroutine(CoWaitBoundTime(boundTime));
    }
    public void Pluck(float pluckMag, float time, Vector3 dir)
    {
        if (isNowNukbackMove == true)
            return;

        isNowNukbackMove = true;
        ResetMove();

        if (dir == Vector3.zero)
        {
            transform.DOMove(transform.position + transform.forward * pluckMag, time).SetEase(Ease.OutCubic).OnComplete(() => isNowNukbackMove = false);
        }
        else
        {
            transform.DOMove(transform.position + dir * pluckMag, time).SetEase(Ease.OutCubic).OnComplete(() => isNowNukbackMove = false);
        }

    }

    IEnumerator CoWaitBoundTime(float time)
    {
        yield return new WaitForSeconds(time);

        isNowBound = false;
    }

    //public void KnockBack(float boundMag, float knockBackMag, float time, float boundTime, Vector3 dir)
    //{
    //    if (isNowBound == true)
    //        return;
    //
    //    isNowBound = true;
    //
    //    ResetMove();
    //
    //    transform.DOJump(transform.position + dir * knockBackMag, boundMag, numJumps: 1, time).SetEase(Ease.OutCubic);
    //    StartCoroutine(CoWaitBoundTime(boundTime));
    //}
}
