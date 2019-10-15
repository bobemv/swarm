using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : Unit
{
    [SerializeField]
    protected float _biteRate;

    [SerializeField]
    protected float _biteRadius;

    protected float biteTime;
    override public void SelectTarget(Unit newTarget) {
        target = newTarget;
        positionToGo = newTarget.transform.position;
    }

    override protected void Bite() {
        if (target == null) {
            return;
        }
        if (biteTime > _biteRate && Vector3.Distance(target.transform.position, transform.position) < _biteRadius) {
            biteTime = 0;
            target.Damage();
        }
        biteTime += Time.deltaTime;
    }

    override protected void UpdateIsMoving() {
        isMoving = Vector3.Distance(positionToGo, transform.position) > _biteRadius;
    }
}
