using UnityEditor;
using UnityEngine.Pool;

namespace UnityEngine.UI
{
    /// <summary>
    /// This is a specialized graphics component that controls an SDF shader.
    /// </summary>
    [ExecuteAlways]
    [RequireComponent(typeof(CanvasRenderer))]
    public class SDF : MaskableGraphic
    {
        public float cornerRadius = 0;
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



        /// <summary>
        /// Awake & Destroy
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            if (!materialInstance) { materialInstance = Instantiate(material); }

            SetCorners(cornerRadius);
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



        /// <summary>
        /// Setters
        /// </summary>
        protected override void OnRectTransformDimensionsChange()
        {
            base.OnRectTransformDimensionsChange();
            if (!materialInstance) { return; }
                
            SetCorners(cornerRadius);
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

        public void SetCorners(float radius)
        {
            cornerRadius = radius;

            #if UNITY_EDITOR
            if (!materialInstance) { return; }
            #endif

            float maxRadius = Mathf.Min(rectTransform.rect.width, rectTransform.rect.height);
            radius = Mathf.Min(radius, maxRadius);

            materialForRendering.SetFloat("_Corner_Radius", radius);
            SetMaterialDirty();
        }

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