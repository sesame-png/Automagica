using UnityEditor;
using UnityEngine.Pool;

namespace UnityEngine.UI
{
    /// <summary>
    /// A specialized graphics component that controls an SDF shader.
    /// </summary>
    [ExecuteAlways]
    [RequireComponent(typeof(CanvasRenderer))]
    public class SDF : MaskableGraphic
    {
        public Vector4 cornerRadii = new Vector4();
        //public bool alphaClip = true;
        public float edgeSoftness = 1f;

        /// <summary>
        /// This overrides MaskableGraphic's built-in materialForRendering, allowing each SDF to use its own unique material instance.
        /// https://docs.unity3d.com/2018.1/Documentation/ScriptReference/UI.Graphic-materialForRendering.html
        /// </summary>
        protected Material m_MaterialInstance;
        public Material materialInstance { get { return m_MaterialInstance; } private set { m_MaterialInstance = value; } }

        public override Material materialForRendering
        {
            get
            {
                var components = ListPool<Component>.Get();
                GetComponents(typeof(IMaterialModifier), components);

                var currentMat = materialInstance;
                for (var i = 0; i < components.Count; i++)
                    currentMat = (components[i] as IMaterialModifier).GetModifiedMaterial(currentMat);
                ListPool<Component>.Release(components);
                return currentMat;
            }
        }



        /// Update Material
        protected override void Awake()
        {
            base.Awake();
            if (!materialInstance) { materialInstance = Instantiate(material); }

            SetCorners(cornerRadii);
            //SetAlphaClip(alphaClip);
            SetSoftness(edgeSoftness);
            SetDimensions();

            UpdateMaterial();
        }

        protected override void OnDestroy()
        {
            DestroyImmediate(materialInstance);
            base.OnDestroy();
        }

        #if UNITY_EDITOR
        protected override void OnValidate()
        {
            if (!materialInstance) { materialInstance = Instantiate(material); }
            Awake();
        }
        #endif



        /// Setters
        protected override void OnRectTransformDimensionsChange()
        {
            base.OnRectTransformDimensionsChange();
            if (!materialInstance) { return; }
                
            SetCorners(cornerRadii);
            SetDimensions();

            UpdateMaterial();
        }

        private void SetDimensions()
        {
            #if UNITY_EDITOR
            if (!materialInstance) { return; }
            #endif

            materialForRendering.SetVector("_Dimensions", rectTransform.rect.size);
            SetMaterialDirty();
        }

        public void SetCorners(Vector4 radii)
        {
            cornerRadii = radii;

            #if UNITY_EDITOR
            if (!materialInstance) { return; }
            #endif

            float maxRadius = Mathf.Min(rectTransform.rect.width, rectTransform.rect.height);
            for (int i = 0; i < 4; i++)
            {
                if (radii[i] > maxRadius) { radii[i] = maxRadius; }
            }

            materialForRendering.SetVector("_Corner_Radii", radii);
            SetMaterialDirty();
        }

        /*private void SetAlphaClip(bool clip)
        {
            #if UNITY_EDITOR
            if (!materialInstance) { return; }
            #endif

            if (clip)
            {
                materialForRendering.SetFloat("_Alpha_Clip", 1);
            }
            else
            {
                materialForRendering.SetFloat("_Alpha_Clip", 0);
            }

            SetMaterialDirty();
        }*/

        public void SetSoftness(float softness)
        {
            edgeSoftness = softness;

            #if UNITY_EDITOR
            if (!materialInstance) { return; }
            #endif

            materialForRendering.SetFloat("_Softness", softness / 100f);
            SetMaterialDirty();
        }
    }
}