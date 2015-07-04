using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public static class NetworkConnector 
{
	public static bool CurrentPlayerIsHost;

    public static bool StartServer(string gameName, string password)
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

    public static void Refresh()
    {
        MasterServer.ClearHostList();
        MasterServer.RequestHostList("GolemRTS_Server_31536000");	
	}

    public static HostData[] showGames(string searchTerm)
    {
		List<HostData> hostData = MasterServer.PollHostList().ToList();
        
		if(searchTerm != "")
			hostData.RemoveAll(o => !o.gameName.Contains(searchTerm));
		
		hostData.RemoveAll(o => o.connectedPlayers >= o.playerLimit);
		
		hostData.OrderBy(o => o.gameName);
		
		return hostData.ToArray();
	}

    public static void Connect(HostData host)
	{
		CurrentPlayerIsHost = false;
		
		NetworkConnectionError code =  Network.Connect(host);
		
		if(code == NetworkConnectionError.NoError)
		{
            Connected();
		}
        
	}

    public static void WaitForOtherPlayer()
    {
        if (Network.connections.Length > 0)
        {
            Application.LoadLevel("GameScene");
        }
            
    }
    	
	public static void CancelServer()
	{
		MasterServer.UnregisterHost();	
	}
	
	[RPC] static void Connected()
	{
		Application.LoadLevel("GameScene");
	}
}
