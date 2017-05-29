using System;
using System.Threading;

namespace ChatServer
{
    class Program
    {
        /// <summary>
        /// Server
        /// </summary>
        static ServerObject server;

        /// <summary>
        /// Thread for listening
        /// </summary>
        static Thread listenThread;
        static void Main(string[] args)
        {
            try
            {
                server = new ServerObject();
                listenThread = new Thread(new ThreadStart(server.Listen));
                listenThread.Start(); //start thread
            }
            catch (Exception ex)
            {
                server.Disconnect();
                Console.WriteLine(ex.Message);
            }
        }
    }
}

