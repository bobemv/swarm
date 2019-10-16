using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleEnemyUnitState : IEnemyUnitState
{

    public IEnemyUnitState CheckChangeState(EnemyUnit unit, PlayManager environment) {
        if (unit.unitTarget == null) {
            return null;
        }
        if (Vector3.Distance(unit.unitTarget.transform.position, unit.transform.position) > unit.radiusStopPosition) {
            return new MovementEnemyUnitState();
        }
        
        if (unit.unitTarget != null) {
            return new BitingEnemyUnitState();
        }
        return null;
    }

    public void Update(EnemyUnit unit, PlayManager environment) {
        return;
    }

}
