using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Structs;
using Targeting;

public class Skill : MonoBehaviour
{
    #region Inspector Parameters

    [Header("General")]
    public Universal universal;

    [Header("Costs")]
    public SkillCosts cost;

    [Header("Targeting")]
    [SerializeField] private TargetingType targeting_mode = TargetingType.SINGLE;

    [Header("Status")]
    public SkillStatusInfo status_info;

    [Header("Damage")]
    public SkillDamageInfo damage_info;

    #endregion


}
