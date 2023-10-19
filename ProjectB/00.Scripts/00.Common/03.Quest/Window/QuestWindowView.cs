using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class QuestWindowView : MonoBehaviour
{
    public GameObject parent;

    public Vector3 openPosition;
    public Vector3 closePosition;

    public float openCloseDuration = 0.25f;

    [Space]

    public Button teleport;

    public void OpenCloseQuestWindow(bool isOpen, bool isAnimation)
    {
        parent.transform.DOLocalMove(isOpen ? openPosition : closePosition, isAnimation ? openCloseDuration : 0);
    }

    public void ActiveTeleport(bool isActive)
    {
        teleport.gameObject.SetActive(isActive);
    }
}
