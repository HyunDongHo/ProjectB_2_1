using BackEnd;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_RankItem : MonoBehaviour
{
    [SerializeField]
    Sprite rank01, rank02, rank03, rank04_10, rankOther;

    [SerializeField]
    TextMeshProUGUI rankText, nickNameText, stageText;

    [SerializeField]
    Image rankImage, backImage;

    [SerializeField]
    Color otherColor, myColor;

    // Start is called before the first frame update
    public void Init(BackendData.Rank.RankUserItem rankUserItem)
    {
        rankText.text = $"{rankUserItem.rank}위";
        nickNameText.text = rankUserItem.nickname;
        stageText.text = $"스테이지 {rankUserItem.score}";

        if(Backend.UserNickName == rankUserItem.nickname)
        {
            backImage.color = myColor;
        }
        else
        {
            backImage.color = otherColor;
        }

        SetRankImage(int.Parse(rankUserItem.rank));
    }

    private void SetRankImage(int rank)
    {
        if(rank == 1)
        {
            rankImage.sprite = rank01;
        }
        else if(rank ==2) 
        {
            rankImage.sprite = rank02;
        }
        else if(rank ==3) 
        {
            rankImage.sprite = rank03;
        }
        else if(rank >= 4 && rank <= 10) 
        {
            rankImage.sprite = rank04_10;
        }
        else
        {
            rankImage.sprite = rankOther;
        }
    }
}
