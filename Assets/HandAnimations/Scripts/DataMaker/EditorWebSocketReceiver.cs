using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class EditorWebSocketReceiver : MonoBehaviour
{
    [Header("Server Settings")]
    public int port = 8080;

    [Header("UI Reference")]
    public Text connectionStatusText;

    private TcpListener tcpListener;

    async void Start()
    {
        try
        {
            tcpListener = new TcpListener(IPAddress.Any, port);
            tcpListener.Start();
            //UpdateUI($"Server started on port {port}.\nWaiting for a connection...");
            Debug.Log($"Server started on port {port}.\nWaiting for a connection...");

            TcpClient client = await tcpListener.AcceptTcpClientAsync();
            
            //UpdateUI("Client connected!");
            Debug.Log("Client connected!");

            // Receive messages from the client until they disconnect or an error occurs
            // Read data in chunks to handle large messages efficiently
            NetworkStream stream = client.GetStream();

            byte[] buffer = new byte[4096]; // Increased buffer size to handle large data
            StringBuilder completeMessage = new StringBuilder();

            while (true)
            {
                int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                if (bytesRead > 0)
                {
                    string receivedPart = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    completeMessage.Append(receivedPart);

                    if (receivedPart.Contains("\n")) // Detect end of message
                    {
                        string finalMessage = completeMessage.ToString().Trim();
                        Debug.Log($"Received ({finalMessage.Length} chars): {finalMessage}");
                        UpdateUI($"Received Data ({finalMessage.Length} chars)");
                        completeMessage.Clear(); // Reset buffer
                    }
                }
                else
                {
                    Debug.Log("Client disconnected.");
                    UpdateUI("Client disconnected.");
                    break;
                }
            }

            client.Close();
            tcpListener.Stop();
            UpdateUI("Connection closed.");
        }
        catch (Exception e)
        {
            Debug.LogError("Server error: " + e.Message);
            UpdateUI("Server error.");
        }
    }

    void UpdateUI(string status)
    {
        if (connectionStatusText != null)
        {
            connectionStatusText.text = status;
        }
    }
}
