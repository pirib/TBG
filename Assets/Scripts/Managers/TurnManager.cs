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

    #region Handlers
    [Header("Handlers")]

    public Unit player;
    [SerializeField] private GameObject queue_prefab;
    #endregion

    #region Queue Related
    public List<Unit> queue = new List<Unit>();
    public List<GameObject> queue_GUI = new List<GameObject>();

    [SerializeField] private int pointer = 0;
    #endregion

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
        // Check if the it is the callers turn
        if (is_turn(caller)) { 
            
            // Advance pointer so it points at the next unit
            advance_pointer();

            // Pop the queue icon for the unit
            pop_GUI();

            // Let the unit know it can start its turn
            queue[pointer].start_unit_turn();
        }
        else Debug.Log("End Turn is requested by a unit off the queue. Caller Name: " + caller.name + "\n Current turn is held by " + TurnManager.instance.queue[pointer]);
    }


    #region Queue management
    // Initializes the queue. DOES NOT REMOVE THE OLD ONE
    public void initialize_queue()
    {
        // Add player 
        queue.Add(player);
        
        // TODO check if there are better ways of doing this
        foreach (Unit unit in EnemySpawn.instance.spawn_enemies()) queue.Add(unit);
        
        // Set pointer to 0
        pointer = 0;

        set_queue_GUI();
        pop_GUI();

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

    #endregion

    #region Queue GUI

    // Sets the Queue GUI 
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

            // Set the correct icon
            if (unit.universal.icon != null)
                new_queue.GetComponent<queue_bg>().icon.sprite = unit.universal.icon; 

            // Update the list holding all the references to the GUI
            queue_GUI.Add(new_queue);

            // Align
            float x = Mathf.Floor(Camera.main.aspect * Camera.main.orthographicSize) - queue_bg + 8;
            float y = Mathf.Floor((start_point - i * (queue_bg)));

            new_queue.transform.position = new Vector3(x, y, -i );
            Debug.Log(i);

            i++;
        }
    }

    // Popps the current units turn icon in the queue a bit to the left
    private void pop_GUI()
    {
        // Get the current pointer's x
        float origin_x = queue_GUI[pointer].transform.position.x;

        // Set the old Queue thing back in place
        queue_GUI[get_previous_pointer()].transform.position = new Vector3( origin_x, queue_GUI[get_previous_pointer()].transform.position.y, queue_GUI[get_previous_pointer()].transform.position.z);        

        // Set the current queue thingy a bit out
        queue_GUI[pointer].transform.position = new Vector3(queue_GUI[pointer].transform.position.x-8, queue_GUI[pointer].transform.position.y, queue_GUI[get_previous_pointer()].transform.position.z);

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

    // Returns the previous index of the queue_GUI
    private int get_previous_pointer()
    {
        if (pointer > 1)
            return pointer - 1;

        else if (pointer == 0)
            return queue_GUI.Count - 1;
        
        else
            Debug.Log("Something went wrong with the pointer. Current value " + pointer);
           
        return 0;
    }

    #endregion

    #region AI Helpers

    // Pooling

    public List<Unit> pool_units ( List<pooling> conditions, Skill skill)
    {
        List<Unit> temp = new List<Unit>();

        // If the condition is player, return only a list containing the player
        if (conditions[0] == pooling.PLAYER) 
            temp.Add(player);

        // If the condition is self, return only a list containing the unit itself
        else if (conditions[0] == pooling.SELF) 
            temp.Add(skill.owner_unit);
        
        // Else, loop through each enemy_unit that fits the list of conditions
        else
        {
            foreach(Unit enemy_unit in queue.GetRange(1, queue.Count-1))
            {
                bool passed = false;

                // Check the pooling conditions. 
                // If the pooling condition list contains a given condition and that condition is also true for that enemy. Add that enemy
                // If at least one of the conditions is not met then the enemy is not added to the list of targets that can be pooled.
                if (skill.pooling.Contains(pooling.MINDLESS))
                    if (enemy_unit.is_mindless())
                        passed = true;
                    else
                        passed = false;

                if (skill.pooling.Contains(pooling.CAN_PLAY))
                    if (enemy_unit.can_play)
                        passed = true;
                    else
                        passed = false;

                if (skill.pooling.Contains(pooling.HAS_BASE_DAMAGE))
                    if (enemy_unit.get_base_dmg() > 0)
                        passed = true;
                    else
                        passed = false;

                if (passed)
                    temp.Add(enemy_unit);
            }
        }

        return temp;
    }

    // Picking
    public List<Unit> pick_unit (Skill skill)
    {

        // Temporary list that wil be returned 
        List<Unit> temp = new List<Unit>();

        // Temporary pool of the units as defined by the pooling conditions
        List<Unit> pool = skill.get_skill_pool();

        if (skill.picking == picking.NONE)
            temp.Add(pool[0]);

        else if (skill.picking == picking.LOWEST_HP)
            temp.Add(get_lowest_hp(pool));

        else if (skill.picking == picking.HIGHEST_HP)
            temp.Add(get_highest_hp(pool));

        else if (skill.picking == picking.HIGHEST_DMG)
            temp.Add(get_highest_attack(pool));

        return temp;
    }


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
