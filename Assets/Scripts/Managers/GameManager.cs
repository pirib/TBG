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


    public StatusAbstract get_StatusAbstract_byName(string Status_name)
    {
        int status_index = get_status_index(Status_name);

        if (status_index != -1) return GameManager.instance.statuses[status_index];         
        else
        {
            Debug.Log("Did not find status with a name " + Status_name + "\n Returning null");
            return null;
        }

    }
    
    // Looks for a status with a name Status_name and returns its index. Returns -1 if doesnt find it.
    private int get_status_index(string Status_name)
    {
        foreach (StatusAbstract status in statuses) {
            if ( status.name == Status_name ) return statuses.IndexOf(status);
        }
        return -1;
    }
    


}