using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerPositionCreate : MonoBehaviour
{
    public Color gizmoColor = Color.yellow;
    public float gizmoRadius = 0.5f;

    protected List<Vector3> createPositions = new List<Vector3>();
    private bool isSetCreatePosition = false;

    public List<Vector3> GetCreatePositions()
    {
        if (!isSetCreatePosition)
        {
            isSetCreatePosition = true;
            SetPlayerCreatePosition();
        }

        return createPositions;
    }

    protected abstract void SetPlayerCreatePosition();
    protected abstract void OnDrawGizmos();
}
