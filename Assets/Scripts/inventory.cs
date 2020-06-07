using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inventory : MonoBehaviour
{
    [Header("")]
    [SerializeField] private Relic sword_rune;
    [SerializeField] private Relic amulet;
    [SerializeField] private Relic magic_tome;
    [SerializeField] private Relic armor_rune;

    private List<Artefact> artefacts;

    public Total_artefacts_bonus calculate_artefact_bonuses(string param)
    {
        // Create a new 
        Total_artefacts_bonus temp = new Total_artefacts_bonus();
        temp.reset_bonuses();

        foreach (Artefact artefact in artefacts)
        {
            temp.add(artefact.health_bonus, artefact.armor_bonus, artefact.damage_bonus);
        }

        return temp;

    }

}

class Relic
{
    

}


class Artefact
{
    // Artefacts provide simple bonus to a stat;
    public int health_bonus = 0;
    public int armor_bonus = 0;
    public int damage_bonus = 0;
}


public class Total_artefacts_bonus
{
    private int hp;
    private int armor;
    private int damage;

    // Set all bonuses to zero
    public void reset_bonuses ()
    {
        hp = 0;
        armor = 0;
        damage = 0;
    }

    // Add a value to the total bonus
    public void add (int hp_bonus, int armor_bonus, int damage_bonus)
    {
        hp += hp_bonus;
        armor += armor_bonus;
        damage += damage_bonus;
    }

    public int get_hp() { return hp; }
    public int get_armor() { return armor; }
    public int get_damage() { return damage; }

}