using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class NetworkConnector : MonoBehaviour 
{
	public static bool CurrentPlayerIsHost;

	public bool StartServer(string gameName, string password)
	{
		CurrentPlayerIsHost = true;
		NetworkConnectionError nErr = Network.InitializeServer(1, 25000, !Network.HavePublicAddress());
        if (nErr != NetworkConnectionError.NoError)
        {
            return false;
        }
		MasterServer.RegisterHost("GolemRTS_Server_31536000", gameName, password);
        return true;
	}
	
	public void Refresh()
    {
        MasterServer.ClearHostList();
        MasterServer.RequestHostList("GolemRTS_Server_31536000");	
	}
	
	public HostData[] showGames(string searchTerm)
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
		
//		OpponentSpawn.ToSpawn = "OnlinePlayer";
		Application.LoadLevel("GameScene");
	}
}
