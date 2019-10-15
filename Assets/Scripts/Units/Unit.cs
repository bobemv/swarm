using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class Unit : MonoBehaviour
{
    public Vector3 positionToGo = Vector3.zero;
    protected bool isMoving;
    protected Unit target;

    [SerializeField]
    protected float _maxLives;
    protected float lives;

    [SerializeField]
    protected float _speed, _turnRate;
    

    [SerializeField]
    protected GameObject _livesTextPrefab;
    
    private TextMesh livesText;

    private GameObject livesTextInstance;

    void Start() {

        CreateAndLinkLivesText();
        StartCustomUnit();
    }

    virtual protected void StartCustomUnit() {
        return;
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
    void Update()
    {
        UpdateIsMoving();
        UpdatePosition();
        UpdateRotation();
        UpdateLives();
        Shoot();
        Bite();
        UpdateCustomUnit();
    }

    virtual protected void UpdateIsMoving() {
        isMoving = Vector3.Distance(positionToGo, transform.position) > 0.05;
    }
    virtual protected void UpdatePosition() {
        if (isMoving) {
            transform.Translate(Vector3.Normalize(new Vector3(positionToGo.x - transform.position.x, positionToGo.y - transform.position.y, 0)) * _speed * Time.deltaTime, Space.World);
        }
    }

    virtual protected void UpdateRotation() {
        if (target == null) {
            return;
        }

        Vector3 myLocation = transform.position;
        Vector3 targetLocation = target.transform.position;
        targetLocation.z = myLocation.z;
        
        Vector3 vectorToTarget = targetLocation - myLocation;
        
        Quaternion targetRotation = Quaternion.LookRotation(forward: Vector3.forward, upwards: vectorToTarget);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * _turnRate);
    }
    virtual public void Move(Vector3 newPosition) {
        positionToGo = newPosition;
        
    }
    virtual protected void Shoot() {
        return;
    }

    virtual protected void Bite() {
        return;
    }


    virtual public void SelectTarget(Unit newTarget) {
        target = newTarget;
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

    virtual protected void UpdateCustomUnit() {
        return;
    }
}
