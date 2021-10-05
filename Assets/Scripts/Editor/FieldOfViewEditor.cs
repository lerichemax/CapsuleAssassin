using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FieldOfView))]
public class FieldOfViewEditor : Editor
{
    private void OnSceneGUI()
    {
        FieldOfView fov = (FieldOfView)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov._viewRadius);
        Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov._viewRadius *0.75f);
        Vector3 viewAngleA = fov.DirectionFromAngle(-fov._viewAngle / 2, false);
        Vector3 viewAngleB = fov.DirectionFromAngle(fov._viewAngle / 2, false);

        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleA * fov._viewRadius);
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleB * fov._viewRadius);    
    }
}
