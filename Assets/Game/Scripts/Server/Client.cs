using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Net;

public class Client : MonoBehaviour
{
    private bool socketReady;
    private TcpClient socket;
    private NetworkStream stream;
    private StreamWriter writer;
    private StreamReader reader;
    public bool isHost = false;

    public string clientName;

    private List<GameClient> players = new List<GameClient>();

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public bool ConnectToServer(string host, int port)
    {
        if (socketReady)
            return false;

        try
        {
            socket = new TcpClient(host, port);
            stream = socket.GetStream();
            writer = new StreamWriter(stream);
            reader = new StreamReader(stream);

            socketReady = true;
        }
        catch (Exception e)
        {
            Debug.Log("Socket Error" + e.Message);
        }

        return socketReady;
    }

    private void Update()
    {
        if (socketReady)
        {
            if(stream.DataAvailable)
            {
                string data = reader.ReadLine();
                if (data != null)
                {
                    OnIncomingData(data);
                }
            }
        }
    }

    //Send messages to server
    public void Send(string data)
    {
        if (!socketReady)
            return;

        writer.WriteLine(data);
        writer.Flush();
    }

    //read messages from server
    private void OnIncomingData(string data)
    {
        Debug.Log("Client:" + data);

        string[] aData = data.Split('|');
        switch (aData[0])
        {
            case "SWHO":
                for (int i = 1; i < aData.Length -1; i++)
                {
                    UserConnected(aData[i], false);
                }
                Send("CWHO|" + clientName + "|" + ((isHost)?1:0).ToString());
             
                break;

            case "SCNN":
                UserConnected(aData[1], false);
                break;
        }
    }

    private void UserConnected(string name, bool host)
    {
        GameClient c = new GameClient();
        c.name = name;

        players.Add(c);

        if (isHost)
        {
            Send("SQINFO|1,1,1,1,1,1,3");
        }

        else
        {
            Send("SQINFO|2,2,2,2,2,2,4");
        }

        if (players.Count == 2)
        {
            GameManager.Instance.StartGame();
        }
    }

    private void OnDisable()
    {
        CloseSocket();
    }

    private void OnApplicationQuit()
    {
        CloseSocket();
    }

    private void CloseSocket()
    {
        if (!socketReady)
            return;

        writer.Close();
        reader.Close();
        socket.Close();
        socketReady = false;
    }
}

public class GameClient
{
    public string name;
    public bool isHost;
}
