using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class Unit : MonoBehaviour
{
    [SerializeField] protected float _maxLives;
    [SerializeField] protected GameObject _livesTextPrefab;
    [SerializeField] protected float _turnRate;
    [SerializeField] protected SpriteRenderer _selectionMark;
    [SerializeField] private float _radiusStopPosition;
    [SerializeField] private float _speed;

    protected float lives;
    protected PlayManager _playManager;
    
    private TextMesh livesText;

    private GameObject livesTextInstance;
    private Vector3 positionToGo = Vector3.zero;
    protected Unit unitTarget;
    protected Vector3? pointTarget;
    void Start() {
        StartUnit();
    }

    void Update()
    {
        UpdateUnit();
    }

    virtual protected void StartUnit() {
        _playManager = GameObject.Find("PlayManager").GetComponent<PlayManager>();
        CreateAndLinkLivesText();
    }

    virtual protected void UpdateUnit() {
        UpdateRotation();
        UpdateLives();
    }

    private void CreateAndLinkLivesText() {
        lives = _maxLives;
        livesTextInstance = Instantiate(_livesTextPrefab);
        livesText = livesTextInstance.GetComponent<TextMesh>();
        livesText.text = "";

        ConstraintSource source = new ConstraintSource();
        source.sourceTransform = transform;
        source.weight = 1.0f;
        livesTextInstance.GetComponent<PositionConstraint>().AddSource(source);
    }

    private void UpdateRotation() {
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

    virtual protected void UpdateLives() {
        if (lives < _maxLives) {
            livesText.text = lives + "/" + _maxLives;
        }

        if (lives <= 0) {
            Destroy(livesTextInstance);
            Destroy(gameObject);
        }
    }

    virtual public void SelectUnit() {
        _selectionMark.enabled = true;
    }
    virtual public void UnselectUnit() {
        _selectionMark.enabled = false;
    }
    public void Damage(int damage)
    {
        lives-=damage;
    }

    public float GetRadiusStopPosition() {
        return _radiusStopPosition;   
    }

    public float GetSpeed() {
        return _speed;   
    }

    public Vector3 GetPositionToGo() {
        return positionToGo;   
    }
    public void SetPositionToGo(Vector3 newPositionToGo) {
        positionToGo = newPositionToGo;
    }

    public Unit GetUnitTarget() {
        return unitTarget;   
    }
    public void SetUnitTarget(Unit newUnitTarget) {
        unitTarget = newUnitTarget;
    }

    public Vector3? GetPointTarget() {
        return pointTarget;   
    }
    public void SetPointTarget(Vector3? newPointTarget) {
        pointTarget = newPointTarget;
    }
}
