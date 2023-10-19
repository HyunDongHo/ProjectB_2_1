using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsideEnvironment : MonoBehaviour
{
    public Transform createParent;

    public bool previewShowCreatePosition = false;

    public List<Transform> GetEnemyCreateTransforms()
    {
        List<Transform> createTransformPositions = new List<Transform>();
        for (int i = 0; i < createParent.childCount; i++)
        {
            createTransformPositions.Add(createParent.GetChild(i));
        }
        return createTransformPositions;
    }

    public void OnDrawGizmos()
    {
        if(previewShowCreatePosition)
        {
            Gizmos.color = Color.yellow;

            List<Transform> createTransformPositions = GetEnemyCreateTransforms();
            foreach (var createTransformPosition in createTransformPositions)
            {
                Gizmos.DrawSphere(createTransformPosition.position, 0.5f);
            }
        }
    }
}
