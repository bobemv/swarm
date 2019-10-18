using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosestTargetingAllyUnitState : IAllyUnitState
{

    public IAllyUnitState CheckChangeState(AllyUnit unit, PlayManager environment) {
        if (environment.isUnitSelected(unit)) {
            if (environment.pointTarget.HasValue && environment.unitTarget == null) {
                return new ClosestToPointTargetingAllyUnitState();
            }
        }
        return null;
    }

    public void Update(AllyUnit unit, PlayManager environment) {
        if (environment.unitTarget != null && environment.isUnitSelected(unit)) {
            unit.unitTarget = environment.unitTarget;
            unit.pointTarget = null;
        }
        if (unit.unitTarget != null) {
            return;
        }
        float minDistance = Mathf.Infinity;
        Unit target = null;
        for (int i = 0; i < environment.enemyUnits.Count; i++) {
            float distance;
            distance = Vector3.Distance(environment.enemyUnits[i].transform.position, unit.transform.position);
            if (distance < minDistance) {
                minDistance = distance;
                target = environment.enemyUnits[i];
            }
        }
        unit.unitTarget = target;
    }

}
