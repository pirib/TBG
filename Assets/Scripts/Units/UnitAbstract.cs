using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Structs;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy")]
public class UnitAbstract : ScriptableObject
{
    /* Name, Description, Icon */
    public Universal universal;
    /// <summary>
    /// Name - enemy name (torturer, huldra, etc.)
    /// Description - enemy description 
    /// icon - used in the queue
    /// </summary>

    /* General Unit properties */
    public UnitGen general;

    /* Unit Parameters */
    public UnitParam unit_param;

}
