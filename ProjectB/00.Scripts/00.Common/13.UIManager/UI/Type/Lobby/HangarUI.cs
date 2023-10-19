using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HangarUI : MonoBehaviour
{
    // Start is called before the first frame update

    public Text RemainTimeText;
    private void FixedUpdate()
    {
        if (UserDataManager.instance.RandomItemRefreshTime <= 0)
            return;

        int hour = (int)(UserDataManager.instance.RandomItemRefreshTime / 3600);
        int min = (int)((UserDataManager.instance.RandomItemRefreshTime - (3600 * hour)) / 60);
        int second = (int)((UserDataManager.instance.RandomItemRefreshTime - (3600 * hour)) % 60);

        RemainTimeText.text = string.Format("{0:D2}:{1:D2}:{2:D2}",
                hour,
                min,
                second);
    }
}
