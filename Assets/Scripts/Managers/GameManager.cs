using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    #region Static
    public static GameManager instance;

    private void Awake()
    {
        instance = this;
    }

    #endregion 

    public List<StatusAbstract> statuses = new List<StatusAbstract>();

    public Dictionary<string, int> dic = new Dictionary<string, int>() {
        {"Burning", 0 }
    };

}
