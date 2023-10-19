using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSystem : MonoBehaviour
{
    public float skillInterval = 5f; // 스킬 사용 간격
    private float timer = 0f; // 타이머

    private void Update()
    {
        timer += Time.deltaTime;

        // 일정 시간 간격마다 스킬 사용
        if (timer >= skillInterval)
        {
            UseSkill();
            timer = 0f;
        }
    }

    private void UseSkill()
    {
        // 스킬 사용 로직 처리
        Debug.Log("스킬을 사용했습니다.");
    }
}
