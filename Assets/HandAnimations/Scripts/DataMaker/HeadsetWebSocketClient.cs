// using System;
// using System.Net.Sockets;
// using System.Text;
// using System.Threading.Tasks;
// using UnityEngine;
// using UnityEngine.UI;

// public class HeadsetWebSocketClient : MonoBehaviour
// {
//     [Header("Client Settings")]
//     public string serverIP = "192.0.0.2";    // Replace with your PC's IP address on the local network
//     public int port = 8080;                  // Must match the server port

//     [Header("UI Reference")]
//     public Text connectionStatusText;        // UI text to show connection status

//     async void Start()
//     {
//         await ConnectToServer();
//     }

//     async Task ConnectToServer()
//     {
//         try
//         {
//             TcpClient client = new TcpClient();
//             await client.ConnectAsync(serverIP, port);
//             UpdateUI("Connected to server!");
//             Debug.Log("Connected to server!");

//             // Send the single payload "This is the data"
//             string payload = "This is the data";
//             byte[] dataToSend = Encoding.UTF8.GetBytes(payload);
//             NetworkStream stream = client.GetStream();
//             await stream.WriteAsync(dataToSend, 0, dataToSend.Length);
//             Debug.Log("Sent: " + payload);
//             UpdateUI("Message sent!");

//             // Optionally close the connection after sending the message
//             client.Close();
//         }
//         catch (Exception e)
//         {
//             Debug.LogError("Client error: " + e.Message);
//             UpdateUI("Connection failed.");
//         }
//     }

//     void UpdateUI(string status)
//     {
//         if (connectionStatusText != null)
//         {
//             connectionStatusText.text = status;
//         }
//     }
// }


using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class HeadsetWebSocketClient : MonoBehaviour
{
    [Header("Client Settings")]
    public string serverIP = "192.0.0.2"; // Replace with your PC's local IP address
    public int port = 8080;               // Must match the server port

    [Header("UI Reference")]
    public Text connectionStatusText;     // UI text to show connection status

    private TcpClient client;
    private NetworkStream stream;
    private bool isConnected = false;
    private bool isConnecting = false;

    async void Start()
    {
        AttemptConnection();
    }

    async void AttemptConnection()
    {
        while (!isConnected)
        {
            if (isConnecting)
            {
                await Task.Delay(1000); // Wait before retrying
                continue;
            }

            isConnecting = true;
            UpdateUI($"Server: {serverIP}:{port}\nStatus: Attempting to Connect...");

            try
            {
                client = new TcpClient();
                await client.ConnectAsync(serverIP, port);
                stream = client.GetStream();
                isConnected = true;
                isConnecting = false;

                UpdateUI($"Server: {serverIP}:{port}\nStatus: Connected");
                Debug.Log("Connected to PC");
            }
            catch (Exception e)
            {
                Debug.LogError($"Connection Failed: {e.Message}");
                UpdateUI($"Server: {serverIP}:{port}\nStatus: Connection Failed\nRetrying...");
                isConnecting = false;

                await Task.Delay(1000); // Retry after 1 second
            }
        }
    }

    public async void SendData(string message)
{
    if (!isConnected)
    {
        Debug.LogWarning("Not connected to server. Trying to reconnect...");
        AttemptConnection();
        return;
    }

    try
    {
        string formattedMessage = message + "\n"; // Append newline to indicate end of message
        byte[] encodedMessage = Encoding.UTF8.GetBytes(formattedMessage);

        int chunkSize = 2048; // Increased chunk size to handle large messages
        int totalBytes = encodedMessage.Length;
        int bytesSent = 0;

        while (bytesSent < totalBytes)
        {
            int bytesToSend = Math.Min(chunkSize, totalBytes - bytesSent);
            await stream.WriteAsync(encodedMessage, bytesSent, bytesToSend);
            bytesSent += bytesToSend;
        }

        Debug.Log($"Sent ({totalBytes} bytes): {formattedMessage}");
        UpdateUI($"Data Sent ({totalBytes} bytes)");
    }
    catch (Exception e)
    {
        Debug.LogError($"Error sending data: {e.Message}");
        UpdateUI($"Error Sending Data:\n{e.Message}");
        isConnected = false;
        AttemptConnection(); // Try reconnecting
    }
}


    void UpdateUI(string status)
    {
        if (connectionStatusText != null)
        {
            connectionStatusText.text = status;
        }
    }

    void OnDestroy()
    {
        isConnected = false;
        client?.Close();
    }
}
