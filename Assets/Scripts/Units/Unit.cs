using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

using StatusTypes;
using Structs;

public class Unit : MonoBehaviour
{

    #region Prefabs

    [Header("Prefabs")]
    public GameObject status_pf;
    public GameObject skill_pf;

    public Tilemap hud;

    #endregion

    [Header("General")]
    /* Universal properties */
    public Universal universal;

    /* General Unit properties */
    public UnitGen general;

    /* Unit Parameters */
    public UnitParam unit_param;


    /* In-Fight params */
    [SerializeField] private int hp_cur;
    [SerializeField] private int ap_cur;
    [SerializeField] private int rage_cur;

    public bool can_play;

    // Statuses and skills
    [SerializeField] public List<GameObject> statuses = new List<GameObject>();
    [SerializeField] public List<Skill> skills = new List<Skill>();

    #region Delegates

    // Delegate Damage
    public delegate void damage_register(Unit source_unit);
    public event damage_register OnDamageReceived;

    // Delegate Healing
    public delegate void heal_register();
    public event heal_register OnHealingReceieved;

    // Delegate Status Receiving
    public delegate void status_received_register(StatusType status_type);
    public event status_received_register OnStatusReceived;

    #endregion

    #region Getters

    // TODO change current to cur
    public int get_current_hp()
    {
        return hp_cur;
    }
    public int get_max_hp()
    {
        return unit_param.hp_max;
    }

    public int get_cur_rage()
    {
        return rage_cur;
    }
    public int get_max_rage()
    {
        return unit_param.rage_max;
    }

    public int get_cur_ap()
    {
        return ap_cur;
    }
    public int get_max_ap()
    {
        return unit_param.ap_max;
    }


    public int get_base_dmg()
    {
        return unit_param.base_damage;
    }

    public int get_armor()
    {
        return unit_param.armor;
    }

    public bool is_player()
    {
        return general.is_player;
    }
    
    public bool is_mindless()
    {
        return general.is_mindless;
    }

    public string enemy_name()
    {
        return universal.name;
    }
    #endregion

    #region Setters
    public void update_armor(int change)
    {
        unit_param.armor += change;
    }
    public void update_attack(int change)
    {
        unit_param.base_damage += change;
    }


    #endregion 


    public void ready() { }

    // Start is called before the first frame update
    public void Start()
    {

        // Set correct animations
        set_unit_animations();

        // Set a correct collider
        set_collider();

        // Equalize cur with max
        set_unit_params();

        // Set the hud
        update_hud();


        // Debugging stuff
        if (general.is_player)
        {

        }

        // Prep the units by setting the right skills, statuses, etc.
        if (general.is_player)
        {
            set_player_skills();
            this.tag = "Player";
            add_status("Regenerating");
        } else if (!general.is_player) {
            set_enemy_skills();
            this.tag = "Enemy";

        }
        else
        {
            Debug.Log("Something went horribly wrong with instantiating a unit." + this.GetInstanceID());
        }

    }


    #region inGame

    // Do things at the start of the turn
    public void turn_start()
    {

        // Update cooldowns of the skills
        update_skills_cooldown();

        // Apply Status effects on the start of the turn, update durations and remove the expired ones
        apply_status_effects();
        update_status_durations();
        remove_expired_statuses();

        // Set the AP
        update_ap(unit_param.ap_max);

        // Activate AI if it is not the player controlled unit's turn
        if (can_play && !general.is_player) do_ai();

        // End Turn automatically if the player cannot play (is stunned)
        if (!can_play) TurnManager.instance.end_turn(this);
    }

    // Update skill cooldiwn
    public void update_skills_cooldown()
    {
        foreach (Skill skill in skills)
        {
            skill.update_cooldown();
        }
    }

    // Apply status effects
    public void apply_status_effects()
    {
        foreach (GameObject status in statuses)
        {
            status.GetComponent<Status>().apply_status_effect();

            // Update the hud after every status update
            update_hud();
        }
    }

    // Update duration of the statuses
    public void update_status_durations(int change = -1)
    {
        foreach (GameObject status in statuses)
        {
            status.GetComponent<Status>().update_duration(change);
        }
    }

