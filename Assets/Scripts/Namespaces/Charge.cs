using UnityEngine;

namespace Charge
{
    public enum ChargeCondition
    {
        DAMAGE_RECEIVE,
        HEAL_RECEIVE,
        STATUS_RECEIVE,
        ON_STATUS_SET
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