using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Structs;
using Targeting;

public class Skill : MonoBehaviour
{
    #region Inspector Parameters

    public Universal universal;

    public SkillCosts cost;

    public SkillGain gain;

    public SkillStatusInfo status_info;

    public SkillDamageInfo damage_info;

    [SerializeField] private TargetingType targeting_mode = TargetingType.SINGLE;

    #endregion


}
