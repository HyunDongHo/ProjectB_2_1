using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupBuyProduct : Popup<PopupBuyProduct>
{
    public Text productTitleText;

    public Text productPriceText;
    public Text holdingPriceText;

    [Space]

    public Button buyButton;
    public Button cancelButton;

    public void Init(string productTitle, string productPrice, string holdingPrice)
    {
        productTitleText.text = productTitle;

        productPriceText.text = productPrice;
        holdingPriceText.text = holdingPrice;
    }

    public override PopupBuyProduct GetPopup()
    {
        return this;
    }
}
