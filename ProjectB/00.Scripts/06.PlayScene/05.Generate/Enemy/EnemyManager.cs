using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RNG;
using System.Threading;

public enum EnemyType
{
    Normal,
    Boss,
}

public class EnemyManager : MonoBehaviour
{
    private StageData stageData;
    [SerializeField]private StageTypeNum stageType;
    [SerializeField] private PlayerType dungeonPlayerType;

    public Action<EnemyControl> OnAnnounceEnemyDie;
    public Action<EnemyControl> OnAnnounceEnemySpawn;
    public Action OnSpawnAllEnd;
    public Action OnRemovedAllEnemy;

    private List<StageEnemyData> currentStageEnemyDatas = new List<StageEnemyData>();
    private List<StageEnemyData> previousStageEnemyDatas = new List<StageEnemyData>();
    private List<StageEnemyData> StageMiddleBossDatas = new List<StageEnemyData>();
    private List<StageEnemyData> StageFinalBossDatas = new List<StageEnemyData>();
    private List<int> StageEnemyTotalMonsterCnt = new List<int>();


    //private Dictionary<GameObject, Vector3> createdEnemyDatas = new Dictionary<GameObject, Vector3>();
    //private Dictionary<GameObject, Vector3> createdMiddleBossDatas = new Dictionary<GameObject, Vector3>();
    //private Dictionary<GameObject, Vector3> createdFinalBossDatas = new Dictionary<GameObject, Vector3>();
    private Dictionary<EnemyControl, Vector3> createdEnemyDatas = new Dictionary<EnemyControl, Vector3>();
    private Dictionary<EnemyControl, Vector3> createdMiddleBossDatas = new Dictionary<EnemyControl, Vector3>();
    private Dictionary<EnemyControl, Vector3> createdFinalBossDatas = new Dictionary<EnemyControl, Vector3>();

    public EnemyAvailableCreate enemyAvailableCreate;
    public EnemyPositionCreate enemyPositionCreate;
    public EnemyRotationCreate enemyRotationCreate;
    public EnemyPositionCreate_Random enemyPositionCreate_random;
       
    [Space]

    public bool isStageRepeat = false;
    public bool isStageEnd = false;
    public bool isStageStart = false;
    public bool isEnemyAllDeadBeforeMiddleBoss = false;
    public bool isCheckingMiddleBoss = false;

    [Space]

    public int minCreateEnemyCount = 1;
    private int currentCreateEnemyCount = 0;

    [Space]

    public float creationUpdateTime; // 업데이트 시간
    public float creationTime ;       // 생성 주기  

    [Space]

    public long totalMonsterCount;
    public long dieMonsterCount = 0;
    //public void Start()
    //{
    //    minCreateEnemyCount = 8;
    //    creationUpdateTime = 2.5f;
    //    creationTime = 2;
    //    totalMonsterCount = 20;
    //}

    private BossHpUI _bossHpUI;
    private TDBossHpUI _tdBossHpUI;
    private float stageTime = 30;
    private float nowRemainTime = 30;  
    private bool isStartTimer = false;

    public int chartTotalEnemyCnt ;

    [SerializeField] int bossPosIndex = 0;
    public void Init(StageData stageData)
    {
        if(stageData.stageTypeNum == StageTypeNum.TrainingDungeon)
        {
            previousStageEnemyDatas.Clear();
            currentStageEnemyDatas.Clear();
            StageMiddleBossDatas.Clear();
            StageFinalBossDatas.Clear();
            RemoveAllMonster();

            StageManager.instance.canvasManager.GetUIManager<BossHpUI>().SetActiveParent(false);
            StageManager.instance.canvasManager.GetUIManager<TDBossHpUI>().SetActiveParent(false);
            isStageStart = false;
            this.stageData = stageData;
            this.stageType = stageData.stageTypeNum;  
            Debug.Log("enemy manager 던전");

            SetStageMiddleBossDatas();  
            //isEnemyAllDeadBeforeMiddleBoss = true;
            isCheckingMiddleBoss = false;
            isSpawnedBoss = false;
            isStageRepeat = false;  
            dieMonsterCount = 0;
            currentCreateEnemyCount = 0;
            totalMonsterCount = 0;
            chartTotalEnemyCnt = 0;  

            bossPosIndex = 0;
            CheckAndCreateTDEnemy(stageData);
            //StartCoroutine(UpdateCreateTDEnemy(stageData));  
            //CheckAndCreateMiddleBoss();  

            stageTime = 30;
            nowRemainTime = stageTime;
            isStartTimer = true;
            isStageEnd = false;
        }
        else
        {
            previousStageEnemyDatas.Clear();
            currentStageEnemyDatas.Clear();
            StageMiddleBossDatas.Clear();
            StageFinalBossDatas.Clear();
            RemoveAllMonster();

            dungeonPlayerType = PlayerType.None;
            StageManager.instance.canvasManager.GetUIManager<BossHpUI>().SetActiveParent(false);
            StageManager.instance.canvasManager.GetUIManager<TDBossHpUI>().SetActiveParent(false);
            isStageStart = false;

            StopCoroutine(UpdateCreateEnemy());
            this.stageData = stageData;
            this.stageType = stageData.stageTypeNum;
            SetStageEnemyDatas();
            SetStageMiddleBossDatas();
            SetStageFinalBossDatas();
            SetStageEnemyTotalMonsterCnt();

            chartTotalEnemyCnt = 0;
            foreach (int cnt in StageEnemyTotalMonsterCnt) // 일반 몬스터 cnt 차트 값 받아서 세팅
            {
                chartTotalEnemyCnt += cnt;
            }

            totalMonsterCount = chartTotalEnemyCnt;
            isEnemyAllDeadBeforeMiddleBoss = false;  
            isCheckingMiddleBoss = false;
            isSpawnedBoss = false;
            dieMonsterCount = 0;
            currentCreateEnemyCount = 0;
            bossPosIndex = 0;

            // InitCreateEnemy();

            StartCoroutine(UpdateCreateEnemy());

            //   if (totalMonsterCount == 0) // 50스테이지 라면? 
            //   {
            //       isEnemyAllDeadBeforeMiddleBoss = true;  
            //   }

            stageTime = 30;
            nowRemainTime = stageTime;
            isStartTimer = false;
            isStageEnd = false;
            //Debug.Log($"ClearStageLevel : {UserDataManager.instance.clearStageLevel}");
            //Debug.Log($"NowStageLevel : {UserDataManager.instance.nowStageLevel}");  
        }


    }
    public void SetTrainingDungeonPlayerType(PlayerType type)
    {
        dungeonPlayerType = type;  
    }
    public void Clear()
    {
        //StopCoroutine(UpdateCreateEnemy());

        StageManager.instance.canvasManager.GetUIManager<BossHpUI>().SetActiveParent(false);
        RemoveAllMonster();
    }

