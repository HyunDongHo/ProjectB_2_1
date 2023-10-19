using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProductView : MonoBehaviour
{
    public Text productPrice;
    public Button productButton;

    public void SetPrice(string price)
    {
        productPrice.text = price;
    }
}
