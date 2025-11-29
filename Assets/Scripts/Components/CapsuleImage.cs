using System;

namespace UnityEngine.UI
{
    // A type of Image that calculates and sets the correct pixels per unit multiplier needed to create a perfect capsule from a sliced texture
    // Currently does not support tiled textures
    public class CapsuleImage : Image
    {
        public float ppuMultiplierOffset { get { return m_PPUMultiplierOffset; } set { if (SetPropertyUtility.SetStruct(ref m_PPUMultiplierOffset, value)) SetVerticesDirty(); } }
        [SerializeField] private float m_PPUMultiplierOffset = 0.0f;

        protected override void OnEnable()
        {
            base.OnEnable();
            CalculateCapsule();

        }
        protected override void OnRectTransformDimensionsChange()
        {
            base.OnRectTransformDimensionsChange();
            CalculateCapsule();
        }

        #if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            type = Type.Sliced;
            CalculateCapsule();
        }
        #endif

        public void CalculateCapsule()
        {
            float multiplier = Mathf.Max(mainTexture.width / GetPixelAdjustedRect().width, mainTexture.height / GetPixelAdjustedRect().height);
            pixelsPerUnitMultiplier = multiplier + m_PPUMultiplierOffset;
        }
    }
}