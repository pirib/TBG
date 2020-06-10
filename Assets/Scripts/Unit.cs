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
    [SerializeField] private int hp_cur;
    [SerializeField] private int hp_max;

    [SerializeField] private int rage_cur;
    [SerializeField] private int rage_max;

    [SerializeField] private int armor;
    [SerializeField] private bool is_melee;
    [SerializeField] private bool can_play;

    // Skills/statuses
    [SerializeField] private List<GameObject> statuses = new List<GameObject>();
    private List<GameObject> skills = new List<GameObject>();

    // Delegate
    public delegate int damage_register(int damage);
    public event damage_register OnDamageReceived;


    // Start is called before the first frame update
    void Start()
    {
        // Set the correct tag
        if (!is_player) this.tag = "Enemy";
        else this.tag = "Player";

        add_status("Burning");
        add_status("Enraged");

        TurnManager.instance.end_turn(this);
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

    // Check the incoming damage, and modify it based on the subscribers response
    public void receive_damage (int incoming_damage, Unit unit_dmg_source = null)
    {
        // A new damage variable that will be modified based on subscribers' response (statuses, relics)
        int damage = incoming_damage;

        // Let all the delegate subscribers know that a damage is about to be dealt by another unit, and get their feedback on damage change
        if (unit_dmg_source != null) damage += OnDamageReceived(damage);
       

        if (damage > 0) {
            // ADD receive_damage animation

            // Check with the armor 
            if (damage > armor) hp_cur = hp_cur - damage + armor;
            else hp_cur -= 1;
        }

        //ADD update HUD

        // Call death function if the hp falls below 1
        if (hp_cur < 1) this.death();        
    }

    public void heal (int hp)
    {
        if (hp_cur + hp > hp_max) hp_cur = hp_max;
        else hp_cur = +hp;
    }

    public void update_rage (int rage)
    {
        if (rage_cur + rage > rage_max) rage_cur = rage_max;
        else if (rage_cur + rage < 0) rage_cur = 0;
        else rage_cur += rage;
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
        // Instantiate a new status, fetching it from the StatusManager and add it to the list of active statuses of this unit
        statuses.Add(StatusManager.instance.add_status(Status_name, this));       
    }



    // <================================================== AI

    void do_ai()
    {

    }

}
