using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleEnemyUnitState : IEnemyUnitState
{

    virtual public IEnemyUnitState CheckChangeState(EnemyUnit unit, PlayManager environment) {
        if (unit.GetUnitTarget() == null) {
            return null;
        }
        if (Vector3.Distance(unit.GetUnitTarget().transform.position, unit.transform.position) > unit.GetRadiusStopPosition()) {
            return new MovementEnemyUnitState();
        }
        
        if (unit.GetUnitTarget() != null) {
            return new BitingEnemyUnitState();
        }
        return null;
    }

    virtual public void Update(EnemyUnit unit, PlayManager environment) {
        return;
    }

}
