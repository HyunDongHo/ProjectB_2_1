using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class StatusUI : MonoBehaviour
{
    public Image hpBar;
    public float hpReduceDuration = 0.1f;

    public Image hpRiskLevelImage;
    public float hpRiskAppearPercent = 0.2f;
    
    [Space]

    public Image expBar;

    [Space]

    public Image spBar;
    public float spReduceDuration = 0.1f;

    [Space]

    public Text levelText;

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
        PlayerStats playerStats = StageManager.instance.playerControl.GetStats<PlayerStats>();

        playerStats.hp.OnHpInit += HandleOnHpInit;
        playerStats.hp.OnHpSet += HandleOnHpSet;

        playerStats.exp.OnExpInit += HandleOnExpSet;
        playerStats.exp.OnExpUp += HandleOnExpSet;

        playerStats.sp.OnSpInit += HandleOnSpInit;
        playerStats.sp.OnSpAdd += HandleOnSpSet;

        playerStats.level.OnLevelInit += HandleOnLevelSet;
        playerStats.level.OnLevelSet += HandleOnLevelSet;
    }

    private void RemoveEvent()
    {
        PlayerStats playerStats = StageManager.instance.playerControl.GetStats<PlayerStats>();

        playerStats.hp.OnHpInit -= HandleOnHpInit;
        playerStats.hp.OnHpSet -= HandleOnHpSet;

        playerStats.exp.OnExpInit -= HandleOnExpSet;
        playerStats.exp.OnExpUp -= HandleOnExpSet;

        playerStats.sp.OnSpInit -= HandleOnSpInit;
        playerStats.sp.OnSpAdd -= HandleOnSpSet;

        playerStats.level.OnLevelInit -= HandleOnLevelSet;
        playerStats.level.OnLevelSet -= HandleOnLevelSet;
    }

    private void HandleOnHpInit(float hp)
    {
        SetHpUI(hp, isAnimation: false);
    }

    private void HandleOnHpSet(float hp)
    {
        SetHpUI(hp, isAnimation: true);
    }

    private void SetHpUI(float hp, bool isAnimation)
    {
        PlayerStats playerStats = StageManager.instance.playerControl.GetStats<PlayerStats>();

        float convertHp = hp / playerStats.manager.GetValue(StatsValueDefine.MaxHp);

        if (isAnimation)
        {
            hpBar.DOKill();
            hpBar.DOFillAmount(convertHp, hpReduceDuration);

            if (convertHp <= hpRiskAppearPercent)
            {
                hpRiskLevelImage.DOKill();
                hpRiskLevelImage.DOFade(1 - (convertHp / hpRiskAppearPercent), 1 - (1 - convertHp / hpRiskAppearPercent));
            }
            else
            {
                hpRiskLevelImage.DOFade(0, 0);
            }
        }
        else
        {
            hpBar.fillAmount = convertHp;

            if (convertHp <= hpRiskAppearPercent)
            {
                hpRiskLevelImage.color = new Color(hpRiskLevelImage.color.r, hpRiskLevelImage.color.g, hpRiskLevelImage.color.b, 1 - (convertHp / hpRiskAppearPercent));
            }
            else
            {
                hpRiskLevelImage.color = new Color(hpRiskLevelImage.color.r, hpRiskLevelImage.color.g, hpRiskLevelImage.color.b, 0);
            }
        }
    }

    private void HandleOnExpSet(float exp)
    {
      // PlayerStats playerStats = StageManager.instance.playerControl.GetStats<PlayerStats>();
      //
      // expBar.fillAmount = exp / playerStats.manager.GetValue(PlayerStatsValueDefine.MaxExp);
    }

    private void HandleOnSpInit(float sp)
    {
        SetSpUI(sp, isAnimation: false);
    }

    private void HandleOnSpSet(float sp)
    {
        SetSpUI(sp, isAnimation: true);
    }

    private void SetSpUI(float sp, bool isAnimation)
    {
        PlayerStats playerStats = StageManager.instance.playerControl.GetStats<PlayerStats>();

        float convertSp = sp / playerStats.manager.GetValue(PlayerStatsValueDefine.MaxSp);

        if (isAnimation)
        {
            spBar.DOKill();
            spBar.DOFillAmount(convertSp, isAnimation ? spReduceDuration : 0);
        }
        else
        {
            spBar.fillAmount = convertSp;
        }
    }


    private void HandleOnLevelSet(int level)
    {
        //levelText.text = $"LV.{level}";//
    }
}
