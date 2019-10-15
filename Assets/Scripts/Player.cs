using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private PlayManager _playManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void UpdatePlayer()
    {
        _playManager.allyUnits.ForEach(SearchTargets);
    }

    void SearchTargets(Unit unit) {
        float minDistance = Mathf.Infinity;
        Unit target = null;
        for (int i = 0; i < _playManager.enemyUnits.Count; i++) {
            float distance;
            distance = Vector3.Distance(_playManager.enemyUnits[i].transform.position, unit.transform.position);
            if (distance < minDistance) {
                minDistance = distance;
                target = _playManager.enemyUnits[i];
            }
        }

        if (target != null) {
            TargetCommand targetCommand = new TargetCommand(target);
            targetCommand.Execute(unit);
        }
    }

    public void Spawn(GameObject unitPrefab) {
        Vector3 positionToGo = _playManager.GetEmptyPositionForAllyUnit();
        Unit unitSpawned = _playManager.Spawn(unitPrefab);
        _playManager.AddAllyUnit(unitSpawned);
        MoveCommand moveCommand = new MoveCommand(positionToGo);
        moveCommand.Execute(unitSpawned);
    }
}
