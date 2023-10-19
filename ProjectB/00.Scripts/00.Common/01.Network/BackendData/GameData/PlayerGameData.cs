// Copyright 2013-2022 AFI, INC. All rights reserved.

using System;
using System.Collections.Generic;
using System.Globalization;
using BackEnd;
using LitJson;
using UnityEngine;

namespace BackendData.GameData {
    //===============================================================
    // UserData 테이블의 데이터를 담당하는 클래스(변수)
    //===============================================================
    public class StatData
    {
        public int ItemID;
        public int ItemLevel;
    }
    public partial class PlayerGameData
    {

        public long ClearStageLevel { get; private set; }

        public long NowStageLevel { get; private set; }
        public double DCoin { get; private set; } // gold gem
        public double DSkillCoin { get; private set; } // skill gem
        public long DRuby { get; private set; } // treasure gem
        public double DDiamondCoin { get; private set; } // diamond gem
        //public double DAbilCoin { get; private set; } // abil gem (스탯 특성)

        public PlayerType PlayerType { get; private set; }

        public float DArcherExp { get; private set; }

        public float DArcherHp { get; private set; }

        public int DArcherLevel { get; private set; }

        public float DWarriorExp { get; private set; }

        public float DWarriorHp { get; private set; }

        public int DWarriorLevel { get; private set; }

        public float DWizardExp { get; private set; }

        public float DWizardHp { get; private set; }

        public int DWizardLevel { get; private set; }


        public string LastLoginTime { get; private set; }

        public List<StatData> StatList = new List<StatData>();
    
        /* Player Active Skill */
        public Dictionary<string, double> DWarriorActiveSkillLevel { get; private set; }
        public Dictionary<string, double> DArcherActiveSkillLevel { get; private set; }
        public Dictionary<string, double> DWizardActiveSkillLevel { get; private set; }

        /* Player Passive Skill */
        public Dictionary<string, double> DWarriorPassiveSkillLevel { get; private set; }
        public Dictionary<string, double> DArcherPassiveSkillLevel { get; private set; }
        public Dictionary<string, double> DWizardPassiveSkillLevel { get; private set; }

        public int WarriorUpgradeLevel { get; private set; }
        public int ArcherUpgradeLevel { get; private set; }
        public int WizardUpgradeLevel { get; private set; }

        public string LastInitTime { get; private set; }

        public Dictionary<string, long> TDMaxClearLevelList { get; private set; }
        public Dictionary<string, double> TDKeyList { get; private set; }

    }

    //===============================================================
    // UserData 테이블의 데이터를 담당하는 클래스(함수)
    //===============================================================
    public partial class PlayerGameData : Base.GameData {
        
        // 데이터가 존재하지 않을 경우, 초기값 설정
        protected override void InitializeData() {
            DCoin = 0;
            DDiamondCoin = 0;
            //DAbilCoin = 0;
            DSkillCoin = 0;
            ClearStageLevel = 0;
            NowStageLevel = 1;

            DWarriorLevel = 1;
            DWarriorExp = 0;
            DWarriorHp = 0;

            DArcherLevel = 1;
            DArcherExp = 0;
            DArcherHp = 0;

            DWizardLevel = 1;
            DWizardExp = 0;
            DWizardHp = 0;

            StatList.Add(new StatData() { ItemID = 1, ItemLevel = 0 });

            DWarriorActiveSkillLevel = new Dictionary<string, double>();
            DArcherActiveSkillLevel = new Dictionary<string, double>();
            DWizardActiveSkillLevel = new Dictionary<string, double>();

            DWarriorPassiveSkillLevel = new Dictionary<string, double>();
            DArcherPassiveSkillLevel = new Dictionary<string, double>();
            DWizardPassiveSkillLevel = new Dictionary<string, double>();

            PlayerType = PlayerType.Warrior;

            WarriorUpgradeLevel = 1;
            ArcherUpgradeLevel = 1;
            WizardUpgradeLevel = 1;

            LastLoginTime = string.Format("{0:MM-DD:HH:mm:ss.fffZ}", DateTime.Now.ToString(CultureInfo.InvariantCulture));
            LastInitTime = DateTime.MinValue.ToString();
            TDMaxClearLevelList = new Dictionary<string, long>() { { "Warrior", 0 }, { "Archer", 0 }, { "Wizard", 0 } };
            TDKeyList = new Dictionary<string, double>() { { "Warrior", 0 }, { "Archer", 0 }, { "Wizard", 0 } };
        }

