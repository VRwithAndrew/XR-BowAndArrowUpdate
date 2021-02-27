using UnityEditor;
using UnityEditor.XR.Interaction.Toolkit;

[CustomEditor(typeof(Quiver))]
public class QuiverEditor : XRBaseInteractableEditor
{
    private SerializedProperty arrowPrefab = null;

    protected override void OnEnable()
    {
        base.OnEnable();
        arrowPrefab = serializedObject.FindProperty("arrowPrefab");
    }

    protected override void DrawCoreConfiguration()
    {
        base.DrawCoreConfiguration();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Quiver", EditorStyles.boldLabel);

        EditorGUILayout.PropertyField(arrowPrefab);
    }
}
