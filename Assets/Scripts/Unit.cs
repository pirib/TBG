using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{

    [Header("Prefabs")]
    public GameObject status;


    /* General Unit properties */
    public bool is_player;

    // Attack
    [SerializeField] private int attack_basic;

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

    // Apply things on the start of the turn
    public void turn_start ()
    {
        foreach (GameObject status in statuses)
        {
            status.GetComponent<Status>().apply_status_effect();
        }

        if (!is_player) do_ai();
        

    }

    public void receive_damage (int Damage)
    {
        // ADD receive_damage animation

        // Check with the armor 
        if (Damage - armor > 0) hp = hp - Damage + armor;
        else hp -= Damage;
        
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
        int value;
        if (GameManager.instance.dic.TryGetValue(Status_name, out value) ) {
            // Get a hold of information about the particular status and Instantiate a new empty status
            StatusAbstract status_abstract = GameManager.instance.statuses[value];
            GameObject new_status = Instantiate(status);

            // Assign new status parameters
            assign_status_parameters(ref status_abstract, ref new_status);

            // Add status to the list of active statuses
            statuses.Add( new_status  );
        }
        else
        {
            Debug.Log("Did not find status with a name " + Status_name);
        }
        
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
            ref StatusAbstract.status_icon,
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


class Skill
{

}
