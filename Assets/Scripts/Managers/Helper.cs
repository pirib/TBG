using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helper : MonoBehaviour
{
    #region Declaring Static
    public static Helper instance;

    private void Awake()
    {
        instance = this;
    }

    #endregion 


    public Canvas IN_GAME_UI;

    public float UnitToPixels(float units)
    {
        return units*16;
    }

    public float PixelsToUnit (float pixels)
    {
        return pixels * 0.0625f;
    }

}