     // Expired Status check
    public void remove_expired_statuses() {
        // Number of expired statuses
        int num_expired = 0;

        // Check for the expired ones
        foreach (GameObject status in statuses)
        {
            if (status.GetComponent<Status>().expired) num_expired = num_expired + 1;
        }

        // Remove expired statuses
        for (int i = 0; i <= num_expired; i++)
        {
            foreach (GameObject status in statuses)
            {
                if (status.GetComponent<Status>().expired)
                {
                    statuses.Remove(status);
                    Destroy(status);
                    break;
                }
            }
        }
    }

    // Pick status types
    public List<GameObject> pick_statuses_by_type(StatusType status_type)
    {
        List<GameObject> temp = new List<GameObject>();

        if (status_type == StatusType.ALL) return statuses;

        else
        {
            foreach (GameObject status in statuses)
            {
                if (status_type == StatusType.POSITIVE && status_type == status.GetComponent<Status>().stat_gen.type) temp.Add(status);
                else if (status_type == StatusType.NEGATIVE && status_type == status.GetComponent<Status>().stat_gen.type) temp.Add(status);
            }

            return temp;
        }

    }

    // Receive the incoming damage, and modify it based on the armor, subscribers response etc..
    public void receive_damage (int incoming_damage, bool is_primary = true, bool is_status = false, Unit source_unit = null)
    {

        if (incoming_damage  > 0) {
            // ADD receive_damage animation

            // If damage source is status, ignore armor
            if (is_status) hp_cur = hp_cur - incoming_damage; 
            // Check with the armor 
            else if (incoming_damage > unit_param.armor) hp_cur = hp_cur - incoming_damage + unit_param.armor;
            else hp_cur -= 1;

            // Let the subscribers know that the damage has been received
            if (is_primary)
                try { OnDamageReceived(source_unit); } 
                catch { Debug.Log("Exceptions - no subscribers were found, skipping OnDamageReceived"); }
        } 
        else {
            Debug.Log("Something tried attacking with 0 damage. Unit name" + source_unit.name);
        }

        //Update the Hud
        update_hud();

        // Call death function if the hp falls below 1
        if (hp_cur < 1) this.death();  
    }

    public void heal ( int hp, bool is_primary = true)
    {
        Debug.Log(this.name + " unit is healing by " + hp);
        if (hp_cur + hp > unit_param.hp_max) hp_cur = unit_param.hp_max;
        else hp_cur = hp_cur + hp;

        // Let the subscribers know that the healing has been done
        try { OnHealingReceieved(); }
        catch { Debug.Log("Exceptions - no subscribers were found, skipping OnHealingReceived"); }

        // Update the hud
        update_hud();
    }

    public void update_rage (int rage, bool is_primary = true)
    {
        if (rage_cur + rage >= unit_param.rage_max) rage_cur = unit_param.rage_max;
        else if (rage_cur + rage < 0) rage_cur = 0;
        else rage_cur = rage_cur + rage;

        // Update the hud
        update_hud();
    }

    public void update_ap(int change)
    {
        if (ap_cur + change >= unit_param.ap_max) ap_cur =+ unit_param.ap_max;
        else if (ap_cur + change < 0) Debug.Log("Ap dropped below zero. Unit " + this.universal.name );
        else ap_cur = ap_cur + change;

        // Update the hud
        update_hud();
    }

    public void update_hud()
    {
        HUD_control.instance.update_hud(this, hud );
    }

    void death()
    {
        // ADD death animation

        // If not a player
        if (!general.is_player)
        {

            // ADD Get xp/gold/update stats/whatever

            // Remove from the queue
            TurnManager.instance.remove_from_queue(this);

          
        }
        // If the player died
        else
        {
            // ADD Game_over transition

        }

    }

    void set_unit_params()
    {
        hp_cur = unit_param.hp_max;
        ap_cur = unit_param.ap_max;
        rage_cur = 0;

        can_play = true;
    }


    #endregion

    #region Status

    public void add_status (string Status_name, Unit source_unit = null)
    {
        // Instantiate a new status, fetching it from the StatusManager and add it to the list of active statuses of this unit
        statuses.Add(StatusManager.instance.add_status(Status_name, this));      
        
        // Alert subscribers
        
        // The unit has received a status
        try { OnStatusReceived(statuses[statuses.Count - 1].GetComponent<Status>().stat_gen.type); }
        catch { Debug.Log("Exceptions - no subscribers were found, skipping OnStatusReceived"); }


    }