        // Backend.GameData.GetMyData 호출 이후 리턴된 값을 파싱하여 캐싱하는 함수
        // 서버에서 데이터를 불러오늖 함수는 BackendData.Base.GameData의 BackendGameDataLoad() 함수를 참고해주세요
        protected override void SetServerDataToLocal(JsonData gameDataJson) {
            ClearStageLevel = int.Parse(gameDataJson["ClearStageLevel"].ToString());
            NowStageLevel = int.Parse(gameDataJson["NowStageLevel"].ToString());
            DCoin = double.Parse(gameDataJson["DCoin"].ToString());
            DRuby = gameDataJson.ContainsKey("DRuby") ? long.Parse(gameDataJson["DRuby"].ToString()) : 0;
            DSkillCoin = gameDataJson.ContainsKey("DSkillCoin") ? double.Parse(gameDataJson["DSkillCoin"].ToString()) : 0;  

            LastLoginTime = gameDataJson.ContainsKey("LastLoginTime") ? gameDataJson["LastLoginTime"].ToString() : string.Format("{0:MM-DD:HH:mm:ss.fffZ}", DateTime.Now.ToString(CultureInfo.InvariantCulture));
            LastInitTime = gameDataJson.ContainsKey("LastInitTime") ? gameDataJson["LastInitTime"].ToString() : DateTime.MinValue.ToString();

            DArcherExp = float.Parse(gameDataJson["DArcherExp"].ToString());
            DArcherHp = float.Parse(gameDataJson["DArcherHp"].ToString());
            DArcherLevel = int.Parse(gameDataJson["DArcherLevel"].ToString());

            DWarriorExp = float.Parse(gameDataJson["DWarriorExp"].ToString());
            DWarriorHp = float.Parse(gameDataJson["DWarriorHp"].ToString());
            DWarriorLevel = int.Parse(gameDataJson["DWarriorLevel"].ToString());

            DWizardExp = float.Parse(gameDataJson["DWizardExp"].ToString());
            DWizardHp = float.Parse(gameDataJson["DWizardHp"].ToString());  
            DWizardLevel = int.Parse(gameDataJson["DWizardLevel"].ToString());


            if (gameDataJson.ContainsKey("DDiamondCoin") == true)
                DDiamondCoin = double.Parse(gameDataJson["DDiamondCoin"].ToString());
            else
                DDiamondCoin = 0;

            //if (gameDataJson.ContainsKey("DAbilCoin") == true)
            //    DAbilCoin = double.Parse(gameDataJson["DAbilCoin"].ToString());
            //else
            //    DAbilCoin = 0;

            if (gameDataJson.ContainsKey("PlayerType") == true)
                PlayerType = (PlayerType)(Enum.Parse(typeof(PlayerType), gameDataJson["PlayerType"].ToString()));
            else
                PlayerType = PlayerType.Warrior;

            /* Warrior gold stat */
            if (gameDataJson.ContainsKey("StatList") == true)
                StatList = LitJson.JsonMapper.ToObject<List<StatData>>(gameDataJson["StatList"].ToJson());
            else
                StatList.Add(new StatData() { ItemID = 1, ItemLevel = 0 });    

            /* Warrior Active Skill */
            if (gameDataJson.ContainsKey("DWarriorActiveSkillLevel") == true)
            {
                DWarriorActiveSkillLevel = new Dictionary<string, double>();
                foreach (var column in gameDataJson["DWarriorActiveSkillLevel"].Keys)
                {
                    DWarriorActiveSkillLevel.Add(column, double.Parse(gameDataJson["DWarriorActiveSkillLevel"][column].ToString()));
                }
            }
            else
            {
                DWarriorActiveSkillLevel = new Dictionary<string, double>() { { "SkillLevel_0", 0 }, { "SkillLevel_1", 0 }, { "SkillLevel_2", 0 }, { "SkillLevel_3", 0 } };
            }
            //if (gameDataJson.ContainsKey("DWarriorActiveSkillLevel") == true)
            //{
            //    DWarriorActiveSkillLevel = new List<Dictionary<string, double>>();   
            //    for (int i = 0; i < gameDataJson["DWarriorActiveSkillLevel"].Count; i++)
            //    {
            //        Dictionary<string, double> temp = new Dictionary<string, double>();
            //        foreach (var column in gameDataJson["DWarriorActiveSkillLevel"][i].Keys)
            //        {
            //            temp.Add(column, double.Parse(gameDataJson["DWarriorActiveSkillLevel"][i][column].ToString()));
            //        }
            //        DWarriorActiveSkillLevel.Add(temp);
            //    }  
            //}
            //else
            //{
            //    DWarriorActiveSkillLevel = new List<Dictionary<string, double>>();
            //    for(int i = 0; i < 4; i++)
            //    {
            //        DWarriorActiveSkillLevel.Add(new Dictionary<string, double>() { { "SkillNum", i }, { "SkillLevel", 0 }, { "DamageInfo", 0 }, { "SkillNeedCount", 1 } });
            //    }
            //}
            /* Archer Active Skill */
            if (gameDataJson.ContainsKey("DArcherActiveSkillLevel") == true)
            {
                DArcherActiveSkillLevel = new Dictionary<string, double>();
                foreach (var column in gameDataJson["DArcherActiveSkillLevel"].Keys)
                {
                    DArcherActiveSkillLevel.Add(column, double.Parse(gameDataJson["DArcherActiveSkillLevel"][column].ToString()));
                }
            }
            else
            {
                DArcherActiveSkillLevel = new Dictionary<string, double>() { { "SkillLevel_0", 0 }, { "SkillLevel_1", 0 }, { "SkillLevel_2", 0 }, { "SkillLevel_3", 0 } };
            }

            /* Wizard Active Skill */
            if (gameDataJson.ContainsKey("DWizardActiveSkillLevel") == true)
            {
                DWizardActiveSkillLevel = new Dictionary<string, double>();
                foreach (var column in gameDataJson["DWizardActiveSkillLevel"].Keys)
                {
                    DWizardActiveSkillLevel.Add(column, double.Parse(gameDataJson["DWizardActiveSkillLevel"][column].ToString()));
                }
            }
            else
            {
                DWizardActiveSkillLevel = new Dictionary<string, double>() { { "SkillLevel_0", 0 }, { "SkillLevel_1", 0 }, { "SkillLevel_2", 0 }, { "SkillLevel_3", 0 } };
            }

            /* Warrior Passive Skill */
            if (gameDataJson.ContainsKey("DWarriorPassiveSkillLevel") == true)
            {
                DWarriorPassiveSkillLevel = new Dictionary<string, double>();
                foreach (var column in gameDataJson["DWarriorPassiveSkillLevel"].Keys)
                {
                    DWarriorPassiveSkillLevel.Add(column, double.Parse(gameDataJson["DWarriorPassiveSkillLevel"][column].ToString()));
                }
            }
            else
            {
                DWarriorPassiveSkillLevel = new Dictionary<string, double>() { { "SkillLevel_0", 0 }, { "SkillLevel_1", 0 }, { "SkillLevel_2", 0 }, { "SkillLevel_3", 0 } };
            }

            /* Archer Passive Skill */
            if (gameDataJson.ContainsKey("DArcherPassiveSkillLevel") == true)
            {
                DArcherPassiveSkillLevel = new Dictionary<string, double>();
                foreach (var column in gameDataJson["DArcherPassiveSkillLevel"].Keys)
                {
                    DArcherPassiveSkillLevel.Add(column, double.Parse(gameDataJson["DArcherPassiveSkillLevel"][column].ToString()));
                }
            }
            else
            {
                DArcherPassiveSkillLevel = new Dictionary<string, double>() { { "SkillLevel_0", 0 }, { "SkillLevel_1", 0 }, { "SkillLevel_2", 0 }, { "SkillLevel_3", 0 } };
            }
            /* Wizard Passive Skill */
            if (gameDataJson.ContainsKey("DWizardPassiveSkillLevel") == true)
            {
                DWizardPassiveSkillLevel = new Dictionary<string, double>();
                foreach (var column in gameDataJson["DWizardPassiveSkillLevel"].Keys)
                {
                    DWizardPassiveSkillLevel.Add(column, double.Parse(gameDataJson["DWizardPassiveSkillLevel"][column].ToString()));
                }
            }
            else
            {
                DWizardPassiveSkillLevel = new Dictionary<string, double>() { { "SkillLevel_0", 0 }, { "SkillLevel_1", 0 }, { "SkillLevel_2", 0 }, { "SkillLevel_3", 0 } };
            }

            if (gameDataJson.ContainsKey("WarriorUpgradeLevel") == true)
                WarriorUpgradeLevel = int.Parse(gameDataJson["WarriorUpgradeLevel"].ToString());
            else
                WarriorUpgradeLevel = 1;

            if (gameDataJson.ContainsKey("ArcherUpgradeLevel") == true)
                ArcherUpgradeLevel = int.Parse(gameDataJson["ArcherUpgradeLevel"].ToString());
            else
                ArcherUpgradeLevel = 1;

            if (gameDataJson.ContainsKey("WizardUpgradeLevel") == true)
                WizardUpgradeLevel = int.Parse(gameDataJson["WizardUpgradeLevel"].ToString());
            else
                WizardUpgradeLevel = 1;

            if (gameDataJson.ContainsKey("TDMaxClearLevelList") == true)
            {
                TDMaxClearLevelList = new Dictionary<string, long>();
                foreach (var column in gameDataJson["TDMaxClearLevelList"].Keys)
                {
                    TDMaxClearLevelList.Add(column, long.Parse(gameDataJson["TDMaxClearLevelList"][column].ToString()));
                }
            }
            else
            {
                TDMaxClearLevelList = new Dictionary<string, long>() { { "Warrior", 0 }, { "Archer", 0 }, { "Wizard", 0 } };
            }

            if (gameDataJson.ContainsKey("TDKeyList") == true)
            {
                TDKeyList = new Dictionary<string, double>();
                foreach (var column in gameDataJson["TDKeyList"].Keys)
                {
                    TDKeyList.Add(column, long.Parse(gameDataJson["TDKeyList"][column].ToString()));
                }
            }
            else
            {
                TDKeyList = new Dictionary<string, double>() { { "Warrior", 0 }, { "Archer", 0 }, { "Wizard", 0 } };
            }

        }

