using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{

    [SerializeField]
    private GameObject _alienPrefab;
    [SerializeField]
    private float _spawnTime;
    [SerializeField]
    private int _number;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnEnemy());
    }

    public void DestroyRespawn() {
        StopCoroutine(SpawnEnemy());
        Destroy(gameObject);
    }

    IEnumerator SpawnEnemy() {
        Debug.Log("SPAWN OF " + _alienPrefab.name + " STARTED");
        float currentNumber = 0;
        while (currentNumber < _number) {
            Instantiate(_alienPrefab, transform.position, Quaternion.identity);
            currentNumber++;
            yield return new WaitForSeconds(_spawnTime);
        }
        Debug.Log("SPAWN " + _alienPrefab.name + " DESTROYED");

        Destroy(gameObject);
    }
}
