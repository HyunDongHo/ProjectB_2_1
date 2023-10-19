using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System;

public class PopupPSave : Popup<PopupPSave>
{
    [Space]
    public Image hpImage, spImage, fadeImage;
    public Text saveTimeText, localText, goldText, expText, nowTimeText, batteryText;
    public DragItem dragItem;

    PlayerStats stats = null;

    private float appendSaveTime = 0;

    string text = "절전 모드 ";
    string hourText = "";
    string minText = "";

    public void Start()
    {
        MoveSceneManager.instance.OnEndSceneChanged += HandleOnSectorChanged;

        string cloneName = gameObject.name;
        gameObject.name = cloneName.Substring(0, cloneName.IndexOf("("));
        appendSaveTime = 0;
        AddEvent();
        SetInitUI();
        SoundManager.instance.SetAllSoundVolume(0);
    }

    public void FixedUpdate()
    {
        appendSaveTime += Time.deltaTime;

        int hour = (int)(appendSaveTime / 3600);
        int min = (int)((appendSaveTime - (3600 * hour)) / 60);

        saveTimeText.text = $"{text} {hour}시간 {min}분";
        DateTime NowDateTime = DateTime.Now;

        hourText = NowDateTime.Hour < 10 ? $"0{NowDateTime.Hour}" : NowDateTime.Hour.ToString();
        minText = NowDateTime.Minute < 10 ? $"0{NowDateTime.Minute}" : NowDateTime.Minute.ToString();
        nowTimeText.text = $"{hourText}:{minText}";
       
        UpdateBatteryUI();        
    }

    private void OnDestroy()
    {
        fadeImage.DOKill();
        RemoveEvent();
        MoveSceneManager.instance.OnEndSceneChanged -= HandleOnSectorChanged;
        StageManager.instance.cameraManager.SetCamera(CameraChangeTag.Main); 
        SoundManager.instance.SetAllSoundVolume(1);
    }

    private void SetInitUI()
    {
        goldText.text = StageManager.instance.playerControl.utility.belongings.playerWallet.GetCoin().ToString();
        expText.text = $"{(stats.exp.GetTargetExpPer() * 100)}%";
        hpImage.fillAmount = stats.hp.GetCurrentHpRate();
        spImage.fillAmount = stats.sp.GetCurrentSpRate();
        UpdateSectorName();
    }

    private void HandleOnSectorChanged(LoadSceneMode loadSceneMode)
    {
        RemoveEvent();
        AddEvent();
    }

    private void AddEvent()
    {
        stats = StageManager.instance.playerControl.GetStats<PlayerStats>();

        StageManager.instance.playerControl.utility.belongings.playerWallet.OnGoldSet += HandleOnCoinSet;
        stats.hp.OnHpSet += HandleOnHpSet;
        stats.sp.OnSpSet += HandleOnSpSet;
        stats.exp.OnExpUp += HandleOnExpSet;
        fadeImage.DOFade(1, 1.2f).SetLoops(-1, LoopType.Yoyo); 
        UpdateSectorName();
        StageManager.instance.cameraManager.SetCamera(CameraChangeTag.Sub_2);
        dragItem.OnChanged = ()=> { Destroy(gameObject, 0.1f); };
    }

    private void RemoveEvent()
    {
        StageManager.instance.playerControl.utility.belongings.playerWallet.OnGoldSet -= HandleOnCoinSet;
        stats.hp.OnHpSet -= HandleOnHpSet; 
        stats.exp.OnExpUp -= HandleOnExpSet;
        stats.sp.OnSpSet -= HandleOnSpSet;
        fadeImage.DOKill();
    }

    private void HandleOnHpSet(float hp)
    {
        hpImage.fillAmount = stats.hp.GetCurrentHpRate();
    }

    private void HandleOnSpSet(float hp)
    {
        spImage.fillAmount = stats.sp.GetCurrentSpRate();
    }

    private void HandleOnCoinSet(int coin)
    {
        goldText.text = coin.ToString();
    }

    private void HandleOnExpSet(float exp)
    {
        expText.text = $"{(stats.exp.GetTargetExpPer() * 100)}%";
    }
    public void UpdateBatteryUI()
    {
        float batteryLevel = SystemInfo.batteryLevel;
        //switch (SystemInfo.batteryStatus)
        //{
        //    case BatteryStatus.Charging:
        //        batteryStateImg.sprite = chargeStateSprite;
        //        batteryStateImg.gameObject.SetActive(true);
        //        break;
        //    case BatteryStatus.Discharging:
        //        if (batteryLevel > 0.8f)
        //        {
        //            batteryStateImg.sprite = BatteryList[0];
        //            batteryStateImg.gameObject.SetActive(true);
        //        }
        //        else if (batteryLevel > 0.3f) // 
        //        {
        //            batteryStateImg.sprite = BatteryList[1];
        //            batteryStateImg.gameObject.SetActive(true);
        //        }
        //        else
        //        {
        //            batteryStateImg.sprite = BatteryList[2];
        //            batteryStateImg.gameObject.SetActive(true);
        //        }
        //        break;
        //}

        batteryText.text = $"{(batteryLevel * 100)}%";
    }
    
    public void UpdateSectorName()
    {
        string currentSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.ToLower();
        if (currentSceneName == SceneSettingManager.LOBBY_SCENE.ToLower())
            localText.text = "로비";
        else
            localText.text = SceneSettingManager.instance.GetSectorFullName();
    }

    public override PopupPSave GetPopup()
    {
        return this;
    }
}
