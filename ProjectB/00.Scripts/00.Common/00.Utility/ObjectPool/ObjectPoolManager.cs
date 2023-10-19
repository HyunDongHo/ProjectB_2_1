using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scheduler;
using DG.Tweening;

public class ObjectPoolManager : Singleton<ObjectPoolManager>
{
    public class ObjectPool
    {
        public class Pool
        {
            public GameObject poolObject { get; private set; }
            public ObjectPoolInterface objectPoolInterface { get; private set; }

            public Pool(GameObject poolObject, ObjectPoolInterface objectPoolInterface)
            {
                this.poolObject = poolObject;
                this.objectPoolInterface = objectPoolInterface;
            }
        }
        public List<Pool> pools = new List<Pool>();

        public GameObject poolPrefab { get; private set; }
        private Transform poolParent;

        public ObjectPool(GameObject prefab, Transform poolSubjectParent)
        {
            poolPrefab = prefab;

            poolParent = new GameObject(prefab.name).transform;
            poolParent.SetParent(poolSubjectParent);
        }

        public Pool GetPoolObject()
        {
            Pool pool = pools.Find(data => data.poolObject.activeSelf == false);

            if (pool == null)
            {
                pool = CreatePoolObject();
            }

            pool.poolObject.SetActive(true);

            return pool;
        }
        public Pool GetPoolObject_ver2(GameObject creator)
        {
            Pool pool = pools.Find(data => data.poolObject.activeSelf == false);

            if (pool == null)
            {
                pool = CreatePoolObject();
            }

            // 꺼진 애들 찾았으면 
            //ObjectFollowControl ofc = pool.poolObject.GetComponent<ObjectFollowControl>();
            //if(ofc == null)
            //    ofc = pool.poolObject.AddComponent<ObjectFollowControl>();              

            //ofc.target = creator.transform;
            pool.poolObject.SetActive(true);

            return pool;
        }
        public Pool CreatePoolObject()
        {
            GameObject poolObject = Instantiate(poolPrefab, poolParent, worldPositionStays: true);

            Pool pool = new Pool(poolObject, poolObject.GetComponent<ObjectPoolInterface>());
            pools.Add(pool);

            return pool;
        }

        public bool RemovePoolObject(GameObject gameObject)
        {
            Pool pool = pools.Find(data => data.poolObject == gameObject);

            if (pool != null)
            {
                pool.poolObject.transform.DOKill();

                pool.poolObject.SetActive(false);
                pool.poolObject.transform.SetParent(poolParent);

                pool.poolObject.transform.localScale = poolPrefab.transform.localScale;
                pool.poolObject.transform.position = Vector3.one * 10000;
                return true;
            }

            return false;
        }

        public void RemoveAllPoolObject()
        {
            foreach (var pool in pools)
                RemovePoolObject(pool.poolObject);
        }
    }

    public List<ObjectPool> objectPools = new List<ObjectPool>();

    private ObjectPool GetObjectPool(GameObject prefab)
    {
        ObjectPool objectPool = objectPools.Find(data => data.poolPrefab == prefab);

        if (objectPool == null)
        {
            objectPools.Add(objectPool = new ObjectPool(prefab, transform));
        }

        return objectPool;
    }

    public GameObject CreateObject(GameObject prefab, Transform parent)
    {
        ObjectPool.Pool pool = GetObjectPool(prefab).GetPoolObject();
        GameObject poolObject = pool.poolObject;

        poolObject.transform.SetParent(parent, worldPositionStays: true);
        poolObject.transform.localPosition = Vector3.zero;
        poolObject.transform.localRotation = Quaternion.identity;

        pool.objectPoolInterface?.Respawned();  

        return poolObject;
    }

    public GameObject CreateObject(GameObject prefab, Vector3 position, Quaternion quaternion)
    {
        ObjectPool.Pool pool = GetObjectPool(prefab).GetPoolObject();
        GameObject poolObject = pool.poolObject;

        poolObject.transform.position = position;
        poolObject.transform.rotation = quaternion;

        pool.objectPoolInterface?.Respawned();

        return poolObject;
    }

    public GameObject CreateObject(GameObject prefab, Vector3 position, Quaternion quaternion, GameObject creator)
    {
        ObjectPool.Pool pool = GetObjectPool(prefab).GetPoolObject_ver2(creator); ;

        GameObject poolObject = pool.poolObject;

        poolObject.transform.position = creator.transform.position;  
        pool.objectPoolInterface?.Respawned();

        return poolObject;  
    }

    public void RemoveObject(GameObject gameObject)
    {
        foreach (var objectPool in objectPools)
        {
            if (objectPool.RemovePoolObject(gameObject))
                break;
        }
    }

    public void RemoveObject(GameObject gameObject, float destroyTime)
    {
        Timer.instance.TimerStart(new TimerBuffer(destroyTime), 
            OnComplete: () =>
            {
                foreach (var objectPool in objectPools)
                {
                    if (objectPool.RemovePoolObject(gameObject))
                        break;
                }
            });
    }

    public void ClearAllObject()
    {
        foreach (ObjectPool objectPool in objectPools)
            objectPool.RemoveAllPoolObject();
    }
}
