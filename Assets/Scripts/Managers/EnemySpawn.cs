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

    public List<UnitAbstract> enemy_types;
    

    public List<Unit> spawn_enemies ()
    {
        // Create an empty list
        List<Unit> temp = new List<Unit>();


        // TODO this is where the random generator will kick in
        temp.Add(Instantiate(unit) as Unit);


        // Return a list full of enemies
        return temp;
    }

}
