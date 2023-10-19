using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDataManager : Singleton<PlayerDataManager>
{
    private PlayerControl playerControl;

    private void Awake()
    {
        MoveSceneManager.instance.OnStartSceneChanged += HandleOnStartSceneChanged;
        MoveSceneManager.instance.OnEndSceneChanged += HandleOnEndSceneChanged;
    }

    private void HandleOnStartSceneChanged(LoadSceneMode loadSceneMode)
    {
        // 씬 로드 시작하면 모든 데이터 저장.
        if (loadSceneMode == LoadSceneMode.Single)
        {
            AutoSaveAllPlayerDataToUserData();
            playerControl = null;
        }
    }

    private void HandleOnEndSceneChanged(LoadSceneMode loadSceneMode)
    {
        if (loadSceneMode == LoadSceneMode.Single)
            playerControl = FindObjectOfType<PlayerControl>();
    }

    private void AutoSaveAllPlayerDataToUserData()
    {
        SaveAllPlayerDataToUserData();
    }

    public void SaveAllPlayerDataToUserData()
    {
        if (playerControl == null)
            return;

        SaveAllStats();
        SaveAllPlayerGameData();
    }

    #region 플레이어 Stats 저장.

    private void SaveAllStats()
    {
        PlayerStats stats = playerControl.GetStats<PlayerStats>();

        //UserDataManager.instance.UpdateHp(stats.hp.GetCurrentHp());
        //UserDataManager.instance.UpdateSp(stats.sp.GetCurrentSp());
        //UserDataManager.instance.UpdateExp(stats.exp.GetCurrentExp());
        //UserDataManager.instance.UpdateLevel(stats.level.GetCurrentLevel());

       // UserDataManager.instance.UpdateWarriorLevel(stats.warriorLevel.GetCurrentWarriorLevel());
       // UserDataManager.instance.UpdateArcherLevel(stats.archerLevel.GetCurrentArcherLevel());
       // UserDataManager.instance.UpdateWizardLevel(stats.archerLevel.GetCurrentWizardLevel());
       //
       // UserDataManager.instance.UpdateWarriorHp(stats.warriorHp.GetCurrentWarriorHp());
       // UserDataManager.instance.UpdateArcherHp(stats.archerHp.GetCurrentArcherHp());
       // UserDataManager.instance.UpdateWarriorHp(stats.wizardHp.GetCurrentWizardHp());
       //
       // UserDataManager.instance.UpdateWarriorExp(stats.warriorExp.GetCurrentWarriorExp());
       // UserDataManager.instance.UpdateArcherExp(stats.archerExp.GetCurrentArcherExp());
       // UserDataManager.instance.UpdateWizardExp(stats.wizardExp.GetCurrentWizardExp());

    }

    #endregion

    #region 플레이어 GameData 저장

    private void SaveAllPlayerGameData()
    {
        PlayerWallet wallet = playerControl.utility.belongings.playerWallet;

        //UserDataManager.instance.UpdateCoin(wallet.GetCoin());
        //UserDataManager.instance.UpdateRuby(wallet.GetRuby());
        //UserDataManager.instance.UpdateCore(wallet.GetCore());

        //PlayerQuickSlot quickSlot = playerControl.utility.quickSlot;
        //UserDataManager.instance.UpdateQuickSlot(quickSlot.GetQuickSlot());

        //UserDataManager.instance.UpdateBuff(playerControl.utility.buff.nowBuffs);        

    }

    #endregion

    #region 플레이어 인벤토리 저장


    #endregion

    #region 플레이어 장비 장착 세이브.

    private void ResetPlayerEquipData(ref PlayerEquipData playerEquipData)
    {
        playerEquipData.equipWeaponItemIndex = -1;
        playerEquipData.equipHelmetItemIndex = -1;
        playerEquipData.equipArmorItemIndex = -1;
        playerEquipData.equipGloveItemIndex = -1;
        playerEquipData.equipBeltItemIndex = -1;
        playerEquipData.equipGaiterItemIndex = -1;
        playerEquipData.equipBootsItemIndex = -1;
        playerEquipData.equipEarringItemIndex = -1;
        playerEquipData.equipNecklaceItemIndex = -1;
        playerEquipData.equipRingItemIndex = -1;
        playerEquipData.equipPetItemIndex = -1;
    }

    #endregion
}
