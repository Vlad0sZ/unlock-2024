using System;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    [SerializeField] private Texture2D arrowTexture;
    [SerializeField] private Texture2D handTexture;
    
    private static CursorController _instance;

    private void Awake()
    {
        _instance = this;
        ChangeCursor(CursorType.Arrow);
    }

    public static void ChangeCursor(CursorType cursorType)
    {
        switch (cursorType)
        {
            case CursorType.None:
                Cursor.SetCursor(null, Vector2.zero, CursorMode.ForceSoftware);
                break;
            case CursorType.Arrow:
                Cursor.SetCursor(_instance.arrowTexture, Vector2.zero, CursorMode.ForceSoftware);
                break;
            case CursorType.Hand:
                Cursor.SetCursor(_instance.handTexture, Vector2.zero, CursorMode.ForceSoftware);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(cursorType), cursorType, null);
        }
    }
}