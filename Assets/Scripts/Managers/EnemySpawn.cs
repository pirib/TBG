using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Generate a group of enemies
public class EnemySpawn : MonoBehaviour
{
    #region Declaring Static
    public static EnemySpawn instance;

    private void Awake()
    {
        instance = this;
    }
    #endregion

    [Header("Prefab")] // Tis a prefab zone
    public Unit unit;

    // A list of all Scriptable objects of type Unit
    [Header("List of all in-game Enemies")]
    public List<UnitAbstract> enemy_types;

    // A list of spawn points
    public List<GameObject> spawn_points;


    public List<Unit> spawn_enemies ()
    {
        // Create an empty list
        List<Unit> temp = new List<Unit>();


        // TODO Enemy encounter generator takes into account the skills/level of the player and creates a random encounter


        // TODO remove this, Temporarily used for testing
        temp.Add(spawn_enemy("Cyclops"));
        temp.Add(spawn_enemy("Doctor"));
        temp.Add(spawn_enemy("Hire Dagger"));
        temp.Add(spawn_enemy("Torturer"));
        temp.Add(spawn_enemy("Huldra"));
        temp.Add(spawn_enemy("Dragoon"));


        // Return a list full of enemies
        return temp;
    }


    // Spawns an enemy with the specified name
    private Unit spawn_enemy(string enemy_name)
    {
        // Instantiate an enemy 
        Unit new_enemy = Instantiate(unit, pick_empty_spawn_point(unit).transform) as Unit;

        // Get UnitAbstract based on the enemy_name
        UnitAbstract unitAbstract = get_UnitAbstract_byName(enemy_name);

        // Assign unit parameters
        assign_unit_parameters(ref unitAbstract, ref new_enemy);

        // Tell enemy to get all stuff ready
        new_enemy.ready();

        return new_enemy;
    }

    // Assigning unit parameters a
    private void assign_unit_parameters(ref UnitAbstract unitAbstract, ref Unit enemy)
    {

        // Populating the data
        enemy.universal = unitAbstract.universal;
        enemy.general = unitAbstract.general;
        enemy.unit_param = unitAbstract.unit_param;

    }

    // Picks and returns and empty spawn_unit. If there are none, returns null
    private GameObject pick_empty_spawn_point(Unit unit)
    {
        foreach (GameObject spawn_point in spawn_points)
        {
            if (spawn_point.GetComponent<SpawnPoint>().taken == false)
            {
                spawn_point.GetComponent<SpawnPoint>().taken = true;
                spawn_point.GetComponent<SpawnPoint>().enemy_unit = unit;
                return spawn_point;
            }
        }

        Debug.Log("Found no empty spawn points! Returning null");
        return null;
    }

    #region Unit abstract

    // Returns UnitAbstract if the requested Enemy_name is in the list 
    private UnitAbstract get_UnitAbstract_byName(string enemy_name)
    {
        int enemy_index = get_enemy_index(enemy_name);

        if (enemy_index != -1) return instance.enemy_types[enemy_index];
        else
        {
            Debug.Log("Did not find an enemy with a name " + enemy_name + "\n Returning null");
            return null;
        }

    }

    // Returns an index of the EnemyAbstract in the enemy_tpes list. Returns -1 if doesnt find one
    private int get_enemy_index(string Enemy_name)
    {
        foreach (UnitAbstract enemy in enemy_types)
        {
            if (enemy.name == Enemy_name) return enemy_types.IndexOf(enemy);
        }
        return -1;
    }
    
    #endregion

}
