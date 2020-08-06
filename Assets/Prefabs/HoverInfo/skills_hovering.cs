using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Tilemaps;


public class skills_hovering : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject hover_prefab;

    [Header("Handlers")]
    [SerializeField] private Skill skill;
    [SerializeField] private SpriteRenderer background;
    [SerializeField] private List<Sprite> backgrounds = new List<Sprite>();
    [SerializeField] private TileBase[] tile_cost;


    // Private shit 
    private bool is_hovering;           // Using this to skip unnecessary update checks
    private GameObject hover_info;      // The handlers of the created Hover Info objects

    // When mouse is over the 
    private void OnMouseOver()
    {
        // If we are not already hovering, show the hover info
        if (!is_hovering && !SkillTarget.instance.is_actively_targeting()) 
            show_hover_info();

    }

    // Basically makes things the way they are
    public void show_hover_info()
    {
        if (!is_hovering) { 
            // Started hovering!
            is_hovering = true;

            // Change the background
            background.sprite = backgrounds[1];

            // Create a Hover Info object
            hover_info = Instantiate(hover_prefab /* , this.transform */);


            // Nudge it to the side
            hover_info.transform.position = new Vector3(384/2 - 36, hover_info.transform.position.y, hover_info.transform.position.z);

            // Make some stuff easy to access
            hover_info _hover = hover_info.GetComponent<hover_info>();

            // Set the correct icon
            _hover.skill_icon.sprite = skill.universal.icon;

            // Add the cost
            #region
            int start_point =  -(skill.cost.hp_cost + skill.cost.rage_cost + skill.cost.ap_cost) / 2 ;

            // Center it if the cost is even
            if ( (skill.cost.hp_cost + skill.cost.rage_cost + skill.cost.ap_cost) % 2 == 0)
            {
                Grid temp = _hover.GetComponentInChildren<Grid>();
                temp.transform.position = new Vector3(temp.transform.position.x + 5, temp.transform.position.y, temp.transform.position.z);
            }
        
            for (int i = 0; i < skill.cost.hp_cost; i++) _hover.hud.SetTile(new Vector3Int(start_point + i, 0, 0), tile_cost[0]);
            for (int i = 0; i < skill.cost.rage_cost; i++) _hover.hud.SetTile(new Vector3Int(start_point + skill.cost.hp_cost + i, 0, 0), tile_cost[2]);
            for (int i = 0; i < skill.cost.ap_cost; i++) _hover.hud.SetTile(new Vector3Int(start_point + skill.cost.hp_cost + skill.cost.rage_cost + i , 0, 0), tile_cost[4]);
            #endregion

            // Populate the fields
            _hover.title.text = skill.universal.name;
            _hover.description.text = skill.universal.description;

            // Charge info
            if (skill.charge.chargeable) {
                #region Condition Description
                if (skill.charge.charge_condition == Charge.ChargeCondition.STATUS_RECEIVE_NEGATIVE)
                    _hover.condition_description.text = "Charges every time you receive a negative status.";

                else if (skill.charge.charge_condition == Charge.ChargeCondition.STATUS_RECEIVE_POSITIVE)
                    _hover.condition_description.text = "Charges every time you receive a positive status.";

                else if (skill.charge.charge_condition == Charge.ChargeCondition.DAMAGE_RECEIVE)
                    _hover.condition_description.text = "Charges every time you receive damage.";
            
                else if (skill.charge.charge_condition == Charge.ChargeCondition.HEAL_RECEIVE)
                    _hover.condition_description.text = "Charges every time you heal.";
            
                else 
                    Debug.Log("Charge hover info cannot be loaded because charge condition " + skill.charge.charge_condition + " text has not been added. ");
                #endregion

                #region Mode Description
                // For easy access
                string mode_description = _hover.mode_description.text;

                if (skill.charge.charge_mode == Charge.ChargeMode.COST_AP)
                    _hover.mode_description.text = "Modifies the AP cost by -" + skill.charge.value + ".";

                else if (skill.charge.charge_mode == Charge.ChargeMode.COST_HP)
                    _hover.mode_description.text = "Modifies the HP cost by -" + skill.charge.value + ".";

                else if (skill.charge.charge_mode == Charge.ChargeMode.COST_RAGE)
                    _hover.mode_description.text = "Modifies the Rage cost by -" + skill.charge.value + ".";

                else if (skill.charge.charge_mode == Charge.ChargeMode.DAMAGE_MODIFIER)
                    _hover.mode_description.text = "Increases the damage dealt by " + skill.charge.value + ".";

                else if (skill.charge.charge_mode == Charge.ChargeMode.HEAL)
                    _hover.mode_description.text = "Heals player by " + skill.charge.value + " HP.";

                else if (skill.charge.charge_mode == Charge.ChargeMode.STATUS)
                    _hover.mode_description.text = "Inflicts " + skill.charge.status + ".";

                else
                    Debug.Log("No charge mode has been found");

                #endregion
            }
        
            // If skill is not chargable, destroy unused fields
            else
            {
                Destroy(_hover.condition_title);
                Destroy(_hover.mode_title);
                Destroy(_hover.condition_description);
                Destroy(_hover.mode_description);
            }

        }
    }


    // When the mouse exits
    private void OnMouseExit()
    {
        if (!SkillTarget.instance.is_actively_targeting())
            hide_hover_info();
    }

    public void hide_hover_info()
    {
        // if the skill is not picked by the user, remove the hover_info and set all the colors, etc. to the default values

        // Not hovering anymore
        is_hovering = false;

        // Change the background
        background.sprite = backgrounds[0];

        // Destroy the hover info object
        Destroy(hover_info);

    }



}
