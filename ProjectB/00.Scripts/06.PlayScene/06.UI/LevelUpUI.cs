using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Scheduler;
using DG.Tweening;

public class LevelUpUI : MonoBehaviour
{
    public GameObject parent;
    public Image levelImage;
    public Text levelText;

    public float levelUpSpeedTime = 0.5f;
    public float levelUpShowTime = 1.0f;

    private void Awake()
    {
        AddEvent();
    }

    private void OnDestroy()
    {
        RemoveEvent();
    }

    private void AddEvent()
    {
        StageManager.instance.playerControl.GetStats<PlayerStats>().level.OnLevelUp += HandleOnLevelUp;
    }

    private void RemoveEvent()
    { 
        StageManager.instance.playerControl.GetStats<PlayerStats>().level.OnLevelUp -= HandleOnLevelUp;
    }

    private void HandleOnLevelUp(int level)
    {
        parent.SetActive(true);

        levelText.text = level.ToString();

        levelImage.DOFade(0, 0);
        levelText.DOFade(0, 0);

        Sequence startSequence = DOTween.Sequence();

        startSequence.Join(levelImage.DOFade(1, levelUpSpeedTime));
        startSequence.Join(levelText.DOFade(1, levelUpSpeedTime));

        startSequence.OnComplete(() =>
        {
            Timer.instance.TimerStart(new TimerBuffer(levelUpShowTime),
                OnComplete: () =>
                {
                    Sequence stopSequence = DOTween.Sequence();

                    stopSequence.Join(levelImage.DOFade(0, levelUpSpeedTime));
                    stopSequence.Join(levelText.DOFade(0, levelUpSpeedTime));

                    stopSequence.OnComplete(() =>
                    {
                        parent.SetActive(false);
                    });
                });
        });
    }
}
