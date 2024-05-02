using UnityEngine;

public class UpdateMouseCursor : MonoBehaviour
{
    public static UpdateMouseCursor Instance;
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    [Header("MouseCursor")]
    [SerializeField] Texture2D _mouseCursorDefault;
    [SerializeField] Texture2D _mouseCursorSelected;
    Vector2 _hotSpot = Vector2.zero;
    CursorMode _cursorMode = CursorMode.Auto;

    private void Start()
    {
        Cursor.SetCursor(_mouseCursorDefault, _hotSpot, _cursorMode);
    }

    public void UpdateCursor(bool value)
    {
        if (value) Cursor.SetCursor(_mouseCursorSelected, _hotSpot, _cursorMode);
        else Cursor.SetCursor(_mouseCursorDefault, _hotSpot, _cursorMode);
    }


}