    public void Update()
    {
        if(isStartTimer == true)
        {
            nowRemainTime -= Time.deltaTime;

            if (nowRemainTime < 0) // 지금 시간이 0보다 작으면 
            {
                nowRemainTime = 0;

                isStartTimer = false;
                PlayerControl nowPlayer = PlayersControlManager.instance.GetNowActivePlayer();

                if(stageType == StageTypeNum.TrainingDungeon)
                {
                    StageManager.instance.canvasManager.GetUIManager<UI_DungeonIngame>().ExitTrainingDungeon();    
                }

                for (int i = 0; i < PlayersControlManager.instance.playersContol.Length; ++i)
                {
                    if (PlayersControlManager.instance.playersContol[i].GetStats<Stats>().hp.isAlive == true)
                    {
                        PlayersControlManager.instance.playersContol[i].GetStats<Stats>().hp.ForceDie();
                    }
                }

                //SceneSettingManager.instance.LoadReStartStageScene();
                //PlayersControlManager.instance.ResetHpAllPlayer();
                //PlayersControlManager.instance.ResetAllPlayerState();
                //PlayersControlManager.instance.SetNotActiveAllPlayer();
            }
            else // 아직 시간 가고 있는 중   
            {
                if (stageType == StageTypeNum.TrainingDungeon)
                {
                    PlayerControl nowPlayer = PlayersControlManager.instance.GetNowActivePlayer();
                    if(nowPlayer != null)
                    {
                        if(nowPlayer.GetStats<Stats>().hp.isAlive == false)
                            StageManager.instance.canvasManager.GetUIManager<UI_DungeonIngame>().ExitTrainingDungeon();    
                    }

                }

            }
            if(stageType == StageTypeNum.TrainingDungeon)
            {
                if (_tdBossHpUI != null)
                    _tdBossHpUI.SetTimerText(nowRemainTime);
            }
            else
            {
                if (_bossHpUI != null)
                    _bossHpUI.SetTimerText(nowRemainTime); 
            }

            
        }
    }
    public StageTypeNum GetStageType()
    {
        return stageType;
    }
    public PlayerType GetTDPlayerType()
    {
        return dungeonPlayerType;
    }
    public float GetNowRemainTime()
    {
        return nowRemainTime;
    }
    // Init 되었을 때 Enemy 초기 생성 세팅.
    private void InitCreateEnemy()    
    {
        int previousCreateEnemyCount = -1;
        while (currentCreateEnemyCount < minCreateEnemyCount)        
        {
            // CheckAndCreateEnemy에서 만약 Enemy가 Spawn 되지 않아 현재 enemyCount와 이전 enemyCount가 같으면 생성이 불가한 것이기 때문에 Break하여 빠져나감.
            if (currentCreateEnemyCount != previousCreateEnemyCount)    
            {
                previousCreateEnemyCount = currentCreateEnemyCount;
                CheckAndCreateEnemy();
            }
            else
                break;
        }

        //StaticManager.Backend.Chart.BossEnemyData.GetMonsterMaxHp(1);
    }

    bool isSpawnedBoss = false;

