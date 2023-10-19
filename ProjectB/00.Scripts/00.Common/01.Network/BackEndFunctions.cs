using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using LitJson;

public enum ServerExecution
{
    Asynchronous,
    Synchronous
}

public class BackEndFunctions : Singleton<BackEndFunctions>
{
    public bool IsBackendReady = false;

    private void OnApplicatoinPause(bool isPause)
    {
        if (isPause)
            SendQueue.PauseSendQueue();
        else
            SendQueue.ResumeSendQueue();
    }

    public void BackEndInitialize()
    {
        var bro = Backend.Initialize(true);

        if(bro.IsSuccess())
        {
            SendQueue.StartSendQueue(true);
            IsBackendReady = true;
        }
        else
        {
            IsBackendReady = false;
        }


        //StartCoroutine(BackendAsyncPoll());
    }

    public void Update()
    {
        if(IsBackendReady == true)
        {
            Backend.AsyncPoll();
            SendQueue.Poll();
        }        
    }

    private IEnumerator BackendAsyncPoll()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(0.1f);

        while (true)
        {
            Backend.AsyncPoll();
            SendQueue.Poll();

            yield return waitForSeconds;
        }
    }

    public void AddPush()
    {
        Backend.Android.PutDeviceToken(Backend.Android.GetDeviceToken(),
            (result) =>
            {
                if (!result.IsSuccess())
                {
                    CreateErrorPopup(result);
                }
            });
    }

    public void RemovePush()
    {
        Backend.Android.DeleteDeviceToken(
            (result) =>
            {
                if (!result.IsSuccess())
                {
                    CreateErrorPopup(result);
                }
            });
    }

    public void RefereshToken(Action<BackendReturnObject> OnSuccess = null, Action<BackendReturnObject> OnFail = null)
    {
        Backend.BMember.RefreshTheBackendToken(
            (backendReturnObject) =>
            {
                OnSuccess?.Invoke(backendReturnObject);
            });
    }

    public void AutoLogin(Action<BackendReturnObject> OnSuccess = null, Action<BackendReturnObject> OnFail = null)
    {
        Backend.BMember.LoginWithTheBackendToken(
            (backendReturnObject) =>
            {
                if (backendReturnObject.IsSuccess())
                {
                    OnSuccess?.Invoke(backendReturnObject);
                }
                else
                {
                    OnFail?.Invoke(backendReturnObject);
                }
            });
    }

    public void GuestLogin(Action<BackendReturnObject> OnSuccess = null, Action<BackendReturnObject> OnFail = null)
    {
        Backend.BMember.GuestLogin("GUEST",
           (backendReturnObject) =>
           {
               if (backendReturnObject.IsSuccess())
               {
                   OnSuccess?.Invoke(backendReturnObject);
               }
               else
               {
                   OnFail?.Invoke(backendReturnObject);
               }
           });
    }

    public void GoogleLogin(string accessToken, Action<BackendReturnObject> OnSuccess = null, Action<BackendReturnObject> OnFail = null)
    {
        Backend.BMember.AuthorizeFederation(accessToken, FederationType.Google, "GPGS",
            (backendReturnObject) =>
            {
                if (backendReturnObject.IsSuccess())
                {
                    OnSuccess?.Invoke(backendReturnObject);
                }
                else
                {
                    OnFail?.Invoke(backendReturnObject);
                }
            });
    }

    public string GetNickName()
    {
        return Backend.UserNickName;
    }
    public void LogOut()
    {
        Backend.BMember.Logout();
    }

    public void DeleteAccount()
    {
        Backend.BMember.SignOut();
    }

    public bool GetServerDateTime(ref DateTime dateTime)
    {
        BackendReturnObject servertime = Backend.Utils.GetServerTime();

        if (servertime.IsSuccess())
        {
            string time = servertime.GetReturnValuetoJSON()["utcTime"].ToString();
            dateTime = DateTime.Parse(time);
            dateTime = dateTime.ToUniversalTime().AddHours(9);
            return true;
        }
        else
            return false;

    }
    public void GetAllChartList(Action<JsonData> OnSuccess)
    {
        Backend.Chart.GetChartList(
            (backendReturnObject) =>
            {
                if (backendReturnObject.IsSuccess())
                {
                    OnSuccess?.Invoke(backendReturnObject.GetReturnValuetoJSON());
                }
                else
                {
                    CreateErrorPopup(backendReturnObject);
                }
            });
    }

    public void GetChartToJsonData(string chartField, Action<JsonData> OnSuccess)
    {
        Backend.Chart.GetChartContents(chartField,
            (backendReturnObject) =>
            {
                if (backendReturnObject.IsSuccess())
                {
                    OnSuccess?.Invoke(backendReturnObject.GetReturnValuetoJSON());
                }
                else
                {
                    CreateErrorPopup(backendReturnObject);
                }
            });
    }

    public void GetMyData(string tableName, Where where, int limit = 10, Action<BackendReturnObject> OnSuccess = null, ServerExecution serverExecution = ServerExecution.Asynchronous)
    {
        switch (serverExecution)
        {
            case ServerExecution.Asynchronous:
                SendQueue.Enqueue(Backend.GameData.GetMyData, tableName, where, limit,
                    (asyncBackendReturnObject) =>
                    {
                        if (asyncBackendReturnObject.IsSuccess())
                        {
                            OnSuccess?.Invoke(asyncBackendReturnObject);
                        }
                        else
                        {
                            CreateErrorPopup(asyncBackendReturnObject);
                        }
                    });
                break;
            case ServerExecution.Synchronous:
                BackendReturnObject syncBackendReturnObject = Backend.GameData.GetMyData(tableName, where, limit);
                if (syncBackendReturnObject.IsSuccess())
                {
                    OnSuccess?.Invoke(syncBackendReturnObject);
                }
                else
                {
                    CreateErrorPopup(syncBackendReturnObject);
                }
                break;
        }
    }

    public void InsertData(string tableName, Param param, Action<BackendReturnObject> OnSuccess = null)
    {
        SendQueue.Enqueue(Backend.GameData.Insert, tableName, param, 
            (backendReturnObject) =>
            {
                if (backendReturnObject.IsSuccess())
                {
                    OnSuccess?.Invoke(backendReturnObject);
                }
                else
                {
                    CreateErrorPopup(backendReturnObject);
                }
            });
    }

    public void UpdateData(string tableName, Where where, Param param, Action<BackendReturnObject> OnSuccess = null)
    {
        SendQueue.Enqueue(Backend.GameData.Update, tableName, where, param,
            (backendReturnObject) =>
            {
                if (backendReturnObject.IsSuccess())
                {
                    OnSuccess?.Invoke(backendReturnObject);
                }
                else
                {
                    CreateErrorPopup(backendReturnObject);
                }
            });
    }

    public void DeleteData(string tableName, Where where, Action<BackendReturnObject> OnSuccess = null)
    {
        SendQueue.Enqueue(Backend.GameData.Delete, tableName, where,
            (backendReturnObject) =>
            {
                if (backendReturnObject.IsSuccess())
                {
                    OnSuccess?.Invoke(backendReturnObject);
                }
                else
                {
                    CreateErrorPopup(backendReturnObject);
                }
            });
    }

    public void CreateErrorPopup(BackendReturnObject backendReturnObject)
    {
        PopupErrorMessage popup = PopupManager.instance.CreatePopup<PopupErrorMessage>("PopupErrorMessage").GetPopup();
        popup.SetErrorMessage($"{backendReturnObject.GetStatusCode()} \n {backendReturnObject.GetMessage()}");
        Debug.LogError($"{backendReturnObject.GetStatusCode()} \n {backendReturnObject.GetMessage()}");  
        Debug.LogError($"{backendReturnObject.GetErrorCode()}");  
    }
}