        // 테이블 이름 설정 함수
        public override string GetTableName() {
            return "PlayerGameData";
        }

        // 컬럼 이름 설정 함수
        public override string GetColumnName() {
            return null;
        }

        // 데이터 저장 시 저장할 데이터를 뒤끝에 맞게 파싱하는 함수
        public override Param GetParam() {
            Param param = new Param();

            param.Add("ClearStageLevel", ClearStageLevel);
            param.Add("NowStageLevel", NowStageLevel);
            param.Add("DCoin", DCoin);
            param.Add("DSkillCoin", DSkillCoin);
            param.Add("DRuby", DRuby);
            param.Add("DDiamondCoin", DDiamondCoin);
            //param.Add("DAbilCoin", DAbilCoin);
            param.Add("LastLoginTime", string.Format("{0:MM-DD:HH:mm:ss.fffZ}", DateTime.Now.ToString(CultureInfo.InvariantCulture)));
            param.Add("LastInitTime", LastInitTime);  

            param.Add("DArcherExp", DArcherExp);
            param.Add("DArcherHp", DArcherHp);
            param.Add("DArcherLevel", DArcherLevel);

            param.Add("DWarriorExp", DWarriorExp);
            param.Add("DWarriorHp", DWarriorHp);
            param.Add("DWarriorLevel", DWarriorLevel);

            param.Add("DWizardExp", DWizardExp);
            param.Add("DWizardHp", DWizardHp);
            param.Add("DWizardLevel", DWizardLevel);

            param.Add("StatList", StatList);

            param.Add("DWarriorActiveSkillLevel", DWarriorActiveSkillLevel);
            param.Add("DArcherActiveSkillLevel", DArcherActiveSkillLevel);
            param.Add("DWizardActiveSkillLevel", DWizardActiveSkillLevel);

            param.Add("DWarriorPassiveSkillLevel", DWarriorPassiveSkillLevel);
            param.Add("DArcherPassiveSkillLevel", DArcherPassiveSkillLevel);
            param.Add("DWizardPassiveSkillLevel", DWizardPassiveSkillLevel);

            param.Add("PlayerType", PlayerType);

            param.Add("WarriorUpgradeLevel", WarriorUpgradeLevel);
            param.Add("ArcherUpgradeLevel", ArcherUpgradeLevel);
            param.Add("WizardUpgradeLevel", WizardUpgradeLevel);

            param.Add("TDMaxClearLevelList", TDMaxClearLevelList);
            param.Add("TDKeyList", TDKeyList);
            return param;
        }
        
