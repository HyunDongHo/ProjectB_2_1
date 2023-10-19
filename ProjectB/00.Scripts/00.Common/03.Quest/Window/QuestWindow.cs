using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestWindow : MonoBehaviour, IOpenClose
{
    public QuestWindowView view;

    [Space]

    private List<QuestCell> cells = new List<QuestCell>();

    private QuestCell previousClickedCell = null;
    private QuestCell currentClickedCell = null;

    [Space]

    public GameObject questSubCellPrefab;
    public GameObject questMainCellPrefab;
    public RectTransform questCellParent;

    private void Awake()
    {
        AddEvent();
    }

    private void OnDestroy()
    {
        RemoveEvent();
    }

    private void AddEvent()
    {
        QuestManager.instance.OnInit += HandleOnQuestAdded;
        QuestManager.instance.OnAdded += HandleOnQuestAdded;
        QuestManager.instance.OnRemoved += HandleOnQuestRemoved;

        view.teleport.onClick.AddListener(HandleOnTeleport);
    }

    private void RemoveEvent()
    {
        QuestManager.instance.OnInit -= HandleOnQuestAdded;
        QuestManager.instance.OnAdded -= HandleOnQuestAdded;
        QuestManager.instance.OnRemoved -= HandleOnQuestRemoved;

        view.teleport.onClick.RemoveListener(HandleOnTeleport);
    }

    #region Event

    private void HandleOnQuestAdded(QuestData data)
    {
        AddCellToWindow(data);
    }

    private void HandleOnQuestRemoved(QuestData data)
    {
        RemoveCellToWindow(data);
    }

    private void HandleOnTeleport()
    {
        if (IsAvaliableTeleport())
            ShowTeleportPopup();
    }

    #endregion

    #region Add/Remove Cell

    private QuestCell InstantiateQuestCell(QuestData data)
    {
        switch (data.questSubject)
        {
            case QuestSubject.Main:
                return (Instantiate(questMainCellPrefab, questCellParent, instantiateInWorldSpace: false) as GameObject).GetComponent<QuestCell>();
            case QuestSubject.Sub:
                return (Instantiate(questSubCellPrefab, questCellParent, instantiateInWorldSpace: false) as GameObject).GetComponent<QuestCell>();
            default:
                return (Instantiate(questSubCellPrefab, questCellParent, instantiateInWorldSpace: false) as GameObject).GetComponent<QuestCell>();
        }
    }

    private void AddCellToWindow(QuestData data)
    {
        QuestCell cell = InstantiateQuestCell(data);
        cell.Init(data);

        AddCell(cell);

        SetQuestWindow();
    }

    private void RemoveCellToWindow(QuestData data)
    {
        QuestCell cell = cells.Find(quest => quest.GetQuestData() == data);
        cell.Release(data);

        RemoveCell(cell);

        Destroy(cell.gameObject);

        SetQuestWindow();
    }

    private void AddCell(QuestCell cell)
    {
        cells.Add(cell);

        cell.OnClicked += HanldeOnClicked;
    }

    private void RemoveCell(QuestCell cell)
    {
        ResetClicked(cell);

        cells.Remove(cell);

        cell.OnClicked -= HanldeOnClicked;
    }

    private void HanldeOnClicked(QuestCell cell)
    {
        previousClickedCell = currentClickedCell;

        previousClickedCell?.SetButtonClicked(false);

        currentClickedCell = cell;
        view.ActiveTeleport(IsAvaliableTeleport());

        currentClickedCell?.SetButtonClicked(true);
    }

    private void ResetClicked(QuestCell cell)
    {
        if (cell == previousClickedCell)
            previousClickedCell = null;

        if (cell == currentClickedCell)
            currentClickedCell = null;
    }

    #endregion

    #region Teleport 기능.

    private bool IsAvaliableTeleport()
    {
        // 클릭하고 있는 셀의 퀘스트에 이동할 수 있는 데이터가 있으면 작동.
        QuestData data = currentClickedCell?.GetQuestData();

        if (data != null)
        {
            if (data.IsAvailableAccessLocation() && data.saveData.state == QuestState.InProgress)
                return CheckLocation(data.GetQuestProgressLocation().ToLower());
        }

        return false;
    }

    private bool CheckLocation(string location)
    {
        string currentLocation = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.ToLower();

        string defaultInsideStage = SceneSettingManager.DEFAULT_INSIDE_STAGE_SCENE.ToLower();
        string defaultOutsideStage = SceneSettingManager.DEFAULT_OUTSIDE_STAGE_SCENE.ToLower();

        string bossStage = SceneSettingManager.BOSS_STAGE_SCENE.ToLower();

        // 현재 씬이 외부, 내부이라면
        if (currentLocation == defaultInsideStage || currentLocation == defaultOutsideStage)
        {
            string currentStage = SceneSettingManager.instance.GetCurrentStage().ToLower();
            if (location != currentStage)
                return true;
        }
        // 현재 씬이 보스라면
        else if (currentLocation == bossStage)
        {
            string currentBossStage = SceneSettingManager.instance.GetCurrentBossStage().ToLower();
            if (location != currentBossStage)
                return true;
        }
        else if (location != currentLocation)
        {
            return true;
        }

        return false;
    }

    private void ShowTeleportPopup()
    {
        PopupQuestTeleport popup = PopupManager.instance.CreatePopup<PopupQuestTeleport>("PopupQuestTeleport").GetPopup();
        popup.RunTeleportPopup(currentClickedCell.GetQuestData().progressLocation);
    }

    #endregion

    #region Quest Cell Window

    private void SetQuestWindow()
    {
        SortQuestWindow();

        float totalCellY = 0;

        for (int i = 0; i < cells.Count; i++)
        {
            RectTransform cellTransform = cells[i].GetComponent<RectTransform>();
            totalCellY += cellTransform.rect.height;
        }

        questCellParent.sizeDelta = new Vector2(questCellParent.sizeDelta.x, totalCellY);
    }

    private void SortQuestWindow()
    {
        for (int i = 0; i < cells.Count; i++)
        {
            for (int j = i + 1; j < cells.Count; j++)
            {
                QuestCell a = cells[i];
                QuestCell b = cells[j];

                if (a == null || b == null) continue;

                if ((int)a.GetQuestData()?.questSubject > (int)b.GetQuestData()?.questSubject)
                {
                    cells[i] = b;
                    cells[j] = a;
                }
            }
        }
    }

    #endregion

    #region Window

    public void Open(bool isAnimation)
    {
        view.OpenCloseQuestWindow(isOpen: true, isAnimation);

        previousClickedCell?.SetButtonClicked(false);
        currentClickedCell?.SetButtonClicked(false);

        previousClickedCell = null;
        currentClickedCell = null;
    }

    public void Close(bool isAnimation)
    {
        view.OpenCloseQuestWindow(isOpen: false, isAnimation);

        view.ActiveTeleport(false);
    }

    #endregion
}
