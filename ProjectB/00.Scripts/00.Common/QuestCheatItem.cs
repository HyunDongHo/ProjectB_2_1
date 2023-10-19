using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestCheatItem : MonoBehaviour
{
    public Text QuestNameText;
    public string QuestName;


    public void SetQuest(string QuestName)
    {
        QuestNameText.text = QuestName;
        this.QuestName = QuestName;
    }

    public void PushGetButton()
    {
        for(int i=0;i < QuestManager.instance.quests.Count; ++i)
        {
            QuestManager.instance.RemoveQuestOnly(QuestManager.instance.quests[i]);
        }
        QuestManager.instance.AddQuest(QuestName);
    }
}
