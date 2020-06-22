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


    public Universal universal;

    public SkillGen general;

    public SkillCosts cost;

    public SkillGain gain;

    public SkillStatusInfo status_info;

    public SkillDamageInfo damage_info;

    [SerializeField] private TargetingType targeting_mode = TargetingType.SINGLE;

    #endregion


    // Get information about how much damage this skill deals
    public int get_damage(int unit_base_damage)
    {
        return Convert.ToInt32(damage_info.deal_damage) * (unit_base_damage + damage_info.damage_modifier);
    }


}