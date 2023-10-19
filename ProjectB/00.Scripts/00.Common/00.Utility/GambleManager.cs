using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RNG;

public enum GamebleType
{
    Common,
    Weapon,
    ProtectiveGear,
    Accessary,
    Pet,
}

public class GambleManager : Singleton<GambleManager>
{
    private bool isGamebleSceneOpened = false;
    private string currentSceneName = string.Empty;

    //public ItemData GambleItem(RandomItemData gambleItemData)
    //{
    //    RandomItemSettingContiner randomItem = GetRandomItem(gambleItemData);

    //    return randomItem.itemData;
    //}

    //private RandomItemSettingContiner GetRandomItem(RandomItemData gambleItemData)
    //{
    //    RandomItemSettingContiner randomItemSet = gambleItemData.GetRandomContainer(gambleItemData.GetRandomSettings());

    //    return randomItemSet;
    //}

    //public ItemData[] GambleItem(int itemCount, RandomItemData gambleItemData)
    //{
    //    RandomItemSettingContiner[] randomItems = GetRandomItems(itemCount, gambleItemData);

    //    ItemData[] items = new ItemData[randomItems.Length];
    //    for (int i = 0; i < randomItems.Length; i++)
    //    {
    //        items[i] = randomItems[i].itemData;
    //    }

    //    return items;
    //}

    //private RandomItemSettingContiner[] GetRandomItems(int itemCreateAmount, RandomItemData gambleItemData)
    //{
    //    RandomItemSettingContiner[] randomItemSets = new RandomItemSettingContiner[itemCreateAmount];
    //    for (int i = 0; i < itemCreateAmount; i++)
    //    {
    //        randomItemSets[i] = gambleItemData.GetRandomContainer(gambleItemData.GetRandomSettings());
    //    }

    //    return randomItemSets;
    //}

    public void LoadGamebleScene(string sceneName, Action OnLoaded)
    {
        if (isGamebleSceneOpened)
        {
            UnLoadGambleScene();
        }

        isGamebleSceneOpened = true;
        currentSceneName = sceneName;

        PopupLoading popup = PopupManager.instance.CreatePopup<PopupLoading>("PopupLoading").GetPopup();

        MoveSceneManager.instance.MoveSceneAsync(
            sceneName,
            loadSceneMode: UnityEngine.SceneManagement.LoadSceneMode.Additive,
            OnCompleteLoadScene:
            (data) =>
            {
                popup.RemovePopup();

                RandomLotteryManager randomLottery = GetRandomLotteryManager();
                randomLottery?.Init();

                BesidesGamebleSceneCanvasActive(false);

                OnLoaded?.Invoke();
            });
    }

    public void UnLoadGambleScene()
    {
        MoveSceneManager.instance.UnloadSceneAsync(currentSceneName);

        EndGamble();
    }

    private void EndGamble()
    {
        BesidesGamebleSceneCanvasActive(true);

        isGamebleSceneOpened = false;
    }

    private void BesidesGamebleSceneCanvasActive(bool isActive)
    {
        StageManager.instance.canvasManager?.SetAllUIManagersCanvasActive(isActive);
    }

    public RandomLotteryManager GetRandomLotteryManager()
    {
        if (!isGamebleSceneOpened)
        {
            Debug.LogError("[GambleManager] The gamble scene did not open.");
        }

        return FindObjectOfType<RandomLotteryManager>();
    }
}
