using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSpawn : MonoBehaviour
{
    [SerializeField] private Transform spawnPosition;

    [Header("Destination info")]
    [SerializeField] private LevelType destinationLevel;
    [SerializeField] private int destinationIndex;


    public Vector3 GetSpawnPos() {
        return spawnPosition.position;
    }

    public void GotoDestination() {
        GameHandler.Instance.GotoLevel(destinationLevel, destinationIndex);
    }
}
