using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyUnit : Unit
{

    [SerializeField]
    protected GameObject _bulletPrefab;
    public float fireRate;

    private IAllyUnitState unitState;
    private IAllyUnitState unitTargetingState;
    // Start is called before the first frame update
    override protected void StartUnit()
    {
        base.StartUnit();

        unitState = new IdleAllyUnitState();
        unitTargetingState = new ClosestTargetingAllyUnitState();
    }

    // Update is called once per frame
    override protected void UpdateUnit()
    {

        IAllyUnitState state;
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
        base.UpdateUnit();
    }

    virtual public void Shoot() {
        
        Instantiate(_bulletPrefab, transform.position, transform.rotation);
        return;
    }
}
