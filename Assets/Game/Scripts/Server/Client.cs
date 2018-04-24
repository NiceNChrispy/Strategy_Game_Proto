using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Net.NetworkInformation;

public class Client : MonoBehaviour
{
    private TcpClient m_TCPClient;
    private NetworkStream m_NetworkStream;
    private StreamWriter m_StreamWriter;
    private StreamReader m_StreamReader;
    private bool IsConnected { get { return (m_TCPClient != null) ? m_TCPClient.Connected : false; } }

    public event Action<string> OnReceiveMessage = delegate { };
    public bool isHost = false;

    public string Name;
    public int Score;

    private List<GameClient> players = new List<GameClient>();

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    [NaughtyAttributes.Button("Connect to local server")]
    private void LocalConnect()
    {
        ConnectToServer("localhost", 6321);
    }

    public bool ConnectToServer(string hostName, int port)
    {
        if (IsConnected)
        {
            Debug.Log("Already connected to server");
            return false;
        }
        try
        {
            m_TCPClient = new TcpClient(hostName, port);
            m_NetworkStream = m_TCPClient.GetStream();
            m_StreamWriter = new StreamWriter(m_NetworkStream);
            m_StreamReader = new StreamReader(m_NetworkStream);
            m_StreamWriter.AutoFlush = true;
            Debug.Log("Connected");
            return true;
        }
        catch (Exception exception)
        {
            Debug.Log("Socket Error" + exception.Message);
            return false;
        }
    }

    private void Update()
    {
        if (IsConnected)
        {
            if(m_NetworkStream.DataAvailable)
            {
                string message = m_StreamReader.ReadLine();
                if (message != null)
                {
                    OnReceiveMessage.Invoke(message);
                    Receive(message);
                }
            }
        }
    }

    [NaughtyAttributes.Button("Ping")]
    private void PingTest()
    {
        Ping("www.google.com");
    }

    private void Ping(string hostName)
    {
        try
        {
            System.Net.NetworkInformation.Ping myPing = new System.Net.NetworkInformation.Ping();
            PingReply reply = myPing.Send(hostName, 100000);
            if (reply != null)
            {
                Debug.Log("Status :  " + reply.Status + " \n Time : " + reply.RoundtripTime.ToString() + " \n Address : " + reply.Address);
            }
        }
        catch (Exception exception)
        {
            Debug.Log(exception.Message);
        }
    }

    public void Send(string message)
    {
        if (!IsConnected)
        {
            Debug.Log("Not Connected");
            return;
        }

        m_StreamWriter.WriteLine(message);
    }

    [NaughtyAttributes.Button("Send Score")]
    private void SendScore()
    {
        Send(Server.NetworkMessage.Encode("SCORE", Score.ToString()));
    }

    //Receive messages from server
    private void Receive(string message)
    {
        //Debug.Log(string.Format("Received Message ({0})", message));
        OnReceiveMessage.Invoke(message);

        string[] messageParams = Server.NetworkMessage.Decode(message);
        if(messageParams[0] == Server.WHO_ARE_YOU)
        {
            Send(Server.NetworkMessage.Encode(Server.I_AM, Name));
        }
        //
        //string[] messageParameters = Server.NetworkMessage.Decode(message);
        //
        //switch (messageParameters[0])
        //{
        //    case Server.GET_CONNECTED_CLIENTS:
        //    {
        //        for (int i = 1; i < messageParameters.Length - 1; i++)
        //        {
        //            UserConnected(messageParameters[i], false);
        //        }
        //        string reply = Server.NetworkMessage.Encode(Server.GET_CONNECTED_CLIENTS, clientName, isHost.ToString());
        //        //string reply = string.Join("|", new string[] { "CWHO",  });
        //        Send(reply);
        //        //Send("CWHO|" + clientName + "|" + ((isHost) ? 1 : 0).ToString());
        //        break;
        //    }
        //    case "SCNN":
        //    {
        //        UserConnected(messageParameters[1], false);
        //        break;
        //    }
        //}
    }

    private void UserConnected(string name, bool host)
    {
        GameClient gameClient = new GameClient();
        gameClient.name = name;

        players.Add(gameClient);

        //if (isHost)
        //{
        //    Send("SQINFO|1,1,1,1,1,1,3");
        //}
        //else
        //{
        //    Send("SQINFO|2,2,2,2,2,2,4");
        //}

        //if (players.Count == 2)
        //{
        //    GameManager.Instance.StartGame();
        //}
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
        if (!IsConnected)
        {
            return;
        }

        m_StreamWriter.Close();
        m_StreamReader.Close();
        m_TCPClient.Close();
    }
}

public class GameClient
{
    public string name;
    public bool isHost;
}
