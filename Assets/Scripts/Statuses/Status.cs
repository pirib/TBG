using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Structs;

public class Status : MonoBehaviour
{
    [Header("Handlers")]
    public Unit unit;
    public TextMesh text;

    // Param
    public Universal universal;

    // General
    public StatGen stat_gen;

    // Positive buffs that are applied for the duration of the Status and are removed, once the duration reaches 0
    public BuffDuration buff_duration;

    // Subscribe to damage receiving
    public SubDmgReceive sub_dmg_receive;
    
    // Subscribe to delegates if needed 
    public void subscribe()
    {
        // Subscribing to the damage receiving
        if (sub_dmg_receive.enable) unit.OnDamageReceived += OnDamageReceived;

    }

    // Deal damage, disable skills, at the beginning of the turn
    public void apply_status_effect()
    {
        // Apply damage
        if (stat_gen.duration == 1 && stat_gen.damage_end > 0)
        {
            unit.receive_damage(stat_gen.damage_end);
        }
        else
        {
            unit.receive_damage(stat_gen.damage_turn);
        }

        // ADD Apply other effects

        // Decrrement duration by 1
        update_duration(-1);
        check_expired();
    }

    // Update the duration of the status
    void update_duration(int Turns)
    {
        stat_gen.duration += Turns;
        // Update the duration text
        text.text = stat_gen.duration.ToString();
    }

    // Check if the status effects should be wearing off now
    void check_expired()
    {
        if (stat_gen.duration == 0)
        {
            Destroy(this);
        }
    }

    // Delegate - triggers when the Unit receives damage from any source
    void OnDamageReceived(int damage)
    {
        // Do something (reflect back, heal, get rage, etc.)
        
        if (sub_dmg_receive.rage != 0) unit.update_rage(sub_dmg_receive.rage);

    }

}


