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

    public void end_turn(ref Unit caller)
    {
        if (is_turn(ref caller)) advance_pointer();
        else Debug.Log("End Turn is requested by a unit off the queue. Caller Name: " + caller.name);
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
    private bool is_turn(ref Unit unit)
    {
        if (unit == queue[pointer]) return true;
        else return false;
    }

    private void advance_pointer ()
    {
        // Set pointer to zero if the last unit finished its turn
        if (pointer + 1 >= queue.Count ) pointer = 0;
        else { pointer += 1; }
    }

    #endregion
}