    private IEnumerator UpdateCreateEnemy()
    {
        WaitForSeconds delayUpdateTime = new WaitForSeconds(creationUpdateTime);

        while (true)
        {
            if (!isStageEnd)
            {
                List<StageEnemyData> stageEnemyDatas = GetAvaliableCreateEnemy();  

                if (stageEnemyDatas.Count > 0)
                {
                    if (currentCreateEnemyCount < minCreateEnemyCount)
                    {
                        new WaitForSeconds(creationTime);
                        CheckAndCreateEnemy();    
                    }
                }
                else
                {
                    // if (isStageRepeat) 
                    //     SetStageEnemyDatas(); SetStageMiddleBossDatas(); SetStageFinalBossDatas();
                }

                //if(isStageRepeat == false && totalMonsterCount == 0)
                //{
                //    SetStageMiddleBossDatas();
                //    CheckAndCreateMiddleBoss();  
                //}

                if (isStageRepeat == false && isEnemyAllDeadBeforeMiddleBoss == true)// && dieMonsterCount == chartTotalEnemyCnt)
                {
                    if (totalMonsterCount == 0)
                    {
                        // SetStageMiddleBossDatas();
                        //  SetStageFinalBossDatas();
                        Debug.Log("보스 소환");
                        CheckAndCreateMiddleBoss();
                        CheckAndCreateFinalBoss();
                    }
                }
            }
            else
            {
               //if (isSpawnedBoss == false && isCheckingMiddleBoss == false)
               //{
               //    // SetStageMiddleBossDatas();
               //    //  SetStageFinalBossDatas();
               //    Debug.Log("보스 소환");
               //    CheckAndCreateMiddleBoss();
               //    CheckAndCreateFinalBoss();
               //}
            }

            yield return delayUpdateTime;
        }
    }
    private IEnumerator UpdateCreateTDEnemy(StageData stageData) // Training Dungeon
    {
        WaitForSeconds delayUpdateTime = new WaitForSeconds(creationUpdateTime);

        while (true)
        {
            if (!isStageEnd)
            {
                //List<StageEnemyData> stageEnemyDatas = GetAvaliableCreateEnemy();

                //if (stageEnemyDatas.Count > 0)
                //{
                //    if (currentCreateEnemyCount < minCreateEnemyCount)
                //    {
                //        new WaitForSeconds(creationTime);
                //        CheckAndCreateEnemy();
                //    }
                //}
                //else
                //{
                //    // if (isStageRepeat) 
                //    //     SetStageEnemyDatas(); SetStageMiddleBossDatas(); SetStageFinalBossDatas();
                //}

                //if(isStageRepeat == false && totalMonsterCount == 0)
                //{
                //    SetStageMiddleBossDatas();
                //    CheckAndCreateMiddleBoss();  
                //}

                if (isStageRepeat == false && isEnemyAllDeadBeforeMiddleBoss == true)// && dieMonsterCount == chartTotalEnemyCnt)
                {
                    Debug.Log("보스 소환");
                    CheckAndCreateTDEnemy(stageData);
                }
            }
            else
            {
                //if (isSpawnedBoss == false && isCheckingMiddleBoss == false)
                //{
                //    // SetStageMiddleBossDatas();
                //    //  SetStageFinalBossDatas();
                //    Debug.Log("보스 소환");
                //    CheckAndCreateMiddleBoss();
                //    CheckAndCreateFinalBoss();
                //}
            }

            yield return delayUpdateTime;
        }
    }
    private void CheckAndCreateEnemy()
    {
        List<StageEnemyData> stageEnemyDatas = GetAvaliableCreateEnemy();

        if (stageEnemyDatas.Count > 0)
        {
            Vector3? createEnemyPosition = GetCreateEnemyPosition();  
            Quaternion? createEnemyRotation = GetCreateEnemyRotation();

            if (createEnemyPosition != null && createEnemyRotation != null)
            {
                SpawnEnemy(stageEnemyDatas[UnityEngine.Random.Range(0, stageEnemyDatas.Count)], createEnemyPosition.Value, createEnemyRotation.Value);
            }
        }
    }

