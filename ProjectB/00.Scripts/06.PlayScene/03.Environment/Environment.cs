using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment : MonoBehaviour
{
    public List<StageData> normalStageList;
    public List<StageData> middleStageList;
    public List<StageData> bossStageList;
    public List<StageData> trainingDungeonList;  

    private List<GameObject> normalStageObjList = new List<GameObject>();
    private List<GameObject> middleStageObjList = new List<GameObject>();
    private List<GameObject> bossStageObjList = new List<GameObject>();
    private List<GameObject> trainingDungeonObjList = new List<GameObject>();

    public GameObject environmentObject { get; private set; }

    public void SpawnAllStageObject()
    {
        for(int i= 0; i < normalStageList.Count; i++)
        {
            GameObject stage = Instantiate(normalStageList[i].environmentPrefab, transform);
            normalStageObjList.Add(stage);
        }

        for (int i = 0; i < normalStageList.Count; i++)
        {
            GameObject stage = Instantiate(middleStageList[i].environmentPrefab, transform);
            middleStageObjList.Add(stage);
        }

        for (int i = 0; i < normalStageList.Count; i++)
        {
            GameObject stage = Instantiate(bossStageList[i].environmentPrefab, transform);
            bossStageObjList.Add(stage);
        }
        for (int i = 0; i < trainingDungeonList.Count; i++)
        {
            GameObject stage = Instantiate(trainingDungeonList[i].environmentPrefab, transform);
            trainingDungeonObjList.Add(stage);
        }
    }

    private void AllOffStage()
    {
        for (int i = 0; i < normalStageObjList.Count; i++)
        {
            normalStageObjList[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < middleStageObjList.Count; i++)
        {
            middleStageObjList[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < bossStageObjList.Count; i++)
        {
            bossStageObjList[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < trainingDungeonObjList.Count; i++)
        {
            trainingDungeonObjList[i].gameObject.SetActive(false);
        }
    }

    private void SetLightSetting(StageData stageData)
    {
        StageData.SceneLightingSetting lightingSetting = stageData.lightingSetting;

        RenderSettings.skybox = lightingSetting.skyBox;

        switch (lightingSetting.environmentLighting.source)
        {
            case StageData.SceneLightingSetting.EnvironmentLighting.Source.SkyBox:
                RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Skybox;
                RenderSettings.ambientIntensity = lightingSetting.environmentLighting.intensityMultiplie;
                break;
            case StageData.SceneLightingSetting.EnvironmentLighting.Source.Color:
                RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
                RenderSettings.ambientLight = lightingSetting.environmentLighting.ambientColor;
                break;
        }

        RenderSettings.fog = lightingSetting.isFog;
        if (lightingSetting.isFog)
        {
            RenderSettings.fogColor = lightingSetting.fogColor;
            RenderSettings.fogMode = lightingSetting.fogMode;
            RenderSettings.fogDensity = lightingSetting.fogDensity;
        }
    }

    public StageData SetStage(long stageNum)
    {
        AllOffStage();
        StageData stageData = null;

        if (stageNum % 10 == 0)
        {
            long index = SceneSettingManager.instance.GetNowBossSectorNum() - 1;
            bossStageObjList[(int)index].gameObject.SetActive(true);
            SetLightSetting(bossStageList[(int)index]);
            stageData = bossStageList[(int)index];
        }
        else if (stageNum % 5 == 0)
        {
            long index = SceneSettingManager.instance.GetNowMiddleSectorNum() - 1;
            middleStageObjList[(int)index].gameObject.SetActive(true);
            SetLightSetting(middleStageList[(int)index]);
            stageData = middleStageList[(int)index];
        }
        else
        {         
            long index = SceneSettingManager.instance.GetNowStageSectorNum() - 1;
            normalStageObjList[(int)index].gameObject.SetActive(true);
            SetLightSetting(normalStageList[(int)index]);
            stageData = normalStageList[(int)index];
        }

        return stageData;
    }
    public StageData SetDungeon(int traininDuneonNum)
    {
        AllOffStage();
        StageData traingDungeonData = null;
        trainingDungeonObjList[traininDuneonNum].gameObject.SetActive(true);
        //SetLightSetting(normalStageList[(int)index]);
        traingDungeonData = trainingDungeonList[traininDuneonNum];  

        return traingDungeonData;
    }
    public void Init(StageData stageData)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        environmentObject = Instantiate(stageData.environmentPrefab, transform);

        StageData.SceneLightingSetting lightingSetting = stageData.lightingSetting;

        RenderSettings.skybox = lightingSetting.skyBox;

        switch (lightingSetting.environmentLighting.source)
        {
            case StageData.SceneLightingSetting.EnvironmentLighting.Source.SkyBox:
                RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Skybox;
                RenderSettings.ambientIntensity = lightingSetting.environmentLighting.intensityMultiplie;
                break;
            case StageData.SceneLightingSetting.EnvironmentLighting.Source.Color:
                RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
                RenderSettings.ambientLight = lightingSetting.environmentLighting.ambientColor;
                break;
        }

        RenderSettings.fog = lightingSetting.isFog;
        if(lightingSetting.isFog)
        {
            RenderSettings.fogColor = lightingSetting.fogColor;
            RenderSettings.fogMode = lightingSetting.fogMode;
            RenderSettings.fogDensity = lightingSetting.fogDensity;
        }
    }
}
