using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosestToPointTargetingAllyUnitState : IAllyUnitState
{

    public IAllyUnitState CheckChangeState(AllyUnit unit, PlayManager environment) {
        if (unit == environment.isUnitSelected(unit)) {
            if (environment.unitTarget && !environment.pointTarget.HasValue) {
                return new ClosestTargetingAllyUnitState();
            }
        }
        return null;
    }

    public void Update(AllyUnit unit, PlayManager environment) {
        if (environment.pointTarget != null && environment.isUnitSelected(unit)) {
            unit.unitTarget = null;
            unit.pointTarget = environment.pointTarget;
        }
        if (unit.unitTarget != null) {
            return;
        }
        float minDistance = Mathf.Infinity;
        Unit target = null;
        for (int i = 0; i < environment.enemyUnits.Count; i++) {
            float distance;
            distance = Vector3.Distance(environment.enemyUnits[i].transform.position, unit.pointTarget.GetValueOrDefault());
            if (distance < minDistance) {
                minDistance = distance;
                target = environment.enemyUnits[i];
            }
        }
        unit.unitTarget = target;
        return;
    }

}
