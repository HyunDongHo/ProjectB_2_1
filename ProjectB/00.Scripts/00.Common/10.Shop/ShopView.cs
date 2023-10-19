using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopView : MonoBehaviour
{
    public GameObject shopParent;

    public void OpenCloseInventoryWindow(bool isOpen, bool isAnimation)
    {
        shopParent.SetActive(isOpen);

       
    }
}
