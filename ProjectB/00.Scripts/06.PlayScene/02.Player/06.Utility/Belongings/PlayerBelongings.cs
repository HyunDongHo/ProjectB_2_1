using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerInventoryType
{
    Weapon,
    ProtectiveGear,
    Accessary,
    Etc,
    Pet,
    Max
}

public class PlayerBelongings : MonoBehaviour
{
    private PlayerControl playerControl;


    public PlayerWallet playerWallet;

    public void Init(PlayerControl playerControl)
    {
        this.playerControl = playerControl;
    }

}
