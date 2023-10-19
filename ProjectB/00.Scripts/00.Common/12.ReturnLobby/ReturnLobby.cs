using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReturnLobby : MonoBehaviour
{
    public Button returnButton;

    private void Awake()
    {
        returnButton.onClick.AddListener(Return);
    }

    private void Return()
    {
        PopupReturnLobby popup = PopupManager.instance.CreatePopup<PopupReturnLobby>("PopupReturnLobby").GetPopup();

        popup.okButton.onClick.AddListener(() =>
        {
            SceneSettingManager.instance.LoadLobbyStageScene();
            /* 로비로 돌아갈 때 player control 꺼주기*/
            PlayersControlManager.instance.ResetAllPlayerState();
            PlayersControlManager.instance.ResetRotationAllPlayer();
            PlayersControlManager.instance.SetNotActiveAllPlayer();
            //PlayersControlManager.instance.nowActivePlayer.gameObject.SetActive(false);     

        });

        popup.cancleButton.onClick.AddListener(() =>
        {
            popup.RemovePopup();
        });
    }
}
