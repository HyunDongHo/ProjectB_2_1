using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomLotteryManager_Product : RandomLotteryManager
{
    public Product product { get; set; }

    public Button repurchaseButton;

//  protected override void AddButtonEvent()
//  {
//  //    base.AddButtonEvent();
//
//      repurchaseButton.onClick.AddListener(
//          () =>
//          {
//              product.OnProductClick();
//          });
//  }

    protected override void InitActive()
    {
        base.InitActive();

        repurchaseButton.gameObject.SetActive(false);
    }

    protected override void OpenedAllCard()
    {
        base.OpenedAllCard();

        repurchaseButton.gameObject.SetActive(true);
    }
}
