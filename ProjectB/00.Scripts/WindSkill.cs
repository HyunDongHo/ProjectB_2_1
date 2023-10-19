using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindSkill : MonoBehaviour
{
    public GameObject _targetObj;
    public GameObject PreTarget;

    public PlayerControl _control;

    Dictionary<GameObject, float> monsterDict;

    public void Start()
    {
        monsterDict = new Dictionary<GameObject, float>();
    }
    private void FixedUpdate()
    {
        if (_targetObj != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, _targetObj.transform.position, 8 * Time.deltaTime);
        }
        else
        {
            FindTarget();
            return;
        }

        float length = (_targetObj.transform.position - transform.position).magnitude;

        if (length <= 0.1f)
        {
            FindTarget();
            PreTarget = _targetObj;
        }
    }

    private void FindTarget()
    {
        ProcessSkill(_targetObj);
        _targetObj = (StageManager.instance as GamePlayManager).enemyManager.GetNextMonster(transform.position);    
       //StageData stageData = BackEndServerManager.instance.GetSavedResourceData<StageData>(SceneSettingManager.instance.GetCurrentStage(), isCopy: true);
       //string stageType = stageData.stageTypeNum.ToString();  
       //
       //switch (stageType)
       //{
       //    case "Normal":
       //        ProcessSkill(_targetObj);
       //        _targetObj = (StageManager.instance as GamePlayManager).enemyManager.GetNextMonster(transform.position);             
       //        break;
       //    case "Midddle":
       //        ProcessSkill(_targetObj);
       //        _targetObj = (StageManager.instance as GamePlayManager).enemyManager.GetNextMonster(transform.position);
       //        break;
       //    case "Final":
       //        ProcessSkill(_targetObj);  
       //        break;
       //}
    }

    public void ProcessSkill(GameObject monsterObj)
    {
     
        foreach (var checkMonsterObj in monsterDict)
        {
            if (checkMonsterObj.Key == monsterObj)
            {
                if ((Time.time - checkMonsterObj.Value) > 0.5f && monsterObj !=null)
                {
                    _control.GetAttack<PlayerAttack>().DamageToAttackTargets(monsterObj.GetComponentInParent<Control>(), 1, PlayerHitType.Missile, PlayerAttackType.Skill3);    

                    monsterDict[checkMonsterObj.Key] = Time.time;  
                    return;    
                }
                else
                    return;
            }
        }

        if (monsterObj == null || _control == null)
            return;  

        monsterDict.Add(monsterObj, Time.time);
        _control.GetAttack<PlayerAttack>().DamageToAttackTargets(monsterObj.GetComponentInParent<Control>(), 1, PlayerHitType.Missile ,PlayerAttackType.Skill3);  


    }

}
