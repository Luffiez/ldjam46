using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Spawner : MonoBehaviour
{

    struct SpawnPoint
    {
        Vector3 spawnPosition;
        bool taken;

        public SpawnPoint(Vector3 spawnPosition, bool taken)
        {
            this.spawnPosition = spawnPosition;
            this.taken = taken;
        }
        
        public bool Taken
        { get { return taken; }set { taken = value; } }
        public Vector3 SpawnPosition
        { get { return spawnPosition; } }

    }
    [Tooltip("if UseTaken is used, there can only spawn one object per spawnpoint")]
    [SerializeField]
    bool UseTaken;
    [SerializeField]
    GameObject PrefabObject;
    [SerializeField]
    Transform[] SpawnTransform;
    SpawnPoint[] SpawnPoints;
    void Awake()
    {
        SpawnPoints = new SpawnPoint[SpawnPoints.Length];
        for (int i = 0; i < SpawnTransform.Length; i++)
        {
            SpawnPoints[i] = new SpawnPoint(SpawnTransform[i].position,false);
        }
    }

    public virtual GameObject Spawn()
    {
        Vector3 spawnPosition = Vector3.zero;
        int index = Random.Range(0, SpawnPoints.Length);
        if (UseTaken)
        {
            for (int i = 0; i < SpawnPoints.Length; i++)
            {
                int currentIndex = (index + i) % SpawnPoints.Length;
                if (!SpawnPoints[currentIndex].Taken)
                {
                    SpawnPoints[currentIndex].Taken = true;
                    spawnPosition = SpawnPoints[currentIndex].SpawnPosition;
                    break;
                }
            }
        }
        else
        {
            spawnPosition = SpawnPoints[index].SpawnPosition;
        }
        GameObject spawnObject = Instantiate(PrefabObject, spawnPosition, Quaternion.identity);
        return spawnObject;
    }

}
