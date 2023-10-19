using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SubMenuView : MonoBehaviour
{
    public GameObject parent;
    public RectTransform subMenuFadeRect;
    public RectTransform subMenuBackground;

    public Vector3 closePosition;
    public Vector3 openPosition;

    public float openCloseDuration = 0.25f;

    public void OpenCloseSubMenuWindow(bool isOpen, bool isAnimation)
    {
        subMenuFadeRect.transform.DOKill();

        Vector3 fixedPosition = subMenuBackground.transform.position;

        if (isOpen)
        {
            parent.SetActive(true);
            subMenuFadeRect.DOAnchorPos(openPosition, isAnimation ? openCloseDuration : 0)
                .OnUpdate(() => subMenuBackground.transform.position = fixedPosition);
        }
        else
        {
            subMenuFadeRect.DOAnchorPos(closePosition, isAnimation ? openCloseDuration : 0)
                .OnUpdate(() => subMenuBackground.transform.position = fixedPosition).OnComplete(() => parent.SetActive(false));
        }
    }
}
