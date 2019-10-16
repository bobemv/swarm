using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementAllyUnitState : IAllyUnitState
{

    public IAllyUnitState CheckChangeState(AllyUnit unit, PlayManager environment) {
        if (Vector3.Distance(unit.positionToGo, unit.transform.position) < unit.radiusStopPosition) {
            return new IdleAllyUnitState();
        }
        return null;
    }

    public void Update(AllyUnit unit, PlayManager environment) {
        unit.transform.Translate(Vector3.Normalize(new Vector3(unit.positionToGo.x - unit.transform.position.x, unit.positionToGo.y - unit.transform.position.y, 0)) * unit._speed * Time.deltaTime, Space.World);
    }

}
