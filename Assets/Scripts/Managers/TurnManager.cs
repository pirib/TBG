using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Structs;

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

    public Unit player;
    [SerializeField] private GameObject queue_prefab;
    public List<Unit> queue = new List<Unit>();
    public List<GameObject> queue_GUI = new List<GameObject>();

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

        set_queue_GUI();

        Debug.Log("Queue initialized.");
        }

    // Used when a unit should be removed from the queue / death
    public void remove_from_queue(Unit unit)
    {
        // Remove the unit from the queue
        queue.Remove(unit);

        // Remove it from the queue's GUI
        set_queue_GUI();

        // Destroy the unit itself
        Destroy(unit.gameObject);

    }

    private void set_queue_GUI()
    {
        // Remove the old Queue
        foreach (GameObject queue in queue_GUI) Destroy(queue);
        queue_GUI.Clear();

        // Aligning the Game Objects on the right side
        float queue_bg = 24;
        float start_point = Mathf.Floor((Camera.main.orthographicSize - 15 - (Camera.main.orthographicSize * 2 - queue.Count * queue_bg) / 2));


        int i = 0;
        foreach (Unit unit in queue)
        {
            // Instantiate the queue prefab
            GameObject new_queue = Instantiate(queue_prefab);

            // Update the list holding all the references to the GUI
            queue_GUI.Add(new_queue);

            // Align
            float x = Mathf.Floor(Camera.main.aspect * Camera.main.orthographicSize) - queue_bg + 8;
            float y = Mathf.Floor((start_point - i * (queue_bg)));
            
            new_queue.transform.position = new Vector3(x, y, -i);

            i++;
        }
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

    // Pooling

    public List<Unit> pool_units( List<pooling> conditions, Unit owner_unit, Skill skill)
    {
        List<Unit> temp = new List<Unit>();

        // If the condition is player, return only a list containing the player
        if (conditions[0] == pooling.PLAYER) 
            temp.Add(player);

        // If the condition is self, return only a list containing the unit itself
        else if (conditions[0] == pooling.SELF) 
            temp.Add(owner_unit);
        
        // Else, loop through each enemy_unit that fits the list of conditions
        else
        {
            foreach(Unit enemy_unit in queue.GetRange(1, queue.Count-1))
            {
                bool test = true;
                // Check the pooling conditions. 
                // If the pooling condition list contains a given condition and that condition is also true for that enemy. Add that enemy
                // If at least one of the conditions is not met then the enemy is not added to the list of targets that can be pooled.
                if ( !(skill.pooling.Contains(pooling.MINDLESS) && enemy_unit.is_mindless()) )
                    test = false;
                if ( !(skill.pooling.Contains(pooling.CAN_PLAY) && enemy_unit.can_play) )
                    test = false;
                if ( !(skill.pooling.Contains(pooling.HAS_BASE_DAMAGE) && (enemy_unit.get_base_dmg() > 0 )) )
                    test = false;

                if (test)
                    temp.Add(enemy_unit);
            }
        }

        return temp;
    }

    // Picking

    // Returns the unit with lowest HP
    public Unit get_lowest_hp(List<Unit> pool)
    {
        Unit temp = pool[0];

        foreach (Unit enemy in pool)
        {
            if ( !enemy.is_player() && temp.get_current_hp() > enemy.get_current_hp() ) temp = enemy;            
        }

        return temp;
    }

    // Returns the unit with the highest HP
    public Unit get_highest_hp(List<Unit> pool)
    {
        Unit temp = pool[0];

        foreach (Unit enemy in pool)
        {
            if (!enemy.is_player() && temp.get_current_hp() < enemy.get_current_hp()) temp = enemy;
        }

        return temp;
    }

    // Returns the unit with highest attack
    public Unit get_highest_attack (List<Unit> pool)
    {
        Unit temp = pool[0];

        foreach (Unit enemy in pool)
        {
            if (!enemy.is_player() && temp.get_base_dmg() < enemy.get_base_dmg()) temp = enemy;
        }

        return temp;
    }




    #endregion
}