        // 적 처치 횟수를 갱신하는 함수
        public void CountDefeatEnemy() {
        }

        // 유저의 정보를 변경하는 함수
        public void UpdateUserData(int type, double money) {
            IsChangedData = true;
            DCoin += money;
           
        }
        public void UpdateUserData_Exp(int type, float exp)
        {
            IsChangedData = true;

            switch (type)
            {
                case (int)PlayerType.Warrior:
                    DWarriorExp = exp;
                    break;
                case (int)PlayerType.Archer:
                    DArcherExp = exp;
                    break;
                case (int)PlayerType.Wizard:
                    DWizardExp = exp;
                    break;
            }
        }
        public void UpdateUserData_Hp(int type, float hp)
        {
            IsChangedData = true;
            switch (type)
            {
                case (int)PlayerType.Warrior:
                    DWarriorHp = hp;
                    break;
                case (int)PlayerType.Archer:
                    DArcherHp = hp;
                    break;
                case (int)PlayerType.Wizard:
                    DWizardHp = hp;
                    break;
            }
        }
        public void UpdateUserData_GoldStat(int statId, double money)
        {
            IsChangedData = true;
            DCoin += money;

            BackendData.GameData.StatData statData = null;
            statData = StatList.Find(item => item.ItemID == statId);
            if (statData == null)
            {
                StatList.Add(new StatData() { ItemID = statId, ItemLevel = 1 });
            }
            else
            {
                statData.ItemLevel += 1;
            }
        }

