using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ToggleColorImage : UI_ToggleColor
{
    Image _image;

    public void Awake()
    {
        _image = GetComponent<Image>();
    }

    public override void ToggleSet(bool isToggle)
    {
        if (isToggle == true)        
            _image.color = OnColor;        
        else
            _image.color = OffColor;
    }
}
