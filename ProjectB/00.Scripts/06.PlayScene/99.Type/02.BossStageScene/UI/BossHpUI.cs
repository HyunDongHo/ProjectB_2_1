using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class BossHpUI : UIManagers
{
    private Stats bossEnemyStats;
    [SerializeField]
    TextMeshProUGUI _bossNameText, _bossHpText, _timerText;

    public Image hpBar;
    public float hpReduceDuration = 0.1f;
    public GameObject parent;

    public void Init(Stats enemyStats)
    {
        bossEnemyStats = enemyStats;
        //SetHpUI(bossEnemyStats.manager.GetValue(StatsValueDefine.MaxHp), true);
        SetHpUI(true);
        AddEvent();

    }

    public void Release()
    {
        RemoveEvent();
    }

    private void AddEvent()
    {
        bossEnemyStats.hp.OnHpInit += HandleOnHpInit;
        bossEnemyStats.hp.OnHpSet += HandleOnHpSet;
    }

    private void RemoveEvent()
    {
        bossEnemyStats.hp.OnHpInit -= HandleOnHpInit;
        bossEnemyStats.hp.OnHpSet -= HandleOnHpSet;
    }

    private void HandleOnHpInit(float hp)
    {
        //SetHpUI(hp, isAnimation: false);
        SetHpUI(isAnimation: false);
    }

    private void HandleOnHpSet(float hp)
    {
        //SetHpUI(hp, isAnimation: true);
        SetHpUI(isAnimation: true);  
    }

    private void SetHpUI(float hp, bool isAnimation)
    {
        float convertHp = hp / bossEnemyStats.manager.GetValue(StatsValueDefine.MaxHp);

        if (isAnimation)
        {
            hpBar.DOKill();
            hpBar.DOFillAmount(convertHp, hpReduceDuration);
        }
        else
        {
            hpBar.fillAmount = convertHp;
        }

        float nowHp = Mathf.Clamp(bossEnemyStats.hp.GetCurrentHp(), 0, float.MaxValue);

        _bossHpText.text = $"{nowHp.ToNumberStringCount()} / {bossEnemyStats.manager.GetValue(StatsValueDefine.MaxHp).ToNumberStringCount()}";
    }
    private void SetHpUI(bool isAnimation)
    {
        float convertHp = bossEnemyStats.hp.GetCurrentHp() / bossEnemyStats.hp.MaxHp;

        if (isAnimation)
        {
            hpBar.DOKill();
            hpBar.DOFillAmount(convertHp, hpReduceDuration);
        }
        else
        {
            hpBar.fillAmount = convertHp;
        }

        float nowHp = Mathf.Clamp(bossEnemyStats.hp.GetCurrentHp(), 0, float.MaxValue);

        //_bossHpText.text = $"{nowHp.ToNumberStringCount()} / {bossEnemyStats.manager.GetValue(StatsValueDefine.MaxHp).ToNumberStringCount()}";
        _bossHpText.text = $"{nowHp.ToNumberStringCount()} / {bossEnemyStats.hp.MaxHp.ToNumberStringCount()}";
    }
    public void SetTimerText(float timer)
    {
        _timerText.text = timer.ToString("F1");  
    }

    public void SetActiveParent(bool active)
    {
        parent.SetActive(active);
    }
}
