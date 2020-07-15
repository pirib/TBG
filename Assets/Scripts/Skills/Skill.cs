using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Structs;
using Targeting;
using Charge;

public class Skill : MonoBehaviour
{
    #region Inspector Parameters

    public Universal universal;

    public SkillGen general;

    public SkillCosts cost;

    public SkillGain gain;

    public Structs.SkillStatusInfo status;

    public SkillChargeInfo charge;

    public SkillAdvanced skill_advanced;

    public SkillDamageInfo damage_info;

    #endregion

    #region

    #endregion

    #region Handlers
    [Header("Handlers")]
    public Unit owner_unit;
    public GameObject charge_ui;

    [SerializeField] private Sprite charge0;
    [SerializeField] private Sprite charge1;
    [SerializeField] private Sprite charge2;
    [SerializeField] private Sprite charge3;
    #endregion


    private void Start()
    {
        // Disable the charge_ui for those skills that are not chargeable
        if (!charge.chargeable) charge_ui.SetActive(false);    

    }

    public void execute_skill(List<Unit> targets)
    {

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
        if (charge.chargeable && charge.charge_lvl == 3) {

            if (charge.charge_mode == ChargeMode.DAMAGE_MODIFIER) total_damage = charge.value;
            else if (charge.charge_mode == ChargeMode.COST_AP) ap_cost = charge.value;
            else if (charge.charge_mode == ChargeMode.COST_HP) hp_cost = charge.value;
            else if (charge.charge_mode == ChargeMode.COST_RAGE) rage_cost = charge.value;
            else if (charge.charge_mode == ChargeMode.HEAL) hp_gain = charge.value;
            else if (charge.charge_mode == ChargeMode.STATUS) apply_status = charge.status;

            // ADD change skills charge level to 0 and update ui

        }

        // Set the cooldowns
        general.cooldown_cur = general.cooldown;

        // Pay the cost
        if (hp_cost != 0) owner_unit.receive_damage(hp_cost, true, false, owner_unit);
        if (ap_cost != 0) owner_unit.update_ap(ap_cost);
        if (rage_cost != 0) owner_unit.update_rage(rage_cost);

        // Calculate damage output
        if (damage_info.deal_damage) total_damage = total_damage + (System.Convert.ToInt32(damage_info.use_base_attack) * owner_unit.get_base_dmg()) + damage_info.damage_modifier;


        // Apply damage, status, etc. for each target in the list
        foreach (Unit unit in targets)
        {

            // Apply Status effects
            if (status.apply_status) unit.add_status(apply_status);

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

                if (skill_advanced.status_type != SkillStatusInfo.StatusChoice.ALL)
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
            if (damage_info.deal_damage) unit.receive_damage(total_damage, true, false, owner_unit);

        }

        // Skill gain
        if (hp_gain > 0) owner_unit.heal(hp_gain);
        if (ap_gain > 0) owner_unit.update_ap(ap_gain);
        if (rage_gain > 0) owner_unit.update_rage(rage_gain);

        // Skills cooldown
        if (skill_advanced.skill_cooldown != 0)
            foreach (GameObject skill in owner_unit.skills)
                skill.GetComponent<Skill>().update_cooldown(skill_advanced.skill_cooldown);

    }

    #region Skill execution helpers

    #region Charge

    #endregion

    #endregion

    #region General

    public void update_cooldown(int update_amount = -1)
    {
        if (update_amount == 0)
        {
            Debug.Log("A skill" + universal.name + " tried updating its cooldown by 0");
        } else if (general.cooldown_cur + update_amount < 0)
        {
            general.cooldown_cur = 0;
        } else
        {
            general.cooldown_cur += update_amount;
        }

        // ADD update the cooldown text / make the skill executable

    }

    #endregion


    #region GUI

    private void OnMouseDown()
    {
        Debug.Log("The player is considering using the skill " + this.universal.name);
        SkillTarget.instance.set_skill(this);
    }

    #endregion

}
