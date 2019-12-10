using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtectingAlienStandardState : IEnemyUnitState
{
    private GameObject alienProtecting;
    public ProtectingAlienStandardState(GameObject _alienProtecting) {
        alienProtecting = _alienProtecting;
    }
    public IEnemyUnitState CheckChangeState(EnemyUnit unit, PlayManager environment) {
        return null;
    }

    public void Update(EnemyUnit unit, PlayManager environment) {
        //unit.transform.Translate(Vector3.Normalize(new Vector3(unit.GetUnitTarget().transform.position.x - unit.transform.position.x, unit.GetUnitTarget().transform.position.y - unit.transform.position.y, 0)) * unit.GetSpeed() * Time.deltaTime, Space.World);
        //float r = Vector3.Distance(unit.transform.position, alienProtecting.transform.position);
        //float x = Mathf.Cos(Time.deltaTime / 100) * r + alienProtecting.transform.position.x;
        //float y = Mathf.Sin(Time.deltaTime / 100) * r + alienProtecting.transform.position.y;
        //unit.transform.position = new Vector3(x, y, unit.transform.position.z);
        unit.transform.RotateAround(alienProtecting.transform.position, alienProtecting.transform.forward, 100*Time.deltaTime);
        
        /*unit.transform.RotateAround (alienProtecting.transform.position, Vector3.up, 80f * Time.deltaTime);
        Vector3 desiredPosition = (unit.transform.position - alienProtecting.transform.position).normalized * r + alienProtecting.transform.position;
        unit.transform.position = Vector3.MoveTowards(unit.transform.position, desiredPosition, 0.5f * Time.deltaTime);
        */
    }

}
