﻿using System.Collections;
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

    #region Unit Properties
    [Header("General")]
    /* Universal properties */
    public Universal universal;

    /* General Unit properties */
    public UnitGen general;

    /* Unit Parameters */
    public UnitParam unit_param;
    #endregion

    #region In fight parameters
    /* In-Fight params */
    [SerializeField] private int hp_cur;
    [SerializeField] private int ap_cur;
    [SerializeField] private int rage_cur;

    public bool can_play;

    // Statuses and skills
    [SerializeField] public List<GameObject> statuses = new List<GameObject>();
    [SerializeField] public List<Skill> skills = new List<Skill>();
    #endregion

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
            add_status("Berserk");
            add_status("Chosen");
            add_status("Enraged");
        } else
        {
            add_status("Chosen");
            add_status("Enraged");
        }

        // Prep the units by setting the right skills, statuses, etc.
        if (general.is_player)
        {
            set_player_skills();
            this.tag = "Player";
        } else if (!general.is_player) {
            set_enemy_skills();
            this.tag = "Enemy";

        }
        else
        {
            Debug.Log("Something went horribly wrong with instantiating a unit. Unit name " + universal.name );
        }

    }

    #region Exposers

    public void start_unit_turn ()
    {
        StartCoroutine(turn_start());
         
    }

    #endregion

    #region inGame

    // Do things at the start of the turn
    public IEnumerator turn_start()
    {
        yield return new WaitForSeconds(1);

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

        // Update the hud
        update_hud();
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

        // Update the hud
        update_hud();
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
    public void receive_damage (int incoming_damage, bool is_primary = true, bool is_status = false, Unit source_unit = null , bool piercing = false)
    {

        if (incoming_damage  > 0) {
            // Bool is used to decide whether the unit received the damage after all
            bool damage_received = false;

            // If damage source is status or it is a piercing type, ignore armor
            if (is_status || piercing) { 
                hp_cur = hp_cur - incoming_damage;
                damage_received = true;
            }
            // Else, we are checking with the armor 
            else {  
                
                if (incoming_damage > unit_param.armor) { 
                    hp_cur = hp_cur - incoming_damage + unit_param.armor;
                    damage_received = true;
                }
                
                else if (incoming_damage <= unit_param.armor) {
                    Debug.Log("All damage has been blocked");
                }
            }

            // ADD Play receive damage animation if hp has changed itself
            if (!damage_received) Debug.Log("Damage has been reeived");

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
        Debug.Log(universal.name + " unit is healing by " + hp);
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

            // Update the players rage
            TurnManager.instance.player.update_rage(1);

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
        // Check if a status with current name exists already - destroy the old one, apply the new one
        if (unit_has_status(Status_name)) {
            GameObject status = pick_status_by_name(Status_name);
            
            statuses.Remove(status);
            Destroy(status);
        }
        
        // Instantiate a new status, fetching it from the StatusManager and add it to the list of active statuses of this unit
        statuses.Add(StatusManager.instance.add_status(Status_name, this));

        // Place the status in the right spot
        set_status_GUI();

        // Alert subscribers        
        // The unit has received a status
        try { OnStatusReceived(statuses[statuses.Count - 1].GetComponent<Status>().stat_gen.type); }
        catch { Debug.Log("Exceptions - no subscribers were found, skipping OnStatusReceived"); }

    }

    #region Status Helpers

    private bool unit_has_status(string status_name)
    {
        foreach (GameObject status in statuses)
        {
            if (status.GetComponent<Status>().universal.name == status_name) 
                return true;
        } 

        return false;
    }

    private GameObject pick_status_by_name(string status_name)
    {
        foreach (GameObject status in statuses)
        {
            if (status.GetComponent<Status>().universal.name == status_name)
                return status;
        }

        return null;
    }

    private void set_status_GUI ()
    {
        if (is_player())
            for (int i = 0; i < statuses.Count; i++)
                statuses[i].transform.position = new Vector3(-188 + i*12, 64+8, -3);
        else
        {
            for (int i = 0; i < statuses.Count; i++)
                statuses[i].transform.position = new Vector3(transform.position.x + 24 + i*12 , transform.position.y + 4, -3);
        }

  
        
    }

    #endregion

    #endregion

    #region Skill

    // Cleans and Places players skills
    private void set_player_skills()
    {
        Debug.Log("Setting player skills.");

        for (int skill_index = general.skills.Count-1; skill_index >= 0; skill_index -- )
        {
            if (general.skills[skill_index] != null) Destroy(skills[skill_index]);
        }

        // Add all skills for each Relic in users inventory
        foreach (Relic relic in Inventory.instance.inventory)
        {
            foreach (string skill_name in relic.skills)
            {
                // TODO change skills
                skills.Add(SkillManager.instance.add_skill( skill_name, this) );
            }
        }

        // Aligning

        float skill_icon_height = 28;
        float start_point = Mathf.Floor((Camera.main.orthographicSize - 14 - (Camera.main.orthographicSize * 2 - (skills.Count) * skill_icon_height/2) /2));

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

                if (skill_script.charge.chargeable) 
                    skill_script.charge_ui.transform.position = new Vector3(-9.5f, -9.5f); 
            }
            else // Odd ones are nudged to the right
            {
                x = -Mathf.Floor( Camera.main.aspect * Camera.main.orthographicSize) + skill_icon_height + 8;
                y = Mathf.Floor(start_point - i * (skill_icon_height/2));

                if (skill_script.charge.chargeable) 
                    skill_script.charge_ui.transform.position = new Vector3( 9.5f, 9.5f);
            }

            skill.transform.position = new Vector3( x, y );
            i++;
        } 
    }

    // Sets the enemy's skills
    private void set_enemy_skills()
    {
        // Giving an enemy unit control over the skills
        foreach (string skill_name in general.skills)
        {
            skills.Add(SkillManager.instance.add_skill(skill_name, this));
        }

        // Moving the skills somewhere away from the camera
        // TODO Maybe hide them from direct view?
        foreach (Skill skill in skills)
        {
            skill.transform.position = new Vector3(-500,-500);
        }
    }

    #endregion

    #region AI

    void do_ai()
    {
        Debug.Log("Unit " + universal.name + " is doing ai stuff");
        
        // Use skills while there are any usable ones. First skills have higher priority.
        while (usable_skills() > 0)
        {
            Debug.Log("Found some usable skills");
            // Loop through the skills
            foreach (Skill skill in skills)
            {
                // If a skill is usable
                if ( skill.is_skill_usable() ) {

                    // Execute it on the the selected pool
                    skill.execute_skill(skill.get_picked_pool());
                } ;
            }
        }
        

        // End the turn, since no other action can be taken now
        TurnManager.instance.end_turn(this);
    }


    #region AI Helpers
  
    // Get the number of usable skills
    public int usable_skills()
    {
        int temp = 0;
        foreach (Skill skill in skills)
        {
            if ( skill.is_skill_usable() ) temp++ ;
        }
        Debug.Log("Usable skill num " + temp);
        return temp;
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
    // Sets all unit animations based on the ACO
    private void set_unit_animations()
    {
        this.gameObject.GetComponent<Animator>().runtimeAnimatorController = general.unit_animations;
    }

    // Play the animation based on the SkillAnimation type 
    public void play(SkillAnimation animation)
    {
        string animation_name = "idle";

        if (animation == SkillAnimation.ATTACK_NORMAL)
        {
            animation_name = "attack";
        }
        else if (animation == SkillAnimation.MAGIC_TARGET)
        {
            animation_name = "magic_target";
        }
        else
        {
            Debug.Log("The unit " + universal.name + " requested a non existent animation to be player " + animation);
        }

        GetComponent<Animator>().Play(animation_name);
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
