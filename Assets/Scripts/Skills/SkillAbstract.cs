using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Skill", menuName = "Skill")]
public class SkillAbstract : ScriptableObject
{
    #region Inspector Parameters

    [Header("General")]
    [SerializeField] private new string name = "";
    [SerializeField] private string description = "";
    [SerializeField] private Sprite icon;

    [Header("Costs")]
    [SerializeField] private int ap_cost = 0;
    [SerializeField] private int hp_cost = 0;
    [SerializeField] private int rage_cost = 0;

    [Header("Targeting")]
    [SerializeField] private Targeting targeting_mode = Targeting.Single;

    [Header("Status")]
    [SerializeField] private bool apply_status = false;
    [SerializeField] private string status_name;

    [Header("Damage")]
    [SerializeField] private bool deal_damage = false;
    [SerializeField] private bool use_base_attack = false;
    [SerializeField] private int damage_modifier = 0;   // modifies by adding this to the final output

    #endregion

    #region Helpers

    private enum Targeting { Single, Enemies, All, Self };

    #endregion


    // Get information about how much damage this skill deals
    public int get_damage( int unit_base_damage)
    {
        return Convert.ToInt32(deal_damage) * (unit_base_damage + damage_modifier);
    }


}
