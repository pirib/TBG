using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skills_hovering : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject hover_prefab;

    [Header("Handlers")]
    [SerializeField] private Skill skill;
    [SerializeField] private SpriteRenderer background;
    [SerializeField] private List<Sprite> backgrounds = new List<Sprite>();

    // Private shit 
    private bool is_hovering;           // Using this to skip unnecessary update checks
    private GameObject hover_info;      // The handlers of the created Hover Info objects

    // When mouse is over the 
    private void OnMouseOver()
    {
        // DEBUG
        Debug.Log("Over a skill " + skill.universal.name);
        
        // If we are not already hovering, show the hover info
        if (!is_hovering) show_hover_info();

    }

    // Basically makes things the way they are
    private void show_hover_info()
    {
        // Started hovering!
        is_hovering = true;

        // Change the background
        background.sprite = backgrounds[1];

        // Create a Hover Info object
        hover_info = Instantiate(hover_prefab, this.transform );

        // Nudge it a bit to the side
        hover_info.transform.position = new Vector3(this.transform.position.x + 144, hover_info.transform.position.y, hover_info.transform.position.z);

        // Populate the fields
        hover_info.GetComponent<hover_info>().title.text = skill.universal.name;
        hover_info.GetComponent<hover_info>().description.text = skill.universal.description;

        // ADD price infot

    }


    // When the mouse exits
    private void OnMouseExit()
    {
        // Not hovering anymore
        is_hovering = false;

        // Change the background
        background.sprite = backgrounds[0];

        // Destroy the hover info object
        Destroy(hover_info);
    }




}
