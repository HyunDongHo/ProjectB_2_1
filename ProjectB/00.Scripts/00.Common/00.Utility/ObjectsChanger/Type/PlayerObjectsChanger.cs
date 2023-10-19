using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public enum PlayerChangeTag
//{
//    BeforeTransform,
//    AfterTransform
//}

public enum PlayerChangeTag
{
    Upgrade_01,
    Upgrade_02,
    Upgrade_03,
    Upgrade_04
}

public class PlayerObjectsChanger : ObjectsChanger<PlayerChangeTag>
{
}
