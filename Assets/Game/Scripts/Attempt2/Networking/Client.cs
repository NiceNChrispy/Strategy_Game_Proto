using System.IO;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

namespace Networking
{
    public class Client : MonoBehaviour
    {
        TcpClient clientSocket;
        NetworkStream networkStream;

        public string message = "";

        private void Awake()
        {
            clientSocket = new TcpClient();
            
        }

        [NaughtyAttributes.Button("Local Connect")]
        public void ConnectToLocalServer()
        {
            IPHostEntry ipHostEntry = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostEntry.AddressList[0];
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, 8888);

            Connect(ipAddress.ToString(), 8888);
        }

        public void Connect(string ipAddress, int port)
        {
            clientSocket.Connect(ipAddress, port);
            networkStream = clientSocket.GetStream();
        }

        [NaughtyAttributes.Button("Send Message")]
        public void SendMessage()
        {
            Send(message);
        }
        
        public void Send(string message)
        {
            //Write code here to send data
            StreamWriter streamWriter = new StreamWriter(networkStream);
            Debug.Log(message);
            streamWriter.Write(message);
            streamWriter.Flush();
        }

        public void Close()
        {
            clientSocket.Close();
        }

        //public string Receive()
        //{
        //
        //}
    }
}
