using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyUnitState
{
    IEnemyUnitState CheckChangeState(EnemyUnit unit, PlayManager environment);
    void Update(EnemyUnit unit, PlayManager environment);
}
