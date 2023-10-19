using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UI_ToggleColor : MonoBehaviour
{
    public Color OnColor;
    public Color OffColor;

    public abstract void ToggleSet(bool isToggle);    

}
