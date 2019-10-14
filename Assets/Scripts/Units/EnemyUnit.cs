using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : Unit
{
    override public void SelectTarget(Unit newTarget) {
        target = newTarget;
        positionToGo = newTarget.transform.position;
    }
}
