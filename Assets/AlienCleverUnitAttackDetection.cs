using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienCleverUnitAttackDetection : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Attack") {
            float prob = Random.Range(0f, 1.0f);
            if (prob < transform.parent.GetComponent<AlienCleverUnit>()._dodgeChance) {
                transform.parent.GetComponent<AlienCleverUnit>().Dash(other.transform.position);
            }
        }
    }
}
