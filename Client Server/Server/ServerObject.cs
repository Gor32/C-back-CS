using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;

namespace ChatServer
{
    public class ServerObject
    {
        /// <summary>
        /// Server for Listen
        /// </summary>
        static TcpListener tcpListener;
        /// <summary>
        /// All connectivity users
        /// </summary>
        List<ClientObject> clients = new List<ClientObject>(); 


        /// <summary>
        /// Add ClientObject to clients List 
        /// </summary>
        /// <param name="clientObject"></param>
        protected internal void AddConnection(ClientObject clientObject)
        {
            clients.Add(clientObject);
        }

        /// <summary>
        /// RemoveConnection by ID
        /// </summary>
        /// <param name="id">ID user</param>
        protected internal void RemoveConnection(string id)
        {
            ClientObject client = clients.FirstOrDefault(c => c.Id == id);
            if (client != null)
                clients.Remove(client);
        }

        /// <summary>
        /// Listening for incoming connections
        /// </summary>
        protected internal void Listen()
        {
            try
            {
                tcpListener = new TcpListener(IPAddress.Any,8888);
                tcpListener.Start();
                Console.WriteLine("Server started. Wait clinets...");

                while (true)
                {
                    TcpClient tcpClient = tcpListener.AcceptTcpClient();

                    ClientObject clientObject = new ClientObject(tcpClient, this);
                    Thread clientThread = new Thread(new ThreadStart(clientObject.Process));
                    clientThread.Start();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Disconnect();
            }
        }

        /// <summary>
        ///  Broadcast message to connected clients
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="id">User id</param>
        protected internal void BroadcastMessage(string message, string id)
        {
            byte[] data = Encoding.Unicode.GetBytes(message);
            for (int i = 0; i < clients.Count; i++)
            {
                if (clients[i].Id != id) 
                {
                    clients[i].Stream.Write(data, 0, data.Length);
                }
            }
        }

        /// <summary>
        /// Disabling all clients
        /// </summary>
        protected internal void Disconnect()
        {
            tcpListener.Stop(); //Stop server

            for (int i = 0; i < clients.Count; i++)
            {
                clients[i].Close(); //Disconnect clinets
            }
            Environment.Exit(0); //end of process
        }
    }
}