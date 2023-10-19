using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleSelectImage : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    Image SelectImage;
    
    public void Toggle(bool isOn)
    {
        if(isOn == true)        
            SelectImage.gameObject.SetActive(true);        
        else
            SelectImage.gameObject.SetActive(false);
    }

}
