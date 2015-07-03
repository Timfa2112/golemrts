using UnityEngine;
using System.Collections;
using System.Timers; 

public class LobbyView : MonoBehaviour {

    string playerName = "Player";
    string previousPlayerName;
    string hostName;
    string dots = "";
    Timer refreshTimer;
    HostData[] hosts;
    bool updateHostList;
    bool successfullyStartedServer;


    [SerializeField]
    private NetworkConnector connector;


	// Use this for initialization
	void Start () {
        hostName = playerName + "'s server";

        refreshTimer = new Timer();
        refreshTimer.Elapsed += new ElapsedEventHandler(refreshTimerHandler);
        refreshTimer.Interval = 5000;
        refreshTimer.Start();

        hosts = new HostData[0];

        connector.Refresh();
	}

    private void refreshTimerHandler(object sender, ElapsedEventArgs e)
    {
        updateHostList = true;
        dots += ".";
        if (dots.Equals("......"))
        {
            dots = "";
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (updateHostList)
        {
            connector.Refresh();
            updateHostList = false;
        }
	}

    void OnGUI()
    {
        // Player name
        GUI.Box(new Rect(40, 245, 600, 30), "");
        GUI.Label(new Rect(50, 250, 100, 20), "Name:");
        playerName = GUI.TextField(new Rect(100, 250, 100, 20), playerName);

        //server name
        GUI.Box(new Rect(40, 290, 600, 40), "");
        GUI.Label(new Rect(50, 300, 100, 20), "Host name:");
        hostName = GUI.TextField(new Rect(130, 300, 150, 20), hostName);
        if (GUI.Button(new Rect(500, 300, 100, 20), "Start host"))
        {
            successfullyStartedServer = connector.StartServer(hostName, "");
        }
        if (successfullyStartedServer)
        {
            GUI.Label(new Rect(300, 300, 100, 20), "Host online!");
        }


        GUI.Box(new Rect(40, 340, 600, 200), "");
        GUI.Label(new Rect(50, 350, 200, 20), "Dedicated hosts list:");
        if (hosts.Length == 0) {
            GUI.Label(new Rect(300, 380, 200, 20), "Nobody seems to be here" + dots);
        }
        /*
        hosts = connector.showGames("");

        for (int i = 0; i < hosts.Length; i++)
        {
            HostData host = hosts[i];
            GUI.Box(new Rect(42, 380+i*50, 596, 48), "");
            GUI.Label(new Rect(50, 382 + i * 50, 150, 48), host.gameName);
            if (GUI.Button(new Rect(500, 382 + i * 50, 136, 44), "Connect!"))
            {
                connector.Connect(host);
            }
        }*/
    }
}
