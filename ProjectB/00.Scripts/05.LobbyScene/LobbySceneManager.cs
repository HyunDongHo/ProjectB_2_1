using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.Video;
using BackEnd;
using Data;

public class LobbySceneManager : StageManager
{
    public enum LobbyLocation
    {
        Left,
        Center,
        Right,
        Max,
    }
    public LobbyLocation lobbyLocation = LobbyLocation.Center;

    [Space]
    public SelectRandomCard nowSelectRandomCard;
    public RandomCardTable randomCardTable;
    [Space]

    public Button leftMove;
    public Button rightMove;
    public Button startButton;
    public Button randomItemBuyButton;
    public Button randomItemRefreshButton;    

    LobbyCamera lobbyCamera = null;
    public int HargarSelectItemIndex = -1;

    private LobbyState nowLobbyState = LobbyState.Center;

    public LobbyPlayerPositionCreate lobbyPlayerPos;
    public LobbyState NowLobbyState
    {
        get { return nowLobbyState; }
        set
        {
            SetLobbyCamera(value);
        }
    }
    protected override void Start()
    {
        base.Start();
        CheckIsFirst();
   

        /* 내가 추가한 코드 */
        (playerControl as PlayerControl_Lobby).playersControl = PlayersControlManager.instance.playersContol;
        List<Vector3> lobbyAllPlayerPos = lobbyPlayerPos.GetCreatePositions();
        (playerControl as PlayerControl_Lobby).SetPlayerModel(3, lobbyAllPlayerPos);      
        (playerControl as PlayerControl_Lobby).StartLobby();    

        SoundManager.instance.PlaySound("Lobby_BGM_01");
     //   playerControl.utility.belongings.GetInventory(PlayerInventoryType.Etc).Add(Resources.Load("Enchant_Stone") as ItemData, 10);
       
        // QuestDataEventManager.instance.PlayerMoveScene("Lobby");
        
        lobbyCamera = GameObject.Find("CenterCamera").GetComponent<LobbyCamera>();
        if(lobbyCamera != null)
        { 
            Debug.Log("Lobby camera null");    
        }  
        //GameObject randomShopTableObj = GameObject.Find("Random_Shop_Table");
        //randomShopTableObj.GetComponent<RandomShopTable>().endAction = () =>
        //{

        //};

    }

    protected override void AddEvent()
    {
        base.AddEvent();

        leftMove.onClick.AddListener(PushLobbyBackButton);
        rightMove.onClick.AddListener(PushLobbyNextButton);
        startButton.onClick.AddListener(HandleOnGameStart);
        randomItemRefreshButton.onClick.AddListener(PushRefreshButton);
        randomItemBuyButton.onClick.AddListener(SelectCardBuy);
    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();

        leftMove.onClick.RemoveListener(PushLobbyBackButton);
        rightMove.onClick.RemoveListener(PushLobbyNextButton);
        startButton.onClick.RemoveListener(HandleOnGameStart); 
        randomItemRefreshButton.onClick.RemoveListener(PushRefreshButton); 
        randomItemBuyButton.onClick.RemoveListener(SelectCardBuy);
    }

    private void HandleOnGameStart()
    {
        RemoveEvent();
     //   SceneSettingManager.instance.LoadDefaultStageScene();
        //SetLobbyCamera(LobbyState.Center, () =>
        //{
        //    lobbyLocation = LobbyLocation.Center;
        ////    ChangedLobbyLocation();

        //    SceneSettingManager.instance.LoadDefaultStageScene();  
        //});
    }

    private void HandleOnLeftMoveClicked()
    {
        if((int)lobbyLocation - 1 >= 0)
        {
            lobbyLocation--;

            ChangedLobbyLocation();
        }
    }

    private void HandleOnRightMoveClicked()
    {
        if ((int)lobbyLocation + 1 < (int)LobbyLocation.Max)
        {
            lobbyLocation++;

            ChangedLobbyLocation();
        }
    }

