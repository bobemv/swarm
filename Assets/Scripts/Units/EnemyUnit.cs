using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : Unit
{
    public float biteRate;

    protected IEnemyUnitState unitState;
    protected IEnemyUnitState unitTargetingState;

    //public float biteRadius;

    // Start is called before the first frame update
    override protected void StartUnit()
    {
        base.StartUnit();

        unitState = new IdleEnemyUnitState();
        unitTargetingState = new ClosestTargetingEnemyUnitState();

        transform.rotation = Quaternion.Euler(0, 0, 180);
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

    virtual public void Bite() {
        
        unitTarget.Damage();
        return;
    }
}
