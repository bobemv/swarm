using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementEnemyUnitState : IEnemyUnitState
{

    virtual public IEnemyUnitState CheckChangeState(EnemyUnit unit, PlayManager environment) {
        if (unit.GetUnitTarget() == null) {
            return new IdleEnemyUnitState();
        }
        if (Vector3.Distance(unit.GetUnitTarget().transform.position, unit.transform.position) < unit.GetRadiusStopPosition()) {
            return new IdleEnemyUnitState();
        }
        return null;
    }

    virtual public void Update(EnemyUnit unit, PlayManager environment) {
        unit.transform.Translate(Vector3.Normalize(new Vector3(unit.GetUnitTarget().transform.position.x - unit.transform.position.x, unit.GetUnitTarget().transform.position.y - unit.transform.position.y, 0)) * unit.GetSpeed() * Time.deltaTime, Space.World);
    }

}
