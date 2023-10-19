using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class QuestCellView : MonoBehaviour
{
    public Text titleText;

    public GameObject teleportIcon;

    [Space]

    public GameObject notAcceptedText;
    public GameObject achivedText;

    [Space]

    public Image highlight;
    public float highlightBlinkingDuration = 1.0f;
    private float highlightOriginAlpha = 0.0f;

    [Space]

    public Button button;

    public void Init()
    {
        highlightOriginAlpha = highlight.color.a;
    }

    #region 처음 설정.

    public void SetTitleText(string text)
    {
        titleText.text = text;
    }

    public void SetTeleportIcon(bool isActive)
    {
        teleportIcon.SetActive(isActive);
    }

    #endregion

    #region 상태 설정.

    public void SetNotAccepted(bool active)
    {
        notAcceptedText.SetActive(active);
    }

    public void SetAcheived(bool active)
    {
        achivedText.SetActive(active);
    }

    public void SetHighlightBlinking(bool isActive)
    {
        SetHighlightActive(isActive);

        highlight.DOFade(0.125f, highlightBlinkingDuration).SetLoops(-1, LoopType.Yoyo);
    }

    public void SetHighlightActive(bool isActive)
    {
        highlight.DOKill();

        highlight.DOFade(highlightOriginAlpha, 0);
        highlight.gameObject.SetActive(isActive);
    }

    #endregion
}
