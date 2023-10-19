using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UpgradeWindowView : MonoBehaviour
{
    public GameObject upgradeParent;

    public Vector3 openPosition;
    public Vector3 closePosition;

    public float openCloseDuration = 0.25f;

    public Text Core;

    public void OpenCloseUpgradeWindow(bool isOpen, bool isAnimation)
    {
        upgradeParent.transform.DOLocalMove(isOpen ? openPosition : closePosition, isAnimation ? openCloseDuration : 0);
    }

    public void SetCoreAmount(int core)
    {
        if (Core == null)
            return;      
        Core.text = $"Core : {core.ToString()}";
    }
}
