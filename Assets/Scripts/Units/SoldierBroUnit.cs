using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierBroUnit : Unit
{
    [SerializeField]
    private GameObject _bulletPrefab;
    
    [SerializeField]
    protected float _fireRate;
    private float _currentTime = 0;
    override protected void Shoot() {
        
        if (_currentTime > _fireRate) {
            _currentTime = 0;
            Instantiate(_bulletPrefab, transform.position, transform.rotation);
        }
        else {
            _currentTime += Time.deltaTime;
        }
        return;
    }
}
