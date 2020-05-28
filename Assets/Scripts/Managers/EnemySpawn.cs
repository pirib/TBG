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

    

    public List<Unit> spawn_enemies ()
    {
        // Create an empty list
        List<Unit> temp = new List<Unit>();

        Unit new_unit = Instantiate(unit) as Unit;
        temp.Add(new_unit);
        
        // Return a list full of enemies
        return temp;
    }

}
