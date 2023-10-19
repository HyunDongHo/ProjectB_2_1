using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Create Resource Data", menuName = "Custom/Create Resource Data")]
public class CreateResourceData : ScriptableObject
{
    public GameObject createdObject = null;

    [Space]

    public bool isSetCreatorParent = false;

    public bool isRelativeOffset = false;

    [Space]

    public bool isLocalPosition = false;

    public Vector3 positionOffset = Vector3.zero;

    public bool isfirstPositionToTarget = false;
    public bool isFollowTargetPosition = false;

    [Space]

    public bool isLocalRotation = false;

    public Vector3 rotationOffset = Vector3.zero;

    public bool isfirstRotationToTarget = false;
    public bool isFollowTargetRotation = false;
}
