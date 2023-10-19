using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_StageAdsItem : MonoBehaviour
{
    private int _stageAdsID;
    public UI_LoadingImage loadingImage;
    public Button button;

    public void SetStageAdsId(int id)
    {
        _stageAdsID = id;
    }
    public int GetStageAdsId()
    {
        return _stageAdsID;
    }
    public void ToggleSetUI(bool isOn)
    {
        if (isOn)
        {
            loadingImage.SetLoadingState(true);
        }
        else
        {
            loadingImage.SetLoadingState(false);
        }
    }

}
