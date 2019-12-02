using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienCleverUnit : EnemyUnit
{
    public float _dodgeChance;
    public void Dash(Vector3 attack) {
        Vector2 positionToDash =  Vector2.Perpendicular(new Vector2(attack.x - transform.position.x, attack.y - transform.position.y));
        //Debug.DrawRay(transform.position, new Vector3(positionToDash.x - transform.position.x, positionToDash.y - transform.position.y, transform.position.z), Color.white, 2.0f);

        //transform.position = new Vector3(positionToDash.x, positionToDash.y, transform.position.z);
        transform.Translate(positionToDash.x, positionToDash.y, 0);
    }

    override protected void UpdateLives() {
        if (lives <= 0) {
            EnemyUnit.alienCleverUnitDestroyed++;
        }
        base.UpdateLives();
    }
}
