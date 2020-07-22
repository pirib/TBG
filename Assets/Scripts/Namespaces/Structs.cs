using UnityEngine;
using System.Collections.Generic;

// Custom
using Charge;
using Targeting;
using StatusTypes;

namespace Structs
{
    // Enums
    public enum SkillAnimation
    {
        ATTACK_NORMAL,
        ATTACK_RAGE,
        MAGIC_SELF,
        MAGIC_TARGET,
        MAGIC_MEGA
    }


    [System.Serializable]
    public struct Universal
    {
        public string name;
        public string description;
        public Sprite icon;
    }

    #region Status
    [System.Serializable]
    public struct StatGen
    {
        public StatusType type;           // true - positive effect, flase - negative
        public int duration;        // the duration of the 
        public int damage_turn;     // Damage dealt per turn
        public int damage_end;      // Damage dealt at the end of its duration
        
        public bool stun;           // Affected Unit will skip its turn if this is set to true

        public int heal_turn;       // Healing per turn
        public int rage_turn;       // Rage per turn
        public int ap_turn;         // AP per turn


    }

    [System.Serializable]
    public struct BuffDuration
    {
        public int armor;           // Additional armor the buff grants to the affected unit
        public int attack;          // Additional attack
    }

    [System.Serializable]
    public struct SubDmgReceive
    {
        public bool enable;         // Enable Subscription to Damage Receive. Any time damage is received, params below will be used

        public int rage;            // Get this amount of rage on receiving damage
        public int hp;              // Get this amount of healing on receiving damage // UNUSED
        public bool weaken;         // Affected Unit will get 1 damage after receiving damage

        public int reflect_damage;  // Reflected damage 
    }

    [System.Serializable]
    public struct SubHealReceive
    {
        public bool enable;         // Enable Subscription to Healing received. Any time healing is received, params below will be used

        public int hp;              // On healing, heal additional value

        public int rage;           // On healing, get rage
    }


        #endregion

    #region Skills

    [System.Serializable]
    public struct SkillGen
    {
        public int cooldown;
        public TargetingType targeting_mode;
        public SkillAnimation skill_animation;
    }

    [System.Serializable]
    public struct SkillCosts
    {
        public int ap_cost;
        public int hp_cost;
        public int rage_cost;
    }

    [System.Serializable]
    public struct SkillGain
    {
        public int hp;
        public int ap;
        public int rage;
    }

    [System.Serializable]
    public struct SkillStatusInfo
    {
        public bool apply_status;
        public string status_name;
    }

    [System.Serializable]
    public struct SkillDamageInfo
    {
        public bool deal_damage;
        public bool use_base_attack;
        public int damage_modifier;     // modifies the base attack by adding this to the final output
        public bool vampire_attack;     // heal the caster by the final damage output
    }

    [System.Serializable]
    public struct SkillChargeInfo
    {
        public bool chargeable;


        public ChargeCondition charge_condition;
        public ChargeMode charge_mode;
        
        public string status;       // Applied if charge_mode is set to STATUS
        public int value;
    }


    [System.Serializable]
    public struct SkillAdvanced
    {
        public int cooldown_change;         // How change the cooldown
        public StatusType status_type;    // Which Status type to affect (positive, negative, all)
        public bool remove;                 // Remove statuses?
        
        public bool count;                  // Counts the types of statuses
        
        public bool deal_damage;            // Deal Damage based on the number of statuses

        public bool heal_self;              // Heal yourself based on the number of statuses

        public int skill_cooldown;          // Change all skills cooldown by this value

        public bool swap;
        public StatusType status_swap_self;
        public StatusType status_swap_target;
    }

    #endregion

    #region Unit

    [System.Serializable]
    public struct UnitGen
    {
        public bool is_player;

        public bool is_mindless;

        public AnimatorOverrideController unit_animations;

        public List<string> skills;

    }

    [System.Serializable]
    public struct UnitParam
    {
        // HP
        public int hp_max;

        // Rage
        public int rage_max;

        // AP
        public int ap_max;

        // Attack
        public int base_damage;

        // Armor
        public int armor;
               
    }

    #endregion

    #region AI Helpers

    // Conditions a unit must pass to get to the selected pool of potential targets
    public enum pooling
    {
        PLAYER,
        SELF,
        MINDLESS,
        HAS_BASE_DAMAGE,
        CAN_PLAY
    }

    // picking from a pool
    public enum picking
    {
        LOWEST_HP,
        HIGHEST_HP,
        HIGHEST_DMG,
        EARLIEST_IN_QUEUE,
        LATEST_IN_QUEUE
    }

    #endregion
}
