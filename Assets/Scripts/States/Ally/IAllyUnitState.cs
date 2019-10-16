using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAllyUnitState
{
    IAllyUnitState CheckChangeState(AllyUnit unit, PlayManager environment);
    void Update(AllyUnit unit, PlayManager environment);
}
