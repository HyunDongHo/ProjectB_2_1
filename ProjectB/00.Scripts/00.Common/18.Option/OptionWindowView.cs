using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionWindowView : MonoBehaviour
{
    public GameObject parent;

    public void OpenCloseBossWindow(bool isOpen, bool isAnimation)
    {
        parent.SetActive(isOpen);
    }
}
