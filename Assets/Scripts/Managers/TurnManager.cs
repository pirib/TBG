using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{

    #region Declaring Static
    public static TurnManager instance;

    private void Awake()
    {
        instance = this;
    }

    #endregion 

    [Header("Handlers")]
    [SerializeField] private Unit player;
    public List<Unit> queue = new List<Unit>();

    [SerializeField] private int pointer = 0;

    void Start()
    {
        initialize_queue();
    }

    private void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            end_turn(player);
        }
    }

    public void end_turn( Unit caller)
    {
        if (is_turn(caller)) { 
            advance_pointer();
            queue[pointer].turn_start();
        }
        else Debug.Log("End Turn is requested by a unit off the queue. Caller Name: " + caller.name + "\n Current turn is held by " + TurnManager.instance.queue[pointer]);
    }


    #region Initialisation
    public void initialize_queue()
    {
        // Add player 
        queue.Add(player);
        // TODO check if there are better ways of doing this
        foreach (Unit unit in EnemySpawn.instance.spawn_enemies()) queue.Add(unit);
        // Set pointer to 0
        pointer = 0;
        Debug.Log(queue.Count);
    }

    // Used when a unit should be removed from the queue / death
    public void remove_from_queue(Unit unit)
    {
        queue.Remove(unit);
    }


    #endregion

    #region Pointer
    private bool is_turn( Unit unit)
    {
        if (unit == queue[pointer]) return true;
        else return false;
    }

    private void advance_pointer()
    {
        // Set pointer to zero if the last unit finished its turn
        if (pointer + 1 >= queue.Count) pointer = 0;
        else { pointer += 1; }
    }

    #endregion

    #region AI Helpers

    public bool exists_name(string enemy_name)
    {

        foreach (Unit enemy in queue)
        {
            if (!enemy.is_player && enemy.enemy_name == enemy_name) return true;
        }

        return false;
    }

    // Returns the unit with lowest HP
    public Unit get_lowest_hp ()
    {
        Unit temp = queue[1];

        foreach (Unit enemy in queue)
        {
            if ( !enemy.is_player && temp.get_current_hp() > enemy.get_current_hp() ) temp = enemy;            
        }

        return temp;
    }

    public Unit get_highest_hp()
    {
        Unit temp = queue[1];

        foreach (Unit enemy in queue)
        {
            if (!enemy.is_player && temp.get_current_hp() < enemy.get_current_hp()) temp = enemy;
        }

        return temp;
    }

    // Returns the unit with highest attack
    public Unit get_highest_attack ()
    {
        Unit temp = queue[1];

        foreach (Unit enemy in queue)
        {
            if (!enemy.is_player && temp.get_base_dmg() < enemy.get_base_dmg()) temp = enemy;
        }

        return temp;
    }




    #endregion
}
