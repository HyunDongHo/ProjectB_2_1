using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Sound Data", menuName = "Custom/Sound Data")]
public class SoundData : ScriptableObject
{
    public SoundType soundType;
    public AudioClip audioClip;

    [Space]

    public bool isLoop = false;
    public bool is3DSound = false;
}
