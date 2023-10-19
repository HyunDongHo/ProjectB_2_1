using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoveSceneEventManager : Singleton<MoveSceneEventManager>
{
    private void Awake()
    {
        MoveSceneManager.instance.OnStartSceneChanged +=
            (loadSceneMode) =>
            {
                if (loadSceneMode == UnityEngine.SceneManagement.LoadSceneMode.Single)
                {
                    DOTween.KillAll();

                    Scheduler.Timer.instance.ClearAllTimer();

                    ObjectPoolManager.instance.ClearAllObject();

                    SoundManager.instance.ClearAllSound();
                }
            };
    }
}
