using UnityEngine;
using System.Collections;

public class SmoothSynch : MonoBehaviour 
{
	private static float smoothStrength = 0.2f;
	private Vector3 pos;
	private Quaternion rot;
	private bool started;
	
	void OnSerializeNetworkView (BitStream stream, NetworkMessageInfo info) 
	{
		if(stream.isWriting)
		{
			if(Network.isServer || true)
			{
				pos = transform.localPosition;
				rot = transform.localRotation;
				
				stream.Serialize(ref pos);
				stream.Serialize(ref rot);
			}
		}
		else
		{
			if(Network.isClient)
			{
				stream.Serialize(ref pos);
				stream.Serialize(ref rot);
				started = true;
			}
		}
	}
	
	void FixedUpdate()
	{
		if(started)
		{
			transform.localPosition = Vector3.Lerp(transform.localPosition, pos, smoothStrength);
			transform.localRotation = Quaternion.Slerp(transform.localRotation, rot, smoothStrength);
		}
	}
}
