using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelHandler : MonoBehaviour
{
    [SerializeField] private List<LevelSpawn> spawnPoints;

    public LevelSpawn GetSpawn(int index) {
        return spawnPoints[index];
    }
}
