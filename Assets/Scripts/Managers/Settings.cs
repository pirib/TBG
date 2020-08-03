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

    // Mouse Parallax 
    [Header("Parallax parameters")]
    [SerializeField] private Vector3 StartPos;
    [SerializeField] private float modifier;
    [SerializeField] public GameObject parralax_object;


    public void Start()
    {
        // Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);

        StartPos = parralax_object.transform.position;
    }
    
    /*
    void Update()
    {
        var pz = Camera.main.ScreenToViewportPoint(Input.mousePosition);

        parralax_object.transform.position = pz;
        parralax_object.transform.position = new Vector3(StartPos.x + (pz.x * modifier), StartPos.y + (pz.y * modifier), parralax_object.transform.position.z);
    }
    */
}
