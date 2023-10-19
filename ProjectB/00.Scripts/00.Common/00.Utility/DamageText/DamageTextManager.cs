using DamageNumbersPro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

public class DamageTextManager : Singleton<DamageTextManager>
{
    [SerializeField]
    DamageNumber PlayerNormalDamage;

    [SerializeField]
    DamageNumber NormalDamage;

    [SerializeField]
    DamageNumber CriticalDamage;

    [SerializeField]
    DamageNumber MissDamage;

    [SerializeField]
    DamageNumber DoubleDamage;

    public bool IsDamageOption = true;

    public void CreatePlayerNormalDamage(double damage, Vector3 position)
    {
        CreateNumberText(PlayerNormalDamage, damage.ToNumberStringCount(), position);
    }

    public void CreateNormalDamage(float damage, Vector3 position)
    {
        CreateNumber(NormalDamage, damage, position);
    }

    public void CreateNormalDamage(long damage, Vector3 position)
    {
        CreateNumberText(NormalDamage, damage.ToNumberStringCount(), position);
    }

    public void CreateCriticalDamage(long damage, Vector3 position)
    {
        CreateNumberText(CriticalDamage, damage.ToNumberStringCount(), position);
    }

    public void CreateNormalDamage(double damage, Vector3 position)
    {
        CreateNumberText(NormalDamage, damage.ToNumberStringCount(), position);
    }

    public void CreateMiss(Vector3 position)
    {
        CreateNumber(MissDamage, 0, position);
    }

    public void CreateNumber(DamageNumber numberPrefab, float number, Vector3 position)
    {
        if (IsDamageOption == false)
            return;

        DamageNumber newDamageNumber = numberPrefab.Spawn(position, number);
        //  DamageNumber damageNumber = numberPrefab.CreateNew(number, position);
        //  numberPrefab.
    }

    public void CreateNumberText(DamageNumber numberPrefab, string text, Vector3 position)
    {
        if (IsDamageOption == false)
            return;

        //float number = 0.1f;

        DamageNumber newDamageNumber = numberPrefab.Spawn(position, text);
        //newDamageNumber.
        // DamageNumber damageNumber = numberPrefab.CreateNew(Mathf.RoundToInt(number), position);
        // damageNumber.prefix = text;
    }


}

