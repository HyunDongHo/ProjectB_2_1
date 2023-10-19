// Copyright 2013-2022 AFI, INC. All rights reserved.

using System;
using System.Collections.Generic;
using System.Reflection;
using LitJson;

namespace BackendData.Chart.Mission {
    public enum MissionType
    {
        Daily,
        Repeat,
        DailyTotal,
        None,
    }

    public enum RewardItemType
    {
        Item,
        Weapon
    }
    public enum MissionContentType
    {
        MonsterDefeat,  // 몬스터 처치
        SpawnInven,     //  장비 소환
        SpawnPartner,   //  용병 소환
        EnterDungeon,   //  던전 입장
        GamePlayTime,   //  게임 플레이 시간
        AchiveInven,    //  장비 강화
        AchiveSkill,    //  스킬 강화
        AchivePartner,  //	용병 강화
        None,           // Daily Total Content
    }
    //===============================================================
    // Quest 차트의 각 row 데이터 클래스
    //===============================================================
    public class Item {

        public class RewardItemClass {

            public RewardItemType RewardItemType { get; private set; }
            public int Id { get; private set; }
            public float Count { get; private set; }

            public RewardItemClass(string type, int id, float count) {
                if (!Enum.TryParse<RewardItemType>(type, out var rewardItemType)) {
                    throw new Exception("지정되지 않은 RewardItemType 입니다.");
                }

                RewardItemType = rewardItemType;
                Id = id;
                Count = count;
            }
        }

        public int MissionID { get; private set; }
        public string MissionContent { get; private set; }
        public float RequestCount { get; private set; }
        public MissionType MissionType { get; private set; }
        public List<RewardItemClass> RewardItem { get; private set; }

        public MissionContentType MissionContentType { get; private set; }

        public Item(JsonData json) {
            MissionID = int.Parse(json["MissionID"].ToString());
            MissionContent = json["MissionContent"].ToString();
            RequestCount = float.Parse(json["RequestCount"].ToString());

            if (!Enum.TryParse<MissionType>(json["MissionType"].ToString(), out var missionType)) {
                throw new Exception($"Q{MissionID} - 지정되지 않은 MissionType 입니다.");
            }

            this.MissionType = missionType;

            if (!Enum.TryParse<MissionContentType>(json["MissionContentType"].ToString(), out var missionContentType))
            {
                throw new Exception($"Q{MissionID} - 지정되지 않은 MissionContentType 입니다.");
            }

            this.MissionContentType = missionContentType;

            try {
                string rewardItemString = json["RewardItem"].ToString();

                if (string.IsNullOrEmpty(rewardItemString) == false) {
                    RewardItem = new List<RewardItemClass>();
                    JsonData rewardStatJson = JsonMapper.ToObject(rewardItemString);

                    foreach (JsonData tempJson in rewardStatJson) {
                        string type = tempJson["Type"].ToString();
                        int id = int.Parse(tempJson["Id"].ToString());
                        float count = float.Parse(tempJson["Count"].ToString());

                        RewardItem.Add(new RewardItemClass(type, id, count));
                    }
                }
            }
            catch (Exception e) {
                throw new Exception($"Q{MissionID} - RewardItem 파싱 도중 에러가 발생했습니다.\n{e.StackTrace}");
            }
          
        }
    }
}
