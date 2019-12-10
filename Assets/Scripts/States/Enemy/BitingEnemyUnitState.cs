using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BitingEnemyUnitState : IEnemyUnitState
{

    private float bitingTime = 0;
    public IEnemyUnitState CheckChangeState(EnemyUnit unit, PlayManager environment) {
        if (unit.GetUnitTarget() == null) {
            return new IdleEnemyUnitState();
        }
        if (Vector3.Distance(unit.GetUnitTarget().transform.position, unit.transform.position) > unit.GetRadiusStopPosition()) {
            return new MovementEnemyUnitState();
        }
        
        if (unit.GetUnitTarget() == null) {
            return new IdleEnemyUnitState();
        }
        return null;
    }

    public void Update(EnemyUnit unit, PlayManager environment) {
        if (bitingTime > unit.biteRate) {
            bitingTime = 0;
            unit.Bite();
            //Instantiate(_bulletPrefab, transform.position, transform.rotation);
        }
        else {
            bitingTime += Time.deltaTime;
        }
        return;
    }

}
