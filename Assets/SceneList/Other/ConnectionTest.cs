using UnityEngine;
using System.Collections;
using SocketIOClient;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

[Serializable]
public class LaunchAppData
{
    public string appId;
}

public class ConnectionTest: MonoBehaviour
{
    private SocketIO socket;

    [Tooltip("Unique identifier for this device (e.g., PHONE_001)")]
    public string deviceId = "PHONE_001";

    [Tooltip("Socket.IO server URL")]
    public string serverUrl = "http://localhost:3000";

    private async void Start()
    {
        try
        {
            // Initialize socket with server URL
            socket = new SocketIO(serverUrl);

            // Set up event handlers before connecting
            SetupEventHandlers();

            // Connect to the server
            await socket.ConnectAsync();
            Debug.Log("Connected to Socket.IO server");

            // Send identification after connection
            await SendIdentification();

            // Start battery status reporting
            StartCoroutine(SendBatteryStatusRoutine());
        }
        catch (Exception e)
        {
            Debug.LogError($"Socket.IO Error: {e.Message}");
        }
    }

    private void SetupEventHandlers()
    {
        // Listen for connection events
        socket.OnConnected += (sender, e) =>
        {
            Debug.Log("Socket.IO connected");
        };

        socket.OnDisconnected += (sender, e) =>
        {
            Debug.Log("Socket.IO disconnected");
        };

        // Listen for the "launchApp" event
        socket.On("launchApp", (response) =>
        {
            try
            {
                // Convert the response to a string and parse with Unity's JSON utility
                string jsonData = response.ToString();
                LaunchAppData data = JsonUtility.FromJson<LaunchAppData>(jsonData);

                // Ensure we're running on the main Unity thread
                UnityMainThreadDispatcher.Instance().Enqueue(() =>
                {
                    if (data != null && !string.IsNullOrEmpty(data.appId))
                    {
                        Debug.Log($"Launch command for app: {data.appId}");
                        // Place your logic here to launch a scene or perform another action
                    }
                    else
                    {
                        Debug.LogWarning("Received launchApp event with invalid or missing appId");
                    }
                });
            }
            catch (Exception e)
            {
                Debug.LogError($"Error processing launchApp event: {e.Message}");
            }
        });
    }

    private async Task SendIdentification()
    {
        try
        {
            var data = new { deviceId = this.deviceId };
            await socket.EmitAsync("identify", data);
        }
        catch (Exception e)
        {
            Debug.LogError($"Error sending identification: {e.Message}");
        }
    }

    private IEnumerator SendBatteryStatusRoutine()
    {
        while (true)
        {
            string divname = SystemInfo.deviceName;
            float battery = SystemInfo.batteryLevel;
            if (battery >= 0f)
            {
                SendBatteryStatus(divname);
            }
            yield return new WaitForSeconds(5f);
        }
    }

    private async void SendBatteryStatus(string batteryLevel)
    {
        try
        {
            //int batteryPercent = Mathf.RoundToInt(batteryLevel * 100);
            var data = new
            {
                deviceId = this.deviceId,
                battery = batteryLevel
            };
            await socket.EmitAsync("batteryStatus", data);
        }
        catch (Exception e)
        {
            Debug.LogError($"Error sending battery status: {e.Message}");
        }
    }

    private async void OnDestroy()
    {
        if (socket != null)
        {
            await socket.DisconnectAsync();
            socket.Dispose();
        }
    }
}

// Unity Main Thread Dispatcher (add this as a separate script)
public class UnityMainThreadDispatcher : MonoBehaviour
{
    private static UnityMainThreadDispatcher instance;
    private readonly Queue<Action> executionQueue = new Queue<Action>();

    public static UnityMainThreadDispatcher Instance()
    {
        if (instance == null)
        {
            instance = FindObjectOfType<UnityMainThreadDispatcher>();
            if (instance == null)
            {
                var go = new GameObject("UnityMainThreadDispatcher");
                instance = go.AddComponent<UnityMainThreadDispatcher>();
                DontDestroyOnLoad(go);
            }
        }
        return instance;
    }

    public void Enqueue(Action action)
    {
        lock (executionQueue)
        {
            executionQueue.Enqueue(action);
        }
    }

    private void Update()
    {
        lock (executionQueue)
        {
            while (executionQueue.Count > 0)
            {
                executionQueue.Dequeue().Invoke();
            }
        }
    }
}