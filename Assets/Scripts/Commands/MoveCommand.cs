using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCommand : ICommand
{
    private Vector3 position;
    public MoveCommand(Vector3 _position) {
        position = _position;
    }
    public void Execute(Unit unit) {
        //unit.Move(position);
    }
}
