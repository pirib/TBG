using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Structs;

public class Status : MonoBehaviour
{

    #region Handlers
    
    [Header("Handlers")]
    public Unit unit;
    public TextMesh text;

    #endregion

    #region Parameters

    // Param
    public Universal universal;

    // General
    public StatGen stat_gen;

    // Positive buffs that are applied for the duration of the Status and are removed, once the duration reaches 0
    public BuffDuration buff_duration;

    private bool buff_duration_applied = false;

    // Subscribe to damage receiving
    public SubDmgReceive sub_dmg_receive;

    // Subscribe to heal receiving 
    public SubHealReceive sub_heal_receive;

    #endregion

    // Deal damage, disable skills, at the beginning of the turn
    public void apply_status_effect()
    {
        // Apply Duration buffs once
        if (!buff_duration_applied) 
        { 
            unit.update_armor(buff_duration.armor);
            unit.update_attack(buff_duration.attack);
            buff_duration_applied = true;
        }

        // Apply positive per turn effects
        if (stat_gen.heal_turn > 0) unit.heal(stat_gen.heal_turn, true);
        if (stat_gen.rage_turn > 0) unit.update_rage(stat_gen.rage_turn);

        // Apply turn / end turn damage
        if (stat_gen.duration == 1 && stat_gen.damage_end > 0) unit.receive_damage(stat_gen.damage_end, true, true);
        else if (stat_gen.damage_turn > 0) unit.receive_damage(stat_gen.damage_turn, true, true);

        // Apply per turn negative effects
        if (stat_gen.rage_turn < 0) unit.update_rage(stat_gen.rage_turn);

    }

    #region Status Duration

    // Update the duration of the status
    public void update_duration(int Turns)
    {
        stat_gen.duration += Turns;
        // Update the duration text
        text.text = stat_gen.duration.ToString();

        // Check if the status has expired
        check_expired();
    }

    // Check if the status effects should be wearing off now
    public bool expired = false;    // TODO make this properly
    void check_expired()
    {
        if (stat_gen.duration == 0)
        {
            // Rolling back armor and attack effects of the status
            unit.update_armor(-buff_duration.armor);
            unit.update_attack(-buff_duration.attack);

            expired = true;
        }
    }

    #endregion

    #region Delegates

    // Subscribe to delegates if needed 
    public void subscribe()
    {
        // Subscribing to the damage receiving
        if (sub_dmg_receive.enable) unit.OnDamageReceived += OnDamageReceived;
        if (sub_heal_receive.enable) unit.OnHealingReceieved += OnHealingReceived; 

    }

    // Delegate - triggers when the Unit receives damage from any source
    void OnDamageReceived(Unit source_unit = null)
    {
        // Do something (reflect back, heal, get rage, etc.)
        
        if (sub_dmg_receive.rage != 0) unit.update_rage(sub_dmg_receive.rage);
        if (sub_dmg_receive.weaken) unit.receive_damage(1, false, true);
        if (sub_dmg_receive.reflect_damage > 0 && source_unit != null) source_unit.receive_damage(sub_dmg_receive.reflect_damage, false);
    }

    void OnHealingReceived ()
    {

        if (sub_heal_receive.hp > 0) unit.heal(sub_heal_receive.hp, false);
        if (sub_heal_receive.rage > 0) unit.update_rage(sub_heal_receive.rage);

    }


    #endregion

}


