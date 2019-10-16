using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelection : MonoBehaviour
{
    private Unit _unit;
    // Start is called before the first frame update
    void Start()
    {
        _unit = GetComponentInParent<Unit>();

        if (_unit == null) {
            Debug.LogError("Unit Selection should be added as a direct child of a game object with a Unit Script!");
        }
    }
    public Unit GetUnit() {
        return _unit;
    }
}
