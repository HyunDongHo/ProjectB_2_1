using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerArrow : MonoBehaviour
{
    [System.Serializable]
    public class ArrowSetting
    {
        public Vector3 arrowPosition;
        public Vector3 currentArrowPosition { get; set; }

        public float attackRange = 1;
    }
    public ArrowSetting[] arrowSettings;

    public GameObject arrow;

    public float arrowJumpPower = 1;
    public float arrowMoveDuration = 2.5f;
    public float arrowShootInterval = 0.25f;

    public string arrowName;

    public Action<Collider> OnAttack;

    public Arrow CreateArrow(GameObject target)
    {
        return CreateResourceManager.instance.CreateResource(target, arrowName).GetComponent<Arrow>();
    }

    public void ForwardArrow()
    {
       // transform.DOMove(transform.position + (transform.forward * 5), 1.5f);
    }


    public void SetArrowEnd(Action<Collider> OnAttack)
    {
    }

    private void SetMissileReachPositions(Transform target)
    {
        foreach (var arrowSetting in arrowSettings)
        {
            arrowSetting.currentArrowPosition = target.position +
                target.right * arrowSetting.arrowPosition.x +
                target.up * arrowSetting.arrowPosition.y +
                target.forward * arrowSetting.arrowPosition.z;
        }
    }

    private Vector3 GetArrowPosition(int index)
    {
        return arrowSettings[index].currentArrowPosition;
    }
}
