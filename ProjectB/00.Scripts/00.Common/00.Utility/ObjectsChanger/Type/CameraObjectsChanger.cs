using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraChangeTag
{
    Main,
    Sub_1,
    Sub_2,
    Sub_3,
    Sub_4,
    Sub_5,
}

public class CameraObjectsChanger : ObjectsChanger<CameraChangeTag>
{
}
