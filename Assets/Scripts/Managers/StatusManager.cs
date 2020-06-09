using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusManager : MonoBehaviour
{

    #region Static
    public static StatusManager instance;

    private void Awake()
    {
        instance = this;
    }

    #endregion


    [Header("Prefabs")]
    public GameObject status;

    [Header("List of all in-game Statuses")]
    [SerializeField] private List<StatusAbstract> statuses = new List<StatusAbstract>();

    // Returns StatusAbstract if the name Status_name is within the list of all Statuses
    private StatusAbstract get_StatusAbstract_byName(string Status_name)
    {
        int status_index = get_status_index(Status_name);

        if (status_index != -1) return instance.statuses[status_index];         
        else
        {
            Debug.Log("Did not find status with a name " + Status_name + "\n Returning null");
            return null;
        }

    }
    
    // TODO look into Generics  
    // Looks for a status with a name Status_name and returns its index. Returns -1 if doesnt find it.
    private int get_status_index(string Status_name)
    {
        foreach (StatusAbstract status in statuses) {
            if ( status.name == Status_name ) return statuses.IndexOf(status);
        }
        return -1;
    }

    // TODO fix this
    // Moves over the parameters of StatusAbstract to NewStatus of the unit
    private void assign_status_parameters(ref StatusAbstract StatusAbstract, ref GameObject NewStatus, ref Unit unit)
    {
        // Get a handle on the script
        Status new_status = NewStatus.GetComponent<Status>();

        // Set handlers
        new_status.unit = unit;

        // Populate data
        new_status.inst(
            StatusAbstract.name,
            StatusAbstract.description,
            ref StatusAbstract.icon,
            StatusAbstract.duration,
            StatusAbstract.damage_turn,
            StatusAbstract.damage_end
            );
    }

    public GameObject add_status ( string Status_name, Unit unit)
    {
        // Instantiate a status object
        GameObject new_status = Instantiate(status, unit.gameObject.transform);

        // Get the specified statusAbstract based on the Status_name
        StatusAbstract statusAbstract = get_StatusAbstract_byName(Status_name);

        // Assign statusAbstract parameters to a status new_status
        assign_status_parameters(ref statusAbstract, ref new_status, ref unit);

        // return the new_status
        return new_status;
    }


}