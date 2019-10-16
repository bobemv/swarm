using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierDrunkUnit : AllyUnit
{

    override public void Shoot() {
        
        Instantiate(_bulletPrefab, transform.position, transform.rotation);
        Instantiate(_bulletPrefab, transform.position, Quaternion.Inverse(transform.rotation));
        return;
    }
}