    private void CheckAndCreateMiddleBoss()  // checking middle boss
    {
        List<StageEnemyData> stageMiddleBossDatas = GetAvaliableCreateMiddleBoss();

        if (stageMiddleBossDatas.Count > 0 && isSpawnedBoss == false)
        {
            Debug.Log("stageMiddleBossDatas.Count가 남아 았다.");
            Vector3? createMiddleBossPosition = GetMiddleBossPosition();
            Quaternion? createMiddleBossRotation = GetCreateEnemyRotation();

            if (createMiddleBossPosition != null && createMiddleBossRotation != null)
            {
                EnemyControl enemyControl = SpawnEnemy(stageMiddleBossDatas[0], createMiddleBossPosition.Value, createMiddleBossRotation.Value);
                isSpawnedBoss = true;

                if (enemyControl != null)
                {
                    enemyControl.OnHpExhausted += StopTimer;

                    isStartTimer = true;
                    _bossHpUI = StageManager.instance.canvasManager.GetUIManager<BossHpUI>();
                    StageManager.instance.canvasManager.GetUIManager<BossHpUI>().SetActiveParent(true);
                    StageManager.instance.canvasManager.GetUIManager<TDBossHpUI>().SetActiveParent(false);
                    StageManager.instance.canvasManager.GetUIManager<BossHpUI>().Init(enemyControl.GetStats<Stats>());
                }
            }
        }
        else
        {
            Debug.Log("MiddleBoss Data 없음 ");
            CheckRemoveAllEnemy();
        }

        isCheckingMiddleBoss = true;
    }
    private void CheckAndCreateTDEnemy(StageData stageData)  // 던전용 
    {
        isCheckingMiddleBoss = false;
        StageEnemyData trainingDungeonEnemyData = StageMiddleBossDatas.Find(data => data.createCount > 0);  
        if (trainingDungeonEnemyData == null)
            return;

        Debug.Log("stageMiddleBossDatas.Count가 남아 았다.");
        //Vector3? createMiddleBossPosition = GetMiddleBossPosition();
        Quaternion? createMiddleBossRotation = GetCreateEnemyRotation();

        if ( createMiddleBossRotation != null)
        {
            GameObject trainingDungeon = stageData.environmentPrefab;
            if (trainingDungeon == null)
                return;

            TrainingDungeon td = trainingDungeon.GetComponent<TrainingDungeon>();
            //nowActivePlayer.gameObject.transform.localPosition = new Vector3(5, 0, -10);
            if (td.GetBossPositionList() == null)
                return;
            List<Transform> BossPosList = td.GetBossPositionList();

            EnemyControl enemyControl = null;

            if (BossPosList[bossPosIndex] == null)
                return;
            enemyControl = SpawnTDEnemy(trainingDungeonEnemyData, BossPosList[bossPosIndex].position, createMiddleBossRotation.Value);

            //EnemyControl enemyControl = SpawnTDEnemy(stageMiddleBossDatas[0], GetRandomPosition(), createMiddleBossRotation.Value);
            //isSpawnedBoss = true;

            if (enemyControl != null)
            {
                //enemyControl.OnHpExhausted += StopTimer;  

                isStartTimer = true;
                _tdBossHpUI = StageManager.instance.canvasManager.GetUIManager<TDBossHpUI>();
                StageManager.instance.canvasManager.GetUIManager<TDBossHpUI>().SetActiveParent(true);
                StageManager.instance.canvasManager.GetUIManager<BossHpUI>().SetActiveParent(false);
                StageManager.instance.canvasManager.GetUIManager<TDBossHpUI>().Init(enemyControl.GetStats<Stats>());
            }
            else  
            {
                Debug.Log($"enemy control 없음 ");  
            }
        }

        //isCheckingMiddleBoss = true;
    }
    private void StopTimer(EnemyControl enemyControl)
    {
        isStartTimer = false;
    }

    private void CheckAndCreateFinalBoss() // checking final boss
    {
        List<StageEnemyData> stageFinalBossDatas = GetAvaliableCreateFinalBoss();

        if (stageFinalBossDatas.Count > 0)
        {
            Debug.Log("stageFinalBossDatas.Count가 남아 았다.");
            Vector3? createMiddleBossPosition = GetMiddleBossPosition();
            Quaternion? createMiddleBossRotation = GetCreateBossRotation();    

            if (createMiddleBossPosition != null && createMiddleBossRotation != null && isSpawnedBoss == false)
            {
                EnemyControl enemyControl = SpawnEnemy(stageFinalBossDatas[0], createMiddleBossPosition.Value, createMiddleBossRotation.Value);

                isSpawnedBoss = true;

                if (enemyControl != null)
                {
                    isStartTimer = true;
                    enemyControl.OnHpExhausted += StopTimer;
                    _bossHpUI = StageManager.instance.canvasManager.GetUIManager<BossHpUI>();
                    StageManager.instance.canvasManager.GetUIManager<BossHpUI>().SetActiveParent(true);
                    StageManager.instance.canvasManager.GetUIManager<TDBossHpUI>().SetActiveParent(false);
                    StageManager.instance.canvasManager.GetUIManager<BossHpUI>().Init(enemyControl.GetStats<Stats>());
                }
            }
        }
        else
        {
            Debug.Log("Final Boss Data 없음 ");  
            CheckRemoveAllEnemy();
        }

        isCheckingMiddleBoss = true;
    }

    #region Enemy Transform

    private Vector3? GetCreateEnemyPosition()
    {
        List<Vector3> enemyCreatePositions = new List<Vector3>();

        List<Vector3> enemyCreateAllPositions = enemyPositionCreate.GetCreatePositions();

        for (int i = 0; i < enemyCreateAllPositions.Count; i++)
        {
            Vector3 createPosition = enemyCreateAllPositions[i];
            if (enemyAvailableCreate.IsAvailableCreateEnemy(createPosition))
            {
                if (!createdEnemyDatas.ContainsValue(createPosition))
                {
                    enemyCreatePositions.Add(createPosition);
                }
            }
        }

        if (enemyCreatePositions.Count > 0)
        {
            //Vector3 useCreatePosition = enemyCreatePositions[UnityEngine.Random.Range(0, enemyCreatePositions.Count)];
            Vector3 useCreatePosition = GetRandomPosition();  
            return useCreatePosition;
        }
        else
        {
            enemyAvailableCreate.NullCreatePosition(enemyPositionCreate.GetCreatePositions(), createdEnemyDatas.Count);
            return null;
        }
    }

