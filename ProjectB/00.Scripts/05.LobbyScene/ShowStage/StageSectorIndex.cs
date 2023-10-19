using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageSectorIndex : MonoBehaviour
{
    public Action<int> OnSectorIndexClicked;
    public ButtonToggle buttonToggle;

    [System.Serializable]
    public class SectorState
    {
        public bool isOpened { get; set; } = false;

        public Button clickButton;
        public GameObject lockImage;
    }
    public SectorState[] sectorStates;

    private void Awake()
    {
        AddEvent();
    }

    private void OnDestroy()
    {
        RemoveEvent();
    }

    private void AddEvent()
    {
        buttonToggle.OnButtonClicked += HandleOnClickFloorButton;
    }

    private void RemoveEvent()
    {
        buttonToggle.OnButtonClicked -= HandleOnClickFloorButton;
    }

    private void HandleOnClickFloorButton(int buttonType)
    {
        if (buttonType == -1) return;

        /* 임시 테스트 코드*/
        //Debug.Log($"buttonType : {buttonType}, button is opened : {sectorStates[buttonType].isOpened}");
        sectorStates[buttonType].isOpened = true;
        /* 임시 테스트 코드*/

        if (sectorStates[buttonType].isOpened)
            OnSectorIndexClicked?.Invoke(buttonType);
    }

    public void SetSectorIndexLock(int sectorIndex, bool isOpened)
    {
        SectorState sectorState = sectorStates[sectorIndex];

        sectorState.isOpened = isOpened;

        sectorState.clickButton.interactable = isOpened;
        sectorState.lockImage.SetActive(!isOpened);
    }

    public bool GetIsOpened(int sectorIndex)
    {
        return sectorStates[sectorIndex].isOpened;
    }
}
