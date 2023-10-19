using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSystem : MonoBehaviour
{
    public float skillInterval = 5f; // ��ų ��� ����
    private float timer = 0f; // Ÿ�̸�

    private void Update()
    {
        timer += Time.deltaTime;

        // ���� �ð� ���ݸ��� ��ų ���
        if (timer >= skillInterval)
        {
            UseSkill();
            timer = 0f;
        }
    }

    private void UseSkill()
    {
        // ��ų ��� ���� ó��
        Debug.Log("��ų�� ����߽��ϴ�.");
    }
}
