using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomCardTable : MonoBehaviour
{
    RandomCardItem[] randomCards = null;
    const float originXPosition = 12.9f;
    public bool IsInteract { get; set; } = true;
    public bool IsCreated { get; set; } = false;
    private void Start()
    {
        if (randomCards == null)
            randomCards = GetComponentsInChildren<RandomCardItem>();

        if(randomCards != null)
        {
            for(int i=0; i < randomCards.Length; ++i)
            {
                randomCards[i].cardNum = i;

                DragUnit dragUnit = randomCards[i].gameObject.GetComponentInChildren<DragUnit>();
                dragUnit.CardNum = i;

                dragUnit.selectAction = (index) =>
                {
                    //LobbyScene lobbyScene = GameObject.Find("LobbyScene").GetComponent<LobbyScene>();
                    LobbySceneManager lobbySceneManager = (StageManager.instance) as LobbySceneManager;

                    if(lobbySceneManager != null)
                        lobbySceneManager.SetSelectCard(index);

                    for (int j = 0; j < randomCards.Length; ++j)
                        randomCards[j].SelectCard.SetActive(false);

                   // if(!UserDataManager.instance.HangarItemBuyList.Contains(index))
                   //     randomCards[index].SelectCard.SetActive(true);
                };

            }
        }
    }
      
    public void OffRandomCardPriceItem()
    {
        if (randomCards == null)
            return;

        for (int i = 0; i < randomCards.Length; ++i)
        {
            randomCards[i].OffRandomCardPriceItem();
        }
    }

    public void OnRandomCardPriceItem()
    {
        if (randomCards == null)
            return;

        for (int i = 0; i < randomCards.Length; ++i)
        {
            randomCards[i].OnRandomCardPriceItem();
        }
    }

    public void OffRandomCardItem()
    {
        if(randomCards == null)
        {
            randomCards = GetComponentsInChildren<RandomCardItem>();
        }

        for (int i = 0; i < randomCards.Length; ++i)
        {
            randomCards[i].gameObject.SetActive(false);            
        }
    }

    public void OnRandomCardItem()
    {
        if (randomCards == null)
        {
            randomCards = GetComponentsInChildren<RandomCardItem>();
        }

        for (int i = 0; i < randomCards.Length; ++i)
        {
            randomCards[i].gameObject.SetActive(true);
        }
    }

    public void RefreshRandomItem(List<string> itemList, List<int> randomPrice, bool isCreate = false, bool isAnimPlay = true)
    {
        if(IsCreated == false || isCreate)
        {
          //  transform.position = new Vector3(originXPosition, transform.position.y, transform.position.z);
            IsCreated = true;
        }

        if (randomCards != null)
        {
            for (int i = 0; i < randomCards.Length; ++i)
            {
               // if (UserDataManager.instance.HangarItemBuyList.Contains(i))
               // {
               //     randomCards[i].MeshRenderer.enabled = false;
               //     randomCards[i].OffRandomCardPriceItem();
               // }
               // else
               // {
               //     randomCards[i].MeshRenderer.enabled = true;
               //     randomCards[i].ChangeMaterial(itemList[i], randomPrice[i]);
               // }
               //
               // randomCards[i].SelectCard.SetActive(false);
            }
        }

        if (isAnimPlay)
        {
          // transform.position = new Vector3(originXPosition, transform.position.y, transform.position.z);
          //
          // GameObject.Find("Random_Shop_Table").GetComponent<Animation>().Play();
          // GameObject.Find("Random_Shop_Table").GetComponent<RandomShopTable>().endAction = () =>
          // {
          //     for (int i = 0; i < randomCards.Length; ++i)
          //     {
          //         OnRandomCardItem();
          //         if (!UserDataManager.instance.HangarItemBuyList.Contains(i))
          //             randomCards[i].OnRandomCardPriceItem();
          //
          //         StageManager.instance.canvasManager.GetUIManager<UIManager_Hangar>().IsShowParent(true);
          //     }
          // };
        }
    }
}
