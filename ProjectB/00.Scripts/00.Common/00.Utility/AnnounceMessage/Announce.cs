using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Announce : MonoBehaviour, ObjectPoolInterface
{
    public GameObject parent;

    public CanvasGroup announceCanvas;

    public Image announceBackground;
    public Text announceText;

    private const float announceBackgroundSideSpace = 25.0f;

    private const float announceUp = 50.0f;
    private const float announceUpTime = 0.75f;

    private const float announceUpAfterDestroyTime = 1.0f;

    public void ShowAnnounce(string text)
    {
        announceText.text = text;
        announceBackground.rectTransform.sizeDelta = new Vector2(announceText.preferredWidth + announceBackgroundSideSpace * 2,
                                                                 announceBackground.rectTransform.rect.height);

        parent.transform.DOLocalMove(parent.transform.localPosition + (Vector3.up * announceUp), announceUpTime).OnComplete(
            () =>
            {
                announceCanvas.DOFade(0, announceUpAfterDestroyTime).OnComplete(
                    () =>
                    {
                        ObjectPoolManager.instance.RemoveObject(this.gameObject);
                    });
            });
    }

    public void Respawned()
    {
        parent.transform.DOKill();
        announceCanvas.DOKill();

        parent.transform.localPosition = Vector3.zero;
        announceCanvas.DOFade(1, 0);
    }
}
