using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomShopTable : MonoBehaviour
{
    //LobbyScene lobbyScene = null;

    public Action endAction = null;

    public void EndOpenAnim()
    {
        endAction?.Invoke();
    }
}
