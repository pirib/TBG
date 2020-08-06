using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class status_hover : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject hover_prefab;

    [Header("Handlers")]
    [SerializeField] private Status status;


    // Private shit 
    private bool is_hovering;           // Using this to skip unnecessary update checks
    private GameObject hover_info;      // The handlers of the created Hover Info objects

    // When the mouse enters the area above a status effect
    private void OnMouseEnter()
    {
        if (!is_hovering) show_hover_info();
    }

    private void show_hover_info()
    {
        // we started hovering
        is_hovering = true;

        // Create the hover info object
        hover_info = Instantiate(hover_prefab);

        // Get an easy handler
        hover_handler _hover_info = hover_info.GetComponent<hover_handler>();

        // Set information
        _hover_info.title.text = status.universal.name;
        _hover_info.description.text = status.universal.description;
        _hover_info.cooldown.text = status.stat_gen.duration.ToString();

        if (status.stat_gen.icon_big != null)
            _hover_info.big_icon.sprite = status.stat_gen.icon_big;

    }

    private void OnMouseExit()
    {
        hide_hover_info();
    }

    private void hide_hover_info()
    {
        // Not hovering anymore
        is_hovering = false;

        // Destroying the hover info object
        Destroy(hover_info);
    }

}
