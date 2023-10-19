using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using CodeStage.AntiCheat.Storage;
using BackEnd;

public class LoginManager : MonoBehaviour
{
    public GameObject signUpParent;

    public Button guestButton;
    public Button googleButton;
    public Button deleteButton;

    private void Start()
    {
        //PlayGamesClientConfiguration config = new PlayGamesClientConfiguration
        //    .Builder()
        //    .RequestServerAuthCode(false)
        //    .RequestEmail()
        //    .RequestIdToken()
        //    .Build();

        //PlayGamesPlatform.InitializeInstance(config);
        //PlayGamesPlatform.DebugLogEnabled = false;

        //PlayGamesPlatform.Activate();
    }

    public void SignUpActive(bool isActive)
    {
        signUpParent.SetActive(isActive);
    }

    public void AutoLogin(Action OnLoginSuccess)
    {
        //PlayerPrefs.DeleteAll();

        guestButton.onClick.AddListener(() => GuestLogin(OnLoginSuccess));
        googleButton.onClick.AddListener(() => GoogleLogin(OnLoginSuccess));
        deleteButton.onClick.AddListener(() => AllDataRemove());

        SignUpActive(true);
        //BackEndFunctions.instance.RefereshToken(
        //    OnSuccess: (result) =>
        //    {
        //        BackEndFunctions.instance.AutoLogin(
        //            OnSuccess: (success) =>
        //            {
        //                CompleteAccountLogin(OnLoginSuccess);
        //            },
        //            OnFail: (fail) =>
        //            {
        //                if (fail.GetStatusCode() == ServerErrorDefine.AccessTokenError || fail.GetStatusCode() == ServerErrorDefine.DifferentDeviceLogin)
        //                {
        //                    PlayerPrefs.DeleteAll();

        //                    guestButton.onClick.AddListener(() => GuestLogin(OnLoginSuccess));
        //                    googleButton.onClick.AddListener(() => GoogleLogin(OnLoginSuccess));

        //                    SignUpActive(true);
        //                }
        //                else
        //                {
        //                    if (fail.GetStatusCode() == ServerErrorDefine.GamerNotFound)
        //                    {
        //                        DeleteBackEndFile();
        //                    }

        //                    BackEndFunctions.instance.CreateErrorPopup(fail);
        //                }
        //            });
        //    });
    }

    private void GuestLogin(Action OnLoginSuccess)
    {
        guestButton.onClick.RemoveAllListeners();

        BackEndFunctions.instance.GuestLogin(
            OnSuccess: (success) =>
            {
                CompleteAccountLogin(OnLoginSuccess);
            },
            OnFail: (fail) =>
            {
                if (fail.GetStatusCode() == "401")
                {
                    DeleteBackEndFile();

                    GuestLogin(OnLoginSuccess);
                }

                BackEndFunctions.instance.CreateErrorPopup(fail);
            });
    }

    private void GoogleLogin(Action OnLoginSuccess)
    {
        googleButton.onClick.RemoveAllListeners();

        //if (!PlayGamesPlatform.Instance.localUser.authenticated)
        //{
        //    Social.localUser.Authenticate(result =>
        //    {
        //        if (result)
        //        {
        //            BackEndFunctions.instance.GoogleLogin(GetGoogleTokens(),
        //                OnSuccess: (success) =>
        //                {
        //                    CompleteAccountLogin(OnLoginSuccess);
        //                },
        //                OnFail: (fail) =>
        //                {
        //                     BackEndFunctions.instance.CreateErrorPopup(fail);
        //                });
        //        }
        //        else
        //        {
        //            Debug.Log("구글 로그인 실패");
        //            return;
        //        }
        //    });
        //}
    }

    private void DeleteBackEndFile()
    {
        string backendFile = $"{Application.persistentDataPath}/backend.dat";

        if (System.IO.File.Exists(backendFile))
            System.IO.File.Delete(backendFile);
    }

    public void AllDataRemove()
    {
        string backendFile = $"{Application.persistentDataPath}/backend.dat";

        if (System.IO.File.Exists(backendFile))
            System.IO.File.Delete(backendFile);

        ObscuredPrefs.DeleteAll();
        Backend.BMember.DeleteGuestInfo();
    }

    private string GetGoogleTokens()       
    {
        return "";
        //if (PlayGamesPlatform.Instance.localUser.authenticated)
        //{
        //    return PlayGamesPlatform.Instance.GetIdToken();
        //}
        //else
        //{
        //    Debug.Log("접속되어있지 않습니다. 잠시 후 다시 시도하세요.");
        //    return string.Empty;
        //}
    }

    private void CompleteAccountLogin(Action OnLoginSuccess)
    {
        OnLoginSuccess?.Invoke();

        SignUpActive(false);
    }
}
