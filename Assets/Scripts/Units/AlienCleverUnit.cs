using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienCleverUnit : Unit
{
    public float _dodgeChance;
    override public void SelectTarget(Unit newTarget) {
        target = newTarget;
        positionToGo = newTarget.transform.position;
    }

    override public void Damage() {
        _lives--;
        if (_lives == 0) {
            Destroy(gameObject);
        }

    }
    public void Dash(Vector3 attack) {
        Vector2 positionToDash =  Vector2.Perpendicular(new Vector2(attack.x - transform.position.x, attack.y - transform.position.y));
        Debug.DrawRay(transform.position, new Vector3(positionToDash.x - transform.position.x, positionToDash.y - transform.position.y, transform.position.z), Color.white, 2.0f);

        //transform.position = new Vector3(positionToDash.x, positionToDash.y, transform.position.z);
        transform.Translate(positionToDash.x, positionToDash.y, 0);
    }
}
