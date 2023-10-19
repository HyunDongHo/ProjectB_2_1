using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using LitJson;

public class Test : MonoBehaviour
{
    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            QuestManager.instance.AddQuest("Main_Quest_1");
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            QuestManager.instance.AddQuest("QuestData");
        }

        if (Input.GetKeyDown(KeyCode.F3))
        {
            QuestManager.instance.AddQuest("QuestData1");
        }

        if (Input.GetKeyDown(KeyCode.F4))
        {
            QuestManager.instance.AddQuest("QuestData2");
        }
    }
}
