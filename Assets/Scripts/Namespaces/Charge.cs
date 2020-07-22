using UnityEngine;

namespace Charge
{
    public enum ChargeCondition
    {
        DAMAGE_RECEIVE,
        HEAL_RECEIVE,
        STATUS_RECEIVE_POSITIVE,
        STATUS_RECEIVE_NEGATIVE
    }

    public enum ChargeMode
    {
        DAMAGE_MODIFIER,
        HEAL,
        COST_RAGE,
        COST_HP,
        COST_AP,
        STATUS
    }

}