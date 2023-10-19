using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scheduler;
using RNG;
using LitJson;
using BackEnd;
using UnityEngine.UI;

public class SettingSceneManager : MonoBehaviour
{
    public bool isSkipIntro = false;
    public bool isShowFPS = false;
    public bool isForcePerformanceDown = false;
    public bool isKorean = true;

    public InputField text;  
    public Text text1;

    private void Awake()
    {
        BackEndFunctions.instance.BackEndInitialize();
        Application.targetFrameRate = 60;

        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        //string hash = Backend.Utils.GetGoogleHash();
        //Debug.Log(hash);
        //text.text = hash;
        //text1.text = hash;

        //FPS Check 임시 코드
        if (isShowFPS)
        {
            FindObjectOfType<FPSCheck>().ShowFPS();
        }

        if(isForcePerformanceDown)
        {
            ForcePerformanceDown.instance.Init();
        }

        if (isKorean)
            UserDataManager.instance.SetIsKorean(true);
        else
            UserDataManager.instance.SetIsKorean(false);

       // PlayersControlManager.instance.Init();
       // PlayersControlManager.instance.DontDestroyControls();
    }

    private void Start()
    {
        QuestDataEventManager.instance.Init();
        MoveSceneEventManager.instance.Init();
        DataManager.instance.Init();
        DamageTextManager.instance.Init();

        QuestsManager.instance.Init();
        RewardManager.instance.Init();
        AdsBuffManager.instance.Init();
        //MoveSceneManager.instance.MoveSceneAsync(isSkipIntro ? SceneSettingManager.ACCOUNT_SCENE : SceneSettingManager.INTRO_SCENE);
        StartCoroutine(CoStartAccountScene());
    }

    IEnumerator CoStartAccountScene()
    {
        yield return null;
        MoveSceneManager.instance.MoveSceneAsync(isSkipIntro ? SceneSettingManager.ACCOUNT_SCENE : SceneSettingManager.INTRO_SCENE);
    }
}
