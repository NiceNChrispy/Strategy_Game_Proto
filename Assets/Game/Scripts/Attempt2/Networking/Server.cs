using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

namespace Networking
{
    public class Server : MonoBehaviour
    {
        private static TcpListener serverSocket;
        private List<TcpClient> connectedClients = new List<TcpClient>();

        [NaughtyAttributes.Button("Start Server")]
        public static void StartServer()
        {
            try
            {
                IPHostEntry ipHostEntry = Dns.GetHostEntry(Dns.GetHostName());
                IPAddress ipAddress = ipHostEntry.AddressList[0];
                IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, 8888);
                serverSocket = new TcpListener(ipEndPoint);
                serverSocket.Start();
                Debug.Log("Asynchonous server socket is listening at: " + ipEndPoint.Address.ToString());
                WaitForClients();
            }
            catch (Exception exception)
            {
                Debug.Log("Socket Error" + exception.Message);
            }
        }

        private static void WaitForClients()
        {
            serverSocket.BeginAcceptTcpClient(new AsyncCallback(OnClientConnected), serverSocket);
        }

        private static void OnClientConnected(IAsyncResult asyncResult)
        {
            try
            {
                TcpClient clientSocket = serverSocket.EndAcceptTcpClient(asyncResult);

                if (clientSocket != null)
                {
                    Debug.Log("Received connection request from: " + clientSocket.Client.RemoteEndPoint.ToString());
                }
            }
            catch (Exception exception)
            {
                Debug.Log(exception.Message);
            }
            WaitForClients();
        }

        private void Update()
        {
            foreach (TcpClient client in connectedClients)
            {
                NetworkStream networkStream = client.GetStream();
                if (networkStream.DataAvailable)
                {
                    StreamReader reader = new StreamReader(networkStream, true);
                    string message = reader.ReadLine();

                    if (message != null)
                    {
                        Debug.Log("Server received: " + message);
                        //OnRecieveMessage.Invoke(client, message);
                    }
                }
            }
        }
    }
}
