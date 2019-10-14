using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public Vector3 positionToGo = Vector3.zero;
    private bool isMoving;
    protected Unit target;

    [SerializeField]
    protected float _lives;

    [SerializeField]
    protected float _speed;

    void Update()
    {

        UpdatePosition();
        UpdateRotation();
        Shoot();
    }

    virtual protected void UpdatePosition() {
        isMoving = Vector3.Distance(positionToGo, transform.position) > 0.05;
        if (isMoving) {
            transform.Translate(Vector3.Normalize(new Vector3(positionToGo.x - transform.position.x, positionToGo.y - transform.position.y, 0)) * _speed * Time.deltaTime, Space.World);
        }
    }

    virtual protected void UpdateRotation() {
        if (target == null) {
            return;
        }

        Vector3 myLocation = transform.position;
        Vector3 targetLocation = target.transform.position;
        targetLocation.z = myLocation.z;
        
        Vector3 vectorToTarget = targetLocation - myLocation;
        
        Quaternion targetRotation = Quaternion.LookRotation(forward: Vector3.forward, upwards: vectorToTarget);
        transform.rotation = targetRotation;
    }
    virtual public void Move(Vector3 newPosition) {
        positionToGo = newPosition;
        
    }
    virtual protected void Shoot() {
        return;
    }

    virtual public void SelectTarget(Unit newTarget) {
        target = newTarget;
    }

    virtual public void Damage() {
        _lives--;
        if (_lives == 0) {
            Destroy(gameObject);
        }
    }
}
