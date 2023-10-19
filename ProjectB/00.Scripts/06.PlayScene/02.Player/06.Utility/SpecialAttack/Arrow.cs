using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour, ObjectPoolInterface
{
    public Vector3[] arrowOriginPositions;
    public Transform hitPosition;

    public List<GameObject> arrowObjects = new List<GameObject>();
    public Vector3 dirVec;
    public float speed = 3;
    public float damage = 20;

    public GameObject efx;

    public CreateResourceData hitObj;
    public Action<Collider> OnAttack;

    public Collider collider;
    public bool isPierce = false;
    //public Action OnArriveTarget;

    private void Awake()
    {
        arrowOriginPositions = new Vector3[arrowObjects.Count];
        for (int i = 0; i < arrowOriginPositions.Length; i++)
        {
            arrowOriginPositions[i] = arrowObjects[i].transform.position;
        }
    }

    public void SetTargetPosition(Vector3 position, Vector3 dir)
    {
        transform.forward = dir;
        transform.DOMove(position, 1.5f).OnComplete(()=> {
            ObjectPoolManager.instance.RemoveObject(gameObject);
        });
    }
    public void SetTargetPosition(Vector3 position, Vector3 dir, EnemyControl enemyControl)
    {
        transform.forward = dir;
        transform.DOMove(position, 1.5f).OnComplete(() => {
            ObjectPoolManager.instance.RemoveObject(gameObject);
            enemyControl.GetAttack<EnemyAttack>().shootDone = true;   
        });
    }

    public void SetTargetPosition(Vector3 position, Vector3 dir, float duration, Action OnArriveTarget = null)
    {
        transform.forward = dir;
        transform.DOMove(position, duration).OnComplete(() => {
            OnArriveTarget?.Invoke();
            ObjectPoolManager.instance.RemoveObject(gameObject);
        });
    }
    public void SetTargetPosition_ver2(Vector3 position, Vector3 dir, float duration)
    {
        transform.forward = dir;
        transform.DOMove(position, duration);
    }

    public void SetTargetPosition_curve(Vector3[] path, float duration)
    {
        transform.DOPath(path, duration , PathType.CatmullRom, PathMode.Full3D).OnComplete(() => {
            ObjectPoolManager.instance.RemoveObject(gameObject);  
        }); 
    }
    public void SetMissileActive(int index, bool isActive)
    {
        arrowObjects[index].SetActive(isActive);
    }

    public void Respawned()
    {
        for (int i = 0; i < arrowObjects.Count; i++)
        {
            arrowObjects[i].transform.position = arrowOriginPositions[i];
            SetMissileActive(i, isActive: true);
        }
    }

    public void SetDelayArrowCollider(float time)
    {
        if (collider == null)
            return;

        Invoke("OnCollider", time);
    }

    private void OnCollider()
    {
        collider.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        OnAttack?.Invoke(other);

        //GameObject hitObjh = GameObject.Instantiate(hitObj);
        //hitObj.transform.position = hitPosition.position;
        //GameObject.Destroy(hitObjh, 3);

        int index = gameObject.name.IndexOf("(");
        if(isPierce == false)
        {
            ObjectPoolManager.instance.RemoveObject(gameObject);  
        }
        
        //CreateResourceManager.instance.r
    }

}
