using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;
using System;

public enum NicknameCameraState
{
    Normal,
    Zoom,
}
public enum LobbyState
{
    Left,
    Center,
    Right
}

public class Define
{
    public const int RankOutNum = 10;
    public const int RankUpdageMinLevel = 5;
    public const int RankUpdateMinTime = 1;
    public const int PostUpdateMinTime = 3;

    public const double RandomShopRefreshTime = 600;
    public enum ExpressionType
    {
        LevelExp,
        BossHP,
        BossATK,
        NormalHP,
        NormalATK,
        GetGold,
        GetExp,
        WarriorTrainingHP,
        WarriorTrainingATK,
        ArcherTrainingHP,
        ArcherTrainingATK,
        WizardTrainingHP,
        WizardTrainingATK,
        GetTrainingReward,      
    }
    public enum EnhancementResult
    {
        Upgrade,
        Destroy,
    }

    public enum UIEvent
    {
        Click,
        Drag,
        PointerDown,
        PointerUp,
    }

    public enum EnumEquipmentType
    {
        Sword,
        Bow,
        Staff,
        None
    }

    public enum EnumSpawnItemType
    {
        Sword,
        Bow,
        Staff,
        Partner,
        Treasure,
        None
    }

    public enum StatType
    {
        AttackRatio,
        HpRatio,
        CriticalPer,
        CriticalRatio,
        BossDamage,
        DungeonDamage,
        AttackSpeed,
        MoveSpeed,
        GoldGetRatio,
        ExpGetRatio,
        ItemGetRatio
    }

    public enum ItemType
    {
        Equipment,
        Goods,
        None
    }

    public static Dictionary<StatType, string> StatStatTitle = new Dictionary<StatType, string>()
    {
        {StatType.AttackRatio, "공격력" },
        {StatType.HpRatio, "체력" },
        {StatType.CriticalPer, "치명타 확률" },
        {StatType.CriticalRatio, "치명타 피해량" },
        {StatType.BossDamage, "보스 피해량" },
        {StatType.DungeonDamage, "던전 데미지" },
        {StatType.AttackSpeed, "공격속도" },
        {StatType.MoveSpeed, "이동속도" },
        {StatType.GoldGetRatio, "골드 획득량" },
        {StatType.ExpGetRatio, "경험치 획득량" },
        {StatType.ItemGetRatio, "아이템 획득 확률" },
    };

    public static Dictionary<int, string> DiceGradeStat = new Dictionary<int, string>()
    {
        {0, ""},
        {1 ,"D" },
        {2, "C" },
        {3, "B" },
        {4, "A" },
        {5, "S"},
        {6, "SS" },
        {7, "SSS" }
    };

    public static Dictionary<int, Color> DiceGradeColor = new Dictionary<int, Color>()
    {
        {0, Color.gray},
        {1 ,Color.green },
        {2, Color.blue },
        {3, Color.cyan },
        {4, Color.yellow },
        {5, Color.red},
        {6, Color.red },
        {7, Color.red }
    };

    public static Dictionary<int, string> ChapterNames = new Dictionary<int, string>()
    {
        {1 ,"태고의 숲" },
        {2, "혹한의 땅" },
        {3, "붉은 대지"},
        {4, "황폐한 사막" },
        {5, "악령의 동굴"},

    };
    public enum ContentType
    {
        Top,
        None
    }
    public class Util
    {
        public static float GetExpressionValue(ExpressionType expressionType, float x)
        {
            float returnValue = -1;

            BackendData.Chart.Expression.Item item = StaticManager.Backend.Chart.Expression.FindItem(expressionType);

            if (item == null)
                return returnValue;
            else
                return item.A * Mathf.Pow(x, item.B) + (x * item.C);




        }



    }


}
