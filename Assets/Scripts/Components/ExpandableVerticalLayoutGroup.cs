namespace UnityEngine.UI
{
    // Layout class for arranging child elements vertically, with one expandable object which expands vertically to fill available space
    public class ExpandableVerticalLayoutGroup : VerticalLayoutGroup
    {
        // Expandable Object
        public RectTransform expandableObject { get { return m_ExpandableObject; } set { SetProperty(ref m_ExpandableObject, value); } }
        [SerializeField] private RectTransform m_ExpandableObject;

        // Overridden Variables
        new public bool childForceExpandHeight { get { return m_ChildForceExpandHeight; } private set { SetProperty(ref m_ChildForceExpandHeight, value); } }
        new public bool childControlHeight { get { return m_ChildControlHeight; } private set { SetProperty(ref m_ChildControlHeight, value); } }
        new public bool childScaleHeight { get { return m_ChildScaleHeight; } private set { SetProperty(ref m_ChildScaleHeight, value); } }

        protected ExpandableVerticalLayoutGroup()
        {
            m_ChildForceExpandHeight = false;
            m_ChildControlHeight = false;
            m_ChildScaleHeight = false;
        }



        // Set Dirty
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



        // Set Layout
        public override void SetLayoutVertical()
        {
            UpdateExpandableObject();
            base.SetLayoutVertical();
        }

        void UpdateExpandableObject()
        {
            float totalHeight = rectTransform.rect.height;
            float childrenHeight = padding.top + padding.bottom + (spacing * (transform.childCount - 1));

            foreach (RectTransform child in transform)
            {
                childrenHeight += child.rect.height;
            }

            m_ExpandableObject.sizeDelta = new Vector2(m_ExpandableObject.rect.width, m_ExpandableObject.rect.height + (totalHeight - childrenHeight));
        }
    }
}