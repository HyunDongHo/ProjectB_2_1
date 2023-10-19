using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPositionCreate_Random : EnemyPositionCreate
{
    //public float[] mapCircleEnemyCreateBorders;

    public float[] MyMapCircleEnemyCreateBorders_x;
    public float[] MyMapCircleEnemyCreateBorders_z;

    public int createAmount;  

    public void Start()
    {
        MyMapCircleEnemyCreateBorders_x = new float[] { 17, 17, 17, 30, 30, 42, 42, 42, 48, 11, 11, 11, 50, 50};
        MyMapCircleEnemyCreateBorders_z = new float[] { 46, 33 ,21, 21, 44, 44, 33, 21, 33, 33, 42, 25, 25, 42};    

    }

    //protected override void SetEnemyCreatePosition()
    //{
    //    for (int i = 0; i < createAmount; i++)
    //    {
    //        float mapCircleBorderPlayerAngle = Random.Range(0, 360);

    //        int randomBorderIndex = Random.Range(0, mapCircleEnemyCreateBorders.Length);

    //        Vector3 createPosition = Vector3.zero;
    //        createPosition.x = Mathf.Cos(mapCircleBorderPlayerAngle) * mapCircleEnemyCreateBorders[randomBorderIndex];
    //        createPosition.z = Mathf.Sin(mapCircleBorderPlayerAngle) * mapCircleEnemyCreateBorders[randomBorderIndex];

    //        createPositions.Add(createPosition);
    //    }
    //}  
    protected override void SetEnemyCreatePosition()    
    {

        for (int i = 0; i < createAmount; i++)
        {
            float mapCircleBorderPlayerAngle = Random.Range(0, 360);  

            int randomBorderIndex = Random.Range(0, MyMapCircleEnemyCreateBorders_x.Length);

            Vector3 createPosition = Vector3.zero;
            createPosition.x = MyMapCircleEnemyCreateBorders_x[i];
            createPosition.z = MyMapCircleEnemyCreateBorders_z[i];  

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
