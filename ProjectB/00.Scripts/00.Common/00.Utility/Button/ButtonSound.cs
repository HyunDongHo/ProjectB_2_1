using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSound : MonoBehaviour
{
    private void Start()
    {
        transform.GetComponent<Button>().onClick.AddListener(() => SoundManager.instance.PlaySound("Button_01"));
    }
}
