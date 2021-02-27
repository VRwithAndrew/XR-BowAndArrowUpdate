using UnityEditor;
using UnityEditor.XR.Interaction.Toolkit;

[CustomEditor(typeof(PullMeasurer))]
public class PullMeasurerEditor : XRBaseInteractableEditor
{
    private SerializedProperty start = null;
    private SerializedProperty end = null;

    protected override void OnEnable()
    {
        base.OnEnable();
        start = serializedObject.FindProperty("start");
        end = serializedObject.FindProperty("end");
    }

    protected override void DrawCoreConfiguration()
    {
        base.DrawCoreConfiguration();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("References", EditorStyles.boldLabel);

        EditorGUILayout.PropertyField(start);
        EditorGUILayout.PropertyField(end);
    }
}
