using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleAlienStandardState : IdleEnemyUnitState
{

    override public IEnemyUnitState CheckChangeState(EnemyUnit unit, PlayManager environment) {
        if (unit.GetUnitTarget() == null) {
            return null;
        }
        if (Vector3.Distance(unit.GetUnitTarget().transform.position, unit.transform.position) > unit.GetRadiusStopPosition()) {
            return new MovementAlienStandardState();
        }
        
        if (unit.GetUnitTarget() != null) {
            return new BitingEnemyUnitState();
        }

        return null;
    }

}
