using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayManager : MonoBehaviour
{
    [SerializeField]
    private int _maxAllyUnits;
    public List<Unit> enemyUnits {get; private set;}
    public List<Unit> allyUnits {get; private set;}

    private Vector3 leffMostPosition = new Vector3(-6.5f, -2.16f, -1f);
    private Vector3 rightMostPosition = new Vector3(6.5f, -2.16f, -1f);

    private float offsetBetweenAllyUnits;

    #region Unit Managers
    [SerializeField]
    private IANormal _IANormal;
    [SerializeField]
    private Player _player;
    #endregion

    [SerializeField]
    private List<Button> _unitButtons;

    public delegate void MultiDelegate();
    private MultiDelegate updateDelegate;
    // Start is called before the first frame update
    void Start()
    {
        offsetBetweenAllyUnits = (rightMostPosition.x - leffMostPosition.x) / (_maxAllyUnits - 1);
        allyUnits = new List<Unit>();
        enemyUnits = new List<Unit>();

        
        updateDelegate += _IANormal.UpdateIA;
        updateDelegate += _player.UpdatePlayer;
    }

    // Update is called once per frame
    void Update()
    {
        allyUnits = allyUnits.FindAll(unit => unit != null);
        enemyUnits = enemyUnits.FindAll(unit => unit != null);
        DisableButtonsAllyUnits();
        updateDelegate();
    }

    public Vector3 GetEmptyPositionForAllyUnit() {
        int position = 0;
        Vector3 emptyPosition = leffMostPosition + new Vector3(position * offsetBetweenAllyUnits, 0, 0);

        while (allyUnits.Exists(unit => unit.positionToGo == emptyPosition)) {
            emptyPosition += new Vector3(offsetBetweenAllyUnits, 0, 0);
        }

        return emptyPosition;
    }

    public Unit Spawn(GameObject unitPrefab) {
        GameObject unitSpawned = null;
        unitSpawned = Instantiate(unitPrefab);
        
        return unitSpawned ? unitSpawned.GetComponent<Unit>() : null;
    }

    public void AddAllyUnit(Unit unit) {
        allyUnits.Add(unit);
    }

    public void AddEnemyUnit(Unit unit) {
        enemyUnits.Add(unit);
    }

    private void DisableButtonsAllyUnits() {
        _unitButtons.ForEach(button => button.interactable = !(allyUnits.Count == _maxAllyUnits));
    }
}
