using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class QueueActionManager : MonoBehaviour{

    bool isServer;
    bool queueCleared;

    // attributes for host only
    int queueActionIndex;
    int currentQueueActionSequenceID;
    CreatureAction idleAction;

    List<CreatureAction> actionQueue;

    public QueueActionManager()
    {
        isServer = Network.isServer;
        actionQueue = new List<CreatureAction>();
        
        currentQueueActionSequenceID = 0;
        queueActionIndex = 0;
        idleAction = CreatureAction.create(CreatureAction.ActionType.NoAction, 0, Vector3.zero, 0, 0);
        //TODO register this instance to be watched by the networkview!
    }

    public CreatureAction getCurrentAction()
    {
        if (!isServer)
        {
            throw new Exception("Only the host is allowed to use this method!");
        }
        if (actionQueue.Count > 0 && currentQueueActionSequenceID < actionQueue[0].sequenceID) {
            queueActionIndex = 0;
            currentQueueActionSequenceID = actionQueue[0].sequenceID;
        }
        return (queueActionIndex >= actionQueue.Count) ? idleAction : actionQueue[queueActionIndex];
    }

    public CreatureAction completeCurrentAction()
    {
        if (!isServer)
        {
            throw new Exception("Only the host is allowed to use this method!");
        }

        queueActionIndex++;
        if (queueActionIndex < actionQueue.Count)
        {
            currentQueueActionSequenceID = actionQueue[queueActionIndex].sequenceID;
        }
        return getCurrentAction();
    }

    public void clear()
    {
        actionQueue.Clear();
    }

    public void addAction(CreatureAction ca)
    {
        while (actionQueue[actionQueue.Count - 1].lowPrio)
        {
            actionQueue.RemoveAt(actionQueue.Count - 1);
        }
        actionQueue.Add(ca);
    }

    void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
    {
        // The host must also send this information to all clients
        if (stream.isWriting)
        {
            serializeQueue(stream);
        }
        else
        {
            deserializeQueue(stream);
        }
    }

    private void serializeQueue(BitStream stream)
    {
        int size = actionQueue.Count;

        int[] rawActiontype = new int[size];
        int[] sequenceID = new int[size];
        Vector3[] location = new Vector3[size];
        int[] targetID = new int[size];


        stream.Serialize(ref size);
        for (int i = 0; i < size; i++)
        {
            rawActiontype[i] = actionQueue[i].rawActiontype;
            sequenceID[i] = actionQueue[i].sequenceID;
            location[i] = actionQueue[i].location;
            targetID[i] = actionQueue[i].targetID;

            stream.Serialize(ref rawActiontype[i]);
            stream.Serialize(ref sequenceID[i]);
            stream.Serialize(ref location[i]);
            stream.Serialize(ref targetID[i]);
        }
    }

    private void deserializeQueue(BitStream stream)
    {
        int size = 0;
        stream.Serialize(ref size);

        actionQueue.Clear();

        for (int i = 0; i < size; i++)
        {
            int rawActionType = 0, sequenceID = 0, targetID = 0;
            Vector3 location = Vector3.zero;

            stream.Serialize(ref rawActionType);
            stream.Serialize(ref sequenceID);
            stream.Serialize(ref location);
            stream.Serialize(ref targetID);

            actionQueue.Add(CreatureAction.create(rawActionType, sequenceID, location, targetID));
        }
    }

}
