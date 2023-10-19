using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BossEndUI : MonoBehaviour
{
    public Button bossEndButton;

    public Vector3 loopBossButtonGrowScale = new Vector3(1.25f, 1.25f, 1.25f);
    public float loopBossButtonGrowDurtaion = 0.5f;

    public void ShowBossEnd()
    {
        bossEndButton.onClick.AddListener(OnBossEndButtonClicked);

        bossEndButton.gameObject.SetActive(true);
        bossEndButton.transform.DOScale(loopBossButtonGrowScale, loopBossButtonGrowDurtaion).SetLoops(-1, LoopType.Yoyo);
    }

    private void OnBossEndButtonClicked()
    {
        bossEndButton.onClick.RemoveListener(OnBossEndButtonClicked);

        bossEndButton.gameObject.SetActive(false);
    }
}
