#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(ClickTracer))]
public class ClickTracerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        ClickTracer clickTracer = (ClickTracer)target;

        clickTracer.NameButton = EditorGUILayout.TextField("Name Button", clickTracer.NameButton);
        clickTracer.IsJoystick = EditorGUILayout.Toggle("Is Joystick", clickTracer.IsJoystick);

        if (clickTracer.IsJoystick)
        {
            clickTracer.MovementLimit = EditorGUILayout.FloatField("Movement Limit", clickTracer.MovementLimit);
            clickTracer.MovementThreashold = EditorGUILayout.FloatField("Movement Threashold", clickTracer.MovementThreashold);

        }
    }
}

#endif
