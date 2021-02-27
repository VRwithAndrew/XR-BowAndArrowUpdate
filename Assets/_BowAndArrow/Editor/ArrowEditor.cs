using UnityEditor;
using UnityEditor.XR.Interaction.Toolkit;

[CustomEditor(typeof(Arrow))]
public class ArrowEditor : XRGrabInteractableEditor
{
    private SerializedProperty speed = null;
    public SerializedProperty tip = null;
    public SerializedProperty layerMask = null;

    protected override void OnEnable()
    {
        base.OnEnable();

        speed = serializedObject.FindProperty("speed");
        tip = serializedObject.FindProperty("tip");
        layerMask = serializedObject.FindProperty("layerMask");
    }

    protected override void DrawCoreConfiguration()
    {
        base.DrawCoreConfiguration();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Arrow", EditorStyles.boldLabel);

        EditorGUILayout.PropertyField(speed);
        EditorGUILayout.PropertyField(tip);
        EditorGUILayout.PropertyField(layerMask);
    }
}
