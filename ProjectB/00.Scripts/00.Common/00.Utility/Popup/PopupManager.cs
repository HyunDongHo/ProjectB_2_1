using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupManager : Singleton<PopupManager>
{
    public Popup<T> CreatePopup<T>(string popupName) where T : Popup<T>
    {
        T popup = Instantiate(ResourceManager.instance.Load<GameObject>(popupName)).GetComponent<T>();

        return popup;
    }
}
