using System;
using System.Net.Sockets;
using System.Text;

namespace ChatServer
{
    public class ClientObject
    {
        /// <summary>
        /// Id is uniqy for Client
        /// </summary>
        protected internal string Id { get; private set; }

        /// <summary>
        /// Which stream connect client
        /// </summary>
        protected internal NetworkStream Stream { get; private set; }

        /// <summary>
        /// Name of user
        /// </summary>
        string userName;
        
        /// <summary>
        /// TCP network connections
        /// </summary>
        TcpClient client;

        /// <summary>
        /// Server objcet who connected
        /// </summary>
        ServerObject server; 

        /// <summary>
        /// ClientObject .contr
        /// </summary>
        /// <param name="tcpClient">TCP network </param>
        /// <param name="serverObject">Server</param>
        public ClientObject(TcpClient tcpClient, ServerObject serverObject)
        {
            Id = Guid.NewGuid().ToString();
            client = tcpClient;
            server = serverObject;
            serverObject.AddConnection(this);
        }

        /// <summary>
        /// Which will do when connect to server (server creat thread and runing the method Process)
        /// </summary>
        public void Process()
        {
            try
            {
                Stream = client.GetStream();
                // set user name
                string message = GetMessage();
                userName = message;

                message = userName + " Entered chat";
                //send message all users (to server too)
                server.BroadcastMessage(message, this.Id);
                Console.WriteLine(message);
                // infinity loop get message for users
                while (true)
                {
                    try
                    {
                        message = GetMessage();
                        message = String.Format("{0}: {1}", userName, message);
                        Console.WriteLine(message);
                        server.BroadcastMessage(message, this.Id);
                    }
                    catch
                    {
                        message = String.Format("{0}: Left the chat", userName);
                        Console.WriteLine(message);
                        server.BroadcastMessage(message, this.Id);
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                // server out loop ending
                server.RemoveConnection(this.Id);
                Close();
            }
        }

        /// <summary>
        /// Geting Message to users
        /// </summary>
        /// <returns>Message</returns>
        private string GetMessage()
        {
            byte[] data = new byte[64];
            StringBuilder builder = new StringBuilder();
            int bytes = 0;
            do
            {
                bytes = Stream.Read(data, 0, data.Length);
                builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
            }
            while (Stream.DataAvailable);

            return builder.ToString();
        }

        /// <summary>
        /// Close network
        /// </summary>
        protected internal void Close()
        {
            if (Stream != null)
                Stream.Close();
            if (client != null)
                client.Close();
        }
    }
}