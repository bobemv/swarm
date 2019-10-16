using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosestTargetingEnemyUnitState : IEnemyUnitState
{

    public IEnemyUnitState CheckChangeState(EnemyUnit unit, PlayManager environment) {
        /*if (unit == environment.unitSelected) {
            if (environment.pointTarget.HasValue) {
                return new ClosestToPointTargetingEnemyUnitState();
            }
        }*/
        return null;
    }

    public void Update(EnemyUnit unit, PlayManager environment) {
        if (unit.unitTarget != null) {
            return;
        }
        float minDistance = Mathf.Infinity;
        Unit target = null;
        for (int i = 0; i < environment.allyUnits.Count; i++) {
            float distance;
            distance = Vector3.Distance(environment.allyUnits[i].transform.position, unit.transform.position);
            if (distance < minDistance) {
                minDistance = distance;
                target = environment.allyUnits[i];
            }
        }
        unit.unitTarget = target;
    }

}
