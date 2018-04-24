using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Net;

public class Server : MonoBehaviour
{
    public int port = 6321;

    [SerializeField] private List<TcpClient> m_ConnectedClients = new List<TcpClient>();
    [SerializeField] private List<TcpClient> m_DisconnectedClients = new List<TcpClient>();

    public event Action<TcpClient, string> OnRecieveMessage = delegate { };

    private TcpListener m_TCPListner;
    private bool m_IsServerRunning;

    string allUsers = "";

    public const string WHO_ARE_YOU             = "WHO_ARE_YOU";
    public const string I_AM                    = "I_AM";
    public const string ON_CLIENT_CONNECTED     = "ON_CLIENT_CONNECTED";
    public const string ON_CLIENT_DISCONNECTED  = "ON_CLIENT_DISCONNECTED";

    [SerializeField] MessageLogger m_MessageLog;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        InitLog();
    }

    [NaughtyAttributes.Button("Create Log File")]
    void InitLog()
    {
        string logPath = string.Format("{0}/{1}", Application.persistentDataPath, "ServerLog.txt");

        m_MessageLog = new MessageLogger(logPath);
        m_MessageLog.Clear();

        //if (File.Exists(logPath))
        //{
        //    string[] message = new string[] { "HELLO THIS IS A TEST", "THE TEST WAS SUCCESSFUL" };
        //
        //    StreamWriter streamWriter = new StreamWriter(logPath, true);
        //    foreach (string line in message)
        //    {
        //        streamWriter.WriteLine(line);
        //    }
        //    streamWriter.Close();
        //}
    }

    //private void OnDestroy()
    //{
    //    m_MessageLog.Write();
    //}

    [NaughtyAttributes.Button("Start Server")]
    public void Begin()
    {
        try
        {
            IPAddress address = IPAddress.Any;
            m_TCPListner = new TcpListener(address, port);
            m_TCPListner.Start();
            StartListening();
            m_IsServerRunning = true;
            Debug.Log(string.Format("Server started ({0}:{1})", address, port));
        }
        catch (Exception exception)
        {
            Debug.Log("Socket Error" + exception.Message);
        }
    }

    private void Update()
    {
        if (!m_IsServerRunning)
        {
            return;
        }

        foreach (TcpClient client in m_ConnectedClients)
        {
            if (!IsConnected(client))
            {
                client.Close();
                m_DisconnectedClients.Add(client);
                continue;
            }
            else
            {
                NetworkStream networkStream = client.GetStream();
                if (networkStream.DataAvailable)
                {
                    StreamReader reader = new StreamReader(networkStream, true);
                    string message = reader.ReadLine();

                    if (message != null)
                    {
                        Debug.Log("Server received: " + message);
                        OnRecieveMessage.Invoke(client, message);
                    }
                }
            }
        }
    }

    private bool IsConnected(TcpClient client)
    {
        try
        {
            if (client != null && client.Client != null && client.Client.Connected)
            {
                if (client.Client.Poll(0, SelectMode.SelectRead))
                {
                    return (client.Client.Receive(new byte[1], SocketFlags.Peek) == 1);
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        catch
        {
            return false;
        }
    }

    private void StartListening()
    {
        m_TCPListner.BeginAcceptTcpClient(OnClientConnected, m_TCPListner);
    }

    private void OnClientConnected(IAsyncResult aSyncResult)
    {
        TcpListener listener = (TcpListener)aSyncResult.AsyncState;

        TcpClient clientSocket = listener.EndAcceptTcpClient(aSyncResult);

        if (clientSocket != null)
        {
            Debug.Log("Received connection request from: " + clientSocket.Client.RemoteEndPoint.ToString());
        }

        m_ConnectedClients.Add(clientSocket);

        StartListening();

        Broadcast(NetworkMessage.Encode(WHO_ARE_YOU), clientSocket);
    }

    private void Broadcast(string message, params TcpClient[] clients)
    {
        foreach (TcpClient client in clients)
        {
            try
            {
                StreamWriter writer = new StreamWriter(client.GetStream());
                writer.WriteLine(message);
                if(m_MessageLog != null)
                {
                    Debug.Log("NOT NULL");
                }
                m_MessageLog.Log(message);
                Debug.Log("Broadcast: " + message);
                writer.Flush();
            }
            catch (Exception exception)
            {
                Debug.Log("Write Error" + exception.Message);
            }
        }
    }

    public class NetworkMessage
    {
        static string separatorString = " ";
        static char separatorChar = ' ';

        public static string Encode(params string[] parameters)
        {
            return string.Join(separatorString, parameters );
        }

        public static string[] Decode(string message)
        {
            return message.Split(separatorChar);
        }
    }
}

public class ServerClient : TcpClient
{
    public string ClientName;

    public ServerClient(string hostname, int port) : base(hostname, port)
    {

    }
}