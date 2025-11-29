using UnityEditor;
using UnityEngine.Pool;

namespace UnityEngine.UI
{
    // A specialized graphics component that controls an SDF shader
    [ExecuteAlways]
    [RequireComponent(typeof(CanvasRenderer))]
    public class SDF : MaskableGraphic
    {
        public Vector4 cornerRadii = new Vector4();
        //public bool alphaClip = true;
        public float edgeSoftness = 1f;

        // This overrides MaskableGraphic's built-in materialForRendering, allowing each SDF to use its own unique material instance
        // https://docs.unity3d.com/2018.1/Documentation/ScriptReference/UI.Graphic-materialForRendering.html
        public Material materialInstance { get { return _materialInstance; } private set { _materialInstance = value; } }
        private Material _materialInstance;

        public override Material materialForRendering
        {
            get
            {
                var components = ListPool<Component>.Get();
                GetComponents(typeof(IMaterialModifier), components);

                var currentMat = _materialInstance;
                for (var i = 0; i < components.Count; i++)
                    currentMat = (components[i] as IMaterialModifier).GetModifiedMaterial(currentMat);
                ListPool<Component>.Release(components);
                return currentMat;
            }
        }



        // Update Material
        protected override void Awake()
        {
            base.Awake();
            if (!_materialInstance) { _materialInstance = Instantiate(material); }

            SetCorners(cornerRadii);
            //SetAlphaClip(alphaClip);
            SetSoftness(edgeSoftness);
            SetDimensions();

            UpdateMaterial();
        }

        protected override void OnDestroy()
        {
            DestroyImmediate(_materialInstance);
            base.OnDestroy();
        }

        #if UNITY_EDITOR
            protected override void OnValidate()
            {
                if (!_materialInstance) { _materialInstance = Instantiate(material); }
                Awake();
            }
        #endif



        // Setters
        protected override void OnRectTransformDimensionsChange()
        {
            base.OnRectTransformDimensionsChange();
            if (!_materialInstance) { return; }
                
            SetCorners(cornerRadii);
            SetDimensions();

            UpdateMaterial();
        }

        private void SetDimensions()
        {
            #if UNITY_EDITOR
                if (!_materialInstance) { return; }
            #endif

            materialForRendering.SetVector("_Dimensions", rectTransform.rect.size);
            SetMaterialDirty();
        }

        public void SetCorners(Vector4 radii)
        {
            cornerRadii = radii;

            #if UNITY_EDITOR
                if (!_materialInstance) { return; }
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
            if (!_materialInstance) { return; }
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
                if (!_materialInstance) { return; }
            #endif

            materialForRendering.SetFloat("_Softness", softness / 100f);
            SetMaterialDirty();
        }
    }
}