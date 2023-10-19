using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour, ObjectPoolInterface
{
    public Vector3[] missileOriginPositions;

    public List<GameObject> missileObjects = new List<GameObject>();

    private void Awake()
    {
        missileOriginPositions = new Vector3[missileObjects.Count];
        for (int i = 0; i < missileOriginPositions.Length; i++)
        {
            missileOriginPositions[i] = missileObjects[i].transform.position;
        }
    }

    public void SetMissileActive(int index, bool isActive)
    {
        missileObjects[index].SetActive(isActive);
    }

    public void Respawned()
    {
        for (int i = 0; i < missileObjects.Count; i++)
        {
            missileObjects[i].transform.position = missileOriginPositions[i];
            SetMissileActive(i, isActive: true);
        }
    }
}
