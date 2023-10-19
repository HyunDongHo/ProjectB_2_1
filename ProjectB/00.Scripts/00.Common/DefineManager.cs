using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DefineManager : MonoBehaviour
{
    //CSV
    public const string NPC_SOUND = "npc_sound";

    //Random Data
    public const string SUCCESS = "Success";

    //FADE
    public const float DEFAULT_FADE_DURATION = 0.25f;

    //Fusion
    public const int DEFAULT_FUSION_SLOT_COUNT = 4;

    //Object Changer
    public const int OBJECT_CHANGER_SELECTED = 0;

    //Camera
    public const float DEFAULT_CAMERA_TIME = 0.25f;
}

public class ChartDefine
{
    public const string PLAYER_DATA_CHART = "PlayerData";
    public const string PLAYER_UPGRADE = "PlayerUpgrade";
    public const string ENEMY_DATA_CHART = "EnemyData";
    public const string ENEMY_REWARD_CHART = "EnemyReward";
    public const string BOSS_REWARD_CHART = "BossReward";
    public const string STAGE_ENEMY_DATA = "StageEnemyData";
    public const string STAGE_ADJUST_DATA = "StageAdjustData";
    public const string PET_DATA = "PetData";
    public const string PET_OPTION = "PetOption";
    public const string GAMBLE_CARD_OPTION = "GambleCardOption";
    public const string EQUIPMENET_DATA = "EquipmentData";
    public const string EQUIPMENET_OPTION = "EquipmentOption";
    public const string EQUIPMENET_Level = "EquipmentLevel";
    public const string EQUIPMENET_ENHANCEMENT = "EquipmentEnhancement";
    public const string QUEST_DATA = "QuestData";
    public const string QUEST_TEXT = "QuestText";
    public const string LOCALIZE_DATA = "LocallizeData";
    public const string ITEM_DATA = "ItemData";
    public const string PLAYER_ENHANCEMENT_DATA = "PlayerEnhancementData";

    public const string SORT_NUMBER = "{0}.";
}

public class ServerGameDefine
{
    public const string PLAYER_GAME_DATA = "PlayerGameData";
    public const string PLAYER_INVENTORY = "PlayerInventory";
    public const string PLAYER_EQUIPMENT = "PlayerEquipment";
    public const string PLAYER_INFO = "PlayerInfo";
    public const string PLAYER_QUEST_DATA = "PlayerQuestData";
    public const string PLAYER_STAGE_DATA = "PlayerStageData";
    public const string PLAYER_UPGRADE = "PlayerUpgrade";
    public const string PLAYER_QUICKSLOT = "PlayerQuickSlot";
}

public class Scene
{
}

//public class ItemLogic
//{
//    public static EquipmentWrapType GetEquipmentWrapKind(EquipmentItemData equipmentItemData)
//    {
//        EquipmentWrapType equipmentWrapType = EquipmentWrapType.Max;

//        switch (equipmentItemData)
//        {
//            case WeaponItemData weaponItemData:
//                equipmentWrapType = EquipmentWrapType.Weapon;
//                break;
//            case ProtectiveGearItemData protectiveGearItemData:
//                equipmentWrapType = EquipmentWrapType.ProtectiveGear;
//                break;
//            case AccessaryItemData accessaryItemData:
//                equipmentWrapType = EquipmentWrapType.Accessary;
//                break;
//            case PetItemData petItemData:
//                equipmentWrapType = EquipmentWrapType.Pet;
//                break;
//        }

//        return equipmentWrapType;
//    }

//    public static EquipmentType GetEquipmentItemKind(EquipmentItemData equipmentItemData)
//    {
//        EquipmentType equipmentType = EquipmentType.Max;

