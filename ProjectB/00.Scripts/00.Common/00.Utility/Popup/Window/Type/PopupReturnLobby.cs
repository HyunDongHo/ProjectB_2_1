using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupReturnLobby : Popup<PopupReturnLobby>
{
    public Button okButton;
    public Button cancleButton;

    public override PopupReturnLobby GetPopup()
    {
        return this;
    }
}
