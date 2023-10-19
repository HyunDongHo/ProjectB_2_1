using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonOnOffViewText : ButtonOnOffView
{
    public Text state;

    public string onText;
    public string offText;

    public override void ButtonStateChanged(bool isOn)
    {
        state.text = isOn ? onText : offText;
    }
}
