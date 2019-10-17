using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class Unit : MonoBehaviour
{
    public Vector3 positionToGo = Vector3.zero;
    public Unit unitTarget;
    public Vector3? pointTarget;
    public Vector3 pointTargetv2;

    [SerializeField]
    protected float _maxLives;
    protected float lives;

    [SerializeField]
    protected GameObject _livesTextPrefab;
    
    private TextMesh livesText;

    private GameObject livesTextInstance;

    [SerializeField]
    protected float _turnRate;

    public float _speed;
    

    [SerializeField]
    protected MeshRenderer _selectionMark;

    public float radiusStopPosition;

    protected PlayManager _playManager;

    void Start() {
        StartUnit();
    }

    virtual protected void StartUnit() {
        _playManager = GameObject.Find("PlayManager").GetComponent<PlayManager>();
        CreateAndLinkLivesText();
    }

    void Update()
    {
        UpdateUnit();
    }

    virtual protected void UpdateUnit() {
        pointTargetv2 = pointTarget.HasValue ? pointTarget.GetValueOrDefault() : Vector3.zero;
        UpdateRotation();
        UpdateLives();
    }

    virtual protected void CreateAndLinkLivesText() {
        lives = _maxLives;
        livesTextInstance = Instantiate(_livesTextPrefab);
        livesText = livesTextInstance.GetComponent<TextMesh>();
        livesText.text = "";

        ConstraintSource source = new ConstraintSource();
        source.sourceTransform = transform;
        source.weight = 1.0f;
        livesTextInstance.GetComponent<PositionConstraint>().AddSource(source);
    }

    virtual protected void UpdateRotation() {
        if (unitTarget == null) {
            return;
        }

        Vector3 myLocation = transform.position;
        Vector3 targetLocation = unitTarget.transform.position;
        targetLocation.z = myLocation.z;
        
        Vector3 vectorToTarget = targetLocation - myLocation;
        
        Quaternion targetRotation = Quaternion.LookRotation(forward: Vector3.forward, upwards: vectorToTarget);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * _turnRate);
    }

    virtual public void Damage() {
        lives--;
    }

    virtual protected void UpdateLives() {
        if (lives < _maxLives) {
            livesText.text = lives + "/" + _maxLives;
        }

        if (lives <= 0) {
            Destroy(gameObject);
            Destroy(livesTextInstance);
        }
    }

    virtual public void SelectUnit(bool isSelected) {

        _selectionMark.enabled = isSelected;
    }


    public IEnumerator UnitSelected() {
        SelectUnit(true);
        while(_playManager.isUnitSelected(this)) {
            if (unitTarget != null) {
                unitTarget.SelectUnit(true);
            }
            if (pointTarget != null) {
                Debug.DrawLine(pointTarget.GetValueOrDefault() - Vector3.up, pointTarget.GetValueOrDefault() + Vector3.up, Color.red);
                Debug.DrawLine(pointTarget.GetValueOrDefault() - Vector3.left, pointTarget.GetValueOrDefault() + Vector3.left, Color.red);
            }
            yield return null;
        }
        SelectUnit(false);
        if (unitTarget != null) {
            unitTarget.SelectUnit(false);
        }

    }
}
