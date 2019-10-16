using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IANormal : MonoBehaviour
{
    [SerializeField]
    private PlayManager _playManager;

    [SerializeField]
    private GameObject _alienCleverPrefab, _alienStandardPrefab;
    [SerializeField]
    private float _alienCleverRespawnRate, _alienStandardRespawnRate;

    private float alienCleverTimer, alienStandardTimer;

    public delegate void MultiDelegate();
    private MultiDelegate spawnDelegate;

    [SerializeField]
    private List<GameObject> _hordesPrefabs;
    // Start is called before the first frame update
    public void StartIA()
    {
        //spawnDelegate += SpawnAlienStandard;
        //spawnDelegate += SpawnAlienClever;
        StartCoroutine(StartHordes());
    }

    IEnumerator StartHordes() {
        GameObject currentHordeInstance;
        int numHordesBeat = 0;

        while (numHordesBeat < _hordesPrefabs.Count) {
            currentHordeInstance = Instantiate(_hordesPrefabs[numHordesBeat]);

            while(currentHordeInstance != null) {
                yield return new WaitForSeconds(0.5f);
            }
            numHordesBeat++;
        }

        Debug.Log("YOU WIN!!!");
    }

    public void StopIA()
    {
        //spawnDelegate -= SpawnAlienStandard;
        //spawnDelegate -= SpawnAlienClever;
    }


    // Update is called once per frame
    public void UpdateIA()
    {
        //_playManager.enemyUnits.ForEach(SearchTargets);
        /*if (spawnDelegate != null) {
            spawnDelegate();
        }*/

    }

    void SearchTargets(Unit unit) {
        float minDistance = Mathf.Infinity;
        Unit target = null;
        for (int i = 0; i < _playManager.allyUnits.Count; i++) {
            float distance;
            distance = Vector3.Distance(_playManager.allyUnits[i].transform.position, unit.transform.position);
            if (distance < minDistance) {
                target = _playManager.allyUnits[i];
                minDistance = distance;
            }
        }

        if (target != null) {
            TargetCommand targetCommand = new TargetCommand(target);
            targetCommand.Execute(unit);
        }
    }

    public void SpawnAlienStandard() {
        if (alienStandardTimer < _alienStandardRespawnRate) {
            alienStandardTimer += Time.deltaTime;
            return;
        }
        alienStandardTimer = 0;
        Unit unitSpawned = Instantiate(_alienStandardPrefab).GetComponent<Unit>();
        unitSpawned.transform.position = new Vector3(Random.Range(-6.5f, 6.5f), unitSpawned.transform.position.y, unitSpawned.transform.position.z);
        _playManager.AddEnemyUnit(unitSpawned);
    }

    public void SpawnAlienClever() {
        if (alienCleverTimer < _alienCleverRespawnRate) {
            alienCleverTimer += Time.deltaTime;
            return;
        }
        alienCleverTimer = 0;
        Unit unitSpawned = Instantiate(_alienCleverPrefab).GetComponent<Unit>();
        unitSpawned.transform.position = new Vector3(Random.Range(-6.5f, 6.5f), unitSpawned.transform.position.y, unitSpawned.transform.position.z);
        _playManager.AddEnemyUnit(unitSpawned);
    }
}
