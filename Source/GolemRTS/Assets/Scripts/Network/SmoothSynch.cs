using UnityEngine;
using System.Collections;

public abstract class SmoothSynch : MonoBehaviour 
{
	private static float smoothStrength = 0.2f;
	private Vector3 pos;
	private Quaternion rot;
	private bool started;
	
	void OnSerializeNetworkView (BitStream stream, NetworkMessageInfo info) 
	{
		if(stream.isWriting)
		{
			pos = transform.localPosition;
			rot = transform.localRotation;
			
			stream.Serialize(ref pos);
			stream.Serialize(ref rot);

			NetworkWrite(stream, info);
		}
		else
		{
			stream.Serialize(ref pos);
			stream.Serialize(ref rot);
			started = true;

			NetworkRead(stream, info);
		}
	}

	protected virtual void NetworkWrite(BitStream stream, NetworkMessageInfo info)
	{

	}

	protected virtual void NetworkRead(BitStream stream, NetworkMessageInfo info)
	{
		
	}

	void FixedUpdate()
	{
		if(Network.isClient)
		{
			if(started)
			{
				transform.localPosition = Vector3.Lerp(transform.localPosition, pos, smoothStrength);
				transform.localRotation = Quaternion.Slerp(transform.localRotation, rot, smoothStrength);
			}
		}
	}
}
