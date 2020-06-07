using UnityEngine;


[CreateAssetMenu(fileName = "New Status", menuName = "Status")]
public class StatusAbstract : ScriptableObject
{

    public new string name;
    public string description;
    public Sprite icon = null;

    public int duration = 1;

    // Negative effects
    public int damage_turn = 0;
    public int damage_end = 0;

    public bool stun = false;
    
    // Positive effects
    public int armor_buff = 0;
    public int attack_buff = 0; 


}
