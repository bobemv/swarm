using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    [SerializeField] private float _speedBullet;
    [SerializeField] private int _damageBullet;
    // Update is called once per frame
    void Update()
    {
        
        transform.Translate(Vector3.up * _speedBullet * Time.deltaTime);

        if (transform.position.x > 10 || transform.position.x < -10 || transform.position.y > 10 || transform.position.y < -10) {
            Destroy(gameObject);
        }
    }


    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Alien" && other.gameObject.GetComponent<Unit>()) {
            other.gameObject.GetComponent<Unit>().Damage(_damageBullet);
            Destroy(gameObject);
        }
    }
}
