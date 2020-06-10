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
        foreach (StatusAbstract status in statuses)
        {
            if (status.name == Status_name) return statuses.IndexOf(status);
        }
        return -1;
    }

    // Moves over the parameters of StatusAbstract to NewStatus of the unit
    private void assign_status_parameters(ref StatusAbstract StatusAbstract, ref GameObject NewStatus, ref Unit unit)
    {
        // Get a handle on the script of the NewStatus
        Status new_status = NewStatus.GetComponent<Status>();

        // Set handlers
        new_status.unit = unit;

        // Populate data
        new_status.universal = StatusAbstract.universal;
        new_status.stat_gen = StatusAbstract.stat_gen;
        new_status.buff_duration = StatusAbstract.buff_duration;
        new_status.sub_dmg_receive = StatusAbstract.sub_dmg_receive;

    }

    public GameObject add_status(string Status_name, Unit unit)
    {
        // Instantiate a status object
        GameObject new_status = Instantiate(status, unit.gameObject.transform);

        // Get the specified statusAbstract based on the Status_name
        StatusAbstract statusAbstract = get_StatusAbstract_byName(Status_name);

        // Assign statusAbstract parameters to a status new_status
        assign_status_parameters(ref statusAbstract, ref new_status, ref unit);

        // Return the new_status to the Unit, so it knows what new Status it has
        return new_status;
    }


}