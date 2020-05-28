using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{
    [Header("Handlers")]
    public Unit unit;
    public TextMesh text;

    // General
    string description;
    int duration;

    // Negative effects
    int damage_end;
    int damage_turn;
    bool stun;

    // Positive effects
    int armor;
    int attack;


    public Status inst(string Name, string Description, ref Sprite Status_icon, int Duration, int Damage_turn, int Damage_end)
    {
        name = Name;
        description = Description;

        update_duration(Duration);

        damage_end = Damage_end;
        damage_turn = Damage_turn;

        // Set the sprite icon
        this.GetComponent<SpriteRenderer>().sprite = Status_icon;

        return this;
    }

    // Deal damage, disable skills, etc.
    public void apply_status_effect()
    {
        // Apply damage
        if (duration == 1 && damage_end > 0)
        {
            unit.receive_damage(damage_end);
        }
        else
        {
            unit.receive_damage(damage_turn);
        }

        // ADD Apply other effects

        // Decrrement duration by 1
        update_duration(-1);
        check_expired();
    }

    void update_duration(int Turns)
    {
        duration += Turns;
        // Update the duration text
        text.text = duration.ToString();
    }

    void check_expired()
    {
        if (duration == 0)
        {
            Destroy(this);
        }

    }

}