    private Vector3 GetRandomPosition()
    {
        int index = UnityEngine.Random.Range(0, enemyPositionCreate_random.MyMapCircleEnemyCreateBorders_x.Length);
        return new Vector3(enemyPositionCreate_random.MyMapCircleEnemyCreateBorders_x[index], 0, enemyPositionCreate_random.MyMapCircleEnemyCreateBorders_z[index]);  
    }

    public Vector3 GetMiddleBossPosition()
    {
        Vector3 middleBossPosition = new Vector3(enemyPositionCreate_random.MyMapCircleEnemyCreateBorders_x[0] + 20, 0, enemyPositionCreate_random.MyMapCircleEnemyCreateBorders_z[0]);
        return middleBossPosition;
    }
      
    private Quaternion? GetCreateEnemyRotation()  
    {
        return enemyRotationCreate.GetRotation();
    }
    private Quaternion? GetCreateBossRotation()
    {
        return Quaternion.Euler(new Vector3(0, 74, 0));
    }
    #endregion

    #region Enemy Add

    public int order = 0;
    private EnemyControl SpawnEnemy(StageEnemyData stageEnemyData, Vector3 createPosition, Quaternion createRotation)
    {
        EnemyControl enemyControl = ObjectPoolManager.instance.CreateObject(stageEnemyData.enemyPrefab, createPosition, createRotation).GetComponent<EnemyControl>();
        enemyControl.gameObject.name = enemyControl.gameObject.name + $"_{++order}";
        enemyControl.GetModel<EnemyModel>().animationControl.ResetAnimationState();
        enemyControl.GetAttack<EnemyAttack>().CompleteAttackWait();  
        enemyControl.GetAttack<EnemyAttack>().shootDone = false;

        enemyControl.OnHpExhausted -= HandleOnEnemyDie;
        enemyControl.OnHpExhausted += HandleOnEnemyDie;

        SetEnemyReward(stageEnemyData, enemyControl);
        SetEnemyStats(enemyControl);  

        if(stageType != StageTypeNum.TrainingDungeon)
            AddEnemy(stageEnemyData, createPosition, enemyControl);  

        return enemyControl;
    }
    private EnemyControl SpawnTDEnemy(StageEnemyData stageEnemyData, Vector3 createPosition, Quaternion createRotation)
    {
        EnemyControl enemyControl = ObjectPoolManager.instance.CreateObject(stageEnemyData.enemyPrefab, createPosition, createRotation).GetComponent<EnemyControl>();
        enemyControl.gameObject.name = enemyControl.gameObject.name + $"_{++order}";
        enemyControl.GetModel<EnemyModel>().animationControl.ResetAnimationState();
        enemyControl.GetAttack<EnemyAttack>().CompleteAttackWait();
        enemyControl.GetAttack<EnemyAttack>().shootDone = false;

        enemyControl.OnHpExhausted -= HandleOnTDEnemyDie;
        enemyControl.OnHpExhausted += HandleOnTDEnemyDie;

        SetEnemyReward(stageEnemyData, enemyControl);
        if(dungeonPlayerType != PlayerType.None)
        {
            SetEnemyStats(enemyControl, dungeonPlayerType, dieMonsterCount);
        }

        AddTDEnemy(stageEnemyData, createPosition, enemyControl);  

        return enemyControl;
    }
    private void SetEnemyReward(StageEnemyData stageEnemyData, EnemyControl enemyControl)
    {
        switch (enemyControl.enemyType)
        {
         
        }
    }

    private void SetEnemyStats(EnemyControl enemyControl)
    {
        // StatsManager enemyStatsManager = enemyControl.GetStats<Stats>().manager;
        //
        // float ratio = (StageManager.instance as GamePlayManager).stageData.stageAdjustmentRatio;
        //
        // enemyStatsManager.RemoveAdditionValue(EnemyStatsValueDefine.DamageRatio, "AdjustmentDamage", Operation.Ratio, ratio);
        // enemyStatsManager.SetAdditionValue(EnemyStatsValueDefine.DamageRatio, new AdditionValue("AdjustmentDamage", Operation.Ratio, ratio));
        Stats enemyStat = enemyControl.GetStats<Stats>();
        enemyStat.hp.SetHpMax(enemyControl.enemyType);


    }
    private void SetEnemyStats(EnemyControl enemyControl, PlayerType playerType, long level)
    {
        Stats enemyStat = enemyControl.GetStats<Stats>();
        enemyStat.hp.SetHpMax_TrainingDungeon(playerType, level);
    }


