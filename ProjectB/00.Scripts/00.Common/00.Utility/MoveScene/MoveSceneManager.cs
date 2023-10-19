using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Scheduler;

public class MoveSceneManager : Singleton<MoveSceneManager>
{
    private bool isChangineScene = false;

    public Action<LoadSceneMode> OnStartSceneChanged;
    public Action<LoadSceneMode> OnEndSceneChanged;

    public void MoveSceneAsync(string sceneName, bool isFade = false,
        string loadingSceneName = SceneSettingManager.LODING_SCENE_DEFAULT, LoadSceneMode loadSceneMode = LoadSceneMode.Single,
        Action<AsyncOperation> OnCompleteLoadScene = null)
    {
        if (!isChangineScene)
        {
            isChangineScene = true;
        }
        else
            return;

        if (FadeInOut.instance.fadePanel != null && isFade)
        {
            FadeInOut.instance.FadeOut(DefineManager.DEFAULT_FADE_DURATION, () => MoveSceneAfterLoading(sceneName, loadingSceneName, loadSceneMode, OnCompleteLoadScene));
        }
        else
        {
            MoveSceneAfterLoading(sceneName, loadingSceneName, loadSceneMode, OnCompleteLoadScene);
        }
    }

    private void MoveSceneAfterLoading(string sceneName, string loadingSceneName, LoadSceneMode loadSceneMode, Action<AsyncOperation> OnCompleteLoadScene)
    {
        OnStartSceneChanged?.Invoke(loadSceneMode);

        SceneManager.LoadSceneAsync(loadingSceneName, loadSceneMode).completed += (loadingData) =>
        {
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName, loadSceneMode);
            asyncOperation.completed +=
                (loadedData) =>
                {
                    if (loadSceneMode == LoadSceneMode.Additive)
                        UnloadSceneAsync(loadingSceneName);

                    OnCompleteLoadScene?.Invoke(loadedData);
                    OnEndSceneChanged?.Invoke(loadSceneMode);

                    isChangineScene = false;
                };

            FindObjectOfType<LoadingSceneManagers>().Init(asyncOperation);
        };
    }

    public void UnloadSceneAsync(string sceneName)
    {
        SceneManager.UnloadSceneAsync(sceneName);
    }
}
