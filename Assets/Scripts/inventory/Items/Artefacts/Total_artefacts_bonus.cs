using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Total_artefacts_bonus
{
    private int hp;
    private int armor;
    private int damage;

    // Set all bonuses to zero
    public void reset_bonuses()
    {
        hp = 0;
        armor = 0;
        damage = 0;
    }

    // Add a value to the total bonus
    public void add(int hp_bonus, int armor_bonus, int damage_bonus)
    {
        hp += hp_bonus;
        armor += armor_bonus;
        damage += damage_bonus;
    }

    public int get_hp() { return hp; }
    public int get_armor() { return armor; }
    public int get_damage() { return damage; }

}