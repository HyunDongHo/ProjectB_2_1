using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scheduler;
using LitJson;

public class GetDataManager : MonoBehaviour
{
    public void GetData(Action OnDataLoaded)
    {
        GetAllServerCharts(
            () =>
            {
                OnDataLoaded?.Invoke();
            });
    }

    private void GetAllServerCharts(Action OnSuccess)
    {
        BackEndFunctions.instance.GetAllChartList(
             (data) =>
             {
                 int currentRow = 0;

                 int rowsCount = data["rows"].Count;
                 for (int i = 0; i < rowsCount; i++)
                 {
                     GetServerChart(data["rows"][i],
                         OnSuccess: () =>
                         {
                             if(currentRow++ == rowsCount - 1)
                                OnSuccess?.Invoke();
                         });
                 }
             });
    }

    private void GetServerChart(JsonData jsonData, Action OnSuccess)
    {
        TimerBuffer getDataDelay = new TimerBuffer(0.25f);
        Timer.instance.TimerStart(getDataDelay,
            OnComplete: () =>
            {
                BackEndServerManager.instance.AddChartData(Logic.ChangeObjectToValue<string>(jsonData["chartName"]["S"].ToString()), (int)Logic.ChangeObjectToValue<float>(jsonData["selectedChartFileId"]["N"].ToString()),
                    OnChartLoaded: () => OnSuccess?.Invoke());
            });
    }
}
