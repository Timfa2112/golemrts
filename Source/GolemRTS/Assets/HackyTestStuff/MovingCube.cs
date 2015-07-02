using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TimGame.Engine;

public class MovingCube : MonoBehaviour 
{
	private List<Vector2> currentPath = new List<Vector2>();
	private float lerpPoint = 0;

	void Start () 
	{

	}
	
	void Update () 
	{
		if(currentPath != null && currentPath.Count > 0)
		{
			Vector2 target2D = PathfindConstants.GridToWorld(currentPath[0]);
			Vector3 targetPosition = new Vector3(target2D.x, 0, target2D.y);
			transform.position = Vector3.Lerp(transform.position, targetPosition, lerpPoint);

			lerpPoint += Time.deltaTime * 3;

			if(Vector3.Distance(transform.position, targetPosition) < 0.01f)
			{
				currentPath.RemoveAt(0);
				lerpPoint = 0;
			}
		}
		else
		{
			Debug.Log("Making new path");
			currentPath = Pathfinder.FindPathToGridPoint(PathfindConstants.WorldToGrid(transform.position.x, transform.position.z), CellSpawner.rooms[Random.Range(0, CellSpawner.rooms.Count)].GridPos);
			Debug.Log(currentPath.Count + " steps");
		}

		foreach(RoomData room in CellSpawner.rooms)
		{
			Vector2 worldPos = PathfindConstants.GridToWorld(room.GridPos);
			Debug.DrawLine(new Vector3(worldPos.x, 0, worldPos.y), new Vector3(worldPos.x, 1, worldPos.y), room.type == RoomData.Type.Passable? Color.green : Color.red);
		}
	}
}