//        switch (equipmentItemData)
//        {
//            case WeaponItemData weaponItemData:
//                equipmentType = EquipmentType.Weapon;
//                break;
//            case ProtectiveGearItemData protectiveGearItemData:
//                switch (protectiveGearItemData.itemKind)
//                {
//                    case SubItemKind_ProtectiveGear.Helmet:
//                        equipmentType = EquipmentType.Helmet;
//                        break;
//                    case SubItemKind_ProtectiveGear.Armor:
//                        equipmentType = EquipmentType.Armor;
//                        break;
//                    case SubItemKind_ProtectiveGear.Glove:
//                        equipmentType = EquipmentType.Glove;
//                        break;
//                    case SubItemKind_ProtectiveGear.Belt:
//                        equipmentType = EquipmentType.Belt;
//                        break;
//                    case SubItemKind_ProtectiveGear.Gaiter:
//                        equipmentType = EquipmentType.Gaiter;
//                        break;
//                    case SubItemKind_ProtectiveGear.Boots:
//                        equipmentType = EquipmentType.Boots;
//                        break;
//                }
//                break;
//            case AccessaryItemData accessaryItemData:
//                switch (accessaryItemData.itemKind)
//                {
//                    case SubItemKind_Accessary.Earring:
//                        equipmentType = EquipmentType.Earring;
//                        break;
//                    case SubItemKind_Accessary.Necklace:
//                        equipmentType = EquipmentType.Necklace;
//                        break;
//                    case SubItemKind_Accessary.Ring:
//                        equipmentType = EquipmentType.Ring;
//                        break;
//                }
//                break;
//            case PetItemData petItemData:
//                equipmentType = EquipmentType.Pet;
//                break;
//        }

//        return equipmentType;
//    }
//}

public class EnchancementLogic
{
    public static Color GetLevelColor(int level)
    {
        if (level <= 5)
        {
            return new Color(1.0f, 1.0f, 1.0f);
        }
        else if (level <= 10)
        {
            return new Color(1.0f, 1.0f, 0.7f);
        }
        else if (level <= 15)
        {
            return new Color(0.65f, 1.0f, 0.7f);
        }
        else
        {
            return new Color(1.0f, 0.15f, 0.15f);
        }
    }

    public static string GetConvertLevelText(int level)
    {
        if (level > 0)
            return $"+{level}";
        else
            return string.Empty;
    }

    public static string GetOriginalResultText()
    {
        return "장비 강화";
    }

    public static Color GetOriginalResultTextColor()
    {
        return Color.black;
    }

    public static Color GetOriginalResultBackgroundColor()
    {
        return Color.white;
    }

    //    public static string GetResultText(int enhancementLevel, EnhancementResult enchancementResult)
    //    {
    //        switch (enchancementResult)
    //        {
    //            case EnhancementResult.Upgrade:
    //                return $"+{enhancementLevel} 강화에 성공했습니다";
    //            case EnhancementResult.Destroy:
    //                return "강화에 실패하여 장비가 증발했습니다.";
    //        }

    //        return GetOriginalResultText();
    //    }

    //    public static Color GetResultTextColor(EnhancementResult enchancementResult)
    //    {
    //        switch (enchancementResult)
    //        {
    //            case EnhancementResult.Upgrade:
    //                return new Color(23, 255, 116, 255) / 255.0f;
    //            case EnhancementResult.Destroy:
    //                return Color.red;
    //        }

    //        return GetOriginalResultTextColor();
    //    }

    //    public static Color GetResultBackgroundColor(EnhancementResult enchancementResult)
    //    {
    //        switch (enchancementResult)
    //        {
    //            case EnhancementResult.Upgrade:
    //                return Color.white;
    //            case EnhancementResult.Destroy:
    //                return Color.white;
    //        }

