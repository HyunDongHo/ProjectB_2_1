using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public enum StageType
//{
//    Boss,
//    InSide,
//    OutSide,
//}

public enum StageTypeNum
{
    Normal,
    Middle,
    Final,
    TrainingDungeon,
}

[System.Serializable]
public class StageEnemyData
{
    public GameObject enemyPrefab;
    public int createCount = 0;

    public StageEnemyData(GameObject enemyPrefab, int createCount)
    {
        this.enemyPrefab = enemyPrefab;
        this.createCount = createCount;
    }
}

[CreateAssetMenu(fileName = "New Stage Data", menuName = "Custom/Stage Data")]
public class StageData : ScriptableObject
{
    public StageTypeNum stageTypeNum;

    [Header("Stage Visual Set")]

    public GameObject environmentPrefab;

    [System.Serializable]
    public class SceneLightingSetting
    {
        public Material skyBox;

        [System.Serializable]
        public class EnvironmentLighting
        {
            public enum Source
            {
                SkyBox,
                Color,
            }
            public Source source = Source.SkyBox;

            [ConditionalHide("source", Source.SkyBox, hideInInspector = true)] public float intensityMultiplie = 1.0f;
            [ConditionalHide("source", Source.Color, hideInInspector = true)] public Color ambientColor;
        }
        public EnvironmentLighting environmentLighting;

        public bool isFog;
        [ConditionalHide("isFog", true, hideInInspector = true)] public FogMode fogMode = FogMode.ExponentialSquared;
        [ConditionalHide("isFog", true, hideInInspector = true)] public Color fogColor = Color.black;
        [ConditionalHide("isFog", true, hideInInspector = true)] public float fogDensity = 0.01f;
    }

    public SceneLightingSetting lightingSetting;  

    [Header("Stage Sound Set")]

    public SoundData soundData;

    [Header("Stage Enemy Set")]

    public List<StageEnemyData> stageEnemyDatas;
    public List<int> stageEnemyDatasTotalMonsterCnt;
    public List<StageEnemyData> stageMiddleBossDatas;
    public List<StageEnemyData> stageFinalBossDatas;

    public float stageAdjustmentRatio = 1;
}