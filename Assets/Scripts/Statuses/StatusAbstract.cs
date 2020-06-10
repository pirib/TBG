using UnityEngine;
using Structs;

[CreateAssetMenu(fileName = "New Status", menuName = "Status")]
public class StatusAbstract : ScriptableObject
{
    // Universal
    public Universal universal;

    // General
    public StatGen stat_gen;

    // Positive buffs that are applied for the duration of the Status and are removed, once the duration reaches 0
     public BuffDuration buff_duration;

    // Subscribe to damage receiving
    public SubDmgReceive sub_dmg_receive;

}
