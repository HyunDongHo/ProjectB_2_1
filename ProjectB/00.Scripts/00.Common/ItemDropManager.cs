using RNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropManager : Singleton<ItemDropManager>
{
    int [] dropItemList = new int[20];
    double[] dropItemCountList = new double[20];


    public Dictionary<int, double> NormalStageDropItem()
    {
        BackendData.Chart.StageDrop.Item stageItem = StaticManager.Backend.Chart.StageDrop.GetItem(StaticManager.Backend.GameData.PlayerGameData.NowStageLevel);
                
        for(int  i= 0; i < dropItemList.Length; ++i)
        {
            dropItemList[i] = -1;
            dropItemCountList[i] = 0;
        }

        int nowDropItemIndex = 0;

        for(int i=0; i < stageItem.DropItemList.Count; ++i)
        {
            if (nowDropItemIndex >= 20)
                break;

            RandomSetting randomData = new RandomSetting(stageItem.DropItemList[i].Percent, stageItem.DropItemList[i].ItemID.ToString());
            RandomSetting result = RNGManager.instance.GetRandom(randomData);

            if (result.percentage == stageItem.DropItemList[i].Percent)
            {
                dropItemList[nowDropItemIndex] = stageItem.DropItemList[i].ItemID;
                dropItemCountList[nowDropItemIndex++] = stageItem.DropItemList[i].Count;
            }
        }

        //List<double> DropItemIDAndCount = new List<double>();
        Dictionary<int, double> ResultDropItems = new();
        for (int i=0; i < dropItemList.Length; ++i)
        {
            if (dropItemList[i] == -1)
                continue;

            Debug.Log($"{dropItemList[i]} x {dropItemCountList[i]}");
            ResultDropItems.Add(dropItemList[i], dropItemCountList[i]);
            //DropItemIDAndCount.Add(dropItemList[i]);
            //DropItemIDAndCount.Add(dropItemCountList[i]);
        }
        
        return ResultDropItems;
    }
    // 골드 획득하는 함수 만들기 (플레이어 위치만 넘겨주면 그 위치에 골드 떨어지고 흡수 )


}
