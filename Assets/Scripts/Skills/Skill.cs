using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Structs;
using Targeting;

public class Skill : MonoBehaviour
{
    #region Inspector Parameters

    public Universal universal;

    public SkillGen general;

    public SkillCosts cost;

    public SkillGain gain;

    public SkillStatusInfo status_info;

    public SkillDamageInfo damage_info;

    [SerializeField] public TargetingType targeting_mode = TargetingType.SINGLE;

    #endregion

    public bool is_condition_met()
    {
        return true;
    }

}
