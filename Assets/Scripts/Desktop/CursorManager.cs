using UnityEngine;
using UnityEngine.UI;
using AYellowpaper.SerializedCollections;

public class CursorManager : Singleton<CursorManager>
{
    //cursor states
    public enum CursorState { pointer, hover, loading, typing, nsArrows, ewArrows, nwseArrows, neswArrows, omniArrows };
    public CursorState state = CursorState.pointer;
    [SerializeField] SerializedDictionary<CursorState, Texture2D> cursors;

    //instance
    public static CursorManager CM => Instance;

    new private void Awake()
    {
        SetCursor(CursorState.pointer);
    }

    public void SetCursor(CursorState state)
    {
        Texture2D sprite;
        cursors.TryGetValue(state, out sprite);

        Cursor.SetCursor(sprite, Vector2.zero, CursorMode.ForceSoftware);
    }
}
