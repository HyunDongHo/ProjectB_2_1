using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInfoDataManager : MonoBehaviour
{
    public void Test(Action OnDataLoaded)
    {
        BackEndFunctions.instance.GetMyData(ServerGameDefine.PLAYER_GAME_DATA, new BackEnd.Where(),
            OnSuccess: (data) =>
            {
                if (data.GetReturnValuetoJSON()["rows"].Count <= 0)
                {
                    CreateNewGameData();
                }
                 
                OnDataLoaded();
            });
    }

    private void CreateNewGameData()
    {
        BackEndFunctions.instance.InsertData(ServerGameDefine.PLAYER_GAME_DATA, new BackEnd.Param());

        BackEnd.Param param = new BackEnd.Param();

        param.Add("Level", 1);
        param.Add("Coin", 0);
        param.Add("Ruby", 0);

        BackEndFunctions.instance.UpdateData(ServerGameDefine.PLAYER_GAME_DATA, new BackEnd.Where(), param);
    }
}
