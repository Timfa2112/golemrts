using UnityEngine;
using System.Collections;
using TimGame.Engine;

public class WalkableTime : MonoBehaviour 
{
	void Start () 
	{
		CellSpawner.DefineCell (new Vector2 (transform.position.x, transform.position.z), RoomData.Type.Passable);
	}
}
