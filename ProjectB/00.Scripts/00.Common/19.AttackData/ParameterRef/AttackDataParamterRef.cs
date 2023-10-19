using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Create Attack Data Parameter", menuName = "Custom/Attack Data Parameter")]
public class AttackDataParamterRef : ScriptableObject
{
    public List<string> methodDefines = new List<string>();
}
