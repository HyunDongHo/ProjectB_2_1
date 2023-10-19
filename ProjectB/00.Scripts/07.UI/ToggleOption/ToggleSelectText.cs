using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ToggleSelectText : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    TextMeshProUGUI _text;

    [SerializeField]
    Color32 _OnColor, _OffColor;

    public void Toggle(bool isOn)
    {
        if (isOn)
            _text.color = _OnColor;
        else
            _text.color = _OffColor;
    }
}
