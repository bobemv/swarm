using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienRazer : EnemyUnit
{
    [SerializeField]
    private GameObject _attractField;
    // Start is called before the first frame update

    // Update is called once per frame
    override protected void UpdateUnit()
    {
        base.UpdateUnit();
        EnemyUnit[] aliensOrbiting = GetComponentsInChildren<EnemyUnit>();
        _attractField.SetActive(aliensOrbiting.Length < 1000);
    }

    override protected void UpdateLives() {
        if (lives <= 0) {
            EnemyUnit.alienRazerUnitDestroyed++;
        }
        base.UpdateLives();
    }
}
