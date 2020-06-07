using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Artefact", menuName = "Artefact")]
public class Artefact : Item
{
    // TODO fix the protection level for parameters
    // Artefacts provide simple bonus to a stat;
     
    // Bonuses
    public int health_bonus = 0;
    public int armor_bonus = 0;
    public int damage_bonus = 0;

}