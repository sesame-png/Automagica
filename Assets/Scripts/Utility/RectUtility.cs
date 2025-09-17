namespace UnityEngine.UI
{
    internal static class RectUtility
    {
        /// <summary>
        /// Moves a RectTransform's pivot without changing its position.
        /// https://discussions.unity.com/t/set-a-recttranforms-pivot-without-changing-its-position/139741
        /// </summary>
        public static Vector2 SetPivotInPlace(RectTransform rectTransform, Vector2 targetPos)
        {
            Vector2 size = rectTransform.rect.size;
            Vector2 pivotDelta = rectTransform.pivot - targetPos;
            Vector3 posDelta = new Vector2(pivotDelta.x * size.x, pivotDelta.y * size.y);

            rectTransform.pivot = targetPos;
            rectTransform.localPosition -= posDelta;

            return posDelta;
        }

        /// <summary>
        /// Moves a RectTransform's pivot in world space, without changing its position.
        /// https://discussions.unity.com/t/moving-just-recttransform-pivot-in-world-space/904634
        /// </summary>
        public static Vector2 SetPivotInWorldSpace(RectTransform rectTransform, Vector3 targetPos)
        {
            Vector3 inversePos = rectTransform.InverseTransformPoint(targetPos);
            Vector2 newPivot = new Vector2((inversePos.x - rectTransform.rect.xMin) / rectTransform.rect.width, (inversePos.y - rectTransform.rect.yMin) / rectTransform.rect.height);

            Vector2 posDelta = newPivot - rectTransform.pivot;
            posDelta.Scale(rectTransform.rect.size);
            Vector3 worldPos = rectTransform.position + rectTransform.TransformVector(posDelta);

            rectTransform.pivot = newPivot;
            rectTransform.position = worldPos;

            return worldPos;
        }

        /// <summary>
        /// Moves a RectTransform's anchors without changing its size or position.
        /// </summary>
        public static void SetAnchorsInPlace(RectTransform rectTransform, Vector2 anchorMin, Vector2 anchorMax)
        {
            RectTransform parentRectTransform = rectTransform.transform.parent.GetComponent<RectTransform>();

            Vector2 pos = NormalizedToLocalPoint(parentRectTransform, rectTransform.anchorMin);
            rectTransform.anchorMin = anchorMin;
            Vector2 newPos = NormalizedToLocalPoint(parentRectTransform, anchorMin);
            Vector2 posDelta = newPos - pos;
            rectTransform.offsetMin -= posDelta;

            pos = NormalizedToLocalPoint(parentRectTransform, rectTransform.anchorMax);
            rectTransform.anchorMax = anchorMax;
            newPos = NormalizedToLocalPoint(parentRectTransform, anchorMax);
            posDelta = pos - newPos;
            rectTransform.offsetMax += posDelta;
        }

        /// <summary>
        /// Converts a normalized point (0 - 1) to a local point within a RectTransform.
        /// </summary>
        public static Vector2 NormalizedToLocalPoint(RectTransform rectTransform, Vector2 pos)
        {
            Vector3[] corners = new Vector3[4];
            rectTransform.GetLocalCorners(corners);

            Vector2 localPos;
            localPos.x = corners[0].x + (corners[2].x - corners[0].x) * pos.x;
            localPos.y = corners[0].y + (corners[2].y - corners[0].y) * pos.y;

            return localPos;
        }

        /// <summary>
        /// Converts a local point to a normalized point (0 - 1) within a RectTransform.
        /// </summary>
        public static Vector2 LocalToNormalizedPoint(RectTransform rectTransform, Vector2 localPos)
        {
            Vector3[] corners = new Vector3[4];
            rectTransform.GetLocalCorners(corners);

            Vector2 pos = localPos;
            pos.x = (localPos.x - corners[0].x) / (corners[2].x - corners[0].x);
            pos.y = (localPos.y - corners[0].y) / (corners[2].y - corners[0].y);

            return pos;
        }
    }
}