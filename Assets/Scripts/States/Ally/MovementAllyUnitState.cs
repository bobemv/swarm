using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementAllyUnitState : IAllyUnitState
{

    public IAllyUnitState CheckChangeState(AllyUnit unit, PlayManager environment) {
        if (Vector3.Distance(unit.GetPositionToGo(), unit.transform.position) < unit.GetRadiusStopPosition()) {
            return new IdleAllyUnitState();
        }
        return null;
    }

    public void Update(AllyUnit unit, PlayManager environment) {
        unit.transform.Translate(Vector3.Normalize(new Vector3(unit.GetPositionToGo().x - unit.transform.position.x, unit.GetPositionToGo().y - unit.transform.position.y, 0)) * unit.GetSpeed() * Time.deltaTime, Space.World);
    }

}
