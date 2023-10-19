using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveArea : MonoBehaviour
{
    [SerializeField] private Transform[] moveAreaTransform;

    public bool CheckMove(int moveArea)
    {
        return moveArea >= 0 && moveArea < moveAreaTransform.Length;
    }

    public Vector3 GetMoveAreaPosition(int index)
    {
        return moveAreaTransform[index].position;
    }
}
