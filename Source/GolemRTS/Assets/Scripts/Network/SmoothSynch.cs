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
			if(Network.isServer)
			{
				pos = transform.localPosition;
				rot = transform.localRotation;
				
				stream.Serialize(ref pos);
				stream.Serialize(ref rot);

				ServerSynchronize(stream, info);
			}
		}
		else
		{
			if(Network.isClient)
			{
				stream.Serialize(ref pos);
				stream.Serialize(ref rot);
				started = true;

				ServerSynchronize(stream, info);
			}
		}
	}

	protected virtual void ServerSynchronize(BitStream stream, NetworkMessageInfo info)
	{

	}

	protected virtual void ClientSynchronize(BitStream stream, NetworkMessageInfo info)
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
