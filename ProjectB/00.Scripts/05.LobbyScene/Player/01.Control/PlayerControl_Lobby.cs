using Scheduler;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl_Lobby : PlayerControl
{
    public PlayerControl[] playersControl;  
    protected override void Start()
    {
        base.Start();
        //SetPlayerModel();
    }
    public void SetPlayerModel(int ModelsCount, List<Vector3> lobbyAllPlayerPos)  
    {
        for (int i = 0; i < ModelsCount; ++i)
        {
            if (playersControl[i] == null)    
                continue;

            //Debug.Log($"{i} x: {playersControl[i].gameObject.transform.eulerAngles.x}");
            //Debug.Log($"{i} y: {playersControl[i].gameObject.transform.eulerAngles.y}");
            //Debug.Log($"{i} z: {playersControl[i].gameObject.transform.eulerAngles.z}");  

            switch ((PlayerType)i)
            {
                case PlayerType.Warrior:
                    playersControl[i].gameObject.transform.position = lobbyAllPlayerPos[i];
                    playersControl[i].gameObject.transform.eulerAngles = new Vector3(0, 180, 0);
                    //playersControl[i].gameObject.transform.eulerAngles = new Vector3(0, -180, 0);
                    break;
                case PlayerType.Archer:
                    playersControl[i].gameObject.transform.position = lobbyAllPlayerPos[i];
                    playersControl[i].gameObject.transform.eulerAngles = new Vector3(0, 180, 0);
                    //playersControl[i].gameObject.transform.eulerAngles = new Vector3(0, -205.347f, 0);

                    break;
                case PlayerType.Wizard:
                    playersControl[i].gameObject.transform.position = lobbyAllPlayerPos[i];
                    playersControl[i].transform.eulerAngles = new Vector3(0, 180, 0);    
                    //playersControl[i].gameObject.transform.eulerAngles = new Vector3(0, -160.199f, 0);  
                    break;
            }
            playersControl[i].gameObject.SetActive(true);
        }
    }

    public void StartLobby(Action OnCompleteStartLobby = null)
    {
        //GetModel<PlayerModel>().animationControl.PlayAnimation("Idle01", isRepeat: true, OnAnimationEnd: () => OnCompleteStartLobby?.Invoke());

        for(int i=0;i< (int)PlayerType.None; i++)
        {
            PlayerModel model = playersControl[i].GetModel<PlayerModel>();
            if (model == null)
                continue;

            model.animationControl.PlayAnimation("Idle01", isRepeat: true, OnAnimationEnd: () => OnCompleteStartLobby?.Invoke());
        }

    }

    public void EndLobby(Action OnCompleteLobby = null)
    {
        //GetModel<PlayerModel>().animationControl.PlayAnimation("Idle01",
        //    OnAnimationFrame: (frame) =>
        //    {
        //        if(frame == 36)
        //        {
        //            utility.weaponModel.SetWeaponModelType(PlayerWeaponModelType.Model);
        //        }
        //    },
        //    OnAnimationEnd: () => OnCompleteLobby?.Invoke());
        for (int i = 0; i < (int)PlayerType.None; i++)
        {
            PlayerModel model = playersControl[i].GetModel<PlayerModel>();
            if (model == null)
                continue;

            model.animationControl.PlayAnimation("Idle01", OnAnimationEnd: () => OnCompleteLobby?.Invoke()) ;    
        }

    }

    //protected override void HandleOnWeaponEquipEnd(EquipmentItem item)
    //{
    //    base.HandleOnWeaponEquipEnd(item);

    //    utility.weaponModel.SetWeaponModelType(PlayerWeaponModelType.Base);
    //}
}
