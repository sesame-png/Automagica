using UnityEngine;
using UnityEngine.UI;
using Unity.Mathematics;

// https://discussions.unity.com/t/none-rectangle-shaped-button/547759
[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Mask))]
public class RaycastMask : MonoBehaviour, ICanvasRaycastFilter
{
    private Image _image;
    private Sprite _sprite;

    void Start()
    {
        _image = GetComponent<Image>();
    }

    public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
    {
        _sprite = _image.sprite;

        RectTransform rectTransform = (RectTransform)transform;
        Vector2 localPositionPivotRelative;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, sp, eventCamera, out localPositionPivotRelative);

        // Convert to bottom-left origin coordinates
        Vector2 localPosition = new Vector2(localPositionPivotRelative.x + rectTransform.pivot.x * rectTransform.rect.width, localPositionPivotRelative.y + rectTransform.pivot.y * rectTransform.rect.height);

        Rect spriteRect = _sprite.textureRect;
        Rect maskRect = rectTransform.rect;

        int x = 0;
        int y = 0;
        // Convert to texture space
        switch (_image.type)
        {
            case Image.Type.Sliced:
                {
                    float ppu = _image.pixelsPerUnit * _image.pixelsPerUnitMultiplier;
                    Vector4 border = _sprite.border;
                    Vector4 adjustedBorder = _sprite.border / ppu;
                    
                    // X Slicing
                    if (localPosition.x < adjustedBorder.x && localPosition.x < maskRect.width / 2)
                    {
                        x = Mathf.FloorToInt(spriteRect.x + (localPosition.x * ppu));
                    }
                    else if (localPosition.x > maskRect.width - adjustedBorder.z && localPosition.x > maskRect.width / 2)
                    {
                        x = Mathf.FloorToInt(spriteRect.x + spriteRect.width - ((maskRect.width - localPosition.x) * ppu));
                    }
                    else
                    {
                        x = Mathf.FloorToInt(math.remap(adjustedBorder.x, maskRect.width - adjustedBorder.z, spriteRect.x + border.x, spriteRect.x + spriteRect.width - border.z, localPosition.x));
                    }

                    // Y Slicing
                    if (localPosition.y < adjustedBorder.y && localPosition.y < maskRect.height / 2)
                    {
                        y = Mathf.FloorToInt(spriteRect.y + (localPosition.y * ppu));
                    }
                    else if (localPosition.y > maskRect.height - adjustedBorder.w && localPosition.y > maskRect.height / 2)
                    {
                        y = Mathf.FloorToInt(spriteRect.y + spriteRect.height - ((maskRect.height - localPosition.y) * ppu));
                    }
                    else
                    {
                        y = Mathf.FloorToInt(math.remap(adjustedBorder.y, maskRect.height - adjustedBorder.w, spriteRect.y + border.y, spriteRect.y + spriteRect.height - border.w, localPosition.y));
                    }
                }
                break;
            case Image.Type.Simple:
            default:
                {
                    // Conversion to uniform UV space
                    x = Mathf.FloorToInt(spriteRect.x + spriteRect.width * localPosition.x / maskRect.width);
                    y = Mathf.FloorToInt(spriteRect.y + spriteRect.height * localPosition.y / maskRect.height);
                }
                break;
        }

        // Destroy component if texture import settings are wrong
        try
        {
            return _sprite.texture.GetPixel(x, y).a > 0;
        }
        catch (UnityException e)
        {
            Debug.LogError("Mask texture not readable, set your sprite to Texture Type 'Advanced' and check 'Read/Write Enabled'");
            Debug.LogError(e);
            Destroy(this);
            return false;
        }
    }
}