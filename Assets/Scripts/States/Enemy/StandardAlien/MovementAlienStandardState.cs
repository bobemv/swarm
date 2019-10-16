using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementAlienStandardState : MovementEnemyUnitState
{

    override public IEnemyUnitState CheckChangeState(EnemyUnit unit, PlayManager environment) {
        if (unit.unitTarget == null) {
            return new IdleEnemyUnitState();
        }
        if (Vector3.Distance(unit.unitTarget.transform.position, unit.transform.position) < unit.radiusStopPosition) {
            return new IdleEnemyUnitState();
        }
        Collider2D[] results = new Collider2D[10];
        unit.GetComponent<Collider2D>().OverlapCollider(new ContactFilter2D(), results);
        foreach (Collider2D col in results)
        {
            if (col != null && col.name == "AttractField") {
                unit.transform.SetParent(col.transform.parent.gameObject.transform);
                return new ProtectingAlienStandardState(col.transform.parent.gameObject);
            }
        }
        return null;
    }

    override public void Update(EnemyUnit unit, PlayManager environment) {
        unit.transform.Translate(Vector3.Normalize(new Vector3(unit.unitTarget.transform.position.x - unit.transform.position.x, unit.unitTarget.transform.position.y - unit.transform.position.y, 0)) * unit._speed * Time.deltaTime, Space.World);
    }

}
