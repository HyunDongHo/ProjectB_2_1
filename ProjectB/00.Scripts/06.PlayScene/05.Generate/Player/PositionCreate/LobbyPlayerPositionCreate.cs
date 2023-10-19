using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyPlayerPositionCreate : PlayerPositionCreate
{
    //public float[] mapCircleEnemyCreateBorders;

    public float[] MyMapCirclePlayerCreateBorders_x;
    public float[] MyMapCirclePlayerCreateBorders_z;

    protected override void SetPlayerCreatePosition()      
    {

        for (int i = 0; i < (int)PlayerType.None; i++)
        {
            Vector3 createPosition = Vector3.zero;
            createPosition.x = MyMapCirclePlayerCreateBorders_x[i];
            createPosition.y = 3.0f;  
            createPosition.z = MyMapCirclePlayerCreateBorders_z[i];  

            createPositions.Add(createPosition);        
        }
    }
    protected override void OnDrawGizmos()
    {
        
        Gizmos.color = gizmoColor;
        foreach (Vector3 enemyCreatePosition in createPositions)
        {
            Gizmos.DrawSphere(enemyCreatePosition, gizmoRadius);
        }
    }
}
