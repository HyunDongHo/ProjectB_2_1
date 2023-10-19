using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdsBuffManager : Singleton<AdsBuffManager>
{
    public void UpdateAdsBuffUI(double count)
    {
        foreach(var chartData in StaticManager.Backend.Chart.AdsBuff.GetChartAdsBuffData())
        {
            BackendData.GameData.AdsBuffData data = null;
            data = StaticManager.Backend.GameData.PlayerAdsBuff.AdsBuffList.Find(item => item.AdsBuffID == chartData.Value.AdsID);
            if (data == null)
                continue;
            if (data.AdsBuffing == false)
                continue;

            /* ���� ������Ʈ */
            StaticManager.Backend.GameData.PlayerAdsBuff.AddNowStep(data.AdsBuffID, count);

            /* UI ������Ʈ */
            if (StageManager.instance.canvasManager.GetUIManager<UI_AdsBuffPopup>() != null)
                StageManager.instance.canvasManager.GetUIManager<UI_AdsBuffPopup>().UpdateAdsBuffCountUI(data.AdsBuffID, data.AdsBuffNowStep);    

        }


    }
}
