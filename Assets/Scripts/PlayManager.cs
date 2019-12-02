using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    [SerializeField]
    private GameObject _startButton, _timerText;

    private float timer;

    private float limitLineTopAllyUnit = -1f;
    private float limitLineBottomAllyUnit = -2.9f;

    public Unit unitSelected;
    public Unit unitTarget;
    public Vector3? pointTarget;

    float massiveSelectionTimeThreshold = 0.2f;
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
        DisableButtonsAllyUnits();
        CheckSelectUnit();

        if (isMassiveSelection && clickUp.HasValue && clickDown.HasValue) {
            LineRenderer line = GetComponent<LineRenderer>();
            Vector3 topLeft = new Vector3(Mathf.Min(clickDown.GetValueOrDefault().x, clickUp.GetValueOrDefault().x), Mathf.Max(clickDown.GetValueOrDefault().y, clickUp.GetValueOrDefault().y), -1);
            Vector3 topRight = new Vector3(Mathf.Max(clickDown.GetValueOrDefault().x, clickUp.GetValueOrDefault().x), Mathf.Max(clickDown.GetValueOrDefault().y, clickUp.GetValueOrDefault().y), -1);
            Vector3 bottomLeft = new Vector3(Mathf.Min(clickDown.GetValueOrDefault().x, clickUp.GetValueOrDefault().x), Mathf.Min(clickDown.GetValueOrDefault().y, clickUp.GetValueOrDefault().y), -1);
            Vector3 bottomRight = new Vector3(Mathf.Max(clickDown.GetValueOrDefault().x, clickUp.GetValueOrDefault().x), Mathf.Min(clickDown.GetValueOrDefault().y, clickUp.GetValueOrDefault().y), -1);

            Vector3[] lineVertices = { topLeft, bottomLeft, bottomRight, topRight, topLeft };
            line.positionCount = 5;
            line.SetPositions(lineVertices);


            unitsSelected = new List<Unit>(allyUnits.Where(allyUnit => {
                return allyUnit.transform.position.x > topLeft.x &&
                       allyUnit.transform.position.x < topRight.x &&
                       allyUnit.transform.position.y > bottomLeft.y &&
                       allyUnit.transform.position.y < topLeft.y;
            }));

            unitsSelected.ForEach(unitSelected => {
                StartCoroutine(unitSelected.UnitSelected());
            });
        }

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

    private List<Unit> unitsSelected = new List<Unit>();

    private Vector3? clickDown;
    private Vector3? clickUp;

    public bool isUnitSelected(Unit unit) {
        return unitsSelected.Exists(unitSelected => unitSelected == unit);
    }

    private void CheckSelectUnit() {
        //mouse 0 keydown
            //0 selected
                //ray
                //if ally, add to selected
                //if enemy, nothing happens
            //>0 selected
                //ray
                //if enemy, set target enemy to all selected
                //if space movable, move units
                //if space no movable, set target point to all selected
            //set initial position for massive selection

        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            clickDown = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 9f));

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            Physics.Raycast(ray, out hit);
            
            if (unitsSelected.Count == 0) {
                if (hit.collider && hit.collider.tag == "AllySelection")
                {
                    //unitsSelected = new List<Unit>();
                    Unit unit = hit.collider.gameObject.GetComponent<UnitSelection>().GetUnit();
                    //unitSelected = unit;
                    //unitsSelected = new List<Unit>();
                    unitsSelected.Add(unit);
                    StartCoroutine(unit.UnitSelected());
                }
                if (hit.collider && hit.collider.tag == "AlienSelection")
                {
                    //
                }
            }
            else if (unitsSelected.Count > 0) {
                if (hit.collider && hit.collider.tag == "AlienSelection")
                {
                    Unit unit = hit.collider.gameObject.GetComponent<UnitSelection>().GetUnit();
                    
                    unitsSelected.ForEach(unitSelected => {
                        unitSelected.unitTarget = unit;
                        unitSelected.pointTarget = null;
                    });
                }
                else
                {
                    Vector3 positionSelected = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 9f));

                    if (positionSelected.y < limitLineTopAllyUnit && positionSelected.y > limitLineBottomAllyUnit) {
                        //unitSelected.positionToGo = positionSelected;
                        unitsSelected.ForEach(unitSelected => {
                            unitSelected.positionToGo = positionSelected;
                        });
                    }
                    else {
                        unitsSelected.ForEach(unitSelected => {
                            unitSelected.unitTarget = null;
                            unitSelected.pointTarget = positionSelected;
                        });
                    }   
                }
            }
        }
        //mouse 0 key
            //add time to massiveSelectionCurrentTime
            //massiveSelectionCurrentTime > massiveSelectionTimeThreshold
                //isMassiveSelection = true
                //set final position por massive selection
        if (Input.GetKey(KeyCode.Mouse0)) {
            massiveSelectionCurrentTime += Time.deltaTime;
            if (massiveSelectionCurrentTime > massiveSelectionTimeThreshold)
            {
                isMassiveSelection = true;
                clickUp = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 9f));
            }
        }

        //mouse 0 keyup
            //isMassiveSelection = false
            //clear unitTarget, pointTarget, massiveSelectioCurrentTime

        if (Input.GetKeyUp(KeyCode.Mouse0)) {
            isMassiveSelection = false;
            clickUp = null;
            clickDown = null;
            unitTarget = null;
            pointTarget = null;
            massiveSelectionCurrentTime = 0f;
            LineRenderer line = GetComponent<LineRenderer>();
            line.positionCount = 0;
            line.SetPositions(new Vector3[0]);
        }

        //mouse 1
            //clear all information: unitsSelected, unitTarget, pointTarget, isMassiveSelection = false, massiveSelectioCurrentTime = 0
        if (Input.GetKeyDown(KeyCode.Mouse1)) {
            //UnselectAll();
            //unitSelected = null;
   
            unitsSelected = new List<Unit>();
            unitTarget = null;
            pointTarget = null;
            isMassiveSelection = false;
            massiveSelectionCurrentTime = 0f;
            clickUp = null;
            clickDown = null;
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
