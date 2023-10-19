using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_PlayerModelSetting : MonoBehaviour
{
    [SerializeField]
    List<GameObject> _playerModel;

    [SerializeField]
    PlayerType _playerType;

    public void OnModel(int index)
    {
        for (int i = 0; i < _playerModel.Count; ++i)
        {
            if (index == i)
                _playerModel[index].gameObject.SetActive(true);
            else
                _playerModel[i].gameObject.SetActive(false);
        }
    }

    public void OffModel()
    {
        for(int i=0; i < _playerModel.Count; ++i)
        {
            _playerModel[i].gameObject.SetActive(false);
        }
    }
}
