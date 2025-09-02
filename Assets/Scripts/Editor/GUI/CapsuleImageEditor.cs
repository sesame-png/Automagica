using UnityEngine;
using UnityEngine.UI;

namespace UnityEditor.UI
{
    /// <summary>
    /// Custom Editor for the CapsuleImage Component.
    /// </summary>
    [CustomEditor(typeof(CapsuleImage), true)]
    [CanEditMultipleObjects]
    public class CapsuleImageEditor : GraphicEditor
    {
        GUIContent m_SpriteContent;
        SerializedProperty m_Sprite;

        SerializedProperty m_Axis;
        SerializedProperty m_PPUMultiplierOffset;

        protected override void OnEnable()
        {
            base.OnEnable();

            m_SpriteContent = EditorGUIUtility.TrTextContent("Source Image");
            m_Sprite = serializedObject.FindProperty("m_Sprite");
            m_PPUMultiplierOffset = serializedObject.FindProperty("m_PPUMultiplierOffset");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(m_Sprite, m_SpriteContent);

            AppearanceControlsGUI();
            RaycastControlsGUI();

            EditorGUILayout.PropertyField(m_PPUMultiplierOffset, true);

            serializedObject.ApplyModifiedProperties();
        }

        // All graphics have a preview.
        public override bool HasPreviewGUI() { return true; }

        // Draw the Image preview.
        public override void OnPreviewGUI(Rect rect, GUIStyle background)
        {
            Image image = target as Image;
            if (image == null) return;

            Sprite sf = image.sprite;
            if (sf == null) return;

            SpriteDrawUtility.DrawSprite(sf, rect, image.canvasRenderer.GetColor());
        }

        // A string containing the Image details to be used as a overlay on the component Preview.
        public override string GetInfoString()
        {
            Image image = target as Image;
            Sprite sprite = image.sprite;

            int x = (sprite != null) ? Mathf.RoundToInt(sprite.rect.width) : 0;
            int y = (sprite != null) ? Mathf.RoundToInt(sprite.rect.height) : 0;

            return string.Format("Image Size: {0}x{1}", x, y);
        }
    }
}