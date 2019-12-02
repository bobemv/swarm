using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienStandardUnit : EnemyUnit
{
    // Start is called before the first frame update
    override protected void StartUnit()
    {
        base.StartUnit();

        unitState = new IdleAlienStandardState();
        unitTargetingState = new ClosestTargetingEnemyUnitState();
    }

    // Update is called once per frame
    override protected void UpdateUnit()
    {
        base.UpdateUnit();

        IEnemyUnitState state;
        state = unitTargetingState.CheckChangeState(this, _playManager);
        if (state != null) {
            unitTargetingState = state;
        }
        unitTargetingState.Update(this, _playManager);

        state = unitState.CheckChangeState(this, _playManager);
        if (state != null) {
            unitState = state;
        }
        unitState.Update(this, _playManager);

    }

    override protected void UpdateLives() {
        if (lives <= 0) {
            EnemyUnit.alienStandardUnitDestroyed++;
        }
        base.UpdateLives();
    }
}
