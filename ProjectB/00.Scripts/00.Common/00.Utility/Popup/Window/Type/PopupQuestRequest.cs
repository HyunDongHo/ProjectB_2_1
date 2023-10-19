using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupQuestRequest : Popup<PopupQuestRequest>
{
    public Button acceptButton;
    public Button cancelButton;

    public void SetQuestItem()
    {

    }

    public override PopupQuestRequest GetPopup()
    {
        return this;
    }
}
