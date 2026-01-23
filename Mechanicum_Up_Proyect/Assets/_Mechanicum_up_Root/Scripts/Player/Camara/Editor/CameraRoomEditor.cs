using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CameraRoom))]
public class CameraRoomEditor : Editor
{
    public override void OnInspectorGUI()
    {
        CameraRoom room = (CameraRoom)target;

        EditorGUILayout.LabelField("Camera Limits", EditorStyles.boldLabel);

        room.xLimits = EditorGUILayout.Vector2Field("X Limits", room.xLimits);
        room.yLimits = EditorGUILayout.Vector2Field("Y Limits", room.yLimits);

        EditorGUILayout.Space(10);

        EditorGUI.BeginChangeCheck();
        room.useCustomOffset = EditorGUILayout.Toggle("Use Custom Offset", room.useCustomOffset);
        if (room.useCustomOffset)
        {
            room.customOffset = EditorGUILayout.Vector3Field("Offset", room.customOffset);
        }

        EditorGUILayout.Space(5);

        room.useCustomSmooth = EditorGUILayout.Toggle("Use Custom Smooth", room.useCustomSmooth);
        if (room.useCustomSmooth)
        {
            room.customSmoothSpeed = EditorGUILayout.FloatField("Smooth Speed", room.customSmoothSpeed);
        }

        EditorGUILayout.Space(10);

        if (GUILayout.Button("Auto-Expand Limits (Collider Bounds)"))
        {
            BoxCollider bc = room.GetComponent<BoxCollider>();
            if (bc != null)
            {
                Vector3 min = bc.bounds.min;
                Vector3 max = bc.bounds.max;

                room.xLimits = new Vector2(min.x, max.x);
                room.yLimits = new Vector2(min.y, max.y);
            }
            else
            {
                Debug.LogWarning("Este objeto no tiene BoxCollider.");
            }
        }

        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(room);
        }
    }
}
