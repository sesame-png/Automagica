namespace UnityEngine.UI
{
    internal static class PivotUtility
    {
        /// <summary>
        /// https://discussions.unity.com/t/set-a-recttranforms-pivot-without-changing-its-position/139741
        /// </summary>
        public static void SetPivot(RectTransform rectTransform, Vector2 targetPos)
        {
            Vector2 size = rectTransform.rect.size;
            Vector2 pivotOffset = rectTransform.pivot - targetPos;
            Vector3 posOffset = new Vector3(pivotOffset.x * size.x, pivotOffset.y * size.y);

            rectTransform.pivot = targetPos;
            rectTransform.localPosition -= posOffset;
        }

        /// <summary>
        /// https://discussions.unity.com/t/moving-just-recttransform-pivot-in-world-space/904634
        /// </summary>
        public static void SetPivotInWorldSpace(RectTransform rectTransform, Vector3 targetPos)
        {
            Vector3 inversePos = rectTransform.InverseTransformPoint(targetPos);
            Vector2 newPivot = new Vector2((inversePos.x - rectTransform.rect.xMin) / rectTransform.rect.width, (inversePos.y - rectTransform.rect.yMin) / rectTransform.rect.height);

            Vector2 offset = newPivot - rectTransform.pivot;
            offset.Scale(rectTransform.rect.size);
            Vector3 worldPos = rectTransform.position + rectTransform.TransformVector(offset);

            rectTransform.pivot = newPivot;
            rectTransform.position = worldPos;
        }
    }
}