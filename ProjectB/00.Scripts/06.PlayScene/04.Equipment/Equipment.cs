using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Equipment : MonoBehaviour
{
    //public Action<EquipmentItem, bool> OnSetEquipment;

    //public EquipmentItem targetItem { get; protected set; }

    //public virtual void EquipEquipmenet(EquipmentItem item)
    //{
    //    if (targetItem != null)
    //    {
    //        UnEquipEquipment();
    //    }

    //    item.SetUseState(EquipmentUseState.Equip);

    //    item.OnRemoved += UnEquipEquipment;
    //    targetItem = item;

    //    OnSetEquipment?.Invoke(targetItem, true);
    //}

    //public virtual void UnEquipEquipment()
    //{
    //    if (targetItem != null)
    //    {
    //        targetItem.SetUseState(EquipmentUseState.Release);

    //        OnSetEquipment?.Invoke(targetItem, false);

    //        targetItem.OnRemoved -= UnEquipEquipment;
    //        targetItem = null;
    //    }
    //}
}
