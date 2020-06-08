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

    public readonly List<StatusAbstract> statuses = new List<StatusAbstract>();

    // Adding all statuses do a dictionary for quick access
    // TODO better way of doing this?

    private readonly Dictionary<string, int> dic = new Dictionary<string, int>() {
        {"Burning", 0 }
    };

    public StatusAbstract find_StatusAbstract_byName(string Status_name)
    {
        int value;
        if (dic.TryGetValue(Status_name, out value))
        {
            // Get a hold of information about the particular status and Instantiate a new empty status
            return GameManager.instance.statuses[value];    
        }
        else
        {
            Debug.Log("Did not find status with a name " + Status_name + "\n Returning null");
            return null;
        }
    }

}
