using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PopupDevelop : Popup<PopupDevelop>
{
    public Button okButton;

    private void Awake()
    {
        //꽦꽦
        okButton.onClick.AddListener(() =>
            {
                RemovePopup();
                SceneSettingManager.instance.LoadLobbyStageScene();    
            });
    }

    public override PopupDevelop GetPopup()
    {
        return this;
    }
}
