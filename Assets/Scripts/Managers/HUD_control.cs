using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HUD_control : MonoBehaviour
{

    #region Declaring Static
    public static HUD_control instance;

    private void Awake()
    {
        instance = this;
    }

    #endregion 


    // Tilemap and tiles for 
    [Header("HUD_player")]
    public TileBase[] tile_player;

    [Header("HUD_enemy")]
    public TileBase[] tile_enemy;


    public void update_hud( Unit unit, Tilemap hud)
    {
      
        // it is the player that needs updating the hud
        if (unit.is_player())
        {
            // Set hud to the correct position relative to the unit
            hud.transform.position = new Vector2(-120,96);


            // Setting the black ribbon
            hud.SetTile(new Vector3Int(-10, 0, -1), tile_player[6]);
            hud.SetTile(new Vector3Int(-10, -1, -1), tile_player[6]);
            hud.SetTile(new Vector3Int(-10, -2, -1), tile_player[6]);


            // Set hp
            for (int i = 0; i < unit.get_max_hp(); i++) hud.SetTile(new Vector3Int(-9 + i, 0, -1), tile_player[1]);
            for (int i = 0; i < unit.get_current_hp(); i++) hud.SetTile(new Vector3Int(-9 + i, 0, -1), tile_player[0]);
            hud.SetTile(new Vector3Int(-9 + unit.get_max_hp(), 0, -1), tile_player[7]);
            hud.SetTile(new Vector3Int(-9 + unit.get_max_hp() + 1, 0, -1), tile_player[8]);

            // Set rage
            for (int i = 0; i < unit.get_max_rage(); i++) hud.SetTile(new Vector3Int(-9 + i, -1, -1), tile_player[3]);
            for (int i = 0; i < unit.get_cur_rage(); i++) hud.SetTile(new Vector3Int(-9 + i, -1, -1), tile_player[2]);
            hud.SetTile(new Vector3Int(-9 + unit.get_max_rage(), -1, -1), tile_player[7]);
            hud.SetTile(new Vector3Int(-9 + unit.get_max_rage() + 1, -1, -1), tile_player[8]);

            // Set AP
            for (int i = 0; i < unit.get_max_ap(); i++) hud.SetTile(new Vector3Int(-9 + i, -2, -1), tile_player[5]);
            for (int i = 0; i < unit.get_cur_ap(); i++) hud.SetTile(new Vector3Int(-9 + i, -2, -1), tile_player[4]);
            hud.SetTile(new Vector3Int(-9 + unit.get_max_ap(), -2, -1), tile_player[7]);
            hud.SetTile(new Vector3Int(-9 + unit.get_max_ap() + 1, -2, -1), tile_player[8]);

        }
        else
        {

            // Set the grid size
            hud.GetComponentInParent<Grid>().cellSize = new Vector3Int(6,6,0);

            // Set hp
            for (int i = 0; i < unit.get_max_hp(); i++) hud.SetTile(new Vector3Int( i, 0, -1), tile_enemy[0]);
            for (int i = 0; i < unit.get_current_hp(); i++) hud.SetTile(new Vector3Int(i, 0, -1), tile_enemy[1]);
            hud.SetTile(new Vector3Int(unit.get_max_hp(), 0, -1), tile_enemy[2]);

            // Set rage
            for (int i = 0; i < unit.get_max_rage(); i++) hud.SetTile(new Vector3Int(i, -1, -1), tile_enemy[3]);
            for (int i = 0; i < unit.get_cur_rage(); i++) hud.SetTile(new Vector3Int(i, -1, -1), tile_enemy[4]);
            hud.SetTile(new Vector3Int(unit.get_max_rage(), -1, -1), tile_enemy[2]);

        }

    }

}
