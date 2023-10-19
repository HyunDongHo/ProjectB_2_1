using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonOnOffViewSprite : ButtonOnOffView
{
    public Image image;

    public Sprite onSprite;
    public Sprite offSprite;

    public override void ButtonStateChanged(bool isOn)
    {
        image.sprite = isOn ? onSprite : offSprite;
    }
}
