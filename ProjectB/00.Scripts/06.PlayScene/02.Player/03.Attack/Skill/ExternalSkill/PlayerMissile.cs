using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerMissile : MonoBehaviour
{
    [System.Serializable]
    public class MissileSetting
    {
        public Vector3 missilePosition;
        public Vector3 currentMissilePosition { get; set; }

        public float attackRange = 1;
    }
    public MissileSetting[] missileSettings;

    public GameObject missle;

    public float missleJumpPower = 5;
    public float missleMoveDuration = 2.5f;
    public float missleShootInterval = 0.25f;

    public Missile CreateMissle(GameObject target)
    {
        return CreateResourceManager.instance.CreateResource(target, "Missile").GetComponent<Missile>();
    }

    public void ShootMissile(Transform target, Missile missile, Action<Collider> OnAttack)
    {
        SetMissileReachPositions(target);

        Sequence sequence = DOTween.Sequence();
        for (int i = 0; i < missile.missileObjects.Count; i++)
        {
            int index = i;

            sequence.Join(missile.missileObjects[index].transform
                          .DOJump(GetMisslePosition(index), missleJumpPower, 1, missleMoveDuration)
                          .SetEase(Ease.InOutQuad)
                          .OnComplete(() => ShootMissileEnd(missile, index, OnAttack)));
            sequence.SetDelay(missleShootInterval);
        }
    }

    private void ShootMissileEnd(Missile missile, int index, Action<Collider> OnAttack)
    {
        CreateResourceManager.instance.CreateResource(this.gameObject,
                                                      "PCK_V01_Skill_03_Ex_Bomb", 
                                                      missileSettings[index].currentMissilePosition, 
                                                      Quaternion.Euler(0, UnityEngine.Random.Range(0, 360), 0));
        Collider[] colliders = Physics.OverlapSphere(missileSettings[index].currentMissilePosition, missileSettings[index].attackRange, ~LayerMask.GetMask("Player") & ~LayerMask.GetMask("Ignore Raycast"));

        foreach (Collider collider in colliders)
        {
            OnAttack?.Invoke(collider);
        }

        missile.SetMissileActive(index, isActive: false);

        if(missile.missileObjects.TrueForAll(data => data == null))
        {
            ObjectPoolManager.instance.RemoveObject(missile.gameObject);
        }
    }

    private void SetMissileReachPositions(Transform target)
    {
        foreach (var missileSetting in missileSettings)
        {
            missileSetting.currentMissilePosition = target.position +
                target.right * missileSetting.missilePosition.x +
                target.up * missileSetting.missilePosition.y +
                target.forward * missileSetting.missilePosition.z;
        }
    }

    private Vector3 GetMisslePosition(int index)
    {
        return missileSettings[index].currentMissilePosition;
    }
}
