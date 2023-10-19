using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model : MonoBehaviour
{
    public Transform bodyOffset;
    public Transform headOffset;
    public Transform ProjectileOffset;

    [Space]

    public List<SkinnedMeshRenderer> meshes;
    public AnimationControl animationControl;
}
