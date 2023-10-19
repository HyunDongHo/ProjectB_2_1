using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Scheduler;

public class DeadUI : MonoBehaviour
{
    public GameObject deadParent;

    [Space]

    public Button returnLobbyButton;
    public Button restartGameButton;

    [Space]

    public float autoReturnLobbyTime = 60;
    public Text autoReturnLobbyTimeText;

    private TimerBuffer autoReturnLobby;

    private void Awake()
    {
        AddEvent();

    }

    private void Start()
    {
        autoReturnLobby = new TimerBuffer(autoReturnLobbyTime);
    }

    private void OnDestroy()
    {
        RemoveEvent();
    }

    private void AddEvent()
    {
        //StageManager.instance.playerControl.OnHpExhausted += HandleOnPlayerDie;
        for(int i = 0; i < (int)PlayerType.None; ++i)
        {
            PlayersControlManager.instance.playersContol[i].OnHpExhausted += HandleOnPlayerDie;
        }

        returnLobbyButton.onClick.AddListener(() => ReturnLobby());
        restartGameButton.onClick.AddListener(() => RestartGame());
    }

    private void RemoveEvent()
    {
        //StageManager.instance.playerControl.OnHpExhausted -= HandleOnPlayerDie;
        for (int i = 0; i < (int)PlayerType.None; ++i)
        {
            PlayersControlManager.instance.playersContol[i].OnHpExhausted -= HandleOnPlayerDie;
        }

        returnLobbyButton.onClick.RemoveAllListeners();
        restartGameButton.onClick.RemoveAllListeners();
    }

    private void HandleOnPlayerDie(PlayerControl playerControl)
    {
        deadParent.SetActive(true);
        Debug.Log($"who die : {StageManager.instance.playerControl.name}");  

        AutoReturnLobby();
    }

    private void AutoReturnLobby()
    {
        Timer.instance.TimerStart(autoReturnLobby,
            OnFrame: () =>
            {
                if (autoReturnLobbyTimeText != null)
                    autoReturnLobbyTimeText.text = Mathf.Ceil(autoReturnLobbyTime - autoReturnLobby.timer).ToString();
            },
            OnComplete: () =>
            {
                ReturnLobby();
            });
    }

    private void ReturnLobby()
    {
        SceneSettingManager.instance.LoadLobbyStageScene();
    }

    private void RestartGame()
    {
        string currentLocation = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.ToLower();

        string defaultInsideStage = SceneSettingManager.DEFAULT_INSIDE_STAGE_SCENE.ToLower();
        string defaultOutsideStage = SceneSettingManager.DEFAULT_OUTSIDE_STAGE_SCENE.ToLower();

        string bossStage = SceneSettingManager.BOSS_STAGE_SCENE.ToLower();

        // 현재 씬이 외부, 내부이라면
        if (currentLocation == defaultInsideStage || currentLocation == defaultOutsideStage)
        {
            SceneSettingManager.instance.LoadDefaultStageScene(StaticManager.Backend.GameData.PlayerGameData.NowStageLevel);                 
        }
        // 현재 씬이 보스라면
        else if (currentLocation == bossStage)
        {
            SceneSettingManager.instance.LoadBossStageScene();
        }
        else
        {
            SceneSettingManager.instance.LoadLobbyStageScene();
        }

        //PlayerStats stats = StageManager.instance.playerControl.GetStats<PlayerStats>();
        //stats.hp.SetHpToMax();
        //stats.sp.SetSpToMax();

        PlayersControlManager.instance.ResetAllPlayerState();  
        PlayersControlManager.instance.ResetHpAllPlayer();
        //(StageManager.instance as GamePlayManager).enemyManager.ResetAllMonsterState();
        //PlayersControlManager.instance.ResetAllPlayerState();    

    }
}
