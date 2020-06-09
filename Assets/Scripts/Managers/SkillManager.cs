using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{

    #region Static
    public static SkillManager instance;

    private void Awake()
    {
        instance = this;
    }

    #endregion

    [SerializeField] private List<SkillAbstract> skills = new List<SkillAbstract>();

    // Looks for a skill with a name Skill_name and returns its index. Returns -1 if doesnt find it.
    private int get_skill_index(string Skill_name)
    {
        foreach (SkillAbstract skill in skills)
        {
            if (skill.name == Skill_name) return skills.IndexOf(skill);
        }
        return -1;
    }



}
