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
    [SerializeField] private int ap_cur;
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
    [SerializeField] private List<GameObject> skills = new List<GameObject>();

    // Delegate
    public delegate void damage_register(int damage);
    public event damage_register OnDamageReceived;


    #region Getters

    public int get_current_hp()
    {
        return hp_cur;
    }

    public int get_base_dmg()
    {
        return base_damage;
    }

    public int get_armor()
    {
        return armor;
    }

    #endregion

    #region Setters
    public void update_armor(int change)
    {
        armor += change;
    }

    public void update_attack(int change)
    {
        base_damage += change;
    }

    #endregion 



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
        if (can_play && !is_player) do_ai();

        // End Turn automatically if the player cannot play (is stunned)
        if (!can_play) TurnManager.instance.end_turn(this);
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

    public void heal ( int hp)
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


    #region Status

    void add_status (string Status_name)
    {
        // Instantiate a new status, fetching it from the StatusManager and add it to the list of active statuses of this unit
        statuses.Add(StatusManager.instance.add_status(Status_name, this));       
    }

    #endregion

    #region Skill

    private void set_player_skills()
    {
        foreach (Relic relic in Inventory.instance.inventory)
        {
            foreach (string skill_name in relic.skills)
            {
                // TODO change skills
                skills.Add(SkillManager.instance.add_skill( skill_name , this) );
            }
        }

        // TODO center skill on the vertical axis

    }

    private void set_enemy_skills ()
    {
        // TODO add nemy skills based on the name of the enemy

        // TODO hide those skills elsewhere
    }

    #endregion

    #region AI

    void do_ai()
    {
        Debug.Log("Unit " + this.name + "is doing ai stuff");

        // Use skills while there are any usable ones. First skills have higher priority.
        while (usable_skills() > 0)
        {
            // Loop through the skill
            foreach (GameObject skill in skills)
            {
                if (is_skill_usable(skill.GetComponent<Skill>())) { 
                    // TODO activate the skill
                } ;
            }
        }

        // End the turn, since no other action can be taken now
        TurnManager.instance.end_turn(this);
    }


    #region AI Helpers
    
    public bool is_rage_lower_than (int rage)
    {
        if (rage_cur < rage) return true;
        else return false;
    }

    public bool is_hp_lower_than(int hp)
    {
        if (hp_cur < hp) return true;
        else return false;
    }

    public bool is_skill_usable(Skill skill)
    {
        // If the unit doesn have the skill's ap/hp/rage cost , or it is on cooldown, return false
        if (skill.cost.ap_cost > ap_cur || skill.cost.rage_cost > rage_cur || skill.cost.rage_cost > hp_cur || skill.general.cooldown_cur != 0 || skill.is_condition_met() ) return false; 
        // Else, return true
        else return true;
    }


    // Get the number of usable skills
    public int usable_skills()
    {
        int temp = 0;
        foreach (GameObject skill in skills)
        {
            if (is_skill_usable(skill.GetComponent<Skill>())) temp+=1;
        }
        return temp;
    }

    #endregion

    #endregion

}
