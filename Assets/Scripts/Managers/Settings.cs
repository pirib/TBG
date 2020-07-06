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

    #region PUBLIC PARAMETERS

    public float SCREEN_HEIGHT;
    public float SCREEN_WIDTH;

    #endregion


    public void Start()
    {
        SCREEN_HEIGHT = Camera.main.orthographicSize*2;
        SCREEN_WIDTH = Camera.main.aspect * SCREEN_HEIGHT*2;
    }

    

}
