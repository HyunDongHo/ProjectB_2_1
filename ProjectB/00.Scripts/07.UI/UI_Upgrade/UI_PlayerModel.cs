using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_PlayerModel : MonoBehaviour
{
    [SerializeField]
    List<UI_PlayerModelSetting> _playerModelSetters;

    public void ChangeModelSetter(PlayerType playerType, int index)
    {
        for(int i=0; i < _playerModelSetters.Count; ++i)
        {
            if ((int)playerType == i)
            {
                _playerModelSetters[i].OnModel(index);
            }
            else
                _playerModelSetters[i].OffModel();
        }

    }

}
