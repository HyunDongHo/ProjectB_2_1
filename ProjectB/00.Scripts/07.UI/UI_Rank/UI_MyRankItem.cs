using BackEnd.Game.Rank;
using BackendData.Rank;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_MyRankItem : MonoBehaviour
{
    [SerializeField]
    Sprite rank01, rank02, rank03, rank04_10, rankOther;

    [SerializeField]
    TextMeshProUGUI rankText, nickNameText, stageText;

    [SerializeField]
    Image rankImage;
    public void Init(BackendData.Rank.RankUserItem rankUserItem)
    {
        if(int.Parse(rankUserItem.rank) > Define.RankOutNum)
            rankText.text = $"순위밖";
        else
            rankText.text = $"{rankUserItem.rank}위";

        nickNameText.text = rankUserItem.nickname;

        stageText.text = $"스테이지 {rankUserItem.score}";
        SetRankImage(int.Parse(rankUserItem.rank));
        rankImage.gameObject.SetActive(true);
    }

    // Start is called before the first frame update
    public void InitNoRank()
    {
        rankText.text = $"-";
        nickNameText.text = "-";
        stageText.text = $"-";

        rankImage.gameObject.SetActive(false);
    }
    private void SetRankImage(int rank)
    {
        if (rank == 1)
        {
            rankImage.sprite = rank01;
        }
        else if (rank == 2)
        {
            rankImage.sprite = rank02;
        }
        else if (rank == 3)
        {
            rankImage.sprite = rank03;
        }
        else if (rank >= 4 && rank <= 10)
        {
            rankImage.sprite = rank04_10;
        }
        else
        {
            rankImage.sprite = rankOther;
        }
    }
}
