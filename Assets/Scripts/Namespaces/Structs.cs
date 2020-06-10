using UnityEngine;

namespace Structs
{

    [System.Serializable]
    public struct Universal
    {
        public new string name;
        public string description;
        public Sprite icon;
    }

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
        public int hp;
        public int rage;
    }

}