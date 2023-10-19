using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonToggle : MonoBehaviour
{
    public Action<int> OnButtonClicked;

    [System.Serializable]
    public class ControlButton
    {
        public int buttonType { get; set; }

        public Button button;

        [System.Serializable]
        public class ButtonView
        {
            public Image buttonImage;

            public Sprite buttonOff;
            public Sprite buttonOn;
        }
        public ButtonView buttonView;
    }

    public ControlButton[] controlButtons;

    public int firstClickButtonType = -1;

    public void Start()
    {
        for (int i = 0; i < controlButtons.Length; i++)
        {
            int currentIndex = i;

            controlButtons[currentIndex].buttonType = currentIndex;
            controlButtons[currentIndex].button.onClick.AddListener(() => Click(controlButtons[currentIndex].buttonType));
        }

        Click(firstClickButtonType);
    }

    public void Click(int buttonType)
    {
        if (buttonType == -1)
        {
            SetOffAllButton();
        }
        else
            ToggleButton(buttonType);

        OnButtonClicked?.Invoke(buttonType);
    }

    private void SetOffAllButton()
    {
        for (int i = 0; i < controlButtons.Length; i++)
        {
            if (controlButtons[i].buttonView.buttonImage == null)
            {
                continue;
            }

            SetButtonView(index: i, isOn: false);
        }
    }

    private void ToggleButton(int buttonType)
    {
        for (int i = 0; i < controlButtons.Length; i++)
        {
            if (controlButtons[i].buttonView.buttonImage == null)
            {
                continue;
            }

            SetButtonView(index: i, isOn: buttonType == controlButtons[i].buttonType);
        }
    }

    public void SetButtonView(int index, bool isOn)
    {
        if (isOn)
        {
            controlButtons[index].buttonView.buttonImage.gameObject.SetActive(controlButtons[index].buttonView.buttonOn != null);
            controlButtons[index].buttonView.buttonImage.sprite = controlButtons[index].buttonView.buttonOn;
        }
        else
        {
            controlButtons[index].buttonView.buttonImage.gameObject.SetActive(controlButtons[index].buttonView.buttonOff != null);
            controlButtons[index].buttonView.buttonImage.sprite = controlButtons[index].buttonView.buttonOff;
        }
    }
}
