using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Structs;

public class Status : MonoBehaviour
{
    [Header("Handlers")]
    public Unit unit;
    public TextMesh text;

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
    public SubHealReceive subb_heal_receive;

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
        if (stat_gen.heal_turn > 0) unit.heal(stat_gen.heal_turn);
        if (stat_gen.rage_turn > 0) unit.update_rage(stat_gen.rage_turn);

        // Apply turn / end turn damage
        if (stat_gen.duration == 1 && stat_gen.damage_end > 0) unit.receive_damage(stat_gen.damage_end);
        else if (stat_gen.damage_turn > 0) unit.receive_damage(stat_gen.damage_turn);

        // Apply per turn negative effects
        if (stat_gen.rage_turn < 0) unit.update_rage(stat_gen.rage_turn);

        // Decrrement duration by 1 and remove this status if has expired
        update_duration(-1);
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
    void check_expired()
    {
        if (stat_gen.duration == 0)
        {

            // Rolling back armor and attack effects of the status
            unit.update_armor(-buff_duration.armor);
            unit.update_attack(-buff_duration.attack);

            Destroy(this);
        }
    }

    #endregion

    #region Delegates

    // Subscribe to delegates if needed 
    public void subscribe()
    {
        // Subscribing to the damage receiving
        if (sub_dmg_receive.enable) unit.OnDamageReceived += OnDamageReceived;

    }

    // Delegate - triggers when the Unit receives damage from any source
    void OnDamageReceived(int damage)
    {
        // Do something (reflect back, heal, get rage, etc.)
        
        if (sub_dmg_receive.rage != 0) unit.update_rage(sub_dmg_receive.rage);

    }



    #endregion

}


