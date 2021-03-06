﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using StatusTypes;
using Structs;
using Targeting;
using Charge;

public class Skill : MonoBehaviour
{
    #region Skill Parameters

    public Universal universal;

    public SkillGen general;

    public SkillCosts cost;

    public SkillGain gain;

    public Structs.SkillStatusInfo status;

    public SkillChargeInfo charge;

    public SkillAdvanced skill_advanced;

    public SkillDamageInfo damage_info;

    public prerequisite prerequisite;

    public List<pooling> pooling;

    public picking picking;


    #endregion

    /* In-game Params */
    [SerializeField]

    #region Handlers
    [Header("Handlers")]
    public Unit owner_unit;
    public GameObject charge_ui;
    public GameObject dmg_casting_effect;
    public SpriteRenderer Icon;

    [Header("Not proud of it")]
    [SerializeField] private Sprite charge0;
    [SerializeField] private Sprite charge1;
    [SerializeField] private Sprite charge2;
    [SerializeField] private Sprite charge3;

    [Header("Cooldown")]
    [SerializeField] private TMPro.TextMeshPro cooldown_text;
    [SerializeField] private SpriteRenderer cooldown_bg;

    [Header("In game")]
    [SerializeField] private int cooldown_cur = 0;
    [SerializeField] private int charge_lvl = 0;

    #endregion

    private void Start()
    {
        // Disable the charge_ui for those skills that are not chargeable
        if (charge.chargeable)
        {
            // Set the ui active
            charge_ui.SetActive(true);
            set_delegates();

        }

        // Update the skills HUD
        update_skill_hud();

        // Set the Skill icons
        set_skill_icon();
    }