    #endregion

    #region Skill

    // Cleans and Places players skills
    private void set_player_skills()
    {
        Debug.Log("Removing old player skills");
        for (int skill_index = general.skills.Count-1; skill_index >= 0; skill_index -- )
        {
            if (general.skills[skill_index] != null) Destroy(skills[skill_index]);
        }

        Debug.Log("Setting player skills");
        // Add all skills for each Relic in users inventory
        foreach (Relic relic in Inventory.instance.inventory)
        {
            foreach (string skill_name in relic.skills)
            {
                // TODO change skills
                skills.Add(SkillManager.instance.add_skill( skill_name, this) );
            }
        }

        Debug.Log("Aligning player skills.");

        float skill_icon_height = 28;
        float start_point = Mathf.Floor((Camera.main.orthographicSize - (Camera.main.orthographicSize*2 - general.skills.Count * skill_icon_height/2)/2));

        int i = 0;
        foreach (Skill skill in skills)
        {
            float x;
            float y;
            Skill skill_script = skill;

            // Even ones stay on the left
            if (i % 2 == 0)
            {
                x = -Mathf.Floor ( Camera.main.aspect * Camera.main.orthographicSize) + skill_icon_height/2 + 8 ;
                y = Mathf.Floor((start_point - i * (skill_icon_height/2) ));
                if (skill_script.charge.chargeable) skill_script.charge_ui.transform.position = new Vector3(-9.5f, -9.5f); 
            }
            else // Odd ones are nudged to the right
            {
                x = -Mathf.Floor( Camera.main.aspect * Camera.main.orthographicSize) + skill_icon_height + 8;
                y = Mathf.Floor(start_point - i * (skill_icon_height/2));
                if (skill_script.charge.chargeable) skill_script.charge_ui.transform.position = new Vector3( 9.5f, 9.5f);
            }

            skill.transform.position = new Vector3( x, y );
            i++;
        } 
    }

    // Sets the enemy's skills
    private void set_enemy_skills ()
    {
        foreach (string skill_name in general.skills)
        {
            SkillManager.instance.add_skill(skill_name, this);
        }
    }

    #endregion

    #region AI

    void do_ai()
    {
        Debug.Log("Unit " + this.name + "is doing ai stuff");
        
        // Use skills while there are any usable ones. First skills have higher priority.
        while (usable_skills() > 0)
        {
            // Loop through the skills
            foreach (Skill skill in skills)
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
        if (hp_cur < rage) return true;
        else return false;
    }

    public bool is_hp_lower_than(int hp)
    {
        if (hp_cur < hp) return true;
        else return false;
    }


    // Get the number of usable skills
    public int usable_skills()
    {
        int temp = 0;
        foreach (Skill skill in skills)
        {
            if (is_skill_usable(skill)) temp += 1;
        }
        return temp;
    }

    // Checks if the skill is usable - e.g. the unit can pay its costs, the cooldown is higher than zero, and the condition check returns true
    public bool is_skill_usable(Skill skill)
    {
        // Return True if the the unit can pay the skill's costs, is not on cooldown and it passes its conditions
        // NB! HP cost check is skipped
        if (skill.cost.ap_cost >= ap_cur && skill.cost.rage_cost >= rage_cur && skill.cooldown() == 0 && skill.passes_conditions()) return false;
        // Else, return true
        else return true;
    }


    #endregion

    #endregion

    #region Controls + Targeting

    private void OnMouseDown()
    {
        if (SkillTarget.instance.is_actively_targeting() && SkillTarget.instance.in_the_targeting_pool(this)) {
            SkillTarget.instance.execute(this);
            return;
        }
        Debug.Log("Unit " + this.name + " is being clicked while not in a targeting pool");
        Debug.Log("Is actively targeting set: " + SkillTarget.instance.is_actively_targeting() );
    }

    #endregion

    #region Animation
    private void set_unit_animations()
    {
        this.gameObject.GetComponent<Animator>().runtimeAnimatorController = general.unit_animations;
    }

    #endregion

    #region Collider

    private void set_collider()
    {
        gameObject.GetComponent<BoxCollider2D>().size = new Vector2(32,32);
        gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(0, 8);
    }

    #endregion



}
