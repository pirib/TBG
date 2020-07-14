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

    [Header("Prefabs")]
    [SerializeField] private GameObject skill;

    [Header("List of all in-game Skills")]
    [SerializeField] private List<SkillAbstract> skills = new List<SkillAbstract>();


    // Returns SkillAbstract if the name Skill_name is within the list of all Skills
    private SkillAbstract get_SkillAbstract_byName(string Skill_name)
    {
        int skill_index = get_skill_index(Skill_name);

        if (skill_index != -1) return instance.skills[skill_index];
        else
        {
            Debug.Log("Did not find skill with a name " + Skill_name + "\n Returning null");
            return null;
        }

    }

    // Looks for a skill with a name Skill_name and returns its index. Returns -1 if doesnt find it.
    private int get_skill_index(string Skill_name)
    {
        foreach (SkillAbstract skill in skills)
        {
            if (skill.name == Skill_name) return skills.IndexOf(skill);
        }
        return -1;
    }

    // Moves over the parameters of SkillAbstract to NewSkill of the unit
    private void assign_skill_parameters(ref SkillAbstract SkillAbstract, ref GameObject NewSkill)
    {
        // Get a handle on the script of the NewSkill
        Skill new_skill = NewSkill.GetComponent<Skill>();

        // Populate data
        new_skill.universal = SkillAbstract.universal;
        new_skill.general = SkillAbstract.general;
        new_skill.cost = SkillAbstract.cost;
        new_skill.gain = SkillAbstract.gain;
        new_skill.status = SkillAbstract.status;
        new_skill.charge = SkillAbstract.charge;
        new_skill.skill_advanced = SkillAbstract.skill_advanced;
        new_skill.damage_info = SkillAbstract.damage_info;

    }

    // Adds a new Skill
    public GameObject add_skill(string Skill_name, Unit unit)
    {
        // Instantiate a skill object
        GameObject new_skill = Instantiate(skill /*, unit.gameObject.transform*/);

        // Get the specified skillAbstract based on the Skill_name
        SkillAbstract skillAbstract = get_SkillAbstract_byName(Skill_name);

        // Assign skillAbstract parameters to a skill new_skill
        assign_skill_parameters(ref skillAbstract, ref new_skill);

        // Assign owner unit
        new_skill.GetComponent<Skill>().owner_unit = unit;

        // Return the new_skill to the Unit, so it knows what new Skill it has
        return new_skill;
    }




}