        public double GetStatRatio(Define.StatType statItemType)
        {
            int statId = -1;
            double increasedAmount = -1;
            Dictionary<int, BackendData.Chart.PlayerEnhancemetData.Item> EnHancementChartData = null;
            EnHancementChartData = StaticManager.Backend.Chart.PlayerEnhancemetData.GetPlayerGoldStatItem();

            foreach (var chartItem in EnHancementChartData)
            {
                if (chartItem.Value.StatName == statItemType.ToString())
                {
                    statId = chartItem.Value.ItemID;
                    increasedAmount = chartItem.Value.IncreasedAmount;
                }
            }
            if (statId == -1 || increasedAmount == -1)
                return 0;

            BackendData.GameData.StatData statData = null;
            statData = StatList.Find(item => item.ItemID == statId);
            if (statData == null)
                return 0;

            double statRatio = statData.ItemLevel * increasedAmount;
            return statRatio;
        }

        public void UpdateUserData_Skill(int playerType, double skillMoney, int itemType, int itemNum)
        {
            IsChangedData = true;
            DSkillCoin += skillMoney;

            if (Enum.GetName(typeof(SkillItemType), itemType) == "None")
                return;

            double outValue1;
            double outValue2;
            double outValue3;
            double outValue4;
            double outValue5;

            if (DWarriorActiveSkillLevel.TryGetValue(Enum.GetName(typeof(SkillTypeNum), itemNum), out outValue1) == false)
            {
                DWarriorActiveSkillLevel = new Dictionary<string, double>() { { "SkillLevel_0", 0 }, { "SkillLevel_1", 0 }, { "SkillLevel_2", 0 }, { "SkillLevel_3", 0 } };
            }
            if (DArcherActiveSkillLevel.TryGetValue(Enum.GetName(typeof(SkillTypeNum), itemNum), out outValue1) == false)
            {
                DArcherActiveSkillLevel = new Dictionary<string, double>() { { "SkillLevel_0", 0 }, { "SkillLevel_1", 0 }, { "SkillLevel_2", 0 }, { "SkillLevel_3", 0 } };
            }
            if (DWizardActiveSkillLevel.TryGetValue(Enum.GetName(typeof(SkillTypeNum), itemNum), out outValue2) == false)
            {
                DWizardActiveSkillLevel = new Dictionary<string, double>() { { "SkillLevel_0", 0 }, { "SkillLevel_1", 0 }, { "SkillLevel_2", 0 }, { "SkillLevel_3", 0 } };
            }
            if (DWarriorPassiveSkillLevel.TryGetValue(Enum.GetName(typeof(SkillTypeNum), itemNum), out outValue3) == false)
            {
                DWarriorPassiveSkillLevel = new Dictionary<string, double>() { { "SkillLevel_0", 0 }, { "SkillLevel_1", 0 }, { "SkillLevel_2", 0 }, { "SkillLevel_3", 0 } };
            }
            if (DArcherPassiveSkillLevel.TryGetValue(Enum.GetName(typeof(SkillTypeNum), itemNum), out outValue4) == false)
            {
                DArcherPassiveSkillLevel = new Dictionary<string, double>() { { "SkillLevel_0", 0 }, { "SkillLevel_1", 0 }, { "SkillLevel_2", 0 }, { "SkillLevel_3", 0 } };
            }
            if (DWizardPassiveSkillLevel.TryGetValue(Enum.GetName(typeof(SkillTypeNum), itemNum), out outValue5) == false)
            {
                DWizardPassiveSkillLevel = new Dictionary<string, double>() { { "SkillLevel_0", 0 }, { "SkillLevel_1", 0 }, { "SkillLevel_2", 0 }, { "SkillLevel_3", 0 } };
            }

            switch (playerType)
            {
                case (int)PlayerType.Warrior:
                    if(itemType == (int)SkillItemType.Active)
                    {
                        Update_Skill(DWarriorActiveSkillLevel, itemNum);
                    }
                    else
                    {
                        Update_Skill(DWarriorPassiveSkillLevel, itemNum);
                    }
                    break;
                case (int)PlayerType.Archer:
                    if (itemType == (int)SkillItemType.Active)
                    {
                        Update_Skill(DArcherActiveSkillLevel, itemNum);
                    }
                    else
                    {
                        Update_Skill(DArcherPassiveSkillLevel, itemNum);
                    }
                    break;
                case (int)PlayerType.Wizard:
                    if (itemType == (int)SkillItemType.Active)
                    {
                        Update_Skill(DWizardActiveSkillLevel, itemNum);
                    }
                    else
                    {
                        Update_Skill(DWizardPassiveSkillLevel, itemNum);
                    }
                    break;
                case (int)PlayerType.None:
                    break;
            }
        }
        public void Update_Skill(Dictionary<string, double> PlayerSkillLevel, int itemNum)
        {
            if (Enum.GetName(typeof(SkillTypeNum), itemNum) == "None" )
                return;

            PlayerSkillLevel[Enum.GetName(typeof(SkillTypeNum), itemNum)] += 1;  

        }

