using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scheduler;
using System.Linq;
using System;

public class PlayerBuffGameData
{
    public string buffName;
    public float buffTime;
}

public class PlayerBuff : MonoBehaviour
{
    private PlayerControl playerControl;
    private Dictionary<string, TimerBuffer> buffs = new Dictionary<string, TimerBuffer>();
    public List<PlayerBuffGameData> nowBuffs = new List<PlayerBuffGameData>();
    
    //public Dictionary<string, float> buffDict = new Dictionary<string, float>();   

    public void Init(PlayerControl playerControl)
    {
        this.playerControl = playerControl;
      //  InitBuff(UserDataManager.instance?.buffs);
      //
      //  if (nowBuffs != null)
      //  {
      //      foreach (PlayerBuffGameData buff in nowBuffs)
      //      {
      //          switch(buff.buffName)
      //          {
      //              case "G_Potion":
      //                  G_Potion_ItemData item = Resources.Load<G_Potion_ItemData>(buff.buffName);
      //                  playerControl.utility.buff.Set(PlayerStatsValueDefine.MoveSpeedRatio, new AdditionValue($"G_Potion_{PlayerStatsValueDefine.MoveSpeedRatio}", Operation.Ratio, item.moveRatioValue), (float)buff.buffTime);
      //                  playerControl.utility.buff.Set(PlayerStatsValueDefine.AttackSpeedRatio, new AdditionValue($"G_Potion_{PlayerStatsValueDefine.AttackSpeedRatio}", Operation.Ratio, item.attackSpeedRatioValue), (float)buff.buffTime);
      //                  break;
      //              case "Pw_G_Potion":
      //                  Pw_G_Potion_ItemData pwItem = Resources.Load<Pw_G_Potion_ItemData>(buff.buffName);
      //                  playerControl.utility.buff.Set(PlayerStatsValueDefine.MoveSpeedRatio, new AdditionValue($"G_Potion_{PlayerStatsValueDefine.MoveSpeedRatio}", Operation.Ratio, pwItem.moveRatioValue), (float)buff.buffTime);
      //                  playerControl.utility.buff.Set(PlayerStatsValueDefine.AttackSpeedRatio, new AdditionValue($"G_Potion_{PlayerStatsValueDefine.AttackSpeedRatio}", Operation.Ratio, pwItem.attackSpeedRatioValue), (float)buff.buffTime);
      //                  break;
      //              case "Grow_Potion":
      //                  GrowPotionItemData growPotion = Resources.Load<GrowPotionItemData>(buff.buffName);
      //                  playerControl.utility.buff.Set(PlayerStatsValueDefine.ExpBonusRatio, new AdditionValue($"Grow_Potion_{PlayerStatsValueDefine.ExpBonusRatio}", Operation.Ratio, growPotion.ratio), (float)buff.buffTime);
      //                  break;
      //              case "Pw_Grow_Potion":
      //                  Pw_GrowPotionItemData pw_growPotion = Resources.Load<Pw_GrowPotionItemData>(buff.buffName);
      //                  playerControl.utility.buff.Set(PlayerStatsValueDefine.ExpBonusRatio, new AdditionValue($"Grow_Potion_{PlayerStatsValueDefine.ExpBonusRatio}", Operation.Ratio, pw_growPotion.ratio), (float)buff.buffTime);
      //                  break;
      //          }
      //
      //      }
      //     
      //  }
    }

    public void FixedUpdate()
    {
        if (nowBuffs != null && nowBuffs.Count < 1)
            return;

        for (int i=0; i < nowBuffs.Count; ++i)
        {
            double buffTime = nowBuffs[i].buffTime - Time.deltaTime;

            buffTime = Math.Round(buffTime, 2, MidpointRounding.ToEven);

            if (buffTime <= 0)
            {
               
            }
            else
                nowBuffs[i].buffTime = (float)buffTime;
        }

        for(int i=0; i < nowBuffs.Count; ++i)
        {
            if (nowBuffs[i].buffTime <= 0)
                nowBuffs.RemoveAt(i);
        }
    }

    public void Release() { }

    public void Set(string targetName, AdditionValue additionValue, float time)
    {
        TimerBuffer buffer = null;

        if (!buffs.ContainsKey(additionValue.additionName))
        {
            buffer = new TimerBuffer(time);
            buffs.Add(additionValue.additionName, buffer);
        }
        else
        {
            buffer = buffs[additionValue.additionName];
        }

        playerControl.GetStats<PlayerStats>().manager.RemoveAdditionValue(targetName, additionValue.additionName, additionValue.operation, additionValue.value);
        playerControl.GetStats<PlayerStats>().manager.SetAdditionValue(targetName, additionValue);
        Timer.instance.TimerStart(buffer,
            OnComplete: () =>
            {
                playerControl.GetStats<PlayerStats>().manager.RemoveAdditionValue(targetName, additionValue.additionName, additionValue.operation, additionValue.value);
                buffs.Remove(additionValue.additionName);
            });
    }

    public void InitBuff(List<PlayerBuffItemData> playerBuff)
    {
        nowBuffs.Clear();

        for (int i = 0; i < playerBuff.Count; ++i)
        {
            PlayerBuffGameData playerBuffGameData = new PlayerBuffGameData();
            playerBuffGameData.buffName = playerBuff[i].buffName;
            playerBuffGameData.buffTime = playerBuff[i].buffTime;

            nowBuffs.Add(playerBuffGameData);
        }
    }

    public bool GetIsAlreadyBuff(string additionName)
    {
        TimerBuffer timerBuffer = null;
        buffs.TryGetValue(additionName, out timerBuffer);

        if (timerBuffer == null)
            return false;

        return true;
    }
}
