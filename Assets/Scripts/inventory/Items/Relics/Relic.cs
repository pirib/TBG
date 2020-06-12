using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Structs;

[CreateAssetMenu(fileName = "New Relic", menuName = "Relic")]
public class Relic : ScriptableObject
{
    // Param
    public Universal universal;

    // Skills this relic has
    public List<Skill> skills;

    
}
