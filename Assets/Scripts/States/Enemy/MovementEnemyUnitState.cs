using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementEnemyUnitState : IEnemyUnitState
{

    public IEnemyUnitState CheckChangeState(EnemyUnit unit, PlayManager environment) {
        if (unit.unitTarget == null) {
            return new IdleEnemyUnitState();
        }
        if (Vector3.Distance(unit.unitTarget.transform.position, unit.transform.position) < unit.radiusStopPosition) {
            return new IdleEnemyUnitState();
        }
        return null;
    }

    public void Update(EnemyUnit unit, PlayManager environment) {
        unit.transform.Translate(Vector3.Normalize(new Vector3(unit.unitTarget.transform.position.x - unit.transform.position.x, unit.unitTarget.transform.position.y - unit.transform.position.y, 0)) * unit._speed * Time.deltaTime, Space.World);
    }

}