    // Execute the skill on the list of targets
    public void execute_skill(List<Unit> targets)
    {
        // Play animation
        owner_unit.play(general.skill_animation);

        // Debug stuff
        Debug.Log("Unit " + owner_unit.universal.name + " is executing skill " + universal.name);

        // Shake
        ShakeBehaviour.instance.TriggerShake();

        // Variables used in executing the skill
        int total_damage = 0;

        int hp_cost = cost.hp_cost;
        int ap_cost = cost.ap_cost;
        int rage_cost = cost.rage_cost;

        int hp_gain = gain.hp;
        int ap_gain = gain.ap;
        int rage_gain = gain.rage;

        string apply_status = status.status_name;

        // If the skill is charged
        if (charge.chargeable && charge_lvl == 3) {

            if (charge.charge_mode == ChargeMode.DAMAGE_MODIFIER) total_damage = charge.value;
            else if (charge.charge_mode == ChargeMode.COST_AP) ap_cost = charge.value;
            else if (charge.charge_mode == ChargeMode.COST_HP) hp_cost = charge.value;
            else if (charge.charge_mode == ChargeMode.COST_RAGE) rage_cost = charge.value;
            else if (charge.charge_mode == ChargeMode.HEAL) hp_gain = charge.value;
            else if (charge.charge_mode == ChargeMode.STATUS) apply_status = charge.status;

            // Setting charge lvl to 0
            update_charge_lvl(-3);
        }

        // Set the cooldowns
        cooldown_cur = general.cooldown;

        // Pay the cost
        if (hp_cost != 0) owner_unit.receive_damage(hp_cost, true, false, owner_unit);
        if (ap_cost != 0) owner_unit.update_ap(-ap_cost);
        if (rage_cost != 0) owner_unit.update_rage(-rage_cost);

        // Update the hud
        owner_unit.update_hud();

        // Calculate damage output
        if (damage_info.deal_damage) total_damage = total_damage + (System.Convert.ToInt32(damage_info.use_base_attack) * owner_unit.get_base_dmg()) + damage_info.damage_modifier;

        // Play the casting effect if exists
        if (general.casting_effect != null)
        {
            GameObject temp = Instantiate(dmg_casting_effect);
            temp.transform.position = new Vector3(owner_unit.gameObject.transform.position.x, owner_unit.gameObject.transform.position.y, -0.5f);
            
            temp.GetComponent<Animator>().runtimeAnimatorController = general.casting_effect;
        }

         // Apply damage, status, etc. for each target in the list
         foreach (Unit unit in targets)
        {
            // Play the damage effect if any exist
            if (general.damage_effect != null) {
                
                GameObject temp = Instantiate(dmg_casting_effect);
                temp.transform.position = new Vector3(unit.gameObject.transform.position.x, unit.gameObject.transform.position.y, -2);

                temp.GetComponent<Animator>().runtimeAnimatorController = general.damage_effect;
            }

            // Apply Status effects because the skill has it, or charged skill has chanrge mode of Status
            if (status.apply_status || (charge_lvl == 3 && charge.charge_mode == ChargeMode.STATUS) ) unit.add_status(apply_status);

            // Vampire attack
            if (damage_info.vampire_attack) owner_unit.heal(total_damage);

            // Skill status tricks
            if (skill_advanced.cooldown_change != 0)
            {
                unit.update_status_durations(skill_advanced.cooldown_change);
                unit.remove_expired_statuses();
            }

            // If the skill heals/deals damage based on the number of statuses in targets
            if (skill_advanced.count)
            {
                // Count the statuses
                int temp_count = 0;

                if (skill_advanced.status_type != StatusType.ALL)
                {
                    foreach (GameObject status in unit.statuses)
                    {
                        if (status.GetComponent<Status>().stat_gen.type == skill_advanced.status_type)
                        {
                            temp_count++;
                        }
                    }
                }
                else temp_count += unit.statuses.Count;

                // Deal Damage / Heal
                if (skill_advanced.deal_damage) unit.receive_damage(temp_count, true, false, owner_unit);
                if (skill_advanced.heal_self) owner_unit.heal(temp_count);
            }

            // Removes all statuses from the targets
            if (skill_advanced.remove)
            {
                // TODO do this properly
                unit.update_status_durations(-10);
                unit.remove_expired_statuses();
            }


            // TODO Status Swapping
            /*
            if (status_change.swap) {
                List<GameObject> temp = owner_unit.get_statuses();
                foreach (GameObject status in unit.get_statuses())
                    owner_unit.add_status(status.get)
            }*/

            // Deal Damage
            if (damage_info.deal_damage) unit.receive_damage(total_damage, true, false, owner_unit, damage_info.is_piercing);

        }

        // Skill gain
        if (hp_gain > 0) owner_unit.heal(hp_gain);
        if (ap_gain > 0) owner_unit.update_ap(ap_gain);
        if (rage_gain > 0) owner_unit.update_rage(rage_gain);

        // Skills cooldown
        if (skill_advanced.skill_cooldown != 0)
            foreach (Skill skill in owner_unit.skills)
                skill.GetComponent<Skill>().update_cooldown(skill_advanced.skill_cooldown);

        // Update hud again
        owner_unit.update_hud();

        // Update tthe skill hud
        update_skill_hud();
    }


    #region General

    // Updates the cooldown of this skill
    public void update_cooldown(int update_amount = -1)
    {
        if (update_amount == 0)
        {
            Debug.Log("A skill" + universal.name + " tried updating its cooldown by 0");
        } else if (cooldown_cur + update_amount < 0)
        {
            cooldown_cur = 0;
        } else
        {
            cooldown_cur += update_amount;
        }

        // Updating the skill cooldown info
        update_skill_hud();
    }

    // Updates the skill hud, including the charge levels
    public void update_skill_hud()
    {            
        if (cooldown_cur > 0)
        {
            cooldown_bg.gameObject.SetActive(true);
            cooldown_text.text = cooldown_cur.ToString();

        } else
        {
            cooldown_bg.gameObject.SetActive(false);
        }

        // Updating the charge
        if (charge.chargeable)
        {
            SpriteRenderer temp = charge_ui.GetComponent<SpriteRenderer>();

            if (charge_lvl == 0) temp.sprite = charge0;
            else if (charge_lvl == 1) temp.sprite = charge1;
            else if (charge_lvl == 2) temp.sprite = charge2;
            else if (charge_lvl == 3) temp.sprite = charge3;
            else Debug.Log("Shit went south in Skill " + universal.name + " charge value is ourside the bounds" );
        }

    }

