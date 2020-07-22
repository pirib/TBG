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

    public Structs.SkillStatusInfo status;

    public SkillChargeInfo charge;

    public SkillAdvanced skill_advanced;

    public SkillDamageInfo damage_info;

    public List<pooling> pooling;

    public List<picking> picking;

    #endregion

}