    private void AddEnemy(StageEnemyData stageEnemyData, Vector3 createPosition, EnemyControl enemyControl)
    {
        OnAnnounceEnemySpawn?.Invoke(enemyControl);

        switch (stageEnemyData.enemyPrefab.name.Substring(0, 1))
        {
            case "E":
                createdEnemyDatas.Add(enemyControl, createPosition);
                //AddGeneralOption(stageEnemyData);
                break;
            case "M":
                createdMiddleBossDatas.Add(enemyControl, createPosition);
                //AddBossOption(stageEnemyData);
                break;
            case "B":
                createdFinalBossDatas.Add(enemyControl, createPosition);
                //AddBossOption(stageEnemyData);
                break;
        }

        stageEnemyData.createCount -= 1;
        currentCreateEnemyCount += 1;

        if (isStageRepeat == false)
        {
            totalMonsterCount -= 1;
        }
        else
        {
            currentStageEnemyDatas = Instantiate(stageData).stageEnemyDatas;
            totalMonsterCount = chartTotalEnemyCnt;
        }

        CheckSpawnAllEnemy();
    }
    private void AddTDEnemy(StageEnemyData stageEnemyData, Vector3 createPosition, EnemyControl enemyControl)
    {
        OnAnnounceEnemySpawn?.Invoke(enemyControl);

        switch (stageEnemyData.enemyPrefab.name.Substring(0, 1))
        {
            case "E":
                createdEnemyDatas.Add(enemyControl, createPosition);
                //AddGeneralOption(stageEnemyData);
                break;
            case "M":
                createdMiddleBossDatas.Add(enemyControl, createPosition);
                //AddBossOption(stageEnemyData);
                break;
            case "B":
                createdFinalBossDatas.Add(enemyControl, createPosition);
                //AddBossOption(stageEnemyData);
                break;
        }

        //stageEnemyData.createCount -= 1;
        currentCreateEnemyCount += 1;

        if (isStageRepeat == false)
        {
            //totalMonsterCount -= 1;
        }
        else
        {
            //currentStageEnemyDatas = Instantiate(stageData).stageEnemyDatas;
            //totalMonsterCount = chartTotalEnemyCnt;  
        }

        CheckSpawnAllEnemy();
    }
    private void AddGeneralOption(StageEnemyData stageEnemyData)
    {
        stageEnemyData.createCount -= 1;
        currentCreateEnemyCount += 1;  

        if (isStageRepeat == false)
        {
            totalMonsterCount -= 1;
        }
        else
        {
            currentStageEnemyDatas = Instantiate(stageData).stageEnemyDatas;
            totalMonsterCount = chartTotalEnemyCnt;
        }

        CheckSpawnAllEnemy();
    }

    private void AddBossOption(StageEnemyData stageEnemyData)
    {
        stageEnemyData.createCount -= 1;
        currentCreateEnemyCount += 1;

        if(isStageRepeat == true)
        {
            currentStageEnemyDatas = Instantiate(stageData).stageEnemyDatas;
            totalMonsterCount = chartTotalEnemyCnt;
        }
        CheckSpawnAllEnemy();
    }

    #endregion

    #region Enemy Remove

    private void HandleOnEnemyDie(EnemyControl enemyControl)
    {
        enemyControl.OnHpExhausted -= HandleOnEnemyDie;  

        RemoveEnemy(enemyControl);
        dieMonsterCount++;

    }

    private void HandleOnTDEnemyDie(EnemyControl enemyControl)
    {
        enemyControl.OnHpExhausted -= HandleOnTDEnemyDie;

        createdMiddleBossDatas.Remove(enemyControl);
       // CheckAndCreateTDEnemy();
        nowRemainTime += 2f;
        if(_tdBossHpUI != null)
            _tdBossHpUI.SetTimerText(nowRemainTime);

        isStartTimer = true;
        //RemoveEnemy(enemyControl);
        dieMonsterCount++;
        if(dungeonPlayerType != PlayerType.None)
        {
            /* 캐릭터 사망시는 저장 안되게 */
            if (StaticManager.Backend.GameData.PlayerGameData.TDMaxClearLevelList[dungeonPlayerType.ToString()] < dieMonsterCount)
                StaticManager.Backend.GameData.PlayerGameData.UpdateTDMaxClearLevel(dungeonPlayerType, dieMonsterCount);      
        }

        bossPosIndex += 1;
        if (bossPosIndex >= 4)
            bossPosIndex = 0;  

        StartCoroutine(CoSpawnTDEnemy());
    }

    IEnumerator CoSpawnTDEnemy()
    {
        yield return new WaitForSeconds(1.5f);
        CheckAndCreateTDEnemy(this.stageData);  
    }

    private void RemoveEnemy(EnemyControl enemyControl)
    {
        OnAnnounceEnemyDie?.Invoke(enemyControl);
        createdEnemyDatas.Remove(enemyControl);
        createdMiddleBossDatas.Remove(enemyControl);
        createdFinalBossDatas.Remove(enemyControl);
        currentCreateEnemyCount -= 1;

        CheckRemoveAllEnemyAndCreateMiddleBoss();
        CheckRemoveAllEnemy();
    }

    #endregion

    private void CheckSpawnAllEnemy()
    {
        if (IsEmptyCurrentEnemyData())
        {
            OnSpawnAllEnd?.Invoke();
        }
    }

