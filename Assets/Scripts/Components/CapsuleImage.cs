using System;

namespace UnityEngine.UI
{
    /// <summary>
    /// A type of Image that calculates and sets the correct pixels per unit multiplier needed to create a perfect capsule from a sliced texture.
    /// Does not currently support tiled textures.
    /// </summary>
    public class CapsuleImage : Image
    {
        [SerializeField] private float m_PPUMultiplierOffset = 0.0f;
        public float ppuMultiplierOffset { get { return m_PPUMultiplierOffset; } set { if (SetPropertyUtility.SetStruct(ref m_PPUMultiplierOffset, value)) SetVerticesDirty(); } }

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
            pixelsPerUnitMultiplier = multiplier + ppuMultiplierOffset;
        }
    }
}