using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

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
    private bool isGameOver = false;
    [Header("UI")]
    [SerializeField]
    private GameObject _startGameUI, _timerText, _gameStats, _alienStandardStat, _alienCleverStat, _alienRazerStat, _gameOver;

    private float timer;

    private float limitLineTopAllyUnit = -1f;
    private float limitLineBottomAllyUnit = -5f;

    public Unit unitSelected;
    public Unit unitTarget;
    public Vector3? pointTarget;

    float massiveSelectionTimeThreshold = 0.12f;
    float massiveSelectionCurrentTime = 0f;
    bool isMassiveSelection = false;

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
        //enemyUnits = enemyUnits.FindAll(unit => unit != null);
        GameObject[] enemyUnitsInstances = GameObject.FindGameObjectsWithTag("Alien");
        enemyUnits = new List<Unit>(enemyUnitsInstances.Select(enemyUnitsInstance => enemyUnitsInstance.GetComponent<Unit>()));
        
        UpdateTimer();
        UpdateGameStats();
        DisableButtonsAllyUnits();
        CheckSelectUnit();
        CheckGameOver();

        if (isMassiveSelection && clickUp.HasValue && clickDown.HasValue) {
            LineRenderer line = GetComponent<LineRenderer>();
            Vector3 topLeft = new Vector3(Mathf.Min(clickDown.GetValueOrDefault().x, clickUp.GetValueOrDefault().x), Mathf.Max(clickDown.GetValueOrDefault().y, clickUp.GetValueOrDefault().y), -1);
            Vector3 topRight = new Vector3(Mathf.Max(clickDown.GetValueOrDefault().x, clickUp.GetValueOrDefault().x), Mathf.Max(clickDown.GetValueOrDefault().y, clickUp.GetValueOrDefault().y), -1);
            Vector3 bottomLeft = new Vector3(Mathf.Min(clickDown.GetValueOrDefault().x, clickUp.GetValueOrDefault().x), Mathf.Min(clickDown.GetValueOrDefault().y, clickUp.GetValueOrDefault().y), -1);
            Vector3 bottomRight = new Vector3(Mathf.Max(clickDown.GetValueOrDefault().x, clickUp.GetValueOrDefault().x), Mathf.Min(clickDown.GetValueOrDefault().y, clickUp.GetValueOrDefault().y), -1);

            Vector3[] lineVertices = { topLeft, bottomLeft, bottomRight, topRight, topLeft };
            line.positionCount = 5;
            line.SetPositions(lineVertices);

            unitsSelected = new List<AllyUnit>();
            allyUnits.ForEach(allyUnit => {
                if (allyUnit.transform.position.x > topLeft.x &&
                    allyUnit.transform.position.x < topRight.x &&
                    allyUnit.transform.position.y > bottomLeft.y &&
                    allyUnit.transform.position.y < topLeft.y)
                {
                    unitsSelected.Add((AllyUnit)allyUnit);
                }
            });

            unitsSelected.ForEach(unitSelected => {
                unitSelected.SelectUnit(true);
            });
        }

        updateDelegate();
    }

    public Vector3 GetEmptyPositionForAllyUnit() {
        int position = 0;
        Vector3 emptyPosition = leffMostPosition + new Vector3(position * cellSizeAllyUnitsPositions, 0, 0);

        while (allyUnits.Exists(unit => unit.GetPositionToGo() == emptyPosition)) {
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
        _unitButtons.ForEach(button => button.interactable = !(allyUnits.Count == _maxAllyUnits) && !isGameOver);
    }

    public void StartGame() {
        isGameStarted = true;
        _startGameUI.SetActive(false);
        //_timerText.SetActive(true);
        _gameStats.SetActive(true);
        _IANormal.StartIA();
    }

    private void UpdateTimer() {
        if (!isGameStarted) {
            return;
        }
        timer += Time.deltaTime;
        _timerText.GetComponent<Text>().text = string.Format("{0:0.0}", timer);
    }

    private void UpdateGameStats() {
        if (!isGameStarted) {
            return;
        }
        _alienStandardStat.GetComponentInChildren<Text>().text = EnemyUnit.alienStandardUnitDestroyed.ToString();
        _alienCleverStat.GetComponentInChildren<Text>().text = EnemyUnit.alienCleverUnitDestroyed.ToString();
        _alienRazerStat.GetComponentInChildren<Text>().text = EnemyUnit.alienRazerUnitDestroyed.ToString();
    }

    private void CheckGameOver() {
        if (isGameStarted && allyUnits.Count == 0)
        {
            isGameStarted = false;
            isGameOver = true;
            _gameOver.SetActive(true);
        }
    }

    public void Restart() {
        SceneManager.LoadScene(0);
    }

    private List<AllyUnit> unitsSelected = new List<AllyUnit>();

    private Vector3? clickDown;
    private Vector3? clickUp;

    public bool isUnitSelected(Unit unit) {
        return unitsSelected.Exists(unitSelected => unitSelected == unit);
    }

    private void CheckSelectUnit() {
        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            clickDown = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 9f));
        }
        if (Input.GetKey(KeyCode.Mouse0)) {
            massiveSelectionCurrentTime += Time.deltaTime;
            if (massiveSelectionCurrentTime > massiveSelectionTimeThreshold)
            {
                isMassiveSelection = true;
                clickUp = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 9f));
                unitsSelected.ForEach(unitSelected => {
                    unitSelected.SelectUnit(false);
                });
                unitsSelected = new List<AllyUnit>();
                unitTarget = null;
                pointTarget = null;
            }
        }

        if (Input.GetKeyUp(KeyCode.Mouse0)) {
            unitTarget = null;
            pointTarget = null;
            isMassiveSelection = false;
            clickUp = null;
            clickDown = null;
            LineRenderer line = GetComponent<LineRenderer>();
            line.positionCount = 0;
            line.SetPositions(new Vector3[0]);

            if (massiveSelectionCurrentTime < massiveSelectionTimeThreshold) {
                unitsSelected.ForEach(unitSelected => {
                    unitSelected.SelectUnit(false);
                });
                unitsSelected = new List<AllyUnit>();
                massiveSelectionCurrentTime = 0f;

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                Physics.Raycast(ray, out hit);
               
                if (hit.collider && hit.collider.tag == "AllySelection")
                {
                    //unitsSelected = new List<Unit>();
                    AllyUnit unit = (AllyUnit) hit.collider.gameObject.GetComponent<UnitSelection>().GetUnit();
                    //unitSelected = unit;
                    //unitsSelected = new List<Unit>();
                    unitsSelected.Add(unit);
                    unit.SelectUnit(true);
                }
                if (hit.collider && hit.collider.tag == "AlienSelection")
                {
                    //
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse1)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            Physics.Raycast(ray, out hit);
            if (hit.collider && hit.collider.tag == "AlienSelection")
            {
                Unit unit = hit.collider.gameObject.GetComponent<UnitSelection>().GetUnit();

                unitsSelected.ForEach(unitSelected => {
                    unitSelected.SetTarget(unit);
                    //unitSelected.unitTarget = unit;
                    unitSelected.SetPointTarget(null);
                });
            }
            else
            {
                Vector3 positionSelected = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 9f));

                if (positionSelected.y < limitLineTopAllyUnit && positionSelected.y > limitLineBottomAllyUnit)
                {
                    unitsSelected.ForEach(unitSelected => {
                        unitSelected.SetPositionToGo(positionSelected);
                    });
                }
                else
                {
                    unitsSelected.ForEach(unitSelected => {
                        unitSelected.SetTarget(null);
                        unitSelected.SetPointTarget(positionSelected);
                    });
                }
            }

        }
    }

    public void Spawn(GameObject unitPrefab) {
        Vector3 positionToGo = GetEmptyPositionForAllyUnit();
        GameObject unitSpawned = Instantiate(unitPrefab);
        
        Unit unit = unitSpawned.GetComponent<Unit>();
        AddAllyUnit(unit);
        unit.SetPositionToGo(positionToGo);
    }
}
