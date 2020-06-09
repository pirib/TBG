using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{

    [Header("Prefabs")]
    public GameObject status;

    [Header("General")]
    /* General Unit properties */
    public bool is_player;

    // AP
    [SerializeField] private int ap;
    [SerializeField] private int ap_max;

    // Attack
    [SerializeField] private int base_damage;

    // Defence
    [SerializeField] private int hp;
    [SerializeField] private int hp_max;

    [SerializeField] private int rage;
    [SerializeField] private int rage_max;

    [SerializeField] private int armor;
    [SerializeField] private bool is_melee;
    [SerializeField] private bool can_play;

    // Skills/statuses
    private List<GameObject> statuses = new List<GameObject>();
    private List<Skill> skills = new List<Skill>();


    // Start is called before the first frame update
    void Start()
    {
        this.tag = "Enemy";
    }


    // <================================================== General

    // Do things at the start of the turn
    public void turn_start ()
    {
        // Apply Status effects on the start of the turn
        foreach (GameObject status in statuses)
        {
            status.GetComponent<Status>().apply_status_effect();
        }

        // Activate AI if it is not the player controlled unit's turn
        if (!is_player) do_ai();
        

    }

    public void receive_damage (int Damage)
    {
        // ADD receive_damage animation

        // Check with the armor 
        if (Damage - armor > 0) hp = hp - Damage + armor;
        else hp -= Damage;
        
        //ADD update HUD

        // Call death function if the hp falls below 1
        if (hp < 1) this.death();        
    }

    void death ()
    {
        // ADD death animation

        // If not a player
        if (!is_player) 
        {

            // ADD Get xp/gold/update stats/whatever

            // Remove from the queue
            TurnManager.instance.remove_from_queue(this);

            // Destroy the object 
            Destroy(this.gameObject);

        }
        // If the player died
        else 
        {
            // ADD Game_over transition

        }

    }


    // <============================================== Status related

    void add_status (string Status_name)
    {
        // Get a hold of information about the particular status 
        StatusAbstract status_abstract = GameManager.instance.get_StatusAbstract_byName(Status_name);
        
        // Instantiate a new empty status
        GameObject new_status = Instantiate(status);

        // Assign new status parameters
        assign_status_parameters(ref status_abstract, ref new_status);

        // Add status to the list of active statuses of this unit
        statuses.Add( new_status );       
    }

    void assign_status_parameters(ref StatusAbstract StatusAbstract, ref GameObject NewStatus)
    {
        // Get a handle on the script
        Status new_status = NewStatus.GetComponent<Status>();

        // Set handlers
        new_status.unit = this;

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



    // <================================================== AI

    void do_ai()
    {

    }

}