    private void ChangedLobbyLocation()
    {
        switch (lobbyLocation)
        {
            case LobbyLocation.Center:
                cameraManager.SetCamera(CameraChangeTag.Main);
                break;
            case LobbyLocation.Left:
                cameraManager.SetCamera(CameraChangeTag.Sub_1);
                break;
            case LobbyLocation.Right:
                cameraManager.SetCamera(CameraChangeTag.Sub_2);
                break;
        }
    }
    public void SetLobbyCamera(LobbyState lobbyState, Action addAction = null)
    {
      //  randomCardTable.OffRandomCardPriceItem();
      //  StageManager.instance.canvasManager?.SetAllUIManagersCanvasActive(false);

        switch (lobbyState)
        {
            case LobbyState.Center:
                lobbyCamera.AnimPlay(LobbyState.Center, () =>
                {
                  //  StageManager.instance.canvasManager?.SetAllUIManagersCanvasActive(true);
                    nowLobbyState = lobbyState; 
                  //  SetSelectCard(-1);
                    startButton.gameObject.SetActive(true);
                   // StageManager.instance.canvasManager.GetUIManager<UIManager_Hangar>().IsShowParent(false);
                    addAction?.Invoke();
                });
                break;
            case LobbyState.Left:
                lobbyCamera.AnimPlay(LobbyState.Left, () =>
                {
                    StageManager.instance.canvasManager?.SetAllUIManagersCanvasActive(true);
                    startButton.gameObject.SetActive(false);
                    nowLobbyState = lobbyState;
                });
                break;
            case LobbyState.Right:
                lobbyCamera.AnimPlay(LobbyState.Right, () => {
                    StageManager.instance.canvasManager?.SetAllUIManagersCanvasActive(true); 
                    startButton.gameObject.SetActive(false);
                    nowLobbyState = lobbyState;                    

                    if (UserDataManager.instance.RandomItemRefreshTime <= 0)
                    {
                        RefreshHangarList();
                    }
                    else
                    {
                        GameObject randomCardAllObj = GameObject.Find("Random_Card_All");
                        //randomCardAllObj.GetComponent<RandomCardTable>().RefreshRandomItem(UserDataManager.instance.HangarItemList, UserDataManager.instance.HangarItemNeedList, true);
                    }
                });

                break;
        }
    }

    public void PushLobbyBackButton()
    {
        if (NowLobbyState == LobbyState.Left)
            return;

        NowLobbyState -= 1;
        //OffLobbyUI();
    }
    public void PushLobbyNextButton()
    {
        if (NowLobbyState == LobbyState.Right)
            return;

        NowLobbyState += 1;
       // OffLobbyUI();
    }

    private void CheckIsFirst()
    {
        if (UserDataManager.instance.GetIsFirstLobby())
        {
            SoundManager.instance.PlaySound("Lobby_Start");
            QuestManager.instance.AddQuest("Main_Quest_1");

            playerControl.GetStats<PlayerStats>().hp.SetHpToMax();

            playerControl.GetStats<PlayerStats>().sp.SetSpToMax();

            UserDataManager.instance.UpdateIsFirstLobby();
        }
    }

    public void EndLobbyScene(Action OnCompleteLobbyScene)
    {
        Logic.CloseAllWindow();

        FindObjectOfType<EventSystem>().enabled = false;
        InputManager.instance.SetIsAvaliableInput(false);
        float returnTime = 0.25f;

        Sequence sequence = DOTween.Sequence();

       // StageManager.instance.canvasManager.GetUIManager<UIManager_Hangar>().IsShowParent(false);
       // sequence.Join(CameraManager.mainCamera.transform.DOMove(CameraManager.mainCamera.GetComponent<LobbyCamera>().playerLook_Min.position, returnTime));
       // sequence.Join(CameraManager.mainCamera.transform.DORotate(CameraManager.mainCamera.GetComponent<LobbyCamera>().playerLook_Min.eulerAngles, returnTime));
       // sequence.Join(playerControl.transform.DOLocalRotate(new Vector3(0, 180, 0), returnTime));
       //
        sequence.OnComplete(() =>
            {
                (playerControl as PlayerControl_Lobby).EndLobby(OnCompleteLobby: () =>
                {
                    PlayersControlManager.instance.ResetAllPlayerState();
                    PlayersControlManager.instance.ResetRotationAllPlayer();
                    PlayersControlManager.instance.SetNotActiveAllPlayer();  

                    /* 위 for 문 추가함 */

                    FindObjectOfType<EventSystem>().enabled = true;
                    InputManager.instance.SetIsAvaliableInput(true);

                    OnCompleteLobbyScene?.Invoke();
                });
            });
    }

