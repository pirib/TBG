using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Structs;
using Targeting;

[CreateAssetMenu(fileName = "New Skill", menuName = "Skill")]
public class SkillAbstract : ScriptableObject
{
    #region Inspector Parameters

    [Header("General")]
    public Universal universal;

    [Header("General")]
    public SkillGen general;

    [Header("Costs")]
    public SkillCosts cost;

    [Header("Gains")]
    public SkillGain gain;

    [Header("Status")]
    public SkillStatusInfo status_info;

    [Header("Damage")]
    public SkillDamageInfo damage_info;

    [Header("Targeting")]
    [SerializeField] private TargetingType targeting_mode = TargetingType.SINGLE;

    #endregion


    // Get information about how much damage this skill deals
    public int get_damage(int unit_base_damage)
    {
        return Convert.ToInt32(damage_info.deal_damage) * (unit_base_damage + damage_info.damage_modifier);
    }


}