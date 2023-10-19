using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Popup<T> : MonoBehaviour
{
    public Action<Popup<T>> OnPopupEnd;

    public abstract T GetPopup();

    public void RemovePopup()
    {
        Destroy(this.gameObject);
    }
}