    private void CheckRemoveAllEnemy()
    {
        if(currentCreateEnemyCount <= 0)
        {
            if (IsEmptyCurrentEnemyData() && totalMonsterCount <= 0)
            {
                if (isCheckingMiddleBoss == true && createdMiddleBossDatas.Count == 0 && createdFinalBossDatas.Count ==0) // middleboss 있는지도 체크 했고 middle 보스 죽었으면 
                {
                    OnRemovedAllEnemy?.Invoke();
                    Debug.Log("OnRemovedAllEnemy");  

                   // UserDataManager.instance.UpdateClearStageLevel();
                    //UserDataManager.instance.UpdateNowStageLevel();
                }
            }
        }
    }
    private void CheckRemoveAllEnemyAndCreateMiddleBoss()  // middle boss 나오기 전에 일반몬스터 다 죽었는지 체크
    {
        if (currentCreateEnemyCount <= 0)
        {
            if (IsEmptyCurrentEnemyData())
            {
                isEnemyAllDeadBeforeMiddleBoss = true;      
            }
        }
    }
    public void SetStageEnemyDatas()            // general
    {
        previousStageEnemyDatas.AddRange(currentStageEnemyDatas);

        currentStageEnemyDatas = Instantiate(stageData).stageEnemyDatas;
    }

    public void SetStageEnemyTotalMonsterCnt()
    {
        StageEnemyTotalMonsterCnt = Instantiate(stageData).stageEnemyDatasTotalMonsterCnt;
    }

    public void SetStageMiddleBossDatas()       // middle boss
    {
        StageMiddleBossDatas = Instantiate(stageData).stageMiddleBossDatas;  
    }
    public void SetStageFinalBossDatas()        // final boss
    {
        StageFinalBossDatas = Instantiate(stageData).stageFinalBossDatas;
    }

    private List<StageEnemyData> GetAvaliableCreateEnemy()      // general
    {
        return currentStageEnemyDatas.FindAll(data => data.createCount > 0);
    }

    private List<StageEnemyData> GetAvaliableCreateMiddleBoss()     // middle boss
    {
        return StageMiddleBossDatas.FindAll(data => data.createCount > 0);  
    }

    private List<StageEnemyData> GetAvaliableCreateFinalBoss()      // final boss
    {
        return StageFinalBossDatas.FindAll(data => data.createCount > 0);
    }
    private bool IsEmptyCurrentEnemyData()
    {
        // currentStageEnemyDatas는 createCount가 모두 0이 되고 stageRepeat 상태일 때 새로운 Data로 바뀌기 때문에 previousStageEnemyDatas로 체크.
        return currentStageEnemyDatas.Find(data => data.createCount != 0) == null;
       // return previousStageEnemyDatas.Find(data => data.createCount != 0) == null;
    }

    public bool IsLargerThanOneSpawnMonster()
    {
        int monsterNumber = createdEnemyDatas.Count + createdMiddleBossDatas.Count + createdFinalBossDatas.Count;
        return monsterNumber > 0 ? true : false;
    }
    
    public GameObject GetNearMonster(Vector3 position)
    {
        float minLen = float.MaxValue;
        GameObject returnObj = null;

        foreach(EnemyControl monster in createdEnemyDatas.Keys)
        {
            if (monster.GetStats<Stats>().hp.GetCurrentHp() <= 0)
                continue;

            float nowLen = (monster.gameObject.transform.position - position).magnitude;

            if (minLen >= nowLen)
            {
                minLen = nowLen;
                returnObj = monster.gameObject;
            }
        }

        foreach (EnemyControl monster in createdMiddleBossDatas.Keys)
        {
            if (monster.GetStats<Stats>().hp.GetCurrentHp() <= 0)
                continue;

            float nowLen = (monster.gameObject.transform.position - position).magnitude;

            if (minLen >= nowLen)
            {
                minLen = nowLen;
                returnObj = monster.gameObject;
            }
        }

        foreach (EnemyControl monster in createdFinalBossDatas.Keys)
        {
            if (monster.GetStats<Stats>().hp.GetCurrentHp() <= 0)
                continue;

            float nowLen = (monster.gameObject.transform.position - position).magnitude;

            if (minLen >= nowLen)
            {
                minLen = nowLen;
                returnObj = monster.gameObject;
            }
        }

        return returnObj;
    }
    public GameObject GetNextMonster(Vector3 position)
    {
        float minLen = float.MaxValue;
        GameObject returnObj = null;

        foreach (EnemyControl monster in createdEnemyDatas.Keys)
        {
            if (monster.GetStats<Stats>().hp.GetCurrentHp() <= 0)
                continue;

            float nowLen = (monster.gameObject.transform.position - position).magnitude;
            if (nowLen < 0.1f)
                continue;

            if (minLen >= nowLen)
            {
                minLen = nowLen;
                returnObj = monster.gameObject;
            }
        }

        foreach (EnemyControl monster in createdMiddleBossDatas.Keys)
        {
            if (monster.GetStats<Stats>().hp.GetCurrentHp() <= 0)
                continue;

            float nowLen = (monster.gameObject.transform.position - position).magnitude;

            if (minLen >= nowLen)
            {
                minLen = nowLen;
                returnObj = monster.gameObject;
            }
        }

        foreach (EnemyControl monster in createdFinalBossDatas.Keys)
        {
            if (monster.GetStats<Stats>().hp.GetCurrentHp() <= 0)
                continue;

            float nowLen = (monster.gameObject.transform.position - position).magnitude;

            if (minLen >= nowLen)
            {
                minLen = nowLen;
                returnObj = monster.gameObject;
            }
        }

        return returnObj;
    }
    public List<GameObject> GetRandomMonster(Vector3 position, float maxDistance)
    {
        float minLen = float.MaxValue;
        List<GameObject> returnObjs = new List<GameObject>();

        foreach (EnemyControl monster in createdEnemyDatas.Keys)
        {
            if (monster.GetStats<Stats>().hp.GetCurrentHp() <= 0)
                continue;

            float nowLen = (monster.gameObject.transform.position - position).magnitude;
            if (nowLen < maxDistance)
            {
                returnObjs.Add(monster.gameObject);
            }
        }

        foreach (EnemyControl middleMonster in createdMiddleBossDatas.Keys)
        {
            if (middleMonster.GetStats<Stats>().hp.GetCurrentHp() <= 0)
                continue;

            float nowLen = (middleMonster.gameObject.transform.position - position).magnitude;
            if (nowLen < maxDistance)
            {
                returnObjs.Add(middleMonster.gameObject);
            }
        }

        foreach (EnemyControl finalMonster in createdFinalBossDatas.Keys)
        {
            if (finalMonster.GetStats<Stats>().hp.GetCurrentHp() <= 0)
                continue;

            float nowLen = (finalMonster.gameObject.transform.position - position).magnitude;
            if (nowLen < maxDistance)
            {
                returnObjs.Add(finalMonster.gameObject);
            }
        }

        return returnObjs;
    }

