using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleMi.ConsoleMiCorps
{
    // Serveur TCP asynchrone
    public class TcpServeur
    {
        private TcpListener listener;
        private List<TcpClient> clients = new List<TcpClient>();
        private bool isRunning = false;

        public event Action<TcpClient> ClientConnected;
        public event Action<TcpClient> ClientDisconnected;
        public event Action<TcpClient, string> MessageReceived;

        public TcpServeur(string ipAddress, int port)
        {
            listener = new TcpListener(IPAddress.Parse(ipAddress), port);
        }

        public async Task StartAsync()
        {
            listener.Start();
            isRunning = true;
            while (isRunning)
            {
                var client = await listener.AcceptTcpClientAsync();
                clients.Add(client);
                ClientConnected?.Invoke(client);
                _ = HandleClientAsync(client);
            }
        }

        public void Stop()
        {
            isRunning = false;
            listener.Stop();
            foreach (var client in clients)
            {
                client.Close();
            }
            clients.Clear();
        }

        private async Task HandleClientAsync(TcpClient client)
        {
            var stream = client.GetStream();
            var buffer = new byte[1024];

            try
            {
                while (isRunning && client.Connected)
                {
                    int byteCount = await stream.ReadAsync(buffer, 0, buffer.Length);
                    if (byteCount == 0)
                        break; // Déconnexion du client

                    string message = Encoding.UTF8.GetString(buffer, 0, byteCount);
                    MessageReceived?.Invoke(client, message);
                }
            }
            catch
            {
                // Log l'erreur si besoin
            }
            finally
            {
                clients.Remove(client);
                ClientDisconnected?.Invoke(client);
                client.Close();
            }
        }

        public async Task BroadcastAsync(string message)
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            foreach (var client in clients)
            {
                if (client.Connected)
                {
                    await client.GetStream().WriteAsync(data, 0, data.Length);
                }
            }
        }
    }

    // Client TCP asynchrone
    public class TcpClientAsync
    {
        private TcpClient client;
        private NetworkStream stream;
        public event Action<string> MessageReceived;
        public event Action Connected;
        public event Action Disconnected;

        public async Task ConnectAsync(string ipAddress, int port)
        {
            client = new TcpClient();
            await client.ConnectAsync(IPAddress.Parse(ipAddress), port);
            stream = client.GetStream();
            Connected?.Invoke();
            _ = ReceiveAsync();
        }

        public async Task SendAsync(string message)
        {
            if (client?.Connected == true)
            {
                byte[] data = Encoding.UTF8.GetBytes(message);
                await stream.WriteAsync(data, 0, data.Length);
            }
        }

        private async Task ReceiveAsync()
        {
            var buffer = new byte[1024];

            try
            {
                while (client.Connected)
                {
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                    if (bytesRead == 0)
                        break; // Déconnecté

                    string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    MessageReceived?.Invoke(message);
                }
            }
            catch
            {
                // Log l'erreur si besoin
            }
            finally
            {
                Disconnected?.Invoke();
                client?.Close();
            }
        }
    }
}