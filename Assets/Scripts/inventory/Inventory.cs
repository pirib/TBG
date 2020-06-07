using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
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