    // Sets the skill icon
    private void set_skill_icon()
    {
        Icon.sprite = universal.icon;
    }


    #endregion

    #region Helpers
    // Returns current cooldown value
    public int cooldown()
    {
        return cooldown_cur;
    }

    // Updates the charge_lvl of this skill
    public void update_charge_lvl(int change = 1)
    {
        if (charge_lvl + change <= 3)
            charge_lvl = charge_lvl + change;
        update_skill_hud();
    }

    // Return a bool indicating whether the owner_unit can use the skill or not
    public bool is_skill_usable()
    {
        // Return True if the the unit can pay the skill's costs, is not on cooldown and it passes its conditions
        // NB! HP cost check is skipped
        if (cost.ap_cost <= owner_unit.get_cur_ap() && cost.rage_cost <= owner_unit.get_cur_rage() && cooldown() == 0 && passes_conditions() && prerequisites_met())
            return true;

        // Else, return false
        else
            return false;
    }

    #endregion

    #region Skill Charge Delegates
    void set_delegates()
    {
        // Subscribing to stuff
        if (charge.charge_condition == ChargeCondition.DAMAGE_RECEIVE)
            owner_unit.OnDamageReceived += OnDamageReceived;
        else if (charge.charge_condition == ChargeCondition.HEAL_RECEIVE)
            owner_unit.OnHealingReceieved += OnHealingReceived;
        else if (charge.charge_condition == ChargeCondition.STATUS_RECEIVE_POSITIVE || charge.charge_condition == ChargeCondition.STATUS_RECEIVE_NEGATIVE)
            owner_unit.OnStatusReceived += OnStatusReceived;
    }

    void OnDamageReceived(Unit source_unit = null)
    {
        update_charge_lvl();
        update_skill_hud();
    }
    
    void OnHealingReceived()
    {
        update_charge_lvl();
        update_skill_hud();
    }

    void OnStatusReceived(StatusType status_type )
    {
        if (status_type == StatusType.POSITIVE && charge.charge_condition == Charge.ChargeCondition.STATUS_RECEIVE_POSITIVE)
            update_charge_lvl();

        else if (status_type == StatusType.NEGATIVE && charge.charge_condition == Charge.ChargeCondition.STATUS_RECEIVE_NEGATIVE)
            update_charge_lvl();
 
        else
            Debug.Log("Status type was " + status_type + ", but the subscriber was expecting " + charge.charge_condition);

        update_skill_hud();
    }


    #endregion

    #region AI

    // Report back if the prerequisites are met for the skill to be used
    public bool prerequisites_met()
    {
        bool passed = false;

        if (prerequisite == prerequisite.NONE)
            passed = true;
        else if ((prerequisite == prerequisite.RAGE_NOT_MAX) && (owner_unit.get_cur_rage() < owner_unit.get_max_rage()))
            passed = true;
        else if ((prerequisite == prerequisite.HP_NOT_MAX) && (owner_unit.get_current_hp() < owner_unit.get_max_hp()))
            passed = true;
        else if ((prerequisite == prerequisite.HP_LESS_THAN_HALF) && (owner_unit.get_current_hp() <= owner_unit.get_max_hp() / 2))
            passed = true;
        else
            Debug.Log("Do not know how to handle the prerequisite " + prerequisite + " in unit " + owner_unit.universal.name);

        return passed;
    }

    // Report back if this skill passes the condition 
    public bool passes_conditions()
    {
        // if a pool for a skill exists, return true
        if (TurnManager.instance.pool_units(pooling, this).Count > 0)
            return true;
        else
            return false;
    }

    // Get back a list of potential targets
    public List<Unit> get_skill_pool()
    {
        return TurnManager.instance.pool_units(pooling , this);
    }

    // Pick a unit from the list of potential targets. Returns the whole list if picking conditions are set to ALL/NONE
    public List<Unit> get_picked_pool()
    {
        return TurnManager.instance.pick_unit(this);
    }

    #endregion

    #region GUI + Control

    private void OnMouseDown()
    {
        Debug.Log("The player is considering using the skill " + this.universal.name);
        SkillTarget.instance.set_skill(this);
    }

    #endregion

    #region Animation

    // Play


    #endregion

}
