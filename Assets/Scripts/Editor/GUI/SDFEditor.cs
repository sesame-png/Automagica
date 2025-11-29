using UnityEngine.UI;
//using UnityEditor.AnimatedValues;

namespace UnityEditor.UI
{
    // Custom Editor for the SDF Component
    [CustomEditor(typeof(SDF), true)]
    [CanEditMultipleObjects]
    public class SDFEditor : GraphicEditor
    {
        SerializedProperty cornerRadii;
        //SerializedProperty alphaClip;
        SerializedProperty edgeSoftness;

        //AnimBool showSoftness;

        protected override void OnEnable()
        {
            base.OnEnable();

            cornerRadii = serializedObject.FindProperty("cornerRadii");
            //alphaClip = serializedObject.FindProperty("alphaClip");
            edgeSoftness = serializedObject.FindProperty("edgeSoftness");

            //showSoftness = new AnimBool(alphaClip.boolValue);
            //showSoftness.valueChanged.AddListener(Repaint);
        }

        /*protected override void OnDisable()
        {
            base.OnDisable();

            showSoftness.valueChanged.RemoveListener(Repaint);
        }*/

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            AppearanceControlsGUI();
            RaycastControlsGUI();

            EditorGUILayout.PropertyField(cornerRadii, true);
            //EditorGUILayout.PropertyField(alphaClip, true);
            EditorGUILayout.PropertyField(edgeSoftness, true);

            /*showSoftness.target = !alphaClip.boolValue;
            if (EditorGUILayout.BeginFadeGroup(showSoftness.faded))
            {
                EditorGUILayout.PropertyField(edgeSoftness, true);
            }
            EditorGUILayout.EndFadeGroup();*/

            serializedObject.ApplyModifiedProperties();
        }
    }
}