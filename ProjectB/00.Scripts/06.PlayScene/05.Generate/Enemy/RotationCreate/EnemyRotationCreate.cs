using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRotationCreate : MonoBehaviour
{
    public virtual Quaternion GetRotation()
    {
        return Quaternion.identity;
    }
}
