using UnityEngine;

namespace Structs
{

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
        public bool buff;
        public int duration;
        public int damage_turn;
        public int damage_end;
        public bool stun;
    }

    [System.Serializable]
    public struct BuffDuration
    {
        public int armor;
        public int attack;
        public int hp;
        public int rage;
        public int ap;
    }

    [System.Serializable]
    public struct SubDmgReceive
    {
        // Enable Subscription to Damage Receive 
        public bool enable;

        // If enabled, modifies Unit's properties by values defined below
        public int rage;
    }

    #endregion

    #region Skills

    [System.Serializable]
    public struct SkillGen
    {
        public int cooldown;
    }

    [System.Serializable]
    public struct SkillCosts
    {
        public int ap_cost;
        public int hp_cost;
        public int rage_cost;
    }

    [System.Serializable]
    public struct SkillGain{
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
        public int damage_modifier; // modifies the base attack by adding this to the final output
    }

    #endregion

}
