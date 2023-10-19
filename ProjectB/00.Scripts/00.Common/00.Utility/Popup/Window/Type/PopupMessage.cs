using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupMessage : Popup<PopupMessage>
{
    public Button okButton;
    public Button cancleButton;

    public Text MessageText;
    public Action okButtonAction = null;

    private void Start()
    {
        okButton.onClick.AddListener(() =>
        {
            okButtonAction?.Invoke();
        });

        cancleButton.onClick.AddListener(RemovePopup);
    }

    private void OnDisable()
    {
        if (okButtonAction != null)
            okButtonAction = null;

        okButton.onClick.RemoveListener(() =>
        {
            okButtonAction?.Invoke();
        });

        cancleButton.onClick.RemoveListener(RemovePopup);
    }

    public void SetMessageText(string text)
    {
        MessageText.text = text;
    }

    public override PopupMessage GetPopup()
    {
        return this;
    }
}
