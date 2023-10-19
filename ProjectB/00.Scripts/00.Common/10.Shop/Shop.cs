using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour, IOpenClose
{
    public ShopView shopView;

    public void Open(bool isAnimation)
    {
        shopView.OpenCloseInventoryWindow(isOpen: true, isAnimation: isAnimation);
    }

    public void Close(bool isAnimation)
    {
        shopView.OpenCloseInventoryWindow(isOpen: false, isAnimation: isAnimation);
    }
}