    #region RandomShop
    public void RefreshHangarList(bool isRubyRefresh = false)
    {
        //PriceViewTransform.gameObject.SetActive(false);
        SetSelectCard(-1);
        randomCardTable.OffRandomCardPriceItem();
        BackendReturnObject callback = Backend.Probability.GetProbabilitys("5206", 10);

        List<string> itemNameList = new List<string>();
        List<int> itemNeedCountList = new List<int>();

        if (callback.IsSuccess())
        {
            var json = callback.GetReturnValuetoJSON();
            var flatton = BackendReturnObject.Flatten(json[0]);

            for (int i = 0; i < flatton.Count; ++i)
            {
                var flattonJosn = BackendReturnObject.Flatten(flatton[i]);
                itemNameList.Add(flattonJosn[0].ToString());

                int minPrice = Int32.Parse(flattonJosn[4].ToString());
                int maxPrice = Int32.Parse(flattonJosn[5].ToString());

                int dividePrice = (maxPrice - minPrice) / 10;
                int priceRandom = UnityEngine.Random.Range(0, 11);
                int price = minPrice + (dividePrice * priceRandom);
                itemNeedCountList.Add(price);
            }

            DateTime NowTime = DateTime.Now;
            //UserDataManager.instance.HangarInitTime = NowTime.ToString();
            //UserDataManager.instance.HangarItemList = itemNameList;
            //UserDataManager.instance.HangarItemBuyList = new List<int>();
            //UserDataManager.instance.HangarItemNeedList = itemNeedCountList;
            //UserDataManager.instance.HangarRefreshCount = 0;
            UserDataManager.instance.RandomItemRefreshTime = Define.RandomShopRefreshTime;            

            UserDataManager.instance.SaveGameData((backendReturnObject) =>
            {                
                GameObject randomCardAllObj = GameObject.Find("Random_Card_All");
                //randomCardAllObj.GetComponent<RandomCardTable>().RefreshRandomItem(itemNameList, UserDataManager.instance.HangarItemNeedList, true);

                if (isRubyRefresh == true)
                {
                    PlayerWallet playerWallet = playerControl.utility.belongings.playerWallet;
                    playerWallet.AddRuby(-1000);
                }
            });
        }
        else
        {
            Debug.Log(callback.GetStatusCode());
            Debug.Log(callback.GetErrorCode());
        }
    }
    public void SetSelectCard(int index)
    {
        //if (UserDataManager.instance.HangarItemList.Count <= index)
        //    return;

        //if (UserDataManager.instance.HangarItemBuyList.Contains(index))
        //{
        //    nowSelectRandomCard.gameObject.SetActive(false);
        //    randomItemBuyButton.gameObject.SetActive(false);
        //    HargarSelectItemIndex = -1;
        //    return;
        //}

        if (index >= 0)
        {
            nowSelectRandomCard.gameObject.SetActive(true);
            nowSelectRandomCard.GetComponent<SelectRandomCard>().SelectIndex = index;
            //nowSelectRandomCard.GetComponent<SelectRandomCard>().ChangeMaterial(UserDataManager.instance.HangarItemList[index]);
            HargarSelectItemIndex = index;
            randomItemBuyButton.gameObject.SetActive(true);
        }
        else
        {
            nowSelectRandomCard.gameObject.SetActive(false);
            randomItemBuyButton.gameObject.SetActive(false);
            HargarSelectItemIndex = -1;
        }
    }

    public void PushRefreshButton()
    {
        PlayerWallet playerWallet = playerControl.utility.belongings.playerWallet;
        if (playerWallet.IsAvailiableRemoveRuby(1000))
        {
            RefreshHangarList(true);
        }
        else
        {
            PopupMessage popupMessage = PopupManager.instance.CreatePopup<PopupMessage>("PopupMessage").GetPopup();
            popupMessage.SetMessageText(DataManager.instance.GetText("NotEnoughItem"));

            popupMessage.okButtonAction = () =>
            {
                popupMessage.RemovePopup();
            };
            return;
        }
    }

