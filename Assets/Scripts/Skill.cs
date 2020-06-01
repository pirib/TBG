using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{

    [Header("Costs")]
    [SerializeField] private int ap_cost = 0;
    [SerializeField] private int hp_cost = 0;
    [SerializeField] private int rage_cost = 0;

    [Header("Status")]
    [SerializeField] private bool apply_status = false;
    [SerializeField] private Status status;

    [Header("Damage")]
    [SerializeField] private bool deal_damage = false;
    [SerializeField] private int damage_modifier = 0;   // modifies by adding this to the final output



    // Start is called before the first frame update
    void Start()
    {
        
    }

    public Status get_effect()
    {
        return status;
    }

    public int get_damage( int unit_base_damage)
    {
        return Convert.ToInt32(deal_damage) * (unit_base_damage + damage_modifier);
    }

}
