using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopDetail : MonoBehaviour
{
    public ButtonToggle shopButtonToggle;

    public GameObject[] shopDetailParents;

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
        shopButtonToggle.OnButtonClicked += HandleOnButtonClick;
    }

    private void RemoveEvent()
    {
        shopButtonToggle.OnButtonClicked -= HandleOnButtonClick;
    }

    private void HandleOnButtonClick(int buttonType)
    {
        for (int i = 0; i < shopDetailParents.Length; i++)
        {
            shopDetailParents[i].SetActive(buttonType == i);
        }
    }
}
