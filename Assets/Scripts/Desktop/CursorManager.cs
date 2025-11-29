using UnityEngine;
using UnityEngine.UI;
using AYellowpaper.SerializedCollections;

public class CursorManager : Singleton<CursorManager>
{
    public enum CursorState { pointer, hover, loading, typing, nsArrows, ewArrows, nwseArrows, neswArrows, omniArrows };
    
    public CursorState cursorState { get { return _cursorState; } private set { _cursorState = value; } }
    private CursorState _cursorState;

    [SerializeField] SerializedDictionary<CursorState, Texture2D> cursors;

    public static CursorManager CM => Instance;

    private void Start()
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