using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorSkill3 : MonoBehaviour
{
    private PlayerControl playerControl;

    public void Init(PlayerControl playerControl)
    {
        this.playerControl = playerControl;
    }

    public void Release() { }


    public void MoveToMonsterOnTop()
    {
        Debug.Log("MoveToMonsterOnTop is activate");
        //TODO : 근접 몬스터로 이동하게
        GameObject activedPlayer = playerControl.utility.gameObject.transform.parent.gameObject;

        List<GameObject> NowMonsters = new List<GameObject>();
        List<float> Distances = new List<float>();
        List<ObjectPoolManager.ObjectPool> pools = ObjectPoolManager.instance.objectPools;
        //Debug.Log($"ObjectPoolManager.instance.objectPools : {ObjectPoolManager.instance.objectPools.Count}");    

        for(int i=0;i< pools.Count; i++)
        {
            if(pools[i].poolPrefab.name.Substring(0, 1) == "E")  
            {
                for(int j=0;j< pools[i].pools.Count; j++)
                {
                    NowMonsters.Add(pools[i].pools[j].poolObject);
                }
            }
        }

        for (int i = 0; i < NowMonsters.Count; i++)
        {
            Distances.Add((activedPlayer.transform.position - NowMonsters[i].transform.position).magnitude);
        }

        float maxValue = Distances[0];
        for (int i = 0; i < Distances.Count; i++)
        {
            if (maxValue > Distances[i]) 
                maxValue = Distances[i];  
        }

        int minIndex = Distances.IndexOf(maxValue);
        activedPlayer.transform.position = new Vector3(NowMonsters[minIndex].transform.position.x+1.5f, 0, NowMonsters[minIndex].transform.position.z+1.5f);    

    }


}
