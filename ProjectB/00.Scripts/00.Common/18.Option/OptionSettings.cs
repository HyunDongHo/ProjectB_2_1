using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionSettings : MonoBehaviour
{
    public AccountSetting accountSetting;
    public AnnounceSetting announceSetting;

    private void Start()
    {
        announceSetting.Init();
    }
}
