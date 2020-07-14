using UnityEngine;
using System.Collections;

namespace Targeting
{
    public enum TargetingType { 
        SINGLE,     // One enemy unit
        ENEMIES,    // Every enemy unit
        ALL,        // Every enemy unit and player
        PLAYER,     // Only player
        ANY         // Any one enemy or player unit
    }

}

