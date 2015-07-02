using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class NetworkConnector : MonoBehaviour 
{
	public static bool CurrentPlayerIsHost;

	public void StartServer(string gameName, string password)
	{
		CurrentPlayerIsHost = true;
		Network.InitializeServer(1, 25000, !Network.HavePublicAddress());
		MasterServer.RegisterHost("GolemRTS_Server_31536000", gameName, password);	
	}
	
	public void Refresh()
	{
		MasterServer.RequestHostList("GolemRTS_Server_31536000");	
	}
	
	public HostData[] FindGames(string searchTerm)
	{
		List<HostData> hostData = MasterServer.PollHostList().ToList();
		
		if(searchTerm != "")
			hostData.RemoveAll(o => !o.gameName.Contains(searchTerm));
		
		hostData.RemoveAll(o => o.connectedPlayers >= o.playerLimit);
		
		hostData.OrderBy(o => o.gameName);
		
		return hostData.ToArray();
	}
	
	public void Connect(HostData host)
	{
		CurrentPlayerIsHost = false;
		
		NetworkConnectionError code =  Network.Connect(host);
		
		if(code == NetworkConnectionError.NoError)
		{
			MasterServer.UnregisterHost();
			Invoke("Message", 1);
		}
	}
	
	void Message()
	{
		networkView.RPC("Connected", RPCMode.All);
	}
	
	public void CancelServer()
	{
		MasterServer.UnregisterHost();	
	}
	
	[RPC] void Connected()
	{
		MasterServer.UnregisterHost();
		
		OpponentSpawn.ToSpawn = "OnlinePlayer";
		Application.LoadLevel("GameScene");
	}
}
