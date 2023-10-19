using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Scheduler;

public class OpenClose : MonoBehaviour
{
    public bool isOpen = false;

    [Space]

    public OpenCloseButton[] toggleButtons;

    public OpenCloseButton[] openButtons;
    public OpenCloseButton[] closeButtons;

    [System.Serializable]
    public class OpenCloseButton
    {
        public Button button;
        public bool isAnimation;
        public bool isMine;
    }

    [Space]

    public GameObject[] activeOpenOnCloseOff;
    public GameObject[] activeCloseOnOpenOff;

    [Space]

    public OpenCloseView openCloseView;

    [System.Serializable]
    public class OpenCloseView
    {
        public Image changeOpenCloseImage;

        public Sprite open;
        public Sprite close;
    }

    public GameObject havingOpenCloseComponentObj;
    private IOpenClose openClose;

    private void Start()
    {
        openClose = havingOpenCloseComponentObj.GetComponent<IOpenClose>();
        if(openClose == null)
        {
            Debug.Log($"[OpenClose] {havingOpenCloseComponentObj.name} 에 IOpenClose가 없습니다.");
        }

        for (int i = 0; i < toggleButtons.Length; i++)
        {
            int index = i;
            toggleButtons[i].button.onClick.AddListener(() => OnToggle(toggleButtons[index].isAnimation, toggleButtons[index].isMine));
        }

        for (int i = 0; i < openButtons.Length; i++)
        {
            int index = i;
            openButtons[i].button.onClick.AddListener(() => SetOpen(openButtons[index].isAnimation, openButtons[index].isMine));
        }

        for (int i = 0; i < closeButtons.Length; i++)
        {
            int index = i;
            closeButtons[i].button.onClick.AddListener(() => SetClose(closeButtons[index].isAnimation, closeButtons[index].isMine));
        }

        SetButtonDelay(isAnimation: false, isMine: false);
    }

    private void OnToggle(bool isAnimation, bool isMine)
    {
        isOpen = !isOpen;

        SetButtonDelay(isAnimation, isMine);
    }

    public void SetOpen(bool isAnimation = false, bool isMine = false)
    {
        if (isOpen) return;

        isOpen = true;

        SetButtonDelay(isAnimation, isMine);
    }

    public void SetClose(bool isAnimation = false, bool isMine = false)
    {
        if (!isOpen) return;

        isOpen = false;

        SetButtonDelay(isAnimation, isMine);
    }

    private void SetButtonDelay(bool isAnimation, bool isMine)
    {
        // 나의 버튼이면 모든 버튼 세팅을 무시하고 실행하기 위해 한 프레임뒤에 사용.
        if (isMine)
        {
            Timer.instance.TimerStart(new TimerBuffer(Time.deltaTime),
                OnComplete: () =>
                {
                    SetButton(isAnimation);
                });
        }
        else
        {
            SetButton(isAnimation);
        }
    }

    private void SetButton(bool isAnimation)
    {
        if (isOpen)
        {
            openClose?.Open(isAnimation);

            if (openCloseView.changeOpenCloseImage != null)
                openCloseView.changeOpenCloseImage.sprite = openCloseView.close;

            for (int i = 0; i < activeOpenOnCloseOff.Length; i++)
                activeOpenOnCloseOff[i].SetActive(true);

            for (int i = 0; i < activeCloseOnOpenOff.Length; i++)
                activeCloseOnOpenOff[i].SetActive(false);
        }
        else
        {
            openClose?.Close(isAnimation);

            if (openCloseView.changeOpenCloseImage != null)
                openCloseView.changeOpenCloseImage.sprite = openCloseView.open;

            for (int i = 0; i < activeOpenOnCloseOff.Length; i++)
                activeOpenOnCloseOff[i].SetActive(false);

            for (int i = 0; i < activeCloseOnOpenOff.Length; i++)
                activeCloseOnOpenOff[i].SetActive(true);
        }
    }
}
