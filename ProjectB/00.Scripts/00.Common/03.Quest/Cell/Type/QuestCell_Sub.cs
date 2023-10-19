using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestCell_Sub : QuestCell
{
    public override void SetButtonClicked(bool isClicked)
    {
        base.SetButtonClicked(isClicked);

        if (isClicked)
        {
            switch (questData.saveData.state)
            {
                case QuestState.NotAccepted:
                    InitSubQuest();
                    break;
                case QuestState.Achieved:
                    EndSubQuest();
                    break;
            }
        }
    }

    private void InitSubQuest()
    {
        questData.SetState(QuestState.InProgress);
    }

    private void EndSubQuest()
    {
        QuestManager.instance.RemoveQuest(questData);
    }
}
