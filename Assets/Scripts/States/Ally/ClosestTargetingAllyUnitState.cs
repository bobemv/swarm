using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosestTargetingAllyUnitState : IAllyUnitState
{

    public IAllyUnitState CheckChangeState(AllyUnit unit, PlayManager environment) {
        if (unit.pointTarget.HasValue) {
            return new ClosestToPointTargetingAllyUnitState();
        }
        return null;
    }

    public void Update(AllyUnit unit, PlayManager environment) {
        if (unit.pointTarget.HasValue) {
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
