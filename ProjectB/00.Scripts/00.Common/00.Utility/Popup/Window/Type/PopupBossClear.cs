using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupBossClear : Popup<PopupBossClear>
{
    public Button closeButton;

    public RewardUI rewardUI;

    public override PopupBossClear GetPopup()
    {
        return this;
    }
}
