using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class NetworkObject
{
    public IPAddress IPAddress;
    public int Port;

    public event Action<TcpClient, string> OnRecieveMessage = delegate { };

}