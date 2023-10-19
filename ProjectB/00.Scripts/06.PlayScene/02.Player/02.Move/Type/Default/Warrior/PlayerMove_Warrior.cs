using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Scheduler;

public class PlayerMoveState_Warrior : PlayerMoveState_GamePlay
{
    public const string RUN = "RUN";
    public const string RUN_WAIT = "RUN_WAIT";
    public const string DASH = "DASH";
    public const string DASH_TO_TARGET = "DASH_TO_TARGET";
}

public class PlayerMove_Warrior : PlayerMove_DefaultStage
{
    public override void DefaultMoveUpdate()
    {
        MoveStateCheck();

        if (!isAvaliableUpdateMove || isNowBound == true) 
            return;

        Run();
    }

    private void Run()
    {
        ChangeState(PlayerMoveState_DefaultStage.RUN);

        if (isAvailableMove)
        {
            RotateModelPivotToIdentity();
            playerControl.GetModel<PlayerModel>().PlayRunAnimation(fadeLength: 0.2f);

            MoveDefault(CalculateMoveSpeed(runSpeed));
        }
    }


    private void RotateModelPivotToIdentity()
    {
        playerControl.utility.modelPivot.localRotation = Quaternion.identity;
    }

    //private float mapCircleBorderPlayerAngle = 0;

    protected override void MoveDefault(float speed)
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, Mathf.Infinity, ~LayerMask.GetMask("Player") & ~LayerMask.GetMask("Ignore Raycast"));

        if (hits.Length > 0)
        {
            int index = Logic.GetNearestObjectIndex(transform.position, hits, "Enemy");

            if (index < 0)
                return;

            Collider nearestObject = hits[index];

            Vector3 reachPosition = nearestObject.ClosestPoint(transform.position);
            reachPosition.y = 0;

            transform.position = Vector3.MoveTowards(transform.position, reachPosition, speed * Time.deltaTime);
            RotateToTarget(nearestObject.transform.position);
        }
    }

    protected override void MoveToTarget(float speed, Collider target)
    {
        Vector3 reachPosition = target.ClosestPoint(transform.position);
        reachPosition.y = 0;

        transform.position = Vector3.MoveTowards(transform.position, reachPosition, speed * Time.deltaTime);
        RotateToTarget(target.transform.position);

        mapCircleBorderPlayerAngle = -(Quaternion.FromToRotation(Vector3.right, transform.position).eulerAngles.y - 360);
    }

    protected override void MoveStateCheck()
    {
        base.MoveStateCheck();
    }

    public void MoveToMonsterOnTop()
    {
        //TODO : 근접 몬스터로 이동하게
        GameObject activedPlayer = gameObject;

        EnemyManager enemyManager = (StageManager.instance as GamePlayManager).enemyManager;

        //for(int i=0; i < enemyManager.boss)

        //for (int i = 0; i < pools.Count; i++)
        //{
        //
        //    if (pools[i].poolPrefab.name.Substring(0, 1) == "E")
        //    {
        //        for (int j = 0; j < pools[i].pools.Count; j++)
        //        {
        //            NowMonsters.Add(pools[i].pools[j].poolObject);  
        //        }
        //    }
        //}
        //
        //for (int i = 0; i < NowMonsters.Count; i++)
        //{
        //    Distances.Add((activedPlayer.transform.position - NowMonsters[i].transform.position).magnitude);
        //}
        //
        //float maxValue = Distances[0];
        //for (int i = 0; i < Distances.Count; i++)
        //{
        //    if (maxValue > Distances[i])
        //        maxValue = Distances[i];
        //}
        //
        //int minIndex = Distances.IndexOf(maxValue);

        GameObject nearMonster = enemyManager.GetNearMonster(transform.position);

        if (nearMonster == null)
            return;
        else
        {
            Vector3 targetPos = nearMonster.transform.position;
            targetPos.y = 0;
            Vector3 dir = targetPos - transform.position;

            activedPlayer.transform.position += dir * 0.5f;
                //new Vector3(nearMonster.transform.position.x, 0, nearMonster.transform.position.z);
        }

    }

}
