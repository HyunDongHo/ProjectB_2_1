using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragUnit : MonoBehaviour
{
    private Vector3 m_Offset;
    private float m_ZCoord;
    private float m_mouseXMoveValue = 0;

    private const float minXCoord = 12.9f;
    private const float maxXCoord = 11.83f;

   // Transform parentTransform = null;
    RandomCardTable randomCardAll = null;
    public Action<int> selectAction = null;
   // LobbyScene lobbyScene = null;
    public int CardNum;
    private void Start()
    {
        randomCardAll = GameObject.Find("Random_Card_All").GetComponent<RandomCardTable>();
        //lobbyScene = GameObject.Find("LobbyScene").GetComponent<LobbyScene>();
    }

    void OnMouseDown()
    {
        if (randomCardAll == null || !randomCardAll.IsInteract)
            return;
        
        //if (randomCardAll != null && randomCardAll.IsInteract)
        { 
            m_ZCoord = Camera.main.WorldToScreenPoint(randomCardAll.transform.position).z;
            m_Offset = randomCardAll.transform.position - GetMouseWorldPosition();
            m_mouseXMoveValue = GetMouseWorldPosition().x;
        }

    }

    void OnMouseDrag()
    {
        if (randomCardAll == null || !randomCardAll.IsInteract)
            return;

        Vector3 position = new Vector3(GetMouseWorldPosition().x + m_Offset.x, randomCardAll.transform.position.y, randomCardAll.transform.position.z);
       
        float priceViewTransform =  ((12.9f - position.x) / 1.07f) * -640;
      
        if (position.x >= minXCoord)
        {
            position.x = minXCoord;
            priceViewTransform = 0;
        }

        if (position.x <= maxXCoord)
        {
            position.x = maxXCoord;
            priceViewTransform = -640;
        }

        randomCardAll.transform.DOLocalMoveX(position.x, 0.08f).SetEase(Ease.InOutSine);
    //    lobbyScene.PriceViewTransform.DOLocalMoveX(priceViewTransform, 0.08f).SetEase(Ease.InOutSine);

    }

    void OnMouseUp()
    {
        if (randomCardAll == null || !randomCardAll.IsInteract)
            return;

        Vector3 position = new Vector3(GetMouseWorldPosition().x + m_Offset.x, randomCardAll.transform.position.y, randomCardAll.transform.position.z);
        float priceViewTransform = ((12.9f - position.x) / 1.07f) * -640;

        if (position.x >= minXCoord)
        {
            position.x = minXCoord;
            priceViewTransform = 0;
        }

        if (position.x <= maxXCoord)
        {
            position.x = maxXCoord;
            priceViewTransform = -640;
        }

        randomCardAll.transform.DOLocalMoveX(position.x, 0.08f).SetEase(Ease.InOutSine);
    //    lobbyScene.PriceViewTransform.DOLocalMoveX(priceViewTransform, 0.08f).SetEase(Ease.InOutSine);

        if (Mathf.Abs(m_mouseXMoveValue - GetMouseWorldPosition().x) < 0.1f)
        {
            selectAction?.Invoke(CardNum);
        }
    }

    Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = m_ZCoord;

        return Camera.main.ScreenToWorldPoint(mousePoint);
    }
}
