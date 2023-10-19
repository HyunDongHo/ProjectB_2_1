using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubMenu : MonoBehaviour, IOpenClose
{
    public SubMenuView view;

    public void Open(bool isAnimation)
    {
        view.OpenCloseSubMenuWindow(true, isAnimation);
    }

    public void Close(bool isAnimation)
    {
        view.OpenCloseSubMenuWindow(false, isAnimation);
    }
}
