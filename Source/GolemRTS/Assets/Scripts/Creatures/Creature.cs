using UnityEngine;
using System.Collections;

public class Creature : MonoBehaviour {

    QueueActionManager queueActionManager;
    public int creatureID; // nieuwe creature ID = PLAYERINDEX + the-amount-of-creatures-this-player-has * PLAYERCOUNT

	void Start () {
        queueActionManager = new QueueActionManager();
	}
	
	void Update () {
	
	}
}
