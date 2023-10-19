using BackEnd;
using BackEnd.Game.Rank;
using BackendData.Post;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_RankPopup : UIManagers
{    public enum RankType
    {
        STAGE
    }

    [SerializeField]
    UI_RankItem rankItemObj;

    [SerializeField]
    UI_MyRankItem myRankItem;

    [SerializeField]
    VerticalLayoutGroup verticalLayoutGroup;

    [SerializeField]
    GameObject _parent;

    // Start is called before the first frame update
    
    List<UI_RankItem> uiRankItems = new List<UI_RankItem>();

    public void Awake()
    {
        InitRankList();

    }
    public void InitRankList()
    {
        if (uiRankItems.Count > 0)
            return;

        for (int i = 0; i < 100; ++i)
        {
            // ������ ����, �������� ���� ��ư�� RemovePost �Լ� ����
            var obj = Instantiate(rankItemObj, verticalLayoutGroup.transform, true);
            obj.transform.localScale = new Vector3(1, 1, 1);
            uiRankItems.Add(obj.GetComponent<UI_RankItem>());
        }
    }

    void OffItems()
    {
        for (int i = 0; i < uiRankItems.Count; i++)
        {
            uiRankItems[i].gameObject.SetActive(false); 
        }

    }

    void SetRankItme(int index)
    {
        OffItems();

        if (index < 0)
            return;

        string title = string.Empty;

        switch ((RankType)index)
        {
            case RankType.STAGE:
                title = "STAGE";
                break;
        }

        for (int i = 0; i < StaticManager.Backend.Rank.List.Count; ++i)
        {
            if (StaticManager.Backend.Rank.List[i].title == title)
            {
                for (int j = 0; j < StaticManager.Backend.Rank.List[i].UserList.Count; j++)
                {
                    uiRankItems[j].gameObject.SetActive(true);
                    uiRankItems[j].Init(StaticManager.Backend.Rank.List[i].UserList[j]);

                    // ���� ��쿡�� ���
                    if (j > uiRankItems.Count)
                    {
                        break;
                    }
                }

                // ������ 10������ �����Ͱ� ���� ��쿡�� ���� �����͸� �Ⱥ��̰� ����
                for (int j = StaticManager.Backend.Rank.List[i].UserList.Count; j < uiRankItems.Count; j++)
                {
                    uiRankItems[j].gameObject.SetActive(false);
                }
                UpdateMyRank(index);
                break;
            }
        }   
    }

    private void UpdateMyRank(int index)
    {
        string title = string.Empty;

        switch ((RankType)index)
        {
            case RankType.STAGE:
                title = "STAGE";
                break;
        }

        int tableIndex = 0;

        for (int i = 0; i < StaticManager.Backend.Rank.List.Count; ++i)
        {
            if (StaticManager.Backend.Rank.List[i].title == title)
            {
                tableIndex = i;
                break;
            }
        }

        BackendData.Rank.RankUserItem myRank = StaticManager.Backend.Rank.List[tableIndex].MyRankItem;

        if (myRank != null && myRank.nickname != "-")
        {
            bool isHaveMyRank = false;
            {
                {
                    myRankItem.Init(myRank);
                    isHaveMyRank = true;
                }
            }
        }
        else
        {
            myRankItem.InitNoRank();
        }
    }


    public void Open()
    {
        SetRankItme(0);

        _parent.SetActive(true);
    }

    public void Close()
    {
        _parent.SetActive(false);
    }


}
