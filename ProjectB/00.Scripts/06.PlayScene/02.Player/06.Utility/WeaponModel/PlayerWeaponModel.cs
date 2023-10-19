using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerWeaponModelType
{
    Base,
    Model
}

public class PlayerWeaponModel : MonoBehaviour
{
    private PlayerControl playerControl;

    public GameObject baseWeaponPrefab;
    private GameObject createdBaseWeapon;

    private GameObject baseWeaponModel;
    private Transform baseWeaponTarget;

    private GameObject weaponModel;
    private Transform weaponModelTarget;

    private PlayerWeaponModelType currentPlayerWeaponModel;

    public bool isScaleChanged = false;

    private void LateUpdate()
    {
        FollowModelToTarget(baseWeaponModel, baseWeaponTarget);
        FollowModelToTarget(weaponModel, weaponModelTarget);
    }

    public void Init(PlayerControl playerControl)
    {
        this.playerControl = playerControl;

        AddEvent();

        return;

     //   CreateBaseWeapon();
        InitModelWeapon();
    }

    public void Release()
    {
        RemoveEvent();
    }

    private void AddEvent()
    {
     //   playerControl.utility.equipment.equipment_Weapon.OnSetEquipment += HandleOnSetEquipment;
    }

    private void RemoveEvent()
    {
      //  playerControl.utility.equipment.equipment_Weapon.OnSetEquipment -= HandleOnSetEquipment;
    }

    //private void HandleOnSetEquipment(EquipmentItem equipmentItem, bool isEquip)
    //{
    //    if (isEquip)
    //    {
    //        if (equipmentItem?.GetData<WeaponItemData>()?.weaponPrefab != null)
    //        {
    //            baseWeaponModel = createdBaseWeapon;
    //            weaponModel = ObjectPoolManager.instance.CreateObject(equipmentItem.GetData<WeaponItemData>().weaponPrefab, transform);

    //            SetCurrentWeaponModelType();
    //        }
    //    }
    //    else
    //    {
    //        SetAllWeaponModelActive(false);

    //        ObjectPoolManager.instance.RemoveObject(weaponModel);

    //        baseWeaponModel = null;
    //        weaponModel = null;
    //    }
    //}

    private void CreateBaseWeapon()
    {
        createdBaseWeapon = ObjectPoolManager.instance.CreateObject(baseWeaponPrefab, transform);

        baseWeaponModel = createdBaseWeapon;
        baseWeaponTarget = playerControl.GetModel<PlayerModel>()?.GetWeaponSocketTransform(PlayerWeaponSocket.BaseWeaponSocket);
    }

    private void InitModelWeapon()
    {
        weaponModelTarget = playerControl.GetModel<PlayerModel>()?.GetWeaponSocketTransform(PlayerWeaponSocket.RealWeaponSocket);
    }

    public Transform GetCurrentActiveWeaponSocket()
    {
        switch (currentPlayerWeaponModel)
        {
            case PlayerWeaponModelType.Base:
                return playerControl?.GetModel<PlayerModel>()?.weaponSocket_BaseWeapon;
            case PlayerWeaponModelType.Model:
                return playerControl?.GetModel<PlayerModel>()?.weaponSocket_RealWeapon;
            default:
                return playerControl?.GetModel<PlayerModel>()?.weaponSocket_RealWeapon;
        }
    }

    public void SetWeaponModelType(PlayerWeaponModelType weaponModelType)
    {
        currentPlayerWeaponModel = weaponModelType;

        SetCurrentWeaponModelType();
    }

    public void SetCurrentWeaponModelType()
    {
        SetAllWeaponModelActive(false);

        switch (currentPlayerWeaponModel)
        {
            case PlayerWeaponModelType.Base:
                // 장비가 장착되어있지 않으면 Base Model도 사라져야 하기 때문에 장비 장착 여부에 따라서 Active 결정.
                SetBaseWeaponActive(true);
                break;
            case PlayerWeaponModelType.Model:
                SetWeaponActive(true);
                break;
        }
    }

    public void SetAllWeaponModelActive(bool isActive)
    {
        SetBaseWeaponActive(isActive);
        SetWeaponActive(isActive);
    }

    private void SetBaseWeaponActive(bool isActive)
    {
        if (baseWeaponModel != null)
        {
            baseWeaponModel.SetActive(isActive);
        }
    }

    private void SetWeaponActive(bool isActive)
    {
        if (weaponModel != null)
        {
            weaponModel.SetActive(isActive);
        }
    }

    public void ChangeWeaponToCurrentPlayerModel()
    {
        baseWeaponTarget = playerControl?.GetModel<PlayerModel>()?.GetWeaponSocketTransform(PlayerWeaponSocket.RealWeaponSocket);
        weaponModelTarget = playerControl?.GetModel<PlayerModel>()?.GetWeaponSocketTransform(PlayerWeaponSocket.BaseWeaponSocket);
    }

    public bool GetBaseWeaponActive()
    {
        bool isActive = false;
        if (baseWeaponModel != null)
            isActive = baseWeaponModel.activeSelf;

        return isActive;
    }

    public bool GetWeaponActive()
    {
        bool isActive = false;
        if (weaponModel != null)
            isActive = weaponModel.activeSelf;

        return isActive;
    }

    private void FollowModelToTarget(GameObject model, Transform target)
    {
        if (model != null)
        {
            if (target != null)
            {
                model.transform.position = target.transform.position;
                model.transform.rotation = target.transform.rotation;

                if (isScaleChanged)
                    model.transform.localScale = target.transform.localScale;
            }
            else
            {
                SetAllWeaponModelActive(false);
            }
        }
    }
}
