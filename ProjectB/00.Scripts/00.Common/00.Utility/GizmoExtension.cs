using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GizmoExtension : MonoBehaviour
{
    public static void DrawWireCapsule(Vector3 pos, float radius, float height, Color color = default(Color))
    {
#if UNITY_EDITOR
        if (color != default(Color))
            Handles.color = color;

        Matrix4x4 angleMatrix = Matrix4x4.TRS(pos, Quaternion.identity, Handles.matrix.lossyScale);
        using (new Handles.DrawingScope(angleMatrix))
        {
            var pointOffset = height / 2;

            Handles.DrawWireArc(Vector3.up * pointOffset, Vector3.left, Vector3.back, -180, radius);
            Handles.DrawLine(new Vector3(0, pointOffset, -radius), new Vector3(0, -pointOffset, -radius));
            Handles.DrawLine(new Vector3(0, pointOffset, radius), new Vector3(0, -pointOffset, radius));
            Handles.DrawWireArc(Vector3.down * pointOffset, Vector3.left, Vector3.back, 180, radius);

            Handles.DrawWireArc(Vector3.up * pointOffset, Vector3.back, Vector3.left, 180, radius);
            Handles.DrawLine(new Vector3(-radius, pointOffset, 0), new Vector3(-radius, -pointOffset, 0));
            Handles.DrawLine(new Vector3(radius, pointOffset, 0), new Vector3(radius, -pointOffset, 0));
            Handles.DrawWireArc(Vector3.down * pointOffset, Vector3.back, Vector3.left, -180, radius);

            Handles.DrawWireDisc(Vector3.up * pointOffset, Vector3.up, radius);
            Handles.DrawWireDisc(Vector3.down * pointOffset, Vector3.up, radius);
        }
#endif
    }
}
