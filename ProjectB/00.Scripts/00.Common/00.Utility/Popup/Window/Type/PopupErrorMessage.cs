using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupErrorMessage : Popup<PopupErrorMessage>
{
    public Button gameQuitButton;
    public Text errorMessage;

    public void SetErrorMessage(string error)
    {
        errorMessage.text = error;
    }

    private void Start()
    {
        gameQuitButton.onClick.AddListener(Application.Quit);
    }

    private void OnDestroy()
    {
        gameQuitButton.onClick.RemoveListener(Application.Quit);
    }

    public override PopupErrorMessage GetPopup()
    {
        return this;
    }
}
