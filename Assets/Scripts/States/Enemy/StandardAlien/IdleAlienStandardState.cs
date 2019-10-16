using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleAlienStandardState : IdleEnemyUnitState
{

    override public IEnemyUnitState CheckChangeState(EnemyUnit unit, PlayManager environment) {
        if (unit.unitTarget == null) {
            return null;
        }
        if (Vector3.Distance(unit.unitTarget.transform.position, unit.transform.position) > unit.radiusStopPosition) {
            return new MovementAlienStandardState();
        }
        
        if (unit.unitTarget != null) {
            return new BitingEnemyUnitState();
        }

        return null;
    }

}
