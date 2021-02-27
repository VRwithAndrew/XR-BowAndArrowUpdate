using UnityEditor;
using UnityEditor.XR.Interaction.Toolkit;

[CustomEditor(typeof(Notch))]
public class NotchEditor : XRSocketInteractorEditor
{
    private SerializedProperty releaseThreshold = null;

    protected override void OnEnable()
    {
        base.OnEnable();
        releaseThreshold = serializedObject.FindProperty("releaseThreshold");
    }

    protected override void DrawCoreConfiguration()
    {
        base.DrawCoreConfiguration();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Notch", EditorStyles.boldLabel);

        EditorGUILayout.PropertyField(releaseThreshold);
    }
}
