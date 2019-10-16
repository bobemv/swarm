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

    private float leftMostPositionX = -6.7f;
    private float rightMostPositionX = -6.7f;

    private float centerLineAllyUnit = -2.16f;


    private float offsetVerticalAllyUnit = 0.5f;
    private float cellSizeAllyUnitsPositions;

    #region Unit Managers
    [SerializeField]
    private IANormal _IANormal;
    #endregion

    [SerializeField]
    private List<Button> _unitButtons;

    public delegate void MultiDelegate();
    private MultiDelegate updateDelegate;

    private bool isGameStarted = false;
    [SerializeField]
    private GameObject _startButton, _timerText;

    private float timer;

    private float limitLineTopAllyUnit = -1.66f;
    private float limitLineBottomAllyUnit = -2.66f;

    public Unit unitSelected;
    public Unit unitTarget;
    public Vector3? pointTarget;

    // Start is called before the first frame update
    void Start()
    {
        cellSizeAllyUnitsPositions = (rightMostPosition.x - leffMostPosition.x) / (_maxAllyUnits - 1);
        allyUnits = new List<Unit>();
        enemyUnits = new List<Unit>();

        
        updateDelegate += _IANormal.UpdateIA;
    }

    // Update is called once per frame
    void Update()
    {
        allyUnits = allyUnits.FindAll(unit => unit != null);
        enemyUnits = enemyUnits.FindAll(unit => unit != null);
        UpdateTimer();
        DisableButtonsAllyUnits();
        CheckSelectUnit();
        updateDelegate();
    }

    public Vector3 GetEmptyPositionForAllyUnit() {
        int position = 0;
        Vector3 emptyPosition = leffMostPosition + new Vector3(position * cellSizeAllyUnitsPositions, 0, 0);

        while (allyUnits.Exists(unit => unit.positionToGo == emptyPosition)) {
            emptyPosition += new Vector3(cellSizeAllyUnitsPositions, 0, 0);
        }

        return emptyPosition;
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

    public void StartGame() {
        isGameStarted = true;
        _startButton.SetActive(false);
        _timerText.SetActive(true);
        _IANormal.StartIA();
    }

    private void UpdateTimer() {
        if (!isGameStarted) {
            return;
        }
        timer += Time.deltaTime;
        _timerText.GetComponent<Text>().text = string.Format("{0:0.0}", timer);
    }

    public void GameOver() {
        //
    }

    private void CheckSelectUnit() {
        if (Input.GetKeyDown(KeyCode.Mouse1)) {
            //UnselectAll();
            unitSelected = null;
            unitTarget = null;
            pointTarget = null;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && unitSelected == null) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            Physics.Raycast(ray, out hit);
            //Debug.Log("tag: " + hit.collider.tag);
            if (hit.collider.tag == "AllySelection")
            {
                Unit unit = hit.collider.gameObject.GetComponent<UnitSelection>().GetUnit();
                unitSelected = unit;
                StartCoroutine(unitSelected.UnitSelected());
                return;
            }
        }
        if (Input.GetKeyDown(KeyCode.Mouse0) && unitSelected != null) {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            Physics.Raycast(ray, out hit);
            if (hit.collider.tag == "AlienSelection")
            {
                Unit unit = hit.collider.gameObject.GetComponent<UnitSelection>().GetUnit();
                
                unitTarget = unit;
                pointTarget = null;
                return;
            }
            if (hit.collider.tag == "AllySelection")
            {
                Unit unit = hit.collider.gameObject.GetComponent<UnitSelection>().GetUnit();
                unitSelected = unit;
                StartCoroutine(unitSelected.UnitSelected());
                pointTarget = null;
                unitTarget = null;

                return;
            }
            else {
                Vector3 positionSelected = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 9f));
                Debug.DrawLine(unitSelected.transform.position, positionSelected, Color.white, 0.5f);
                if (positionSelected.y < limitLineTopAllyUnit && positionSelected.y > limitLineBottomAllyUnit) {
                    unitSelected.positionToGo = positionSelected;
                }
                else {
                    
                    pointTarget = positionSelected;
                    unitTarget = null;
                }
            }
            return;
        }
    }

    public void Spawn(GameObject unitPrefab) {
        Vector3 positionToGo = GetEmptyPositionForAllyUnit();
        GameObject unitSpawned = Instantiate(unitPrefab);
        
        Unit unit = unitSpawned.GetComponent<Unit>();
        AddAllyUnit(unit);
        unit.positionToGo = positionToGo;
    }
}
