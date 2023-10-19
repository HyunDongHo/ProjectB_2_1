using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAvailableCreate_OutsideStage : EnemyAvailableCreate
{
    public Transform createCriteriaTargetStartIndicator;
    public Transform createCriteriaTargetEndIndicator;

    [Space]

    public float criteriaTargetStartIndicatorOffset = 0;
    public float criteriaTargetEndIndicatorOffset = 0;  
    public float mapCircleBorder = 100;

    private float createCriteriaAngle;

    private void Start()
    {
        SetIndicatorPosition();

        Vector3 startIndicatorDir = createCriteriaTargetStartIndicator.position.normalized;
        Vector3 endIndicatorDir = createCriteriaTargetEndIndicator.position.normalized;

        createCriteriaAngle = Logic.ConvertSignedAngle_0_360(Vector3.SignedAngle(startIndicatorDir, endIndicatorDir, Vector3.down));
        //Debug.Log($"!!!!! createCriteriaAngle : {createCriteriaAngle}");
    }

    private void Update()
    {
        SetIndicatorPosition();  
    }

    private void SetIndicatorPosition()
    {
        float mapCircleBorderPlayerAngle = StageManager.instance.playerControl.GetMove<PlayerMove_DefaultStage>().GetMapCircleBorderPlayerAngle();

        Vector3 movement = Vector3.zero;

        movement = Logic.Trigonometry_XZ((mapCircleBorderPlayerAngle + criteriaTargetStartIndicatorOffset) * Mathf.Deg2Rad,  
                                         (mapCircleBorderPlayerAngle + criteriaTargetStartIndicatorOffset) * Mathf.Deg2Rad,
                                         mapCircleBorder);

        createCriteriaTargetStartIndicator.position = movement;

        movement = Logic.Trigonometry_XZ((mapCircleBorderPlayerAngle + criteriaTargetEndIndicatorOffset) * Mathf.Deg2Rad,
                                         (mapCircleBorderPlayerAngle + criteriaTargetEndIndicatorOffset) * Mathf.Deg2Rad,
                                         mapCircleBorder);

        createCriteriaTargetEndIndicator.position = movement;
    }


    //public override bool IsAvailableCreateEnemy(Vector3 createPosition)
    //{
    //    Vector3 criteriaDir = (createCriteriaTargetStartIndicator.position + createCriteriaTargetEndIndicator.position).normalized;
    //    Vector3 dir = createPosition.normalized;

    //    return Vector3.Angle(criteriaDir, dir) < createCriteriaAngle / 2;
    //}

    public override bool IsAvailableCreateEnemy(Vector3 createPosition)
    {
        Vector3 criteriaDir = (new Vector3(0, 0, 100) + new Vector3(100, 0, 0)).normalized;    
        Vector3 dir = createPosition.normalized;

        return Vector3.Angle(criteriaDir, dir) < createCriteriaAngle / 2;
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;

    //    Gizmos.DrawLine(Vector3.zero, createCriteriaTargetStartIndicator.position);
    //    Gizmos.DrawLine(Vector3.zero, createCriteriaTargetEndIndicator.position);  
    //}

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawLine(Vector3.zero, new Vector3(0,0,100));    
        Gizmos.DrawLine(Vector3.zero, new Vector3(100,0,0));
    }
}
