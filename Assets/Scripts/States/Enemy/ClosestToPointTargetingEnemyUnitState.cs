﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosestToPointTargetingEnemyUnitState : IEnemyUnitState
{

    public IEnemyUnitState CheckChangeState(EnemyUnit unit, PlayManager environment) {
        /*if (unit == environment.unitSelected) {
            if (environment.unitTarget) {
                return new ClosestTargetingEnemyUnitState();
            }
        }*/
        return null;
    }

    public void Update(EnemyUnit unit, PlayManager environment) {
        /*if (unit.unitTarget != null) {
            return;
        }
        float minDistance = Mathf.Infinity;
        Unit target = null;
        for (int i = 0; i < environment.enemyUnits.Count; i++) {
            float distance;
            distance = Vector3.Distance(environment.enemyUnits[i].transform.position, unit.pointTarget);
            if (distance < minDistance) {
                minDistance = distance;
                target = environment.enemyUnits[i];
            }
        }
        unit.unitTarget = target;*/
        return;
    }

}
