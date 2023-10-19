using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRaycast : MonoBehaviour
{
    [Header("[Enemy Raycast Range]")]
    public float defaultRaycastNum;
    public List<float> raycastRanges;
    [Space(10f)]

    [Header("[Enemy Raycast Z Offset ]")]
    public float defaultOffsetZ;
    public List<float> offsetsOfZ;
    [Space(10f)]

    public Action<Collider[]> OnDetect;
    public RaycastCheck_Capsule detectRaycast;

    public Action<Collider[]> OnAttackHit;
    public RaycastCheck_Capsule attackRaycast;

    public EnemyControl enemyControl;
    private void Awake()
    {
        detectRaycast.SetUp(~LayerMask.GetMask("Enemy") & ~LayerMask.GetMask("Ignore Raycast") & ~LayerMask.GetMask("Projectile"));
        attackRaycast.SetUp(~LayerMask.GetMask("Enemy") & ~LayerMask.GetMask("Ignore Raycast") & ~LayerMask.GetMask("Projectile"));
    }

    public void SetRaycast(int index)
    {
        attackRaycast.offset.z = offsetsOfZ[index];
        attackRaycast.radius = raycastRanges[index];
    }

    public void SetAttackRaycastOffsetZ(float offsetZ)
    {
        attackRaycast.offset.z = offsetZ;
    }

    public void SetAttackRaycastRadius(float radius)
    {
        attackRaycast.radius = radius;
    }

    public Collider[] GetDetectRaycast()
    {
        return detectRaycast.GetRaycastHit();
    }

    public Collider[] GetAttackRaycast()
    {
        return attackRaycast.GetRaycastHit();
    }

    public void UpdateRaycast()
    {
        Collider[] attack = attackRaycast.GetRaycastHit();

        bool isTargetOn = false;

        for(int i=0; i < attack.Length; ++i)
        {
            if(attack[i] == enemyControl.GetMove<EnemyMove>().moveTarget)
            {
                isTargetOn = true;
                break;
            }
        }        

        if (attack.Length > 0 && isTargetOn == true)
        {
            OnAttackHit?.Invoke(attack);
        }
        else
        {
            OnDetect?.Invoke(detectRaycast.GetRaycastHit());
        }
    }
}
