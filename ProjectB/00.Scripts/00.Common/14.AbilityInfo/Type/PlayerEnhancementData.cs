using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New PlayerEnhancementData", menuName = "Custom/PlayerEnhancementData")]
public class PlayerEnhancementData : ScriptableObject
{
    public string statName;
    public float maxLevel = 0;
    public float needCount = 0;
    public float needCountRatio = 0;

    public string statUIName;  

}
