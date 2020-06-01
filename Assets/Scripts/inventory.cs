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

    public void apply_artefact_bonuses()
    {
        foreach (Artefact artefact in artefacts)
        {

        }


    }


}

class Relic
{
    

}

class Artefact
{

    private int health_bonus;
    private int armor_bonus;
    private int damage_bonus;
    

}