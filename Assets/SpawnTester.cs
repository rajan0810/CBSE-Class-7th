using UnityEngine;

public class ObjectSpawnerTest : MonoBehaviour
{
    public SpawnAth3na spawner; // Reference to the ObjectSpawner script

    void Start()
    {
        spawner.SpawnObject();
    }

}