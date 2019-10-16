using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleAllyUnitState : IAllyUnitState
{

    public IAllyUnitState CheckChangeState(AllyUnit unit, PlayManager environment) {
        if (Vector3.Distance(unit.positionToGo, unit.transform.position) > unit.radiusStopPosition) {
            return new MovementAllyUnitState();
        }
        
        if (unit.unitTarget != null) {
            return new ShootingAllyUnitState();
        }
        return null;
    }

    public void Update(AllyUnit unit, PlayManager environment) {
        return;
    }

}
