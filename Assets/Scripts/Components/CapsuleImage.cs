using System;

namespace UnityEngine.UI
{
    /// <summary>
    /// A child of Image that calculates and sets the correct pixels per unit multiplier needed to create a perfect capsule from a sliced image.
    /// It is possible to set the pixels per unit multiplier and the image type from a script, as these variables are public.
    /// Do not do this.
    /// It defeats the purpose of this component.
    /// Just use an Image.
    /// </summary>
    public class CapsuleImage : Image
    {
        //BUG: Tiled image support not available
        public enum Axis
        {
            Auto,
            Horizontal,
            Vertical
        }

        [SerializeField] private Axis m_Axis = Axis.Auto;

        /// <summary>
        /// Which direction to capsule the sprite in.
        /// Auto will automatically use the longer dimension.
        /// </summary>
        public Axis axis { get { return m_Axis; } set { if (SetPropertyUtility.SetStruct(ref m_Axis, value)) SetVerticesDirty(); } }

        [SerializeField] private float m_PPUMultiplierOffset = 1.0f;

        /// <summary>
        /// Offset the pixels per unit multiplier.
        /// </summary>
        public float ppuMultiplierOffset { get { return m_PPUMultiplierOffset; } set { if (SetPropertyUtility.SetStruct(ref m_PPUMultiplierOffset, value)) SetVerticesDirty(); } }

        protected CapsuleImage()
        { }



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
            m_PPUMultiplierOffset = Mathf.Max(0.01f, m_PPUMultiplierOffset);
            CalculateCapsule();
        }
        #endif



        public void CalculateCapsule()
        {
            float multiplier = 1.0f;

            switch (axis)
            {
                case Axis.Horizontal:
                    multiplier = mainTexture.height / GetPixelAdjustedRect().height;
                    break;
                case Axis.Vertical:
                    multiplier = mainTexture.width / GetPixelAdjustedRect().width;
                    break;
                case Axis.Auto:
                    if (GetPixelAdjustedRect().height > GetPixelAdjustedRect().width)
                    {
                        multiplier = mainTexture.width / GetPixelAdjustedRect().width;
                    }
                    else
                    {
                        multiplier = mainTexture.height / GetPixelAdjustedRect().height;
                    }
                    break;
            }

            pixelsPerUnitMultiplier = multiplier * ppuMultiplierOffset;
        }
    }
}