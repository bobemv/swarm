using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingAllyUnitState : IAllyUnitState
{

    private float shootTime = 0;
    public IAllyUnitState CheckChangeState(AllyUnit unit, PlayManager environment) {
        if (Vector3.Distance(unit.positionToGo, unit.transform.position) > unit.radiusStopPosition) {
            return new MovementAllyUnitState();
        }
        
        if (unit.unitTarget == null) {
            return new IdleAllyUnitState();
        }
        return null;
    }

    public void Update(AllyUnit unit, PlayManager environment) {
        if (shootTime > unit.fireRate) {
            shootTime = 0;
            unit.Shoot();
            //Instantiate(_bulletPrefab, transform.position, transform.rotation);
        }
        else {
            shootTime += Time.deltaTime;
        }
        return;
    }

}
