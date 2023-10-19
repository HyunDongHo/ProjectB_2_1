using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Criteria
{
    Horizontal,
    Vertical
}


public class RandomLotteryManager : MonoBehaviour
{
    public Transform itemHorizontalParent;
    public Transform itemVerticalParent;

    public GameObject itemCardPrefab;

    [System.Serializable]
    public class ItemFrontSpriteType
    {
        public Sprite common;
        public Sprite magic;
        public Sprite rare;
        public Sprite unique;
        public Sprite legendry;
        public Sprite mythic;
    }
    public ItemFrontSpriteType itemCardFrontSpriteType;

 //   protected List<ItemCard> itemCards = new List<ItemCard>();

    public Button closeButton;
    public Button allCheckButton;

    public virtual void Init()
    {
   //     AddButtonEvent();

      //  InitActive();
    }

    //protected virtual void AddButtonEvent()
    //{
    //    allCheckButton.onClick.AddListener(
    //        () =>
    //        {
    //            foreach (ItemCard itemCard in itemCards)
    //            {
    //                if (!itemCard.IsOpened())
    //                    itemCard.view.button.onClick.Invoke();
    //            }
    //        });

    //    closeButton.onClick.AddListener(
    //        () =>
    //        {
    //            GambleManager.instance.UnLoadGambleScene();
    //        });
    //}
    
    protected virtual void InitActive()
    {
        allCheckButton.gameObject.SetActive(true);
        closeButton.gameObject.SetActive(false);
    }

    //private void HandleOnItemCardClicked()
    //{
    //    foreach (ItemCard item in itemCards)
    //    {
    //        if (!item.IsOpened()) return;
    //    }

    //    OpenedAllCard();
    //}

    protected virtual void OpenedAllCard()
    {
        allCheckButton.gameObject.SetActive(false);
        closeButton.gameObject.SetActive(true);
    }

    //public void ShowRandomItem(ItemData[] itemDatas, int criteriaCount, Criteria criteria, TextAnchor anchor = TextAnchor.MiddleCenter)
    //{
    //    Transform parent = null;
    //    for (int i = 0; i < itemDatas.Length; i++)
    //    {
    //        if(i == 0)
    //        {
    //            parent = GetCriteriaParent(criteria, anchor);
    //        }
    //        else if (i % criteriaCount == 0)
    //        {
    //            parent = GetCriteriaParent(criteria, anchor);
    //        }

    //        GameObject itemCardObj = Instantiate(itemCardPrefab, parent);
    //        itemCardObj.GetComponent<RectTransform>().sizeDelta = GetCardSize(itemDatas[i]);

    //        ItemCard itemCard = itemCardObj.GetComponent<ItemCard>();

    //        Sprite itemCardFrontSprite = GetItemCardFrontSprite(itemDatas[i]);

    //        itemCard.Init(cardFrontSprite: itemCardFrontSprite, cardBackSprite: itemDatas[i].GetCardSprite());
    //        itemCard.view.button.onClick.AddListener(HandleOnItemCardClicked);

    //        itemCards.Add(itemCard);
    //    }
    //}

    //private Sprite GetItemCardFrontSprite(ItemData itemData)
    //{
    //    Sprite itemCardFrontSprite = itemCardFrontSpriteType.common;

    //    if (itemData as EquipmentItemData)
    //    {
    //        switch ((itemData as EquipmentItemData).rarity)
    //        {
    //            case EquipmentItemRarity.Magic:
    //                itemCardFrontSprite = itemCardFrontSpriteType.magic;
    //                break;
    //            case EquipmentItemRarity.Rare:
    //                itemCardFrontSprite = itemCardFrontSpriteType.rare;
    //                break;
    //            case EquipmentItemRarity.Unique:
    //                itemCardFrontSprite = itemCardFrontSpriteType.unique;
    //                break;
    //            case EquipmentItemRarity.Legendry:
    //                itemCardFrontSprite = itemCardFrontSpriteType.legendry;
    //                break;
    //            case EquipmentItemRarity.Mythic:
    //                itemCardFrontSprite = itemCardFrontSpriteType.mythic;
    //                break;
    //        }
    //    }

    //    return itemCardFrontSprite;
    //}

    //private Vector2 GetCardSize(ItemData itemData)
    //{
    //    Vector2 cardSize = new Vector2(172, 172);

    //    switch (itemData)
    //    {
    //        case WeaponItemData weaponItemData:
    //            cardSize = new Vector2(249.5f, 345.5f);
    //            break;
    //        case ProtectiveGearItemData protectiveGearItemData:
    //            cardSize = new Vector2(172, 172);
    //            break;
    //        case AccessaryItemData accessaryItemData:
    //            cardSize = new Vector2(172, 172);
    //            break;
    //        case PetItemData petItemData:
    //            cardSize = new Vector2(250, 300);
    //            break;
    //    }

    //    return cardSize;
    //}

    private Transform GetCriteriaParent(Criteria criteria, TextAnchor anchor)
    {
        GameObject criteriaObj = null;

        if (criteria == Criteria.Vertical)
        {
            criteriaObj = new GameObject("Vertical");

            criteriaObj.transform.SetParent(itemHorizontalParent);
            criteriaObj.transform.localScale = Vector3.one;
            criteriaObj.transform.localPosition = Vector3.zero;

            VerticalLayoutGroup verticalLayoutGroup = criteriaObj.AddComponent<VerticalLayoutGroup>();
            verticalLayoutGroup.childAlignment = anchor;
            verticalLayoutGroup.childControlHeight = false;
            verticalLayoutGroup.childControlWidth = false;
        }
        else if(criteria == Criteria.Horizontal)
        {
            criteriaObj = new GameObject("Horizontal");

            criteriaObj.transform.SetParent(itemVerticalParent);
            criteriaObj.transform.localScale = Vector3.one;
            criteriaObj.transform.localPosition = Vector3.zero;

            HorizontalLayoutGroup horizontalLayoutGroup = criteriaObj.AddComponent<HorizontalLayoutGroup>();
            horizontalLayoutGroup.childAlignment = anchor;
            horizontalLayoutGroup.childControlHeight = false;
            horizontalLayoutGroup.childControlWidth = false;
        }

        return criteriaObj.transform;
    }
}
