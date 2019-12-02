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

    private bool isSelected = false;
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
        UpdateSelectionTarget();
        base.UpdateUnit();
    }

    virtual public void Shoot() {
        
        Instantiate(_bulletPrefab, transform.position, transform.rotation);
        return;
    }

    override public void SelectUnit(bool _isSelected) {

        isSelected = _isSelected;
        base.SelectUnit(_isSelected);
    }


    private void UpdateSelectionTarget() {
        if (isSelected)
        {
            if (unitTarget != null) {
                unitTarget.SelectUnit(true);
                if (_selectLine)
                {
                    _selectLine.enabled = true;
                }
            }
            if (pointTarget != null) {
                //Debug.DrawLine(pointTarget.GetValueOrDefault() - Vector3.up, pointTarget.GetValueOrDefault() + Vector3.up, Color.red);
                //Debug.DrawLine(pointTarget.GetValueOrDefault() - Vector3.left, pointTarget.GetValueOrDefault() + Vector3.left, Color.red);
                if (_pointTargetMarkInstance) {
                    _pointTargetMarkInstance.SetActive(true);
                }
                if (_selectLine)
                {
                    _selectLine.enabled = true;
                }
            }
        }
        else {
            SelectUnit(false);
            _selectLine.enabled = false;
            if (_pointTargetMarkInstance) {
                _pointTargetMarkInstance.SetActive(false);
            }
            if (unitTarget != null) {
                unitTarget.SelectUnit(false);
            }
        }
    }
}