        public void UpdateStageLevel()
        {
            IsChangedData = true;

            NowStageLevel += 1;
            ClearStageLevel += 1;
        }

        public void UpdateNowStageLevel(long nowStageLevel)
        {
            IsChangedData = true;
            NowStageLevel = nowStageLevel;
        }

        public void UpdateStageClearLevel()
        {
            IsChangedData = true;
            ClearStageLevel += 1;
        }

        public void ChangeNowPlayerType(PlayerType playerType)
        {
            IsChangedData = true;
            PlayerType = playerType;
        }

        public void UpdatePlayerLevel(PlayerType playerType)
        {
            IsChangedData = true;
            switch (playerType)
            {
                case PlayerType.Warrior:
                    DWarriorLevel += 1;
                    break;
                case PlayerType.Archer:
                    DArcherLevel += 1;
                    break;
                case PlayerType.Wizard:
                    DWizardLevel += 1;
                    break;
            }
        }
        // 레벨업하는 함수
        public void UpdatePlayerUpgradeLevel(PlayerType playerType, int level)
        {
            IsChangedData = true;
            switch (playerType)
            {
                case PlayerType.Warrior:
                    WarriorUpgradeLevel = level;
                    break;
                case PlayerType.Archer:
                    ArcherUpgradeLevel = level;
                    break;
                case PlayerType.Wizard:
                    WizardUpgradeLevel = level;
                    break;
            }
        }
        public double GetPlayerUpgradeRatio(int playerType, Define.StatType statItemType)
        {
            BackendData.Chart.Upgrade.Item item = null;
            switch (playerType)
            {
                case (int)PlayerType.Warrior:
                    item = StaticManager.Backend.Chart.Upgrade.GetUpgradeItem(global::PlayerType.Warrior, WarriorUpgradeLevel);
                    break;
                case (int)PlayerType.Archer:
                    item = StaticManager.Backend.Chart.Upgrade.GetUpgradeItem(global::PlayerType.Archer, ArcherUpgradeLevel);
                    break;
                case (int)PlayerType.Wizard:
                    item = StaticManager.Backend.Chart.Upgrade.GetUpgradeItem(global::PlayerType.Wizard, WizardUpgradeLevel);
                    break;
            }

            if (item == null)
                return 0;

            double finalRatio = 0;
            for (int i = 0; i < item.UpgradeStats.Count; ++i)
            {
                if (item.UpgradeStats[i].StatType == statItemType)
                    finalRatio = item.UpgradeStats[i].AbilValue;
            }

            return finalRatio;

        }
       
