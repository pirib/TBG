using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{

    #region Static
    public static Settings instance;

    private void Awake()
    {
        instance = this;

    }

    #endregion

    [Header("Mouse Settings")]
    public Texture2D cursorTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;


    public void Start()
    {
       // Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
    }

    

}
