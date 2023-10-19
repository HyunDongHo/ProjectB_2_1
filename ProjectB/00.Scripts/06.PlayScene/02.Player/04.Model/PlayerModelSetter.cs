using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModelSetter : MonoBehaviour
{
    private PlayerControl playerControl;

    public Action OnModelChanged;

    public PlayerObjectsChanger playerObjectsChanger;

    public GameObject beforeTransformPrefab;
    public GameObject afterTransformPrefab;

    public List<GameObject> createModelPrefabs;
    public GameObject[] playerModelPrefabs;

    public Transform beforeTransformParent;
    public Transform afterTransformParent;

    public GameObject model01TransformPrefab;
    public GameObject model02TransformPrefab;
    public GameObject model03TransformPrefab;
    public GameObject model04TransformPrefab;

    public Transform model01TransformParent;
    public Transform model02TransformParent;
    public Transform model03TransformParent;
    public Transform model04TransformParent;

    public void Init(PlayerControl playerControl)
    {
        this.playerControl = playerControl;

        if (playerControl as PlayerControl_Lobby)
            return;

        SetBaseModel();

        PlayerAttack.PlayerType playerType = playerControl.GetAttack<PlayerAttack>().playerModelType;
        int nowLevel = 0;

        switch (playerType)
        {
            case PlayerAttack.PlayerType.Warrior:
                nowLevel = StaticManager.Backend.GameData.PlayerGameData.WarriorUpgradeLevel;
                break;
            case PlayerAttack.PlayerType.Archer:
                nowLevel = StaticManager.Backend.GameData.PlayerGameData.ArcherUpgradeLevel;
                break;
            case PlayerAttack.PlayerType.Wizard:
                nowLevel = StaticManager.Backend.GameData.PlayerGameData.WizardUpgradeLevel;
                break;
        }

       // ChangeModel(playerObjectsChanger.objectTag);
        ChangeModel((PlayerChangeTag)nowLevel - 1);
    }

    public void SetBaseModel()
    {
        for (int i = 0; i < beforeTransformParent.childCount; i++)
            Destroy(beforeTransformParent.GetChild(i).gameObject);  

        for (int i = 0; i < afterTransformParent.childCount; i++)
            Destroy(afterTransformParent.GetChild(i).gameObject);

     //   GameObject beforeTransformObject = Instantiate(beforeTransformPrefab, beforeTransformParent);
     //   beforeTransformObject.transform.localPosition = Vector3.zero;
     //
     //   GameObject afterTransformObject = Instantiate(afterTransformPrefab, afterTransformParent);
     //   afterTransformObject.transform.localPosition = Vector3.zero;


        GameObject model01TransformObj = Instantiate(model01TransformPrefab, model01TransformParent);
        model01TransformObj.transform.localPosition = Vector3.zero;

        GameObject model02TransformObj = Instantiate(model02TransformPrefab, model02TransformParent);
        model02TransformObj.transform.localPosition = Vector3.zero;

        GameObject model03TransformObj = Instantiate(model03TransformPrefab, model03TransformParent);
        model03TransformObj.transform.localPosition = Vector3.zero;

        GameObject model04TransformObj = Instantiate(model04TransformPrefab, model04TransformParent);
        model04TransformObj.transform.localPosition = Vector3.zero;

      //  for (int i=0;  i < playerModelPrefabs.Length; ++i)
      //  {
      //      GameObject obj = Instantiate(beforeTransformPrefab, beforeTransformParent);
      //      obj.transform.localPosition = Vector3.zero;
      //      obj.gameObject.SetActive(false);
      //      createModelPrefabs.Add(obj);
      //  }

        PlayerAttack.PlayerType playerType = playerControl.GetAttack<PlayerAttack>().playerModelType;
        int nowLevel = 0;

        switch(playerType)
        {
            case PlayerAttack.PlayerType.Warrior:
                nowLevel = StaticManager.Backend.GameData.PlayerGameData.WarriorUpgradeLevel;
                break;
            case PlayerAttack.PlayerType.Archer:
                nowLevel = StaticManager.Backend.GameData.PlayerGameData.ArcherUpgradeLevel;
                break;
            case PlayerAttack.PlayerType.Wizard:
                nowLevel = StaticManager.Backend.GameData.PlayerGameData.WizardUpgradeLevel;
                break;
        }

        playerObjectsChanger.objectDetails = new ObjectDetail[4];

        playerObjectsChanger.objectDetails[0] = new ObjectDetail();
        playerObjectsChanger.objectDetails[0].objectModel = model01TransformObj;
        playerObjectsChanger.objectDetails[0].objectTag = PlayerChangeTag.Upgrade_01.ToString();

        playerObjectsChanger.objectDetails[1] = new ObjectDetail();
        playerObjectsChanger.objectDetails[1].objectModel = model02TransformObj;
        playerObjectsChanger.objectDetails[1].objectTag = PlayerChangeTag.Upgrade_02.ToString();

        playerObjectsChanger.objectDetails[2] = new ObjectDetail();
        playerObjectsChanger.objectDetails[2].objectModel = model03TransformObj;
        playerObjectsChanger.objectDetails[2].objectTag = PlayerChangeTag.Upgrade_03.ToString();

        playerObjectsChanger.objectDetails[3] = new ObjectDetail();
        playerObjectsChanger.objectDetails[3].objectModel = model04TransformObj;
        playerObjectsChanger.objectDetails[3].objectTag = PlayerChangeTag.Upgrade_04.ToString();


        playerControl.ChangeModel(playerObjectsChanger.ChangeModel((PlayerChangeTag)nowLevel - 1)[DefineManager.OBJECT_CHANGER_SELECTED].GetComponent<PlayerModel>());

       // ChangeModel((PlayerChangeTag)nowLevel - 1);
        //playerControl.ChangeModel(playerObjectsChanger.ChangeModel(playerObjectsChanger.objectTag)[1].GetComponent<PlayerModel>());
    }

    public void ChangeModel(PlayerChangeTag playerChangeTag)
    {
        AnimationControl beforeAnimationControl = playerControl.GetModel<PlayerModel>().animationControl;

        AnimationControlState animationControlState = beforeAnimationControl?.GetAnimationControlState();
        AnimationState animationState = animationControlState?.GetAnimationState();

        float animationNormalizedTime = animationState == null ? 0.0f : animationState.normalizedTime;
        float animationSpeed = animationState == null ? 0.0f : animationState.speed;

        playerControl.ChangeModel(playerObjectsChanger.ChangeModel(playerChangeTag)[DefineManager.OBJECT_CHANGER_SELECTED].GetComponent<PlayerModel>());

        if (animationControlState?.currentClip != null && animationState != null)
        {
            AnimationControl afterAnimationControl = playerControl.GetModel<PlayerModel>().animationControl;

            afterAnimationControl?.ResetAnimationState();

            afterAnimationControl.PlayAnimation(
                clip: animationControlState.currentClip,
                startNormalizedTime: animationNormalizedTime,
                speed: animationSpeed,
                isRepeat: animationControlState.isCurrentClipRepeat, 
                weight: animationControlState.currentClipWeight);
        }

        beforeAnimationControl?.ResetAnimationState();

        OnModelChanged?.Invoke();
    }
}
