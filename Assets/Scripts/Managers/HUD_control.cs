using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HUD_control : MonoBehaviour
{

    // TODO Change the names
    [Header("HUD_player")]
    public Tilemap hud;
    public TileBase[] test;

    [Header("HUD_enemy")]
    public Tilemap hud_enemy;
    public TileBase[] test2;



    private void update_hud(ref Unit unit)
    {
        Vector3 origin = new Vector2(-10, 0);

        // it is the player that needs updating the hud
        if (unit.is_player())
        {
            // Set hp
            for (int i = 0; i < unit.get_max_hp(); i++) hud.SetTile(new Vector3Int(-9 + i, 0, -1), test[1]);
            for (int i = 0; i < unit.get_current_hp(); i++) hud.SetTile(new Vector3Int(-9 + i, 0, -1), test[0]);
            hud.SetTile(new Vector3Int(-9 + unit.get_max_hp(), 0, -1), test[7]);
            hud.SetTile(new Vector3Int(-9 + unit.get_max_hp() + 1, 0, -1), test[8]);

            // Set rage
            for (int i = 0; i < unit.get_max_rage(); i++) hud.SetTile(new Vector3Int(-9 + i, -1, -1), test[3]);
            for (int i = 0; i < unit.get_cur_rage(); i++) hud.SetTile(new Vector3Int(-9 + i, -1, -1), test[2]);
            hud.SetTile(new Vector3Int(-9 + unit.get_max_rage(), -1, -1), test[7]);
            hud.SetTile(new Vector3Int(-9 + unit.get_max_rage() + 1, -1, -1), test[8]);

            // Set AP
            for (int i = 0; i < unit.get_max_ap(); i++) hud.SetTile(new Vector3Int(-9 + i, -2, -1), test[5]);
            for (int i = 0; i < unit.get_cur_ap(); i++) hud.SetTile(new Vector3Int(-9 + i, -2, -1), test[4]);
            hud.SetTile(new Vector3Int(-9 + unit.get_max_ap(), -2, -1), test[7]);
            hud.SetTile(new Vector3Int(-9 + unit.get_max_ap() + 1, -2, -1), test[8]);

        }
        else
        {

        }

    }

}
