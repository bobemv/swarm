using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    [SerializeField]
    private float _speed;
    // Update is called once per frame
    void Update()
    {
        
        transform.Translate(Vector3.up * _speed * Time.deltaTime);

        if (transform.position.x > 10 || transform.position.x < -10 || transform.position.y > 10 || transform.position.y < -10) {
            Destroy(gameObject);
        }
    }


    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("name:" + other.gameObject.name);
        if (other.gameObject.GetComponent<Unit>()) {
            other.gameObject.GetComponent<Unit>().Damage();
            Destroy(gameObject);
        }
    }
}
