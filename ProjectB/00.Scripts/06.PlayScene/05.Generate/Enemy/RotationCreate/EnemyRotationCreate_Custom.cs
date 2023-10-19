using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRotationCreate_Custom : EnemyRotationCreate
{
    public Vector3 eulerAngles;

    public override Quaternion GetRotation()
    {
        return Quaternion.Euler(eulerAngles.x, eulerAngles.y, eulerAngles.z);
    }
}
