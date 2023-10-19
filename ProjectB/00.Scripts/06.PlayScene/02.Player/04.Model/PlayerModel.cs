using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationType
{
    public const string TELEPORT = "Teleport";
    public const string PC_APPEAR_LANDING = "PC_Appear_Landing";
    public const string HIT = "Damage";
}

public enum PlayerAnimationWeight
{
    Default,
    HpReduce,
    DodgeMove,
    PutWeapon,
    DashAttack,
    Skill,
    Bound,
    MoveStage,
    FinishBlow,
    HpExhausted
}

public enum PlayerIdleType
{
    Weapon_On,
    Weapon_Off_01,
    Weapon_Off_02,
}

public enum PlayerWeaponSocket
{
    BaseWeaponSocket,
    RealWeaponSocket,
    Sheath
}

public class PlayerModel : Model
{
    private PlayerIdleType currentPlayerIdleType = PlayerIdleType.Weapon_On;

    public Transform weaponSocket_BaseWeapon;
    public Transform weaponSocket_RealWeapon;
    public Transform weaponSocket_Sheath;

    //public GameObject hairMeshObj;
    //public GameObject skinObj;

    //public void SetHairMesh(GameObject hairMesh)
    //{
    //    if (hairMeshObj == null || hairMeshObj.name == hairMesh.name)
    //        return;
        
    //    GameObject preSkinnedMesh = hairMeshObj;
    //    hairMeshObj = GameObject.Instantiate(hairMesh, transform);
    //    hairMeshObj.name = Logic.DeleteCloneText(hairMesh.name);

    //    GameObject hairBoneObj = skinObj.GetComponent<GetHairBoneObject>().GetHairBoneObj();
    //    hairMeshObj.GetComponent<ResetBoneMap>().SetOriginalBoneObj(hairBoneObj);

    //    meshes[1] = hairMeshObj.GetComponentInChildren<SkinnedMeshRenderer>();
    //    Destroy(preSkinnedMesh);
    //}
    //public void SetBodyMesh(GameObject bodyObject)
    //{
    //    if (meshes[0] == null)
    //        return;

    //    GameObject preSkinMesh = skinObj;

    //    skinObj = GameObject.Instantiate(bodyObject, transform);
    //    animationControl = skinObj.GetComponent<AnimationControl>();
    //    meshes[0] = skinObj.GetComponentInChildren<SkinnedMeshRenderer>();

    //    GameObject hairBoneObj = skinObj.GetComponent<GetHairBoneObject>().GetHairBoneObj();
    //    hairMeshObj.GetComponent<ResetBoneMap>().SetOriginalBoneObj(hairBoneObj);        

    //    Destroy(preSkinMesh.gameObject);
    //}

    //public void SetHairColorMaterial(Material haireMaterial)
    //{
    //    if (hairMeshObj == null)
    //        return;

    //    hairMeshObj.GetComponentInChildren<SkinnedMeshRenderer>().material = haireMaterial;
    //}

    //public void SetFaceColorMaterial(Material faceMaterial)
    //{
    //    if(meshes[0] == null)
    //        return;

    //    SkinnedMeshRenderer skinnedMesh = meshes[0];
    //    Material[] mats = skinnedMesh.materials;

    //    mats[1] = faceMaterial;

    //    skinnedMesh.materials = mats;

    //}

    public Transform GetWeaponSocketTransform(PlayerWeaponSocket playerWeaponSocket)
    {
        switch (playerWeaponSocket)
        {
            case PlayerWeaponSocket.BaseWeaponSocket:
                return weaponSocket_BaseWeapon;
            case PlayerWeaponSocket.RealWeaponSocket:
                return weaponSocket_RealWeapon;
            case PlayerWeaponSocket.Sheath:
                return weaponSocket_Sheath;
        }

        return weaponSocket_RealWeapon;
    }

    #region 애니메이션

    public void SetCurrentPlayerIdleType(PlayerIdleType playerIdleType)
    {
        currentPlayerIdleType = playerIdleType;
    }

    public void PlayCurrentSetIdleAnimation()
    {
        PlayIdleAnimation(currentPlayerIdleType);
    }

    public void PlayIdleAnimation(PlayerIdleType playerIdleType)
    {
        SetCurrentPlayerIdleType(playerIdleType);

        string animationName = string.Empty;

        switch (playerIdleType)
        {
            case PlayerIdleType.Weapon_On:
                animationName = "Idle01";
                break;
            case PlayerIdleType.Weapon_Off_01:
                animationName = "Idle02";
                break;
            case PlayerIdleType.Weapon_Off_02:
                animationName = "Idle03";
                break;
        }

        animationControl.PlayAnimationCrossFade(animationName, fadeLength: 0.15f, isSameAniAvailablePlay: false, isRepeat: true);
    }

    public void PlayDieAnimation(Action OnEnd)
    {
        animationControl.PlayAnimation("Die", weight: (int)PlayerAnimationWeight.HpExhausted, OnAnimationEnd: OnEnd);
    }

    public void PlayHitAnimation(Action OnEnd = null)
    {
        animationControl.PlayAnimation(PlayerAnimationType.HIT, weight: (int)PlayerAnimationWeight.HpReduce, OnAnimationEnd: OnEnd);
    }

    public void PlayRunAnimation(float fadeLength = 0, bool isRepeat = true)
    {
        animationControl.PlayAnimationCrossFade("Run", fadeLength: fadeLength, isSameAniAvailablePlay: false, isRepeat: true);
    }

    public void PlayRunToDash(Action<int> OnFrame, float fadeLength = 0)
    {
        animationControl.PlayAnimationCrossFade("Run", fadeLength: fadeLength, isSameAniAvailablePlay: false, weight: (int)PlayerAnimationWeight.PutWeapon, OnAnimationFrame: OnFrame);
    }

    public void PlayDashAnimation(float fadeLength = 0, bool isRepeat = true)
    {
        animationControl.PlayAnimationCrossFade("Dash", fadeLength: fadeLength, isSameAniAvailablePlay: false, isRepeat: true);
    }

    public void PlayDashToTargetAnimation(float fadeLength = 0, bool isRepeat = true)
    {
        animationControl.PlayAnimationCrossFade("Dash", fadeLength: fadeLength, isSameAniAvailablePlay: false, isRepeat: true);
    }

    public void PlayTeleportAnimation(float speed = 0.5f, Action<int> OnFrame = null)
    {
        animationControl.PlayAnimationCrossFade(PlayerAnimationType.TELEPORT, speed: speed, weight: (int)PlayerAnimationWeight.MoveStage, OnAnimationFrame: OnFrame);
    }

    public void ResetAnimationState()
    {
        animationControl.ResetAnimationState();
    }

    #endregion
}
