namespace UnityEngine.UI
{
    /// <summary>
    /// Layout class for arranging child elements horizontally, with one expandable object which expands horizontally to fill available space.
    /// </summary>
    public class ExpandableHorizontalLayoutGroup : HorizontalLayoutGroup
    {
        /// Expandable Object
        [SerializeField] protected RectTransform m_ExpandableObject;
        public RectTransform expandableObject { get { return m_ExpandableObject; } set { SetProperty(ref m_ExpandableObject, value); } }

        /// Overridden variables
        new public bool childForceExpandWidth { get { return m_ChildForceExpandWidth; } private set { SetProperty(ref m_ChildForceExpandWidth, value); } }
        new public bool childControlWidth { get { return m_ChildControlWidth; } private set { SetProperty(ref m_ChildControlWidth, value); } }
        new public bool childScaleWidth { get { return m_ChildScaleWidth; } private set { SetProperty(ref m_ChildScaleWidth, value); } }

        protected ExpandableHorizontalLayoutGroup()
        {
            m_ChildForceExpandWidth = false;
            m_ChildControlWidth = false;
            m_ChildScaleWidth = false;
        }



        /// Set Dirty
        protected override void OnEnable()
        {
            base.OnEnable();
            SetDirty();
        }

        protected override void OnRectTransformDimensionsChange()
        {
            base.OnRectTransformDimensionsChange();
            SetDirty();
        }

        #if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            SetDirty();
        }
        #endif

        new protected void SetDirty()
        {
            if (!IsActive())
                return;

            LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
        }



        /// Set Layout
        public override void SetLayoutHorizontal()
        {
            UpdateExpandableObject();
            base.SetLayoutHorizontal();
        }

        void UpdateExpandableObject()
        {
            float totalWidth = rectTransform.rect.width;
            float childrenWidth = padding.left + padding.right + (spacing * (transform.childCount - 1));

            foreach (RectTransform child in transform)
            {
                childrenWidth += child.rect.width;
            }

            m_ExpandableObject.sizeDelta = new Vector2(m_ExpandableObject.rect.width + (totalWidth - childrenWidth), m_ExpandableObject.rect.height);
        }
    }
}