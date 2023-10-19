using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_ToggleColorText : UI_ToggleColor
{
    TextMeshProUGUI _text;

    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    public override void ToggleSet(bool isToggle)
    {
        if (isToggle == true)
            _text.color = OnColor;
        else
            _text.color = OffColor;
    }
}
