using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject status_pf;
    public GameObject skill_pf;

    [Header("General")]
    /* General Unit properties */
    public bool is_player;
    public string enemy_name;

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
    [SerializeField] private List<Skill> skills = new List<Skill>();

    // Delegate
    public delegate void damage_register(int damage);
    public event damage_register OnDamageReceived;


    // Start is called before the first frame update
    void Start()
    {

        // Prep the units by setting the right skills, statuses, etc.
        if (is_player)
        {
            set_player_skills();
            this.tag = "Player";
        } else if (!is_player) {
            set_enemy_skills();
            this.tag = "Enemy";
        } else
        {
            Debug.Log("Something went horribly wrong with instantiating a unit." + this.GetInstanceID());
        }
        
    }

    // <================================================== General
    #region General

    // Do things at the start of the turn
    public void turn_start ()
    {
        // Apply Status effects on the start of the turn
        foreach (GameObject status in statuses)
        {
            status.GetComponent<Status>().apply_status_effect();
        }

        // ADD other things that has to be calculated at the start of the turn

        // Activate AI if it is not the player controlled unit's turn
        if (!is_player) do_ai();
        
    }

    // Check the incoming damage, and modify it based on the subscribers response
    public void receive_damage (int incoming_damage)
    {

        if (incoming_damage  > 0) {
            // ADD receive_damage animation

            // Check with the armor 
            if (incoming_damage > armor) hp_cur = hp_cur - incoming_damage + armor;
            else hp_cur -= 1;

            // Let the subscribers know that the damage has been received
            OnDamageReceived(incoming_damage);
            
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

    void death()
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

    #endregion

    // <============================================== Status related
    #region Status

    void add_status (string Status_name)
    {
        // Instantiate a new status, fetching it from the StatusManager and add it to the list of active statuses of this unit
        statuses.Add(StatusManager.instance.add_status(Status_name, this));       
    }

    #endregion

    // <================================================== Skills
    #region Skill

    private void set_player_skills()
    {
        foreach (Relic relic in Inventory.instance.inventory)
        {
            foreach (Skill skill in relic.skills)
            {
                // TODO change skills
                skills.Add(skill);
            }
        }
    }

    private void set_enemy_skills ()
    {
         // TODO add nemy skills based on the name of the enemy


    }

    #endregion

    // <================================================== AI
    #region AI

    void do_ai()
    {
        Debug.Log("Unit " + this.name + "is doing ai stuff");
        TurnManager.instance.end_turn(this);
    }

    #endregion

    #region Getters
    
    public int get_current_hp()
    {
        return hp_cur;
    }

    public int get_base_dmg()
    {
        return base_damage;
    }
    
    #endregion

}