        private void LevelUp() {
            //Exp가 MaxExp를 초과했을 경우를 대비하여 빼기
         
            //기존 경험치에서 1.1배
           // MaxExp = (float)Math.Truncate(MaxExp * 1.1);
           //
           // Level++;
        }
        public void UpdateUserData_DDiamondCoin(double addCoin)
        {
            IsChangedData = true;
            DDiamondCoin += addCoin;
        }
        //public void UpdateUserData_DAbilCoin(double addCoin)
        //{
        //    IsChangedData = true;
        //    DAbilCoin += addCoin;
        //}
        public void UpdateLastInitTime(string time)
        {
            IsChangedData = true;
            LastInitTime = time;
        }

        public void UpdateTDMaxClearLevel(PlayerType type, long level)
        {
            switch (type)
            {
                case PlayerType.Warrior:
                    TDMaxClearLevelList["Warrior"] = level;
                    break;
                case PlayerType.Archer:
                    TDMaxClearLevelList["Archer"] = level;
                    break;
                case PlayerType.Wizard:
                    TDMaxClearLevelList["Wizard"] = level;
                    break;
            }  
        }
        public void UpdateTDKey(BackendData.Chart.DungeonData.RewardItemType type, double count)
        {
            switch (type)
            {
                case BackendData.Chart.DungeonData.RewardItemType.Warrior:
                    TDKeyList["Warrior"] -= count;
                    if (TDKeyList["Warrior"] <= 0)
                        TDKeyList["Warrior"] = 0;
                    break;
                case BackendData.Chart.DungeonData.RewardItemType.Archer:
                    TDKeyList["Archer"] -= count;
                    if (TDKeyList["Archer"] <= 0)
                        TDKeyList["Archer"] = 0;
                    break;
                case BackendData.Chart.DungeonData.RewardItemType.Wizard:
                    TDKeyList["Wizard"] -= count;
                    if (TDKeyList["Wizard"] <= 0)
                        TDKeyList["Wizard"] = 0;
                    break;
            }
        }
    }
}