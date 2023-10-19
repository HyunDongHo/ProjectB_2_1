using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupQuestTeleport : Popup<PopupQuestTeleport>
{
    public Button cancelButton;
    public Button okButton;

    public void RunTeleportPopup(string stageName)
    {
        okButton.onClick.AddListener(() =>
        {
            RemovePopup();

            SceneSettingManager.instance.SetStageWithName(stageName);
        });
        cancelButton.onClick.AddListener(() =>
        {
            RemovePopup();
        });
    }

    public override PopupQuestTeleport GetPopup()
    {
        return this;
    }
}
