using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

//TODO: Rename class to something more suitable

public class GameServer : MonoBehaviour
{
    [SerializeField] private Server m_Server;

    public void Start()
    {
        m_Server.OnRecieveMessage += MessageLookup;
    }

    void MessageLookup(TcpClient serverClient, string message)
    {

    }

    public void ClientConnected()
    {

    }
}