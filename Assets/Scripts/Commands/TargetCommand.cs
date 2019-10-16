using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetCommand : ICommand
{
    private Unit target;
    public TargetCommand(Unit _target) {
        target = _target;
    }
    public void Execute(Unit unit) {
        //unit.SelectTarget(target);
    }
}
