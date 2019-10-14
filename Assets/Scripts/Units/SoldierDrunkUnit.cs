using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierDrunkUnit : Unit
{
    [SerializeField]
    private GameObject _bulletPrefab;
    
    [SerializeField]
    protected float _fireRate;
    private float _currentTime = 0;
    override protected void Shoot() {
        
        if (_currentTime > _fireRate) {
            Instantiate(_bulletPrefab, transform.position, transform.rotation);
            Instantiate(_bulletPrefab, transform.position, Quaternion.Inverse(transform.rotation));
            _currentTime = 0;
        }
        else {
            _currentTime += Time.deltaTime;
        }
        return;
    }
}
