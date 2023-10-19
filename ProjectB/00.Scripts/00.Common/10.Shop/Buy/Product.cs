using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Purchasing;

public abstract class Product : MonoBehaviour
{
    public ProductView productView;

    [Header("물건 이름")]
    public string product_title;
    [Header("물건 가격")]
    public int product_price = 0;
    [Header("랜덤 아이템")]
    public RandomData gambleData;

    [Header("화폐")]
    public Money.Currency product_currency;

    private void Awake()
    {
        productView.productButton.onClick.AddListener(OnProductClick);

        productView.SetPrice($"{product_price} {Money.GetCurrencyName(product_currency)}");
    }

    public void OnProductClick()
    {
        PlayerWallet playerWallet = FindObjectOfType<PlayerWallet>();
        switch (product_currency)
        {
            case Money.Currency.Coin:
                PopupBuyProduct popup_coin = PopupManager.instance.CreatePopup<PopupBuyProduct>("PopupBuyProduct").GetPopup();
                popup_coin.Init(productTitle: product_title,
                           productPrice: $"{product_price} {Money.GetCurrencyName(product_currency)}",
                           holdingPrice: $"{playerWallet.GetCoin()} {Money.GetCurrencyName(product_currency)}");
                SetPopupBuyProduct(popup_coin);
                break;
            case Money.Currency.Crystal:
                PopupBuyProduct popup_crystal = PopupManager.instance.CreatePopup<PopupBuyProduct>("PopupBuyProduct").GetPopup();
                popup_crystal.Init(productTitle: product_title,
                           productPrice: $"{product_price} {Money.GetCurrencyName(product_currency)}",
                           holdingPrice: $"{playerWallet.GetRuby()} {Money.GetCurrencyName(product_currency)}");
                SetPopupBuyProduct(popup_crystal);
                break;
            case Money.Currency.RealMoney:
                IsAvailableBuyProduct();
                break;
        }
    }

    private void SetPopupBuyProduct(PopupBuyProduct popup)
    {
        popup.buyButton.onClick.AddListener(() =>
        {
            if (IsAvailableBuyProduct())
            {
                BuyProduct();
                popup.RemovePopup();
            }
            else
            {
                AnnounceManager.instance.ShowAnnounce("현재 선택하신 상품은 구매 할 수 없습니다.");
            }
        });

        popup.cancelButton.onClick.AddListener(() =>
        {
            popup.RemovePopup();
        });
    }

    private bool IsAvailableBuyProduct()
    {
        PlayerWallet playerWallet = FindObjectOfType<PlayerWallet>();
        switch (product_currency)
        {
            case Money.Currency.Coin:
                if (playerWallet.IsAvailiableRemoveCoin(product_price))
                {
                    playerWallet.AddGold(-product_price);
                    return true;
                }
                break;
            case Money.Currency.Crystal:
                if (playerWallet.IsAvailiableRemoveRuby(product_price))
                {
                    playerWallet.AddRuby(-product_price);
                    return true;
                }
                break;
            case Money.Currency.RealMoney:
                //GetComponent<IAPButton>().onPurchaseComplete.AddListener((product) => BuyProduct());
                //GetComponent<IAPButton>().onPurchaseFailed.AddListener((product, purchaseFailureReason) => Debug.Log(purchaseFailureReason));
                return false;
        }

        return false;
    }

    protected abstract void BuyProduct();
}
