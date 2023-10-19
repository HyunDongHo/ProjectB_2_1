using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ButtonOnOffView : MonoBehaviour
{
    public abstract void ButtonStateChanged(bool isOn);
}
