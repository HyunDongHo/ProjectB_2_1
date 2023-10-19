using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.Android;
using BackEnd;

public class AccountSceneManager : MonoBehaviour
{
    public VideoPlayer videoPlayer;

    [Space]

    public LoginManager loginManager;
    public GetDataManager getDataManager;

    [Space]
    public GameObject PermissionPopup;
    public AccessSetting accessSetting;
    public DownResources downResources;
    public Terms terms;

    [Space]

    public Image pressAnyKey;
    public Text progressText;

    private void Awake()
    {
        AddEvent();
    }

    private void Start()
    {
        DoCheckPermission();
    }

    private void StartAccountProgress()
    {
     //   videoPlayer.Play();
        //UserDataManager.instance.GetAutoPotion();
        UserDataManager.instance.InitGameStartData();
        pressAnyKey.gameObject.SetActive(false);
        FadeInOut.instance.FadeSet(FadeInOut.FADE_OUT);
        SoundManager.instance.PlaySound("Main_Title_BGM_01");

        FadeInOut.instance.FadeIn(DefineManager.DEFAULT_FADE_DURATION, OnComplete: () => StartCoroutine(StartGetData()));
    }

    private void OnDestroy()
    {
        RemoveEvent();
    }

    private void AddEvent()
    {
        videoPlayer.prepareCompleted += OnVideoPlayerPrepareCompleted;
    }

    private void RemoveEvent()
    {
        videoPlayer.prepareCompleted -= OnVideoPlayerPrepareCompleted;
    }

    private void OnVideoPlayerPrepareCompleted(VideoPlayer videoPlayer)
    {
        FadeInOut.instance.FadeIn(DefineManager.DEFAULT_FADE_DURATION, OnComplete: () => StartCoroutine(StartGetData()));
    }

    private IEnumerator StartGetData()
    {
        BackEndServerManager.instance.Init();

        Action<Action>[] asyncs =
        {
            (OnComplete) =>
            {
                progressText.text = "서버 로그인중...";

                loginManager.AutoLogin(() => OnComplete?.Invoke());
            },
            (OnComplete) =>
            {
                progressText.text = "서버 데이터 로드중...";

                getDataManager.GetData(() => OnComplete?.Invoke());
            },
            (OnComplete) =>
            {
                progressText.text = "서버 데이터 로드중...";

                BackEndServerManager.instance.InitServerChartData();
                OnComplete?.Invoke();
            },
            (OnComplete) =>
            {
                progressText.text = "게임 데이터 로드중...";

                PlayerDataManager.instance.Init();
                UserDataManager.instance.InitUserData(() => OnComplete?.Invoke());  
            }
        };

        yield return StartCoroutine(Logic.WaitAsync(asyncs));

        progressText.text = string.Empty;
        StartCoroutine(CheckStart());
    }

    private IEnumerator CheckStart()
    {
        pressAnyKey.gameObject.SetActive(true);

        while (true)
        {
            if (Input.anyKeyDown)
            {
                if (BackEndFunctions.instance.GetNickName() != "")
                    SceneSettingManager.instance.LoadAccountToLobbyStageScene();
                else
                    SceneSettingManager.instance.LoadAccountToNickNameScene();
            }

            yield return null;
        }
    }


    #region Permission

    public void StartResourcesDownload()
    {
        // 리소스 다운 실행

        //

    }

    public void DoCheckPermission()
    {
        // 저장공간(Write) 권한 체크(선택 권한)
        if (Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite) == false)
        {
            ActivateCheckPermission(true);
            PermissionPopup.SetActive(true);
        }
        else
        {
            accessSetting.gameObject.SetActive(false);
            DoCheckDownloadResources();
        }
    }
    void ActivateCheckPermission(bool _bActive)
    {
        PermissionPopup.SetActive(true);
        accessSetting.gameObject.SetActive(true);
    }

    public void OnEventCheckPermission()
    {
        if (true == accessSetting.bOnCheckPermission)
            return;

        StartCoroutine("CheckPermissionCoroutine");
    }

    IEnumerator CheckPermissionCoroutine()
    {
        accessSetting.bOnCheckPermission = true;

        yield return new WaitForEndOfFrame();

        // 저장공간(Write) 권한 체크(선택 권한)
        if (Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite) == false)
        {
            // 권한 요청
            Permission.RequestUserPermission(Permission.ExternalStorageWrite);

            yield return new WaitForSeconds(1f);
            yield return new WaitUntil(() => Application.isFocused == true);

            if (Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite) == false)
            {
                MoveSceneManager.instance.MoveSceneAsync(SceneSettingManager.INTRO_SCENE);
                yield break;
            }
        }
        // 권한이 있으면 다음 Scene으로 이동
        DoCheckDownloadResources();
    }

    void DoCheckDownloadResources()
    {
        // 다운로드 할 파일 있는경우
        if (IsPresentDownloadData())
        {
            ActivateCheckPermission(true);
            PermissionPopup.SetActive(true);
            downResources.gameObject.SetActive(true);
        }
        else // 다운로드 할 파일 없는경우
        {
            bool isFirstTerms = UserDataManager.instance.GetFirstTerms();
            bool isSeconTerms = UserDataManager.instance.GetSecondTerms();
            if (isFirstTerms && isSeconTerms)
            {
                EndTermsProgress();
            }
            else
            {
                ActivateCheckPermission(true);
                PermissionPopup.SetActive(true);
                terms.gameObject.SetActive(true);
            }
        }
    }
    public void EndTermsProgress()
    {
        terms.gameObject.SetActive(false);
        PermissionPopup.SetActive(false);
        StartAccountProgress();
    }

    private bool IsPresentDownloadData()
    {
        return false;
    }
    #endregion
}
