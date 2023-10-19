using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager_Hangar : UIManager
{
    [Space]
    public GameObject Parent;

    public void IsShowParent(bool isShow)
    {        
       Parent.SetActive(isShow);
    }

}