    public void SelectCardBuy()
    {
        //  int selectItemNum = NowSelectCardObj.GetComponent<SelectCard>().SelectIndex;

        //if (UserDataManager.instance.HangarItemBuyList.Contains(HargarSelectItemIndex))
        //{
        //    AnnounceManager.instance.ShowAnnounce(DataManager.instance.GetText("AlreadyBuyItem"));
        //    return;
        //}
        //else
        //{
        //    ItemDataJson itemDataJson = null;
        //    DataManager.instance.ItemDict.TryGetValue(UserDataManager.instance.HangarItemList[HargarSelectItemIndex], out itemDataJson);

        //    if(itemDataJson == null)
        //    {
        //        AnnounceManager.instance.ShowAnnounce(DataManager.instance.GetText("NotNormalItem"));
        //        return;
        //    }

        //    ItemData itemData = ResourceManager.instance.Load<ItemData>(UserDataManager.instance.HangarItemList[HargarSelectItemIndex]);
        //    if (itemData == null)
        //    {
        //        AnnounceManager.instance.ShowAnnounce(DataManager.instance.GetText("NotNormalItem"));
        //        return;
        //    }

        //    PlayerWallet playerWallet = playerControl.utility.belongings.playerWallet;
        //    int needItemCount = UserDataManager.instance.HangarItemNeedList[HargarSelectItemIndex];

        //    switch (itemDataJson.rarity)
        //    {
        //        case "Common":
        //        case "Magic":
        //            if(playerWallet.IsAvailiableRemoveCoin(needItemCount))
        //            {
        //                playerWallet.AddGold(-needItemCount);
        //            }
        //            else
        //            {
        //                PopupMessage popupMessage = PopupManager.instance.CreatePopup<PopupMessage>("PopupMessage").GetPopup();
        //                popupMessage.SetMessageText(DataManager.instance.GetText("NotEnoughItem"));

        //                popupMessage.okButtonAction = () =>
        //                {
        //                    StageManager.instance.canvasManager.GetUIManager<UIManager_Common>().shopWindow.gameObject.GetComponentInChildren<OpenClose>().SetOpen();
        //                    popupMessage.RemovePopup();
        //                };
        //                return;
        //            }
        //            break;
        //        case "Rare":
        //        case "Unique":
        //        case "Legendry":
        //        case "Mythic":
        //            if (playerWallet.IsAvailiableRemoveRuby(needItemCount))
        //            {
        //                playerWallet.AddRuby(-needItemCount);
        //            }
        //            else
        //            {
        //                PopupMessage popupMessage = PopupManager.instance.CreatePopup<PopupMessage>("PopupMessage").GetPopup();
        //                popupMessage.SetMessageText(DataManager.instance.GetText("NotEnoughItem"));

        //                popupMessage.okButtonAction = () => 
        //                {
        //                    StageManager.instance.canvasManager.GetUIManager<UIManager_Common>().shopWindow.gameObject.GetComponentInChildren<OpenClose>().SetOpen();
        //                    popupMessage.RemovePopup();
        //                };
        //                return;
        //            }
        //            break;
        //    }

        //    StageManager.instance.playerControl.utility.belongings.GetInventory(itemData).Add(itemData);
        //    UserDataManager.instance.HangarItemBuyList.Add(HargarSelectItemIndex);
        //    HangarSetting(false);
        //}
    }
    //public void HangarSetting(bool isAnimPlay)
    //{
    //    if (UserDataManager.instance.HangarItemList.Count > 0)
    //    {
    //        GameObject randomCardAllObj = GameObject.Find("Random_Card_All");
    //        randomCardAllObj.GetComponent<RandomCardTable>().RefreshRandomItem(UserDataManager.instance.HangarItemList, UserDataManager.instance.HangarItemNeedList, false, isAnimPlay);
    //    }
    //    else
    //    {
    //        RefreshHangarList();
    //    }

    //    SetSelectCard(-1);
    //}
    #endregion



}
