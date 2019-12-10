using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : Unit
{
    static public int alienStandardUnitDestroyed = 0;
    static public int alienCleverUnitDestroyed = 0;
    static public int alienRazerUnitDestroyed = 0;
    public float biteRate;

    protected IEnemyUnitState unitState;
    protected IEnemyUnitState unitTargetingState;
    [SerializeField] protected float timeFrozenThreshold;
    [SerializeField] protected int biteDamage;

    private Color freezeColor = new Color(0, 126, 255);
    private bool isFrozen = false;
    private float currentTimeFrozen = 0;
    private int unitsTargetingSelf = 0;

    //public float biteRadius;

    // Start is called before the first frame update
    override protected void StartUnit()
    {
        base.StartUnit();

        unitState = new IdleEnemyUnitState();
        unitTargetingState = new ClosestTargetingEnemyUnitState();

        transform.rotation = Quaternion.Euler(0, 0, 180);
    }

    // Update is called once per frame
    override protected void UpdateUnit()
    {
        if (isFrozen)
        {
            currentTimeFrozen += Time.deltaTime;
            if (currentTimeFrozen > timeFrozenThreshold) {
                isFrozen = false;
                GetComponent<SpriteRenderer>().color = Color.white;
                currentTimeFrozen = 0;
            }
            return;
        }
        base.UpdateUnit();

        IEnemyUnitState state;
        state = unitTargetingState.CheckChangeState(this, _playManager);
        if (state != null) {
            unitTargetingState = state;
        }
        unitTargetingState.Update(this, _playManager);

        state = unitState.CheckChangeState(this, _playManager);
        if (state != null) {
            unitState = state;
        }
        unitState.Update(this, _playManager);

    }

    virtual public void Bite() {
        
        unitTarget.Damage(biteDamage);
        return;
    }

    virtual public void Freeze() {
        isFrozen = true;
        GetComponent<SpriteRenderer>().color = freezeColor;

        unitTarget.Damage(biteDamage);
        return;
    }

    static public void ResetStats() {
        alienStandardUnitDestroyed = 0;
        alienCleverUnitDestroyed = 0;
        alienRazerUnitDestroyed = 0;
    }

    override public void SelectUnit(bool isSelected)
    {
        if (isSelected)
        {
            unitsTargetingSelf++;
        }
        else
        {
            unitsTargetingSelf--;
        }

        base.SelectUnit(unitsTargetingSelf > 0);
    }

}
