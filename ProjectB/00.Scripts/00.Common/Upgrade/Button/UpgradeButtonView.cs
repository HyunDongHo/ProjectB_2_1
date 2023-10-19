using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButtonView : MonoBehaviour
{
    public Button upgrade;

    public Text priceText;
    public Text valueText;

    public void SetUpgradeButton(int price, string totalShowValue)
    {
        priceText.text = $"{price} Core";
        valueText.text = totalShowValue;
    }
}
