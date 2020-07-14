using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Generate a group of enemies
public class EnemySpawn : MonoBehaviour
{
    [Header("Prefab Handlers")]
    public Unit unit;

    #region Declaring Static
    public static EnemySpawn instance;

    private void Awake()
    {
        instance = this;
    }
    #endregion


    // A list of all Scriptable objects of type Unit
    public List<UnitAbstract> enemy_types;

    // A list of spawn points
    public List<GameObject> spawn_points;


    public List<Unit> spawn_enemies ()
    {
        // Create an empty list
        List<Unit> temp = new List<Unit>();


        // TODO this is where the random generator will kick in
        temp.Add(Instantiate(unit, pick_empty_spawn_point(unit).transform) as Unit);
        temp.Add(Instantiate(unit, pick_empty_spawn_point(unit).transform) as Unit);
        temp.Add(Instantiate(unit, pick_empty_spawn_point(unit).transform) as Unit);
        temp.Add(Instantiate(unit, pick_empty_spawn_point(unit).transform) as Unit);


        // Return a list full of enemies
        return temp;
    }

    GameObject pick_empty_spawn_point(Unit unit)
    {
        foreach (GameObject spawn_point in spawn_points)
        {
            if (spawn_point.GetComponent<SpawnPoint>().taken == false) {
                spawn_point.GetComponent<SpawnPoint>().taken = true;
                spawn_point.GetComponent<SpawnPoint>().enemy_unit = unit;
                return spawn_point;
            }
        }

        Debug.Log("Found no empty spawn points! Returning null");
        return null;
    }


    /*
    public GameObject spawn_enemy(string enemy_type)
    {
        GameObject spawn_point;

        return spawn_point;
    }
    */

}
