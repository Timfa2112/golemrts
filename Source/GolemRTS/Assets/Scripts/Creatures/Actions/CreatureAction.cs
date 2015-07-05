using UnityEngine;
using System.Collections;
using System;

public class CreatureAction
{
    // for network
    public int rawActiontype;
    public int sequenceID;
    public Vector3 location;
    public int targetID;
    public int what; // attack, repair, etc

    public bool lowPrio = false;
    public ActionType actionType;
    public enum ActionType { NoAction = 0, MoveAction = 1 };


    public static CreatureAction create(int rawActionType, int sequenceID, Vector3 location, int targetID, int what, bool lowPrio = false)
    {
        ActionType at = ActionType.NoAction;
        switch (rawActionType)
        {
            case 1: at = ActionType.MoveAction; break;
            default: throw new Exception("Actiontype with ID "+rawActionType+" does not exist!"); 
        }
        return create(at, sequenceID, location, targetID, what, lowPrio);
    }

    public static CreatureAction create (ActionType actionType, int sequenceID, Vector3 location, int targetID, int what, bool lowPrio = false) {
        CreatureAction ca = new CreatureAction();
        ca.rawActiontype = (int)actionType;
        ca.location = location;
        ca.targetID = targetID;
        ca.lowPrio = lowPrio;
        ca.what = what;
        return ca;
    }
}
