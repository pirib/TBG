using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Targeting;

public class SkillTarget : MonoBehaviour
{

    #region Declaring Static
    public static SkillTarget instance;

    private void Awake()
    {
        instance = this;
    }

    #endregion 

    [Header("Selection Pool")]

    // Is the player currently choosing units?
    [SerializeField] private bool actively_targeting = false;

    // The skill currently waiting for targeting
    [SerializeField] private Skill active_skill;

    // Pool of units the user can select from
    [SerializeField] private List<Unit> selection_pool = new List<Unit>();

    // Pool of units that is passed onto the skill
    [SerializeField] private List<Unit> selected_units = new List<Unit>();

    [Header("Targeting")]
    public GameObject target_indicator;
    public List<GameObject> target_indicators = new List<GameObject>();

    public void set_skill(Skill skill)
    {
        // Get the handle of the skill that is currently being considered for using
        active_skill = skill;
        
        // Meaning a unit/player is actively targeting
        actively_targeting = true;

        // Update the selection pool with possible targets
        set_selection_pool(skill.general.targeting_mode);
    }

    // Sets selection pool to possible targets
    private void set_selection_pool(TargetingType targeting)
    {
        // Clear up the selection pool (just in case?)
        clear_selection_pool();

        // Clear the previously targeted targets
        clear_targeting_indicators();

        // Update the targeting pool with possible targets based on the skills targeting type
        if (targeting == TargetingType.PLAYER)
        {
            selection_pool.Add(TurnManager.instance.player);
        } else if (targeting == TargetingType.SINGLE || targeting == TargetingType.ENEMIES)
        {
            foreach (Unit unit in TurnManager.instance.queue) selection_pool.Add(unit);
            selection_pool.RemoveAt(0);
        } else if (targeting == TargetingType.ANY || targeting == TargetingType.ALL)
        {
            foreach (Unit unit in TurnManager.instance.queue) selection_pool.Add(unit);
        }
        else
        {
            Debug.Log("Invalid targeting type " + targeting);
            return;
        }

        Debug.Log("Selection pool is updated for targeting type " + targeting);
        
        // Set the GUI indicator if potential targets
        foreach(Unit unit in selection_pool)
        {
            // Instantiate and add target_indicators
            target_indicators.Add( Instantiate(target_indicator, unit.transform) );
        }

    }

    // This one is called only if the correct target has been chosen while a unit/player was actively targeting
    public void execute(Unit unit) {

        // Remove the target indicators
        clear_targeting_indicators();

        // Not targeting anymore
        actively_targeting = false;

        // Temporary list to send over to the skill
        List<Unit> execution_list = new List<Unit>();

        // Prepare the selected_units list for the skill
        if (active_skill.general.targeting_mode == TargetingType.SINGLE || active_skill.general.targeting_mode == TargetingType.ANY || active_skill.general.targeting_mode == TargetingType.PLAYER)
            execution_list.Add(unit);
        else if (active_skill.general.targeting_mode == TargetingType.ALL)
            foreach (Unit _unit in TurnManager.instance.queue) execution_list.Add(_unit);
        else if (active_skill.general.targeting_mode == TargetingType.ENEMIES)
        {
            foreach (Unit _unit in TurnManager.instance.queue) execution_list.Add(_unit);
            execution_list.RemoveAt(0);
        }

        // Actually execute the skill
         active_skill.execute_skill(execution_list);

        // Set the active skill to null
        active_skill = null;

        // Clear the pools
        clear_selection_pool();
        clear_selected_pool();
    }


    #region Helpers
    private void clear_selection_pool ()
    {
        selection_pool.Clear();
    }

    private void clear_selected_pool()
    {
        selected_units.Clear();
    }

    public bool in_the_targeting_pool(Unit unit)
    {
        if (selection_pool.Contains(unit)) return true;
        else return false;
    }

    public bool is_actively_targeting()
    {
        return actively_targeting;
    }
    
    public void clear_targeting_indicators ()
    {
        foreach (GameObject target_indicator in target_indicators)
        {
            Destroy(target_indicator);
        }

    }

    #endregion

}
