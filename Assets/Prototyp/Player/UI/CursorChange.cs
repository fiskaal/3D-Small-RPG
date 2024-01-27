using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorChange : MonoBehaviour
{
    public Texture2D cursorTexture;

    private void Awake()
    {
        Cursor.SetCursor(cursorTexture, Vector2.zero , CursorMode.Auto);
    }
}