    //        return GetOriginalResultBackgroundColor();
    //    }
    }

    public class Logic
    {
        public static void CloseAllWindow()
        {
            OpenClose[] openCloses = GameObject.FindObjectsOfType<OpenClose>();

            foreach (OpenClose openClose in openCloses)
            {
                openClose.SetClose(false, false);
            }
        }

        public static int GetNearestObjectIndex(Vector3 myPosition, Component[] otherPositions, string objectLayer, float maxDistance = 60)
        {
            float minDistance = float.MaxValue;
            int minDistanceIndex = -1;

            for (int i = 0; i < otherPositions.Length; i++)
            {
                if (otherPositions[i].gameObject.layer != LayerMask.NameToLayer(objectLayer))
                    continue;

                float distance = Vector3.Distance(myPosition, otherPositions[i].gameObject.transform.position);

                if (minDistance > distance && distance <= maxDistance)
                {
                    minDistance = distance;
                    minDistanceIndex = i;
                }
            }

            return minDistanceIndex;
        }
        public static PlayerControl GetRandomPlayerObjectIndex()
        {
            OutsideStageManager outsideStageManager = (StageManager.instance as OutsideStageManager);
            PlayerControl returnObj = null;

            if (outsideStageManager == null)
                return null;

            for (int i = 0; i < PlayersControlManager.instance.playersContol.Length; ++i)
            {
                if (PlayersControlManager.instance.playersContol[i].GetStats<Stats>().hp.isAlive == false)
                    continue;

                if (returnObj == null)
                    returnObj = PlayersControlManager.instance.playersContol[i];
                else
                {
                    int randNum = UnityEngine.Random.Range(0, 2);

                    if (randNum == 0)
                        continue;
                    else
                        returnObj = PlayersControlManager.instance.playersContol[i];
                }
            }

            return returnObj;
        }

        //public static PlayerControl GetRandomPlayerObjectIndex()
        //{
        //    OutsideStageManager outsideStageManager = (StageManager.instance as OutsideStageManager);
        //    PlayerControl returnObj = null;

        //    if (outsideStageManager == null)
        //        return null;           

        //    for(int i=0; i < outsideStageManager.playersControl.playersContol.Length; ++i)
        //    {
        //        if (outsideStageManager.playersControl.playersContol[i].GetStats<Stats>().hp.isAlive == false)
        //            continue;

        //        if (returnObj == null)
        //            returnObj = outsideStageManager.playersControl.playersContol[i];
        //        else
        //        {
        //           int randNum = UnityEngine.Random.Range(0, 2);

        //            if (randNum == 0)
        //                continue;
        //            else
        //                returnObj = outsideStageManager.playersControl.playersContol[i];
        //        }            
        //    }

        //    return returnObj;
        //}

        public static float ConvertSignedAngle_0_360(float value)
        {
            return value < 0 ? value + 360 : value;
        }

        public static Vector3 Trigonometry_XZ(float cos, float sin, float amplitude)
        {
            Vector3 movement = Vector3.zero;

            movement.x = Mathf.Cos(cos) * amplitude;
            movement.z = Mathf.Sin(sin) * amplitude;

            return movement;
        }

        public static string DeleteCloneText(string targetString)
        {
            return targetString.Replace("(Clone)", string.Empty);
        }

        public static string ToLowercaseTheFirstLetter(string targetStringtargetString)
        {
            return char.ToLower(targetStringtargetString[0]) + targetStringtargetString.Substring(1);
        }

        public static string CustomStringFormat(string targetString, int numberOfDigits = 1, params int[] value)
        {
            string numberOfDigitsString = string.Empty;
            for (int i = 0; i < numberOfDigits; i++)
                numberOfDigitsString += "0";

            for (int i = 0; i < value.Length; i++)
            {
                string pattern = "{" + i + "}";

                targetString = targetString.Replace(pattern, value[i].ToString(numberOfDigitsString));
            }

            return targetString;
        }

        public static T ChangeObjectToValue<T>(object value)
        {
            return ChangeToValue<T>(value);
        }

        public static T ChangeStringToValue<T>(string value)
        {
            return ChangeToValue<T>(value);
        }

        private static T ChangeToValue<T>(object value)
        {
            Type type = typeof(T);

            object convertValue = value;

            if (type == typeof(float))
            {
                if (float.TryParse(value as string, out float f))
                {
                    convertValue = f;
                }
                else if (int.TryParse(value as string, out int n))
                {
                    convertValue = (float)n;
                }
            }
            else if (type == typeof(int))
            {
                Debug.Log("[ChangeStringToValue] 데이터 유실을 없애기 위해 float로 가져온 후 int로 강제 변환 하십시오.");

                if (int.TryParse(value as string, out int n))
                    convertValue = n;
            }
            else if (type == typeof(bool))
            {
                if (bool.TryParse(value as string, out bool b))
                    convertValue = b;
            }

            return (T)convertValue;
        }

        public static string SetParentJson(string parent, params string[] json)
        {
            string connectJson = string.Empty;

            connectJson = "[" + ConnectJson(json) + "]";

            if (!string.IsNullOrEmpty(parent))
            {
                string convertParent = "\"" + parent + "\"" + ":";
                connectJson = "{" + convertParent + connectJson + "}";
            }

            return connectJson;
        }

        public static string ConnectJson(params string[] json)
        {
            string connetJson = string.Empty;

            for (int i = 0; i < json.Length; i++)
            {
                connetJson += json[i];

                if (i != json.Length - 1) connetJson += ",";
            }

            return connetJson;
        }

        // 코루틴 기다리는 함수.
        public static IEnumerator WaitAsync(params Action<Action>[] actions)
        {
            bool isExecuted = false;
            int i = 0;

            while (i < actions.Length || isExecuted)
            {
                if (!isExecuted)
                {
                    isExecuted = true;
                    actions[i++]?.Invoke(() => isExecuted = false);
                }

                yield return null;
            }
        }

        // 레벨 차이에 따른 값 변화 차이.
        public static float GetDebuffValueAccordingToLevelDifference(float value, int levelDifference, int endLevelDifference, params float[] debuffRatios)
        {
            float debuffRatio = 0;
            if (levelDifference < endLevelDifference)
            {
                for (int i = 1; i <= endLevelDifference; i++)
                {
                    if (debuffRatios.Length > i && levelDifference == i)
                        debuffRatio = debuffRatios[i - 1];
                }
            }
            else
                debuffRatio = debuffRatios[debuffRatios.Length - 1];

            float debuffValue = value - value * debuffRatio;

            return debuffValue;
        }

        // 스테이지 51일때 , "2-1"으로 변환하는 함수 만들기 
        public static string GetStageNameIntToString(float value)
        {
            double chapter = System.Math.Truncate(value / 50) + 1;
            float stage = value % 50;
            return chapter.ToString() + "-" + stage.ToString();
        }

        // 스테이지 (1~4,6~9 : normal), (5 : Middle), (10 : Final) 
        public static string GetStageTypeIntToString(int value)
        {
            string stageType = String.Empty;
            switch (value)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                case 5:
                case 6:
                case 7:
                case 8:
                    stageType = "Normal";
                    break;
                case 4:
                    stageType = "Middle";
                    break;
                case 9:
                    stageType = "Final";
                    break;
            }
            return stageType;
        }
    }

    public class Parameter
    {
        private object parameter;

        public Parameter Set<T>(T variable)
        {
            parameter = new Create<T>(variable);

            return this;
        }

        public object Get()
        {
            return parameter;
        }

        public class Create<T>
        {
            public T variable;

            public Create(T variable) => this.variable = variable;

            public T GetValue()
            {
                return variable;
            }
        }
    }

    public class Money
    {
        public enum Currency
        {
            Coin,
            Crystal,
            RealMoney,
        }

        public static string GetCurrencyName(Currency currency)
        {
            return currency.ToString();
        }
    }

    public class AnimationWeight
    {

        public enum Enemy
        {
            Default,
            HpReduce,
            Bound,
            HpExhausted
        }
    }

    public class ColorEtc
    {
        public static Color orange = new Color(1.0f, 0.717f, 0.0f);
    }