    public List<GameObject> GetAllMonster()
    {
        float minLen = float.MaxValue;
        List<GameObject> returnObjs = new List<GameObject>();

        foreach (EnemyControl monster in createdEnemyDatas.Keys)
        {
            returnObjs.Add(monster.gameObject);
        }

        foreach (EnemyControl middleMonster in createdMiddleBossDatas.Keys)
        {
            returnObjs.Add(middleMonster.gameObject);
        }

        foreach (EnemyControl finalMonster in createdFinalBossDatas.Keys)
        {
            returnObjs.Add(finalMonster.gameObject);
        }

        return returnObjs;
    }
    public List<EnemyControl> GetAllMonster_control()
    {
        float minLen = float.MaxValue;
        List<EnemyControl> returnObjs = new List<EnemyControl>();

        foreach (EnemyControl monster in createdEnemyDatas.Keys)
        {
            returnObjs.Add(monster);
        }

        foreach (EnemyControl middleMonster in createdMiddleBossDatas.Keys)
        {
            returnObjs.Add(middleMonster);
        }

        foreach (EnemyControl finalMonster in createdFinalBossDatas.Keys)
        {
            returnObjs.Add(finalMonster);
        }

        return returnObjs;
    }
    public List<GameObject> GetAllMonster(Vector3 position, float maxDistance)
    {
        float minLen = float.MaxValue;
        List<GameObject> returnObjs = new List<GameObject>();

        foreach (EnemyControl monster in createdEnemyDatas.Keys)
        {
            if (monster.GetStats<Stats>().hp.GetCurrentHp() <= 0)
                continue;

            if((monster.gameObject.transform.position - position).magnitude < maxDistance)
                returnObjs.Add(monster.gameObject);  
        }

        foreach (EnemyControl middleMonster in createdMiddleBossDatas.Keys)
        {
            if (middleMonster.GetStats<Stats>().hp.GetCurrentHp() <= 0)
                continue;

            if ((middleMonster.gameObject.transform.position - position).magnitude < 100)
                returnObjs.Add(middleMonster.gameObject);
        }

        foreach (EnemyControl finalMonster in createdFinalBossDatas.Keys)
        {
            if (finalMonster.GetStats<Stats>().hp.GetCurrentHp() <= 0)
                continue;

            if ((finalMonster.gameObject.transform.position - position).magnitude < 100)
                returnObjs.Add(finalMonster.gameObject);
        }

        return returnObjs;
    }

    public void RemoveAllMonster()
    {
        foreach (EnemyControl monster in createdEnemyDatas.Keys)
        {
            ObjectPoolManager.instance.RemoveObject(monster.gameObject);
        }

        foreach (EnemyControl middleMonster in createdMiddleBossDatas.Keys)
        {
            ObjectPoolManager.instance.RemoveObject(middleMonster.gameObject);
        }

        foreach (EnemyControl finalMonster in createdFinalBossDatas.Keys)
        {
            ObjectPoolManager.instance.RemoveObject(finalMonster.gameObject);
        }

        createdEnemyDatas.Clear();
        createdMiddleBossDatas.Clear();
        createdFinalBossDatas.Clear();
    }
}
