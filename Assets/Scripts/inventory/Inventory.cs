using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    #region Static
    public static Inventory instance;

    private void Awake()
    {
        instance = this;
    }

    #endregion

    [Header("")]
    public List<Relic> inventory;
    // Max amount of space this inventory has
    public int capacity;

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